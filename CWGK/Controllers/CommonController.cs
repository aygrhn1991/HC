using CWGK.Models;
using CWGK.Models.model;
using Maticsoft.DBUtility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace CWGK.Controllers
{
    public class CommonController : Controller
    {
        public ActionResult GetSetting()
        {
            string app = ConfigurationManager.AppSettings["app"];
            string jgfs = ConfigurationManager.AppSettings["jgfs"];
            string zhny = ConfigurationManager.AppSettings["zhny"];
            string cwgk = ConfigurationManager.AppSettings["cwgk"];
            string dsfp = ConfigurationManager.AppSettings["dsfp"];
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("app", app);
            dic.Add("jgfs", jgfs);
            dic.Add("zhny", zhny);
            dic.Add("cwgk", cwgk);
            dic.Add("dsfp", dsfp);
            return Json(dic, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetXzqhByAdmin()
        {
            t_admin admin = Common.getAdmin();
            string sql = "select code,name,pcode,substr(code,1,9) as short_code from t_xzqh t order by short_code,pcode";
            DataTable dt = DbHelperMySQL.Query(sql).Tables[0];
            List<t_xzqh> list = dt.ConvertToModel<t_xzqh>().ToList();
            List<t_xzqh> result = new List<t_xzqh>();
            if (admin.xzqhcode.Length != 1)
            {
                foreach (var x in list)
                {
                    if (x.code.ToString().StartsWith(admin.xzqhcode))
                    {
                        result.Add(x);
                    }
                }
            }
            else
            {
                result = list;
            }
            return Json(Result.Success(result.Count, result), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetXzqhByUser()
        {
            string sql = string.Format("select t1.* from t_user_xzqh t left join t_xzqh t1 on t.xzqhcode=t1.code where t.userid={0} order by t1.code", Common.getUser().id);
            DataTable dt = DbHelperMySQL.Query(sql).Tables[0];
            var data = dt.ConvertToModel<t_xzqh>();
            return Json(Result.Success(data.Count, data), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetXzqh()
        {
            string sql = "select code,name,pcode,substr(code,1,9) as short_code from t_xzqh t order by short_code,pcode";
            DataTable dt = DbHelperMySQL.Query(sql).Tables[0];
            var list = dt.ConvertToModel<t_xzqh>();
            return Json(Result.Success(list.Count, list), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetNewsType()
        {
            string sql = "select * from t_news_type t where t.del=0 order by t.id";
            DataTable dt = DbHelperMySQL.Query(sql).Tables[0];
            var data = dt.ConvertToModel<t_news_type>();
            return Json(Result.Success(0, data), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetXzqh_One(string code)
        {
            string sql = string.Format("select * from t_xzqh t where t.code='{0}'", code);
            DataTable dt = DbHelperMySQL.Query(sql).Tables[0];
            var data = dt.ConvertToModel<t_xzqh>().FirstOrDefault();
            return Json(Result.Success(1, data), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetNewsType_One(int id)
        {
            string sql = string.Format("select * from t_news_type t where t.id={0}", id);
            DataTable dt = DbHelperMySQL.Query(sql).Tables[0];
            var data = dt.ConvertToModel<t_news_type>().FirstOrDefault();
            return Json(Result.Success(1, data), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Count_User_News_Duration(int duration)
        {
            try
            {
                Common.Count_User_News_Duration(duration);
            }
            catch (Exception e)
            {
            }
            return Json(Result.Success(0, duration), JsonRequestBehavior.AllowGet);
        }
    }
}