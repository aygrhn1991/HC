using DSFP.Models;
using DSFP.Models.model;
using Maticsoft.DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace DSFP.Controllers
{
    [AdminAuthorize]
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        #region 登录
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(string account, string password)
        {
            string sql = string.Format("select * from t_admin t where t.del=0 and t.account='{0}'", account);
            DataTable dt = DbHelperMySQL.Query(sql).Tables[0];
            var data = dt.ConvertToModel<t_admin>().FirstOrDefault();
            if (data == null)
            {
                return Json(Result.Error("账号不存在", 0));
            }
            if (data.password != password)
            {
                return Json(Result.Error("密码错误", 0));
            }
            else
            {
                return Json(Result.Success(1, data));
            }
        }
        #endregion

        #region 扶贫服务
        public ActionResult TeamAbility()
        {
            return View();
        }
        public ActionResult TeamAbility_GetListByPage(SearchModel model)
        {
            string sql = string.Format("select * from f_team_ability t where t.del=0 order by t.id limit {0}", Util.getPage(model.page, model.limit));
            DataTable dt = DbHelperMySQL.Query(sql).Tables[0];
            var data = dt.ConvertToModel<f_team_ability>();
            sql = "select count(*) from f_team_ability t where t.del=0";
            int count = Convert.ToInt32(DbHelperMySQL.GetSingle(sql));
            return Json(Result.Success(count, data), JsonRequestBehavior.AllowGet);
        }
        public ActionResult TeamAbility_Add(f_team_ability model)
        {
            string sql = string.Format("select count(*) from f_team_ability t where t.del=0 and t.name='{0}'", model.name);
            int count = Convert.ToInt32(DbHelperMySQL.GetSingle(sql));
            if (count != 0)
            {
                return Json(Result.Error("该扶贫服务已存在", -1), JsonRequestBehavior.AllowGet);
            }
            sql = string.Format("insert into f_team_ability(name,del) values('{0}',0)", model.name);
            count = DbHelperMySQL.ExecuteSql(sql);
            return Json(Result.AutoResult(count), JsonRequestBehavior.AllowGet);
        }
        public ActionResult TeamAbility_Edit(f_team_ability model)
        {
            string sql = string.Format("update f_team_ability t set t.name='{0}' where t.id={1}", model.name, model.id);
            int count = DbHelperMySQL.ExecuteSql(sql);
            return Json(Result.AutoResult(count), JsonRequestBehavior.AllowGet);
        }
        public ActionResult TeamAbility_Delete(int id)
        {
            string sql = string.Format("update f_team_ability t set t.del=1 where t.id={0}", id);
            int count = DbHelperMySQL.ExecuteSql(sql);
            return Json(Result.AutoResult(count), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 扶贫小组
        public ActionResult Team()
        {
            return View();
        }
        public ActionResult Team_GetListByPage(SearchModel model)
        {
            string sql1 = "select t.*,t1.name abilityname,t2.name leadername from f_team t left join f_team_ability t1 on t.abilityid=t1.id left join t_user t2 on t.leaderid=t2.id  where t.del=0 ";
            string sql2 = "select count(*) from f_team t where t.del=0";
            if (!string.IsNullOrWhiteSpace(model.string1))
            {
                string and = " and t.name like '%" + model.string1 + "%'";
                sql1 += and;
                sql2 += and;
            }
            sql1 += string.Format(" order by t.id desc limit {0}", Util.getPage(model.page, model.limit));
            DataTable dt = DbHelperMySQL.Query(sql1).Tables[0];
            var data = dt.ConvertToModel<f_team>();
            int count = Convert.ToInt32(DbHelperMySQL.GetSingle(sql2));
            return Json(Result.Success(count, data), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Team_Add(f_team model, List<int> ids)
        {
            string sql = string.Format("insert into f_team(name,leaderid,abilityid,del) values('{0}',{1},{2},0);SELECT @@IDENTITY", model.name, model.leaderid, model.abilityid);
            int teamid = Convert.ToInt32(DbHelperMySQL.GetSingle(sql));
            int result = 0;
            foreach (var item in ids)
            {
                sql = string.Format("insert into f_team_user(teamid,userid) values({0},{1})", teamid, item);
                int count = DbHelperMySQL.ExecuteSql(sql);
                result += count;
            }
            return Json(Result.AutoResult(result == ids.Count ? 1 : 0), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Team_GetTeamUser(int id)
        {
            string sql = string.Format("select t1.*,t2.name abilityname from f_team_user t left join t_user t1 on t.userid=t1.id left join f_team_ability t2 on t1.abilityid=t2.id where t.teamid={0}", id);
            DataTable dt = DbHelperMySQL.Query(sql).Tables[0];
            var data = dt.ConvertToModel<t_user>();
            return Json(Result.Success(data.Count, data), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Team_Edit(f_team model, List<int> ids)
        {
            string sql = string.Format("update f_team t set name='{0}',leaderid={1},abilityid={2} where t.id={3}", model.name, model.leaderid, model.abilityid, model.id);
            int count = DbHelperMySQL.ExecuteSql(sql);
            sql = string.Format("delete from f_team_user where teamid={0}", model.id);
            count = DbHelperMySQL.ExecuteSql(sql);
            int result = 0;
            foreach (var item in ids)
            {
                sql = string.Format("insert into f_team_user(teamid,userid) values({0},{1})", model.id, item);
                count = DbHelperMySQL.ExecuteSql(sql);
                result += count;
            }
            return Json(Result.AutoResult(result == ids.Count ? 1 : 0), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Team_Delete(int id)
        {
            string sql = string.Format("update f_team t set t.del=1 where t.id={0}", id);
            int count = DbHelperMySQL.ExecuteSql(sql);
            return Json(Result.AutoResult(count), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 网格员
        public ActionResult User()
        {
            return View();
        }
        public ActionResult User_GetListByPage(SearchModel model)
        {
            string sql1 = "select t.*,t1.name abilityname from t_user t left join f_team_ability t1 on t.abilityid=t1.id where t.del=0";
            string sql2 = "select count(*) from t_user t where t.del=0";
            if (!string.IsNullOrWhiteSpace(model.string1))
            {
                string and = " and t.name like '%" + model.string1 + "%'";
                sql1 += and;
                sql2 += and;
            }
            sql1 += " order by t.id limit " + Util.getPage(model.page, model.limit);
            DataTable dt = DbHelperMySQL.Query(sql1).Tables[0];
            var data = dt.ConvertToModel<t_user>();
            int count = Convert.ToInt32(DbHelperMySQL.GetSingle(sql2));
            return Json(Result.Success(count, data), JsonRequestBehavior.AllowGet);
        }
        public ActionResult User_Edit(t_user model)
        {
            string sql = string.Format("update t_user t set abilityid={0},position='{1}' where t.id={2}", model.abilityid, model.position, model.id);
            int count = DbHelperMySQL.ExecuteSql(sql);
            return Json(Result.AutoResult(count), JsonRequestBehavior.AllowGet);
        }
        public ActionResult User_GetUserTeam(int id)
        {
            string sql = string.Format("select t1.*,t2.name leadername,t3.name abilityname from f_team_user t left join f_team t1 on t.teamid=t1.id left join t_user t2 on t1.leaderid=t2.id left join f_team_ability t3 on t1.abilityid=t3.id where t1.del=0 and t.userid={0}", id);
            DataTable dt = DbHelperMySQL.Query(sql).Tables[0];
            var data = dt.ConvertToModel<f_team>();
            return Json(Result.Success(data.Count, data), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 贫困村
        public ActionResult Xzqh()
        {
            return View();
        }
        public ActionResult Xzqh_GetListByPage(SearchModel model)
        {
            string sql1 = "select t.*,substr(code,1,9) as short_code from t_xzqh t where t.pcode!='0'";
            string sql2 = "select count(*) from t_xzqh t where t.pcode!='0'";
            if (model.string1 != "-1")
            {
                string and = " and t.pcode='" + model.string1 + "'";
                sql1 += and;
                sql2 += and;
            }
            if (!string.IsNullOrWhiteSpace(model.string2))
            {
                string and = " and t.name like '%" + model.string2 + "%'";
                sql1 += and;
                sql2 += and;
            }
            sql1 += " order by short_code,pcode limit " + Util.getPage(model.page, model.limit);
            DataTable dt = DbHelperMySQL.Query(sql1).Tables[0];
            var data = dt.ConvertToModel<t_xzqh>();
            int count = Convert.ToInt32(DbHelperMySQL.GetSingle(sql2));
            return Json(Result.Success(count, data), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Xzqh_GetXzqhPoor(string code)
        {
            string sql = string.Format("select * from f_poor t where t.del=0 and t.xzqhcode='{0}'", code);
            DataTable dt = DbHelperMySQL.Query(sql).Tables[0];
            var data = dt.ConvertToModel<f_poor>();
            return Json(Result.Success(data.Count, data), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 贫困户
        public ActionResult Poor()
        {
            return View();
        }
        public ActionResult Poor_GetListByPage(SearchModel model)
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
            if (!string.IsNullOrWhiteSpace(model.string2))
            {
                string and = " and t.name like '%" + model.string2 + "%'";
                sql1 += and;
                sql2 += and;
            }
            sql1 += string.Format(" order by t.id desc limit {0}", Util.getPage(model.page, model.limit));
            DataTable dt = DbHelperMySQL.Query(sql1).Tables[0];
            var data = dt.ConvertToModel<f_poor>();
            int count = Convert.ToInt32(DbHelperMySQL.GetSingle(sql2));
            return Json(Result.Success(count, data), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Poor_Add(f_poor model)
        {
            string sql = string.Format("insert into f_poor(name,xzqhcode,population,del) values('{0}','{1}',{2},0)", model.name, model.xzqhcode, model.population);
            int count = DbHelperMySQL.ExecuteSql(sql);
            return Json(Result.AutoResult(count), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Poor_Edit(f_poor model)
        {
            string sql = string.Format("update f_poor t set name='{0}',xzqhcode='{1}',population={2} where t.id={3}", model.name, model.xzqhcode, model.population, model.id);
            int count = DbHelperMySQL.ExecuteSql(sql);
            return Json(Result.AutoResult(count), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Poor_Delete(int id)
        {
            string sql = string.Format("update f_poor t set t.del=1 where t.id={0}", id);
            int count = DbHelperMySQL.ExecuteSql(sql);
            return Json(Result.AutoResult(count), JsonRequestBehavior.AllowGet);
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
            if (!string.IsNullOrWhiteSpace(model.string1))
            {
                string and = " and t.name like '%" + model.string1 + "%'";
                sql1 += and;
                sql2 += and;
            }
            sql1 += string.Format(" order by t.time desc limit {0}", Util.getPage(model.page, model.limit));
            DataTable dt = DbHelperMySQL.Query(sql1).Tables[0];
            var data = dt.ConvertToModel<f_sign>();
            int count = Convert.ToInt32(DbHelperMySQL.GetSingle(sql2));
            return Json(Result.Success(count, data), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Sign_Add(f_sign model)
        {
            string sql = string.Format("insert into f_sign(poorid,name,number,time,datestart,dateend,weight,price,del) values({0},'{1}','{2}','{3}','{4}','{5}',{6},{7},0)",
              model.poorid, model.name, model.number, model.time, model.datestart, model.dateend, model.weight, model.price);
            int count = DbHelperMySQL.ExecuteSql(sql);
            return Json(Result.AutoResult(count), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Sign_Edit(f_sign model)
        {
            string sql = string.Format("update f_sign t set poorid={0},name='{1}',time='{2}',datestart='{3}',dateend='{4}',weight={5},price={6} where t.id={7}",
                model.poorid, model.name, model.time, model.datestart, model.dateend, model.weight, model.price, model.id);
            int count = DbHelperMySQL.ExecuteSql(sql);
            return Json(Result.AutoResult(count), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Sign_Delete(int id)
        {
            string sql = string.Format("update f_sign t set t.del=1 where t.id={0}", id);
            int count = DbHelperMySQL.ExecuteSql(sql);
            return Json(Result.AutoResult(count), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 电商扶贫
        public ActionResult Activity()
        {
            return View();
        }
        public ActionResult Activity_GetListByPage(SearchModel model)
        {
            string sql1 = "select t.*,t1.name teamname,t2.name abilityname from f_activity t left join f_team t1 on t.teamid=t1.id left join f_team_ability t2 on t.abilityid=t2.id where t.del=0 and date_format(t.time,'%Y')='" + model.number1 + "'";
            string sql2 = "select count(*) from f_activity t where t.del=0 and date_format(t.time,'%Y')='" + model.number1 + "'";
            if (!string.IsNullOrWhiteSpace(model.string1))
            {
                string and = " and t.name like '%" + model.string1 + "%'";
                sql1 += and;
                sql2 += and;
            }
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
        public ActionResult Activity_Add(f_activity model, List<int> ids)
        {
            string sql = string.Format("insert into f_activity(name,number,abilityid,teamid,time,state,del) values('{0}','{1}',{2},{3},'{4}',0,0);SELECT @@IDENTITY", model.name, model.number, model.abilityid, model.teamid, model.time);
            int activityid = Convert.ToInt32(DbHelperMySQL.GetSingle(sql));
            int result = 0;
            foreach (var item in ids)
            {
                sql = string.Format("insert into f_activity_poor(activityid,poorid,profit) values({0},{1},0)", activityid, item);
                int count = DbHelperMySQL.ExecuteSql(sql);
                result += count;
            }
            return Json(Result.AutoResult(result == ids.Count ? 1 : 0), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Activity_GetActivityPoor(int id)
        {
            string sql = string.Format("select t1.*,t.profit profit from f_activity_poor t left join f_poor t1 on t.poorid=t1.id where t.activityid={0}", id);
            DataTable dt = DbHelperMySQL.Query(sql).Tables[0];
            var data = dt.ConvertToModel<f_poor>();
            return Json(Result.Success(data.Count, data), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Activity_Edit(f_activity model, List<int> ids)
        {
            string sql = string.Format("update f_activity t set name='{0}',number='{1}',abilityid={2},teamid={3},time='{4}' where t.id={5}", model.name, model.number, model.abilityid, model.teamid, model.time, model.id);
            int count = DbHelperMySQL.ExecuteSql(sql);
            sql = string.Format("delete from f_activity_poor where activityid={0}", model.id);
            count = DbHelperMySQL.ExecuteSql(sql);
            int result = 0;
            foreach (var item in ids)
            {
                sql = string.Format("insert into f_activity_poor(activityid,poorid,profit) values({0},{1},0)", model.id, item);
                count = DbHelperMySQL.ExecuteSql(sql);
                result += count;
            }
            return Json(Result.AutoResult(result == ids.Count ? 1 : 0), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Activity_Delete(int id)
        {
            string sql = string.Format("update f_activity t set t.del=1 where t.id={0}", id);
            int count = DbHelperMySQL.ExecuteSql(sql);
            return Json(Result.AutoResult(count), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Activity_Check(f_activity model)
        {
            string sql = string.Format("update f_activity t set t.state=1,t.result='{0}',t.suggest='{1}' where t.id={2}", model.result, model.suggest, model.id);
            int count = DbHelperMySQL.ExecuteSql(sql);
            return Json(Result.AutoResult(count), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Activity_EditPoorProfit(int activityid, int poorid, float profit)
        {
            string sql = string.Format("update f_activity_poor t set profit={0} where t.activityid={1} and t.poorid={2}", profit, activityid, poorid);
            int count = DbHelperMySQL.ExecuteSql(sql);
            return Json(Result.AutoResult(count), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 修改密码
        public ActionResult Password()
        {
            return View();
        }
        public ActionResult Password_Edit(string oldPassword, string newPassword)
        {
            string sql = string.Format("select * from t_admin t where t.id={0}", Common.getAdmin().id);
            DataTable dt = DbHelperMySQL.Query(sql).Tables[0];
            var data = dt.ConvertToModel<t_admin>().FirstOrDefault();
            if (data.password != oldPassword)
            {
                return Json(Result.Error("旧密码错误", 0), JsonRequestBehavior.AllowGet);
            }
            sql = string.Format("update t_admin t set t.password='{0}' where t.id={1}", newPassword, Common.getAdmin().id);
            int count = DbHelperMySQL.ExecuteSql(sql);
            return Json(Result.AutoResult(count), JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}