using CWGK_SPIDER.util;
using Maticsoft.DBUtility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CWGK_SPIDER.spiders
{
    public static class bjtj
    {
        public static void start()
        {
            //string pageStr = Util.getHtmlStr("http://zwfw.jms.gov.cn/hcx/icity/sunny/index", Encoding.UTF8);
            //IHtmlDocument source = new JumonyParser().Parse(pageStr);
            //string __signature = source.FindFirst("script").InnerText().Trim()
            //    .Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries)[1]
            //    .Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries)[1].Replace("\"", "").Trim();
            string __signature = null;
            string curUrl = "http://zwfw.jms.gov.cn/hcx/api-v2/jiamusi.app.icity.powermanager.PowerManagerCmd/getXAItemListByRegion";
            //开始加密
            string sig = "";
            string chars = "0123456789abcdef";
            if (__signature == null)
            {
                string curTime = new Random().Next(1000, 10000) + "" + Util.ConvertDateTimeToInt(DateTime.Now);
                sig = chars.Substring(10, 1) + "" + curTime;
            }
            else
            {
                sig = __signature;
            }
            string key = "";
            int keyIndex = -1;
            for (int i = 0; i < 6; i++)
            {
                string c = sig.Substring(keyIndex + 1, 1);
                key += c;
                keyIndex = chars.IndexOf(c);
                if (keyIndex < 0 || keyIndex >= sig.Length)
                {
                    keyIndex = i;
                }
            }
            string timestamp = new Random().Next(1000, 10000) + "_" + key + "_" + Util.ConvertDateTimeToInt(DateTime.Now);
            string t = timestamp;
            t = Regex.Replace(t, @"/\+/g", "_");
            curUrl += "?s=" + sig;
            curUrl += "&t=" + t;
            //加密结束，获得加密后url
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("WebRegion", "230826000000");
            string data = Util.httpPost(curUrl, JsonConvert.SerializeObject(dictionary));
            bjtj_model_callback callback = JsonConvert.DeserializeObject<bjtj_model_callback>(data);
            foreach (var cb in callback.data)
            {
                var item = cb.columns;
                //判断是否存在
                string sql = string.Format("select count(*) from t_spider_bjtj t where t.ORG_NAME='{0}'", item.ORG_NAME);
                int count = Convert.ToInt32(DbHelperMySQL.GetSingle(sql));
                if (count == 0)
                {
                    //不存在，插入数据库
                    sql = string.Format("insert into t_spider_bjtj(ORG_NAME,TOTALNUM,INHALLNUM,INHALLRATE,ISONLINENUM,ISONLINE_PER) values('{0}','{1}','{2}','{3}','{4}','{5}')", item.ORG_NAME, item.TOTALNUM, item.INHALLNUM, item.INHALLRATE, item.ISONLINENUM, item.ISONLINE_PER);
                    count = DbHelperMySQL.ExecuteSql(sql);
                }
            }
        }
    }
}
