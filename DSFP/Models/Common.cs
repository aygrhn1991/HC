using DSFP.Models.model;
using Newtonsoft.Json;
using System.Web;

namespace DSFP.Models
{
    public static class Common
    {
        public static t_admin getAdmin()
        {
            string cookie1 = HttpContext.Current.Request.Cookies["admin"].Value;
            string cookie2 = HttpUtility.UrlDecode(cookie1);
            return JsonConvert.DeserializeObject<t_admin>(cookie2);
        }
        public static t_user getUser()
        {
            string cookie1 = HttpContext.Current.Request.Cookies["user"].Value;
            string cookie2 = HttpUtility.UrlDecode(cookie1);
            return JsonConvert.DeserializeObject<t_user>(cookie2);
        }
        //public static void Count_Admin_News_Add()
        //{
        //    string sql = string.Format("update t_admin t set t.count_publish=(t.count_publish+1) where t.id={0}", getAdmin().id);
        //    int count = DbHelperMySQL.ExecuteSql(sql);
        //}
        //public static void Count_Admin_News_Answer()
        //{
        //    string sql = string.Format("update t_admin t set t.count_answer=(t.count_answer+1) where t.id={0}", getAdmin().id);
        //    int count = DbHelperMySQL.ExecuteSql(sql);
        //}
        //public static void Send_News_Add_Message(string xzqhcode)
        //{
        //    string sql = string.Format("update t_user_xzqh t set t.newcount=(t.newcount+1),t.time=now() where t.xzqhcode={0}", xzqhcode);
        //    int count = DbHelperMySQL.ExecuteSql(sql);
        //}
        //public static void Count_User_News_Scan()
        //{
        //    string sql = string.Format("update t_user t set t.count_scan=(t.count_scan+1) where t.id={0}", getUser().id);
        //    int count = DbHelperMySQL.ExecuteSql(sql);
        //}
        //public static void Count_User_News_Question()
        //{
        //    string sql = string.Format("update t_user t set t.count_question=(t.count_question+1) where t.id={0}", getUser().id);
        //    int count = DbHelperMySQL.ExecuteSql(sql);
        //}
        //public static void Count_User_News_Duration(int duration)
        //{
        //    string sql = string.Format("update t_user t set t.count_duration=(t.count_duration+{0}) where t.id={1}", duration, getUser().id);
        //    int count = DbHelperMySQL.ExecuteSql(sql);
        //}
    }
}