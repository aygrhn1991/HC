using CWGK_SPIDER.util;
using Ivony.Html;
using Ivony.Html.Parser;
using Maticsoft.DBUtility;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;

namespace CWGK_SPIDER.spiders
{
    public static class bslc
    {
        public static void start()
        {
            string pageStr = Util.getHtmlStr("http://www.huachuan.gov.cn/zwgk/xxgksyzl/fgfgg/index.html", Encoding.Default);
            IHtmlDocument source = new JumonyParser().Parse(pageStr);
            int totalPage = int.Parse(source.FindFirst("font").InnerText().Split(new string[] { "共" }, StringSplitOptions.RemoveEmptyEntries)[1].Split(new string[] { "页" }, StringSplitOptions.RemoveEmptyEntries)[0]);
            Debug.WriteLine("----->数据总页数<-----：" + totalPage);
            for (int i = 0; i < totalPage; i++)
            {
                getByPage(i);
            }
        }

        public static void getByPage(int page)
        {
            string url = "";
            if (page == 0)
            {
                url = "http://www.huachuan.gov.cn/zwgk/xxgksyzl/fgfgg/index.html";
            }
            else
            {
                string p = page.ToString();
                if (page < 10)
                {
                    p = "0" + p;
                }
                url = "http://www.huachuan.gov.cn/system/more/zwgk/xxgksyzl/fgfgg/index/page_" + p + ".html";
            }
            string pageStr = Util.getHtmlStr(url, Encoding.Default);
            IHtmlDocument source = new JumonyParser().Parse(pageStr);
            var items = source.Find(".listmain ul li");
            foreach (var item in items)
            {
                string id = "0";
                string path = item.FindFirst("div").FindSingle("a").Attribute("href").Value();
                string title = item.FindFirst("div").FindSingle("a").InnerText();
                string time = item.FindFirst("div").NextElement().InnerText();
                if (path.StartsWith("http"))
                {
                    id = new Random().Next(1000000, 9999999).ToString();
                    string sql = string.Format("select count(*) from t_spider_bslc t where t.title='{0}'", title);
                    int count = Convert.ToInt32(DbHelperMySQL.GetSingle(sql));
                    if (count == 0)
                    {
                        sql = string.Format("insert into t_spider_bslc(id,title,time,path) values({0},'{1}','{2}','{3}')", id, title, time, path);
                        count = DbHelperMySQL.ExecuteSql(sql);
                        if (count == 1)
                        {
                            getContent(path);
                        }
                    }
                }
                else
                {
                    id = path.Split('/')[3].Split('.')[0];
                    string sql = string.Format("select count(*) from t_spider_bslc t where t.id={0}", id);
                    int count = Convert.ToInt32(DbHelperMySQL.GetSingle(sql));
                    if (count == 0)
                    {
                        sql = string.Format("insert into t_spider_bslc(id,title,time,path) values({0},'{1}','{2}','{3}')", id, title, time, path);
                        count = DbHelperMySQL.ExecuteSql(sql);
                        if (count == 1)
                        {
                            getContent(path);
                        }
                    }
                }
            }
        }
        public static void getContent(string path)
        {
            if (path.StartsWith("http"))
            {
                return;
            }
            string contentPage = Util.getHtmlStr("http://www.huachuan.gov.cn" + path, Encoding.Default);
            IHtmlDocument source = new JumonyParser().Parse(contentPage);
            string author = source.FindSingle(".maintittwo").FindLast("span").FindSingle("a").InnerText();
            string content = source.FindSingle(".mainnews").InnerHtml();
            string sql = string.Format("update t_spider_bslc t set t.author='{0}',t.content='{1}' where t.path='{2}'", author, content, path);
            int count = DbHelperMySQL.ExecuteSql(sql);
        }


    }
}
