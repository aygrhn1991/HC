using DSFP.Models;
using DSFP.Models.model;
using Maticsoft.DBUtility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DSFP.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login(int id, string url)
        {
            string sql = string.Format("select * from t_user t where t.id={0}", id);
            DataTable dt = DbHelperMySQL.Query(sql).Tables[0];
            var data = dt.ConvertToModel<t_user>();
            HttpCookie cookie = new HttpCookie("user", HttpUtility.UrlEncode(JsonConvert.SerializeObject(data[0])));
            Response.SetCookie(cookie);
            return Redirect(HttpUtility.UrlDecode(url));
        }
        #region 我的
        public ActionResult My()
        {
            return View();
        }
        public ActionResult My_Get()
        {
            string sql = string.Format("select * from t_user t where t.id={0}", Common.getUser().id);
            DataTable dt = DbHelperMySQL.Query(sql).Tables[0];
            var data = dt.ConvertToModel<t_user>();
            return Json(Result.Success(data.Count, data), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 我的小组
        public ActionResult MyTeam()
        {
            return View();
        }
        public ActionResult MyTeam_GetListByPage(SearchModel model)
        {
            string sql1 = "select tt.*,tt1.name leadername from " +
                "(select t1.*,t2.name abilityname from f_team_user t left join f_team t1 on t.teamid=t1.id left join f_team_ability t2 on t1.abilityid=t2.id where t1.del=0 and t.userid=" + Common.getUser().id + ") tt " +
                "left join t_user tt1 on tt.leaderid=tt1.id " +
                "order by tt.id desc limit " + Util.getPage(model.page, model.limit);
            string sql2 = "select count(*) from f_team_user t left join f_team t1 on t.teamid=t1.id where t1.del=0 and t.userid=" + Common.getUser().id;
            DataTable dt = DbHelperMySQL.Query(sql1).Tables[0];
            var data = dt.ConvertToModel<f_team>();
            int count = Convert.ToInt32(DbHelperMySQL.GetSingle(sql2));
            return Json(Result.Success(count, data), JsonRequestBehavior.AllowGet);
        }
        public ActionResult MyTeam_GetTeamUser(int id)
        {
            string sql = string.Format("select t1.*,t2.name abilityname from f_team_user t left join t_user t1 on t.userid=t1.id left join f_team_ability t2 on t1.abilityid=t2.id where t.teamid={0}", id);
            DataTable dt = DbHelperMySQL.Query(sql).Tables[0];
            var data = dt.ConvertToModel<t_user>();
            return Json(Result.Success(data.Count, data), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 贫困村
        public ActionResult MyXzqh()
        {
            return View();
        }
        public ActionResult MyXzqh_GetListByPage(SearchModel model)
        {
            string sql1 = "select t.*,substr(code,1,9) as short_code from t_xzqh t where t.pcode!='0'";
            string sql2 = "select count(*) from t_xzqh t where t.pcode!='0'";
            if (model.string1 != "-1")
            {
                string and = " and t.pcode='" + model.string1 + "'";
                sql1 += and;
                sql2 += and;
            }
            sql1 += " order by short_code,pcode limit " + Util.getPage(model.page, model.limit);
            DataTable dt = DbHelperMySQL.Query(sql1).Tables[0];
            var data = dt.ConvertToModel<t_xzqh>();
            int count = Convert.ToInt32(DbHelperMySQL.GetSingle(sql2));
            return Json(Result.Success(count, data), JsonRequestBehavior.AllowGet);
        }
        public ActionResult MyXzqh_GetXzqhPoor(string code)
        {
            string sql = string.Format("select * from f_poor t where t.del=0 and t.xzqhcode='{0}'", code);
            DataTable dt = DbHelperMySQL.Query(sql).Tables[0];
            var data = dt.ConvertToModel<f_poor>();
            return Json(Result.Success(data.Count, data), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 贫困户
        public ActionResult MyPoor()
        {
            return View();
        }
        public ActionResult MyPoor_GetListByPage(SearchModel model)
        {
            string sql1 = "select t.*,t1.name xzqhname from f_poor t left join t_xzqh t1 on t.xzqhcode=t1.code where t.del=0";
            string sql2 = "select count(*) from f_poor t where t.del=0";
            if (model.string1 != "-1")
            {
                if (model.string1.Length == 9)
                {
                    string and = " and substr(t.xzqhcode,1,9)='" + model.string1 + "'";
                    sql1 += and;
                    sql2 += and;
                }
                else
                {
                    string and = " and t.xzqhcode='" + model.string1 + "'";
                    sql1 += and;
                    sql2 += and;
                }
            }
            sql1 += string.Format(" order by t.id desc limit {0}", Util.getPage(model.page, model.limit));
            DataTable dt = DbHelperMySQL.Query(sql1).Tables[0];
            var data = dt.ConvertToModel<f_poor>();
            int count = Convert.ToInt32(DbHelperMySQL.GetSingle(sql2));
            return Json(Result.Success(count, data), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 签约管理
        public ActionResult Sign()
        {
            return View();
        }
        public ActionResult Sign_GetListByPage(SearchModel model)
        {
            string sql1 = "select t.*,t1.name poorname from f_sign t left join f_poor t1 on t.poorid=t1.id where t.del=0 and date_format(t.time,'%Y')='" + model.number1 + "'";
            string sql2 = "select count(*) from f_sign t where t.del=0 and date_format(t.time,'%Y')='" + model.number1 + "'";
            sql1 += string.Format(" order by t.time desc limit {0}", Util.getPage(model.page, model.limit));
            DataTable dt = DbHelperMySQL.Query(sql1).Tables[0];
            var data = dt.ConvertToModel<f_sign>();
            int count = Convert.ToInt32(DbHelperMySQL.GetSingle(sql2));
            return Json(Result.Success(count, data), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 电商扶贫
        public ActionResult Activity()
        {
            return View();
        }
        public ActionResult Activity_GetListByPage(SearchModel model)
        {
            string sql1 = "select t.*,t1.name teamname,t2.name abilityname " +
                "from f_activity t " +
                "left join f_team t1 on t.teamid=t1.id " +
                "left join f_team_ability t2 on t.abilityid=t2.id " +
                "right join (select * from f_team_user t left join f_team t1 on t.teamid=t1.id where t.userid=" + Common.getUser().id + ") t3 on t.teamid=t3.teamid " +
                "where t.del=0 and date_format(t.time,'%Y')='" + model.number1 + "' ";
            string sql2 = "select count(*) from f_activity t " +
                "right join (select * from f_team_user t left join f_team t1 on t.teamid=t1.id where t.userid=" + Common.getUser().id + ") t3 on t.teamid=t3.teamid " +
                "where t.del=0 and date_format(t.time,'%Y')='" + model.number1 + "'";
            if (model.number2 != -1)
            {
                string and = " and t.state=" + model.number2;
                sql1 += and;
                sql2 += and;
            }
            sql1 += string.Format(" order by t.id desc limit {0}", Util.getPage(model.page, model.limit));
            DataTable dt = DbHelperMySQL.Query(sql1).Tables[0];
            var data = dt.ConvertToModel<f_activity>();
            int count = Convert.ToInt32(DbHelperMySQL.GetSingle(sql2));
            return Json(Result.Success(count, data), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Activity_GetActivityPoor(int id)
        {
            string sql = string.Format("select t1.*,t.profit profit from f_activity_poor t left join f_poor t1 on t.poorid=t1.id where t.activityid={0}", id);
            DataTable dt = DbHelperMySQL.Query(sql).Tables[0];
            var data = dt.ConvertToModel<f_poor>();
            return Json(Result.Success(data.Count, data), JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}