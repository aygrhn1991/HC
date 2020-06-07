using DSFP.Models;
using DSFP.Models.model;
using Maticsoft.DBUtility;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web.Mvc;

namespace DSFP.Controllers
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
        public ActionResult GetTeamAbility()
        {
            string sql = "select * from f_team_ability t where t.del=0 order by t.id";
            DataTable dt = DbHelperMySQL.Query(sql).Tables[0];
            var data = dt.ConvertToModel<f_team_ability>();
            return Json(Result.Success(0, data), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetUser()
        {
            string sql = "select * from t_user t where t.del=0";
            DataTable dt = DbHelperMySQL.Query(sql).Tables[0];
            var data = dt.ConvertToModel<t_user>();
            return Json(Result.Success(data.Count, data), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetTeam()
        {
            string sql = "select * from f_team t where t.del=0";
            DataTable dt = DbHelperMySQL.Query(sql).Tables[0];
            var data = dt.ConvertToModel<f_team>();
            return Json(Result.Success(data.Count, data), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetXzqh()
        {
            string sql = "select code,name,pcode,substr(code,1,9) as short_code from t_xzqh t order by short_code,pcode";
            DataTable dt = DbHelperMySQL.Query(sql).Tables[0];
            var list = dt.ConvertToModel<t_xzqh>();
            return Json(Result.Success(list.Count, list), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPoor(string name)
        {
            string sql = "select t.*,t1.name xzqhname from f_poor t left join t_xzqh t1 on t.xzqhcode=t1.code where t.del=0";
            if (!string.IsNullOrWhiteSpace(name))
            {
                sql += " and t.name like '%" + name + "%'";
            }
            sql += " order by t.id desc";
            DataTable dt = DbHelperMySQL.Query(sql).Tables[0];
            var data = dt.ConvertToModel<f_poor>();
            return Json(Result.Success(data.Count, data), JsonRequestBehavior.AllowGet);
        }
    }
}