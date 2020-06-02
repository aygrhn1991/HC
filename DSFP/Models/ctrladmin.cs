//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web.Mvc;
//using Maticsoft.DBUtility;
//using System.Data;
//using CWGK.Models;
//using CWGK.Models.model;

//namespace CWGK.Controllers
//{
//    [AdminAuthorize]
//    public class AdminController : Controller
//    {
//        public ActionResult Index()
//        {
//            return View();
//        }

//        #region 登录
//        [AllowAnonymous]
//        [HttpGet]
//        public ActionResult Login()
//        {
//            return View();
//        }
//        [AllowAnonymous]
//        [HttpPost]
//        public ActionResult Login(string account, string password)
//        {
//            string sql = string.Format("select * from t_admin t where t.del=0 and t.account='{0}'", account);
//            DataTable dt = DbHelperMySQL.Query(sql).Tables[0];
//            var data = dt.ConvertToModel<t_admin>().FirstOrDefault();
//            if (data == null)
//            {
//                return Json(Result.Error("账号不存在", 0));
//            }
//            if (data.password != password)
//            {
//                return Json(Result.Error("密码错误", 0));
//            }
//            else
//            {
//                return Json(Result.Success(1, data));
//            }
//        }
//        #endregion

//        #region 新闻类型
//        public ActionResult NewsType()
//        {
//            return View();
//        }
//        public ActionResult NewsType_GetListByPage(SearchModel model)
//        {
//            string sql = string.Format("select * from t_news_type t where t.del=0 order by t.id limit {0}", Util.getPage(model.page, model.limit));
//            DataTable dt = DbHelperMySQL.Query(sql).Tables[0];
//            var data = dt.ConvertToModel<t_news_type>();
//            sql = "select count(*) from t_news_type t where t.del=0";
//            int count = Convert.ToInt32(DbHelperMySQL.GetSingle(sql));
//            return Json(Result.Success(count, data), JsonRequestBehavior.AllowGet);
//        }
//        public ActionResult NewsType_Add(t_news_type model)
//        {
//            string sql = string.Format("select count(*) from t_news_type t where t.del=0 and t.name='{0}'", model.name);
//            int count = Convert.ToInt32(DbHelperMySQL.GetSingle(sql));
//            if (count != 0)
//            {
//                return Json(Result.Error("该新闻分类已存在", -1), JsonRequestBehavior.AllowGet);
//            }
//            sql = string.Format("insert into t_news_type(name,del) values('{0}',0)", model.name);
//            count = DbHelperMySQL.ExecuteSql(sql);
//            return Json(Result.AutoResult(count), JsonRequestBehavior.AllowGet);
//        }
//        public ActionResult NewsType_Edit(t_news_type model)
//        {
//            string sql = string.Format("update t_news_type t set t.name='{0}' where t.id={1}", model.name, model.id);
//            int count = DbHelperMySQL.ExecuteSql(sql);
//            return Json(Result.AutoResult(count), JsonRequestBehavior.AllowGet);
//        }
//        public ActionResult NewsType_Delete(int id)
//        {
//            string sql = string.Format("update t_news_type t set t.del=1 where t.id={0}", id);
//            int count = DbHelperMySQL.ExecuteSql(sql);
//            return Json(Result.AutoResult(count), JsonRequestBehavior.AllowGet);
//        }
//        #endregion

//        #region 新闻管理
//        public ActionResult News()
//        {
//            return View();
//        }
//        public ActionResult News_GetListByPage(SearchModel model)
//        {
//            string sql1 = "select t.*,t1.name xzqhname,t2.name typename,t3.name adminname" +
//                " from t_news t" +
//                " left join t_xzqh t1 on t.xzqhcode=t1.code" +
//                " left join t_news_type t2 on t.typeid=t2.id" +
//                " left join t_admin t3 on t.adminid=t3.id" +
//                " where t.del=0";
//            string sql2 = "select count(*) from t_news t where t.del=0";
//            if (model.string1 != "-1")
//            {
//                string and = " and t.xzqhcode='" + model.string1 + "'";
//                sql1 += and;
//                sql2 += and;
//            }
//            else
//            {
//                t_admin admin = Common.getAdmin();
//                if (admin.xzqhcode.Length != 1)
//                {
//                    string and = " and instr(t.xzqhcode,'" + admin.xzqhcode + "')";
//                    sql1 += and;
//                    sql2 += and;
//                }
//            }
//            if (model.number1 != -1)
//            {
//                string and = " and t.typeid=" + model.number1;
//                sql1 += and;
//                sql2 += and;
//            }
//            if (model.number2 != -1)
//            {
//                string and = " and t.state=" + model.number2;
//                sql1 += and;
//                sql2 += and;
//            }
//            if (!string.IsNullOrWhiteSpace(model.string2))
//            {
//                string and = " and t.title like '%" + model.string2 + "%'";
//                sql1 += and;
//                sql2 += and;
//            }
//            sql1 += " order by t.id desc limit " + Util.getPage(model.page, model.limit);
//            DataTable dt = DbHelperMySQL.Query(sql1).Tables[0];
//            var data = dt.ConvertToModel<t_news>();
//            int count = Convert.ToInt32(DbHelperMySQL.GetSingle(sql2));
//            return Json(Result.Success(count, data), JsonRequestBehavior.AllowGet);
//        }
//        public ActionResult News_Add(t_news model)
//        {
//            string sql = string.Format("insert into t_news(xzqhcode,typeid,adminid,title,time,content,scan,state,del) values('{0}','{1}','{2}','{3}',now(),'{4}',0,0,0)", model.xzqhcode, model.typeid, Common.getAdmin().id, model.title, model.content);
//            int count = DbHelperMySQL.ExecuteSql(sql);
//            return Json(Result.AutoResult(count), JsonRequestBehavior.AllowGet);
//        }
//        public ActionResult News_Edit(t_news model)
//        {
//            string sql = string.Format("update t_news t set t.xzqhcode='{0}',t.typeid='{1}',t.title='{2}',t.content='{3}' where t.id={4}", model.xzqhcode, model.typeid, model.title, model.content, model.id);
//            int count = DbHelperMySQL.ExecuteSql(sql);
//            return Json(Result.AutoResult(count), JsonRequestBehavior.AllowGet);
//        }
//        public ActionResult News_Delete(int id)
//        {
//            string sql = string.Format("update t_news t set t.del=1 where t.id={0}", id);
//            int count = DbHelperMySQL.ExecuteSql(sql);
//            return Json(Result.AutoResult(count), JsonRequestBehavior.AllowGet);
//        }
//        public ActionResult News_Check(int id, int state)
//        {
//            string sql = string.Format("update t_news t set t.state={0} where t.id={1}", state, id);
//            int count = DbHelperMySQL.ExecuteSql(sql);
//            try
//            {
//                if (state == 1)
//                {
//                    sql = string.Format("select * from t_news t where t.id={0}", id);
//                    DataTable dt = DbHelperMySQL.Query(sql).Tables[0];
//                    t_news news = dt.ConvertToModel<t_news>().FirstOrDefault();
//                    Common.Count_Admin_News_Add();
//                    Common.Send_News_Add_Message(news.xzqhcode);
//                }
//            }
//            catch (Exception e)
//            {

//            }
//            return Json(Result.AutoResult(count), JsonRequestBehavior.AllowGet);
//        }
//        #endregion

//        #region 新闻互动
//        public ActionResult NewsTalk()
//        {
//            return View();
//        }
//        public ActionResult NewsTalk_GetListByPage(SearchModel model)
//        {
//            string sql1 = "select t.*,t1.title newstitle,t2.name xzqhname,t3.name typename,t4.name adminname,t5.name username" +
//            " from t_news_talk t" +
//            " left join t_news t1 on t.newsid = t1.id" +
//            " left join t_xzqh t2 on t1.xzqhcode = t2.code" +
//            " left join t_news_type t3 on t1.typeid = t3.id" +
//            " left join t_admin t4 on t.adminid = t4.id" +
//            " left join t_user t5 on t.userid = t5.id" +
//            " where t.del=0";
//            string sql2 = "select count(*) from t_news_talk t left join t_news t1 on t.newsid = t1.id where t.del=0";
//            if (model.string1 != "-1")
//            {
//                string and = " and t1.xzqhcode='" + model.string1 + "'";
//                sql1 += and;
//                sql2 += and;
//            }
//            else
//            {
//                t_admin admin = Common.getAdmin();
//                if (admin.xzqhcode.Length != 1)
//                {
//                    string and = " and instr(t1.xzqhcode,'" + admin.xzqhcode + "')";
//                    sql1 += and;
//                    sql2 += and;
//                }
//            }
//            if (model.number1 != -1)
//            {
//                string and = " and t1.typeid=" + model.number1;
//                sql1 += and;
//                sql2 += and;
//            }
//            if (model.number2 != -1)
//            {
//                string and = model.number2 == 0 ? (" and t.answer is null") : (" and t.answer is not null");
//                sql1 += and;
//                sql2 += and;
//            }
//            if (!string.IsNullOrWhiteSpace(model.string2))
//            {
//                string and = " and t1.title like '%" + model.string2 + "%'";
//                sql1 += and;
//                sql2 += and;
//            }
//            sql1 += " order by t.id desc limit " + Util.getPage(model.page, model.limit);
//            DataTable dt = DbHelperMySQL.Query(sql1).Tables[0];
//            var data = dt.ConvertToModel<t_news_talk>();
//            int count = Convert.ToInt32(DbHelperMySQL.GetSingle(sql2));
//            try
//            {
//                Common.Count_Admin_News_Answer();
//            }
//            catch (Exception e)
//            {

//            }
//            return Json(Result.Success(count, data), JsonRequestBehavior.AllowGet);
//        }
//        public ActionResult NewsTalk_GetFile(int id)
//        {
//            string sql = string.Format("select * from t_files t where t.del=0 and t.type=1 and t.typekey='{0}'", id);
//            DataTable dt = DbHelperMySQL.Query(sql).Tables[0];
//            var data = dt.ConvertToModel<t_files>();
//            return Json(Result.Success(data.Count, data), JsonRequestBehavior.AllowGet);
//        }
//        public ActionResult NewsTalk_Edit(t_news_talk model)
//        {
//            string sql = string.Format("update t_news_talk t set t.adminid='{0}',t.answer='{1}',t.read=0 where t.id={2}", Common.getAdmin().id, model.answer, model.id);
//            int count = DbHelperMySQL.ExecuteSql(sql);
//            return Json(Result.AutoResult(count), JsonRequestBehavior.AllowGet);
//        }
//        public ActionResult NewsTalk_Delete(int id)
//        {
//            string sql = string.Format("update t_news_talk t set t.del=1 where t.id={0}", id);
//            int count = DbHelperMySQL.ExecuteSql(sql);
//            return Json(Result.AutoResult(count), JsonRequestBehavior.AllowGet);
//        }
//        #endregion

//        #region 管理员账号
//        public ActionResult Admin()
//        {
//            return View();
//        }
//        public ActionResult Admin_GetListByPage(SearchModel model)
//        {
//            string sql1 = "select t.*,t1.name xzqhname from t_admin t left join t_xzqh t1 on t.xzqhcode=t1.code where t.del=0";
//            string sql2 = "select count(*) from t_admin t where t.del=0";
//            if (model.string1 != "-1")
//            {
//                string and = " and t.xzqhcode='" + model.string1 + "'";
//                sql1 += and;
//                sql2 += and;
//            }
//            else
//            {
//                t_admin admin = Common.getAdmin();
//                if (admin.xzqhcode.Length != 1)
//                {
//                    string and = " and instr(t.xzqhcode,'" + admin.xzqhcode + "')";
//                    sql1 += and;
//                    sql2 += and;
//                }
//            }
//            if (!string.IsNullOrWhiteSpace(model.string2))
//            {
//                string and = " and t.name like '%" + model.string2 + "%'";
//                sql1 += and;
//                sql2 += and;
//            }
//            sql1 += " order by t.id limit " + Util.getPage(model.page, model.limit);
//            DataTable dt = DbHelperMySQL.Query(sql1).Tables[0];
//            var data = dt.ConvertToModel<t_admin>();
//            int count = Convert.ToInt32(DbHelperMySQL.GetSingle(sql2));
//            return Json(Result.Success(count, data), JsonRequestBehavior.AllowGet);
//        }
//        public ActionResult Admin_Add(t_admin model)
//        {
//            string sql = string.Format("select count(*) from t_admin t where t.del=0 and t.account='{0}'", model.account);
//            int count = Convert.ToInt32(DbHelperMySQL.GetSingle(sql));
//            if (count != 0)
//            {
//                return Json(Result.Error("该账号已存在", -1), JsonRequestBehavior.AllowGet);
//            }
//            sql = string.Format("insert into t_admin(level,xzqhcode,account,password,name,phone,count_publish,count_answer,del) values('{0}','{1}','{2}','{3}','{4}','{5}',0,0,0)", model.level, model.xzqhcode, model.account, model.password, model.name, model.phone);
//            count = DbHelperMySQL.ExecuteSql(sql);
//            return Json(Result.AutoResult(count), JsonRequestBehavior.AllowGet);
//        }
//        public ActionResult Admin_Edit(t_admin model)
//        {
//            string sql = string.Format("update t_admin t set t.level={0},t.xzqhcode='{1}',t.name='{2}',t.phone='{3}' where t.id={4}", model.level, model.xzqhcode, model.name, model.phone, model.id);
//            int count = DbHelperMySQL.ExecuteSql(sql);
//            return Json(Result.AutoResult(count), JsonRequestBehavior.AllowGet);
//        }
//        public ActionResult Admin_Delete(int id)
//        {
//            string sql = string.Format("update t_admin t set t.del=1 where t.id={0}", id);
//            int count = DbHelperMySQL.ExecuteSql(sql);
//            return Json(Result.AutoResult(count), JsonRequestBehavior.AllowGet);
//        }
//        public ActionResult Admin_Password(int id, string password)
//        {
//            string sql = string.Format("update t_admin t set t.password={0} where t.id={1}", password, id);
//            int count = DbHelperMySQL.ExecuteSql(sql);
//            return Json(Result.AutoResult(count), JsonRequestBehavior.AllowGet);
//        }
//        #endregion

//        #region 网格员账号
//        public ActionResult User()
//        {
//            return View();
//        }
//        public ActionResult User_GetListByPage(SearchModel model)
//        {
//            string sql1 = "select * from t_user t where t.del=0";
//            string sql2 = "select count(*) from t_user t where t.del=0";
//            if (!string.IsNullOrWhiteSpace(model.string1))
//            {
//                string and = " and t.phone like '%" + model.string1 + "%'";
//                sql1 += and;
//                sql2 += and;
//            }
//            sql1 += " order by t.id limit " + Util.getPage(model.page, model.limit);
//            DataTable dt = DbHelperMySQL.Query(sql1).Tables[0];
//            var data = dt.ConvertToModel<t_user>();
//            int count = Convert.ToInt32(DbHelperMySQL.GetSingle(sql2));
//            return Json(Result.Success(count, data), JsonRequestBehavior.AllowGet);
//        }
//        public ActionResult User_GetXzqh(int id)
//        {
//            string sql = string.Format("select t1.* from t_user_xzqh t left join t_xzqh t1 on t.xzqhcode=t1.code where t.userid={0} order by t1.code", id);
//            DataTable dt = DbHelperMySQL.Query(sql).Tables[0];
//            var data = dt.ConvertToModel<t_xzqh>();
//            return Json(Result.Success(data.Count, data), JsonRequestBehavior.AllowGet);
//        }
//        public ActionResult User_AddXzqh(int id, List<string> list)
//        {
//            string sql = string.Format("delete from t_user_xzqh where userid={0}", id);
//            int count = DbHelperMySQL.ExecuteSql(sql);
//            int result = 0;
//            foreach (var l in list)
//            {
//                sql = string.Format("insert into t_user_xzqh(userid,xzqhcode,newcount,time) values({0},'{1}',0,now())", id, l);
//                count = DbHelperMySQL.ExecuteSql(sql);
//                result++;
//            }
//            return Json(Result.AutoResult(result == list.Count ? 1 : 0), JsonRequestBehavior.AllowGet);
//        }
//        #endregion

//        #region 网格员考评
//        public ActionResult UserRank()
//        {
//            return View();
//        }
//        public ActionResult UserRank_GetList()
//        {
//            string sql = "select * from t_user t order by t.count_scan desc";
//            DataTable dt1 = DbHelperMySQL.Query(sql).Tables[0];
//            var data1 = dt1.ConvertToModel<t_user>();
//            sql = "select * from t_user t order by t.count_question desc";
//            DataTable dt2 = DbHelperMySQL.Query(sql).Tables[0];
//            var data2 = dt2.ConvertToModel<t_user>();
//            sql = "select * from t_user t order by t.count_duration desc";
//            DataTable dt3 = DbHelperMySQL.Query(sql).Tables[0];
//            var data3 = dt3.ConvertToModel<t_user>();
//            return Json(Result.Success(3, new { scan = data1, question = data2, duration = data3 }), JsonRequestBehavior.AllowGet);
//        }
//        #endregion

//        #region 修改密码
//        public ActionResult Password()
//        {
//            return View();
//        }
//        public ActionResult Password_Edit(string oldPassword, string newPassword)
//        {
//            string sql = string.Format("select * from t_admin t where t.id={0}", Common.getAdmin().id);
//            DataTable dt = DbHelperMySQL.Query(sql).Tables[0];
//            var data = dt.ConvertToModel<t_admin>().FirstOrDefault();
//            if (data.password != oldPassword)
//            {
//                return Json(Result.Error("旧密码错误", 0), JsonRequestBehavior.AllowGet);
//            }
//            sql = string.Format("update t_admin t set t.password='{0}' where t.id={1}", newPassword, Common.getAdmin().id);
//            int count = DbHelperMySQL.ExecuteSql(sql);
//            return Json(Result.AutoResult(count), JsonRequestBehavior.AllowGet);
//        }
//        #endregion
//    }
//}