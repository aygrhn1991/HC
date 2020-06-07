using Ivony.Html;
using Ivony.Html.Parser;
using Maticsoft.DBUtility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;

namespace CWGK_SPIDER.spiders
{
    public static class zwgk
    {
        public static void start()
        {
            HttpClient httpClient = new HttpClient();
            httpClient
               .PostAsync("http://hd.huachuan.gov.cn/aspx/gkml_list.aspx", null)
               .ContinueWith((postTask) =>
               {
                   HttpResponseMessage response = postTask.Result;
                   string cookie = response.Headers.GetValues("Set-Cookie").FirstOrDefault().Split(';')[0].Split('=')[1];
                   Debug.WriteLine("----->响应cookie<-----：" + cookie);
                   response.Content.ReadAsStringAsync().ContinueWith((readTask) =>
                   {
                       IHtmlDocument source = new JumonyParser().Parse(readTask.Result);
                       string viewState = source.FindSingle("input[name=__VIEWSTATE]").Attribute("value").Value();
                       int totalPage = int.Parse(source.FindLast("option").InnerText());
                       Debug.WriteLine("----->数据总页数<-----：" + totalPage);
                       for (int i = 1; i <= totalPage; i++)
                       {
                           getByPage(cookie, viewState, i);
                       }
                   });
               });
        }
        public static void getByPage(string cookie, string viewstate, int page)
        {
            HttpClient httpClient = new HttpClient();
            HttpContent postContent = new FormUrlEncodedContent(new Dictionary<string, string>()
            {
               {"__VIEWSTATE", viewstate},
               {"__VIEWSTATEGENERATOR", "7BE8FDE8"},
               {"__EVENTTARGET", "AspNetPager1"},
               {"__EVENTARGUMENT", page.ToString()},
               {"_keywords", ""},
               {"AspNetPager1_input", "1"},
            });
            httpClient
              .PostAsync("http://hd.huachuan.gov.cn/aspx/gkml_list.aspx", postContent)
               .ContinueWith((postTask) =>
               {
                   HttpResponseMessage response = postTask.Result;
                   response.Content.ReadAsStringAsync().ContinueWith((readTask) =>
                   {
                       //Debug.WriteLine(readTask.Result);
                       IHtmlDocument source = new JumonyParser().Parse(readTask.Result);
                       var itemCount = source.Find(".listbox").Count();
                       for (int i = 1; i <= itemCount; i++)
                       {
                           try
                           {
                               string id = source.FindSingle("#four" + i).Attribute("href").Value().Split('=')[1];
                               string author = source.Find("#con_four_" + i).Find(".li1").Last().InnerText().Replace("发布机构：", "");
                               string time = source.Find("#con_four_" + i).Find(".li2").Last().InnerText().Replace("发文日期：", "");
                               string title = source.Find("#con_four_" + i).Find(".infoname").First().InnerText().Replace("名称：", "");
                               //判断第一条是否存在，如果存在，则说明新闻一直未更新，不需要继续下去了
                               //可以使用下边逻辑，continue换成return
                               //判断是否存在
                               string sql = string.Format("select count(*) from t_spider_zwgk t where t.id={0}", id);
                               int count = Convert.ToInt32(DbHelperMySQL.GetSingle(sql));
                               if (count > 0)
                               {
                                   //continue;
                                   return;
                               }
                               //不存在，插入数据库
                               sql = string.Format("insert into t_spider_zwgk(id,title,time,author) values({0},'{1}','{2}','{3}')", id, title.Replace('\'', '"'), time, author);
                               count = DbHelperMySQL.ExecuteSql(sql);
                               if (count == 1)
                               {
                                   getContent(id);
                               }
                           }
                           catch (Exception e)
                           {
                               Debug.WriteLine("----->【" + page + "." + i + "】新闻创建异常<-----：" + e);
                           }
                       }
                   });
               });
        }
        public static void getContent(string id)
        {
            HttpClient httpClient = new HttpClient();
            try
            {
                httpClient
               .PostAsync("http://hd.huachuan.gov.cn/aspx/gkml_view.aspx?id=" + id, null)
               .ContinueWith((postTask) =>
               {
                   HttpResponseMessage response = postTask.Result;
                   response.Content.ReadAsStringAsync().ContinueWith((readTask) =>
                   {
                       try
                       {
                           IHtmlDocument source = new JumonyParser().Parse(readTask.Result);
                           string content = source.FindSingle(".zwnr").InnerHtml();
                           string sql = string.Format("update t_spider_zwgk t set t.content='{0}' where t.id={1}", content, id);
                           int count = DbHelperMySQL.ExecuteSql(sql);
                       }
                       catch (Exception e)
                       {
                           Debug.WriteLine("----->【" + id + "】内容存储异常<-----：" + e);
                       }
                   });
               });
            }
            catch (Exception e)
            {
                Debug.WriteLine("----->【" + id + "】内容存储异常<-----：" + e);
            }
        }
    }
}
