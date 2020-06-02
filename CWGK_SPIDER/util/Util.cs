using System;
using System.IO;
using System.Net;
using System.Text;

namespace CWGK_SPIDER.util
{
    public static class Util
    {
        public static string getHtmlStr(string url, Encoding enc)
        {
            string htmlStr = "";
            WebRequest request = WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream, enc);
            htmlStr = reader.ReadToEnd();
            reader.Close();
            stream.Close();
            response.Close();
            return htmlStr;
        }
        public static string httpPost(string url, string json)
        {
            WebRequest webRequest = WebRequest.Create(url);
            webRequest.Method = "POST";
            webRequest.ContentType = "application/json;charset=UTF-8";
            using (StreamWriter streamWriter = new StreamWriter(webRequest.GetRequestStream()))
            {
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }
            WebResponse webResponse = webRequest.GetResponse();
            using (StreamReader streamReader = new StreamReader(webResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                return result;
            }
        }
        public static string getTimeStamp()
        {
            long t = ConvertDateTimeToInt(DateTime.Now);

            TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }
        public static long ConvertDateTimeToInt(DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (time.Ticks - startTime.Ticks) / 10000;
            return t;
        }
    }
}
