using CWGK.Models;
using CWGK.Models.model;
using CWGK.Models.model_self;
using Maticsoft.DBUtility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CWGK.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Login(int id, string url)
        {
            string sql = string.Format("select * from t_user t where t.id={0}", id);
            DataTable dt = DbHelperMySQL.Query(sql).Tables[0];
            var data = dt.ConvertToModel<t_user>();
            HttpCookie cookie = new HttpCookie("user", HttpUtility.UrlEncode(JsonConvert.SerializeObject(data[0])));
            Response.SetCookie(cookie);
            return Redirect(HttpUtility.UrlDecode(url));
        }
        public ActionResult Index()
        {
            return View();
        }
        #region 公共首页
        public ActionResult NewLogin(int id)
        {
            string sql = string.Format("select * from t_user t where t.id={0}", id);
            DataTable dt = DbHelperMySQL.Query(sql).Tables[0];
            var data = dt.ConvertToModel<t_user>();
            HttpCookie cookie = new HttpCookie("user", HttpUtility.UrlEncode(JsonConvert.SerializeObject(data[0])));
            Response.SetCookie(cookie);
            return RedirectToAction("NewIndex");
        }
        public ActionResult NewIndex()
        {
            return View();
        }
        public ActionResult getNewIndexData()
        {
            Dictionary<string, Object> result = new Dictionary<string, object>();
            //秸秆焚烧数据
            //智慧农业数据
            //村务公开数据
            //////新闻数量
            Dictionary<string, Object> cwgk = new Dictionary<string, object>();
            string sql = "select count(*) from t_news t where t.del=0 and t.state=1";
            int count = Convert.ToInt32(DbHelperMySQL.GetSingle(sql));
            cwgk.Add("news_count", count);
            //////消息数量
            List<t_user_message> messages = new List<t_user_message>();
            sql = string.Format("select count(*) from t_user_xzqh t where t.userid={0} and t.newcount>0", Common.getUser().id);
            count = Convert.ToInt32(DbHelperMySQL.GetSingle(sql));
            sql = string.Format("select count(*) from t_news_talk t where t.userid={0} and t.read=0", Common.getUser().id);
            count += Convert.ToInt32(DbHelperMySQL.GetSingle(sql));
            cwgk.Add("message_count", count);
            result.Add("cwgk", cwgk);
            //电商扶贫数据
            return Json(Result.Success(1, result), JsonRequestBehavior.AllowGet);
        }
        public ActionResult SuYuan()
        {
            return View();
        }
        public ActionResult NewMy()
        {
            return View();
        }
        #endregion

        #region 新闻分类
        public ActionResult NewsType()
        {
            return View();
        }
        #endregion

        #region 新闻列表
        public ActionResult News()
        {
            return View();
        }
        public ActionResult News_GetListByPage(SearchModel model)
        {
            string xzqh = model.string1;
            string type = model.string2;
            string sql1 = string.Format("select * from t_news t where t.del=0 and t.state=1 and t.xzqhcode={0} and t.typeid={1} order by t.id desc limit {2}", xzqh, type, Util.getPage(model.page, model.limit));
            string sql2 = string.Format("select count(*) from t_news t where t.del=0 and t.xzqhcode={0} and t.typeid={1}", xzqh, type);
            DataTable dt = DbHelperMySQL.Query(sql1).Tables[0];
            var data = dt.ConvertToModel<t_news>();
            int count = Convert.ToInt32(DbHelperMySQL.GetSingle(sql2));
            return Json(Result.Success(count, data), JsonRequestBehavior.AllowGet);
        }
        public ActionResult NewsCwgk_GetListByPage(SearchModel model)
        {
            string sql1 = string.Format("select * from t_spider_zwgk t order by t.time desc limit {0}", Util.getPage(model.page, model.limit));
            string sql2 = "select count(*) from t_spider_zwgk";
            DataTable dt = DbHelperMySQL.Query(sql1).Tables[0];
            var data = dt.ConvertToModel<t_spider_cwgk>();
            int count = Convert.ToInt32(DbHelperMySQL.GetSingle(sql2));
            return Json(Result.Success(count, data), JsonRequestBehavior.AllowGet);
        }
        public ActionResult NewsBjtj_GetListByPage(SearchModel model)
        {
            string sql1 = string.Format("select * from t_spider_bjtj t order by t.id limit {0}", Util.getPage(model.page, model.limit));
            string sql2 = "select count(*) from t_spider_bjtj";
            DataTable dt = DbHelperMySQL.Query(sql1).Tables[0];
            var data = dt.ConvertToModel<t_spider_bjtj>();
            int count = Convert.ToInt32(DbHelperMySQL.GetSingle(sql2));
            return Json(Result.Success(count, data), JsonRequestBehavior.AllowGet);
        }
        public ActionResult NewsBjgs_GetListByPage(SearchModel model)
        {
            string sql1 = string.Format("select * from t_spider_bjgs t order by t.id limit {0}", Util.getPage(model.page, model.limit));
            string sql2 = "select count(*) from t_spider_bjgs";
            DataTable dt = DbHelperMySQL.Query(sql1).Tables[0];
            var data = dt.ConvertToModel<t_spider_bjgs>();
            int count = Convert.ToInt32(DbHelperMySQL.GetSingle(sql2));
            return Json(Result.Success(count, data), JsonRequestBehavior.AllowGet);
        }
        public ActionResult NewsBslc_GetListByPage(SearchModel model)
        {
            string sql1 = string.Format("select * from t_spider_bslc t order by t.time desc limit {0}", Util.getPage(model.page, model.limit));
            string sql2 = "select count(*) from t_spider_bslc";
            DataTable dt = DbHelperMySQL.Query(sql1).Tables[0];
            var data = dt.ConvertToModel<t_spider_bslc>();
            int count = Convert.ToInt32(DbHelperMySQL.GetSingle(sql2));
            return Json(Result.Success(count, data), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 新闻详情
        public ActionResult NewsDetail()
        {
            return View();
        }
        public ActionResult NewsDetailForDataV()
        {
            return View();
        }
        public ActionResult NewsDetail_Get(int id)
        {
            string sql = string.Format("select * from t_news t where t.id={0}", id);
            DataTable dt = DbHelperMySQL.Query(sql).Tables[0];
            var data = dt.ConvertToModel<t_news>();
            try
            {
                Common.Count_User_News_Scan();
                Common.News_Scan(id);
            }
            catch (Exception e)
            {

            }
            return Json(Result.Success(data.Count, data), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 新闻详情(爬虫)
        public ActionResult NewsDetailSpider()
        {
            return View();
        }
        public ActionResult NewsDetailSpider_Zwgk_Get(int id)
        {
            string sql = string.Format("select * from t_spider_zwgk t where t.id={0}", id);
            DataTable dt = DbHelperMySQL.Query(sql).Tables[0];
            var data = dt.ConvertToModel<t_spider_cwgk>();
            return Json(Result.Success(data.Count, data), JsonRequestBehavior.AllowGet);
        }
        public ActionResult NewsDetailSpider_Bslc_Get(int id)
        {
            string sql = string.Format("select * from t_spider_bslc t where t.id={0}", id);
            DataTable dt = DbHelperMySQL.Query(sql).Tables[0];
            var data = dt.ConvertToModel<t_spider_bslc>();
            return Json(Result.Success(data.Count, data), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 新闻互动
        public ActionResult NewsTalk()
        {
            return View();
        }
        public ActionResult NewsTalk_GetList(int id)
        {
            string sql = string.Format("select *,t1.name username from t_news_talk t left join t_user t1 on t.userid=t1.id where t.del=0 and t.newsid={0}", id);
            DataTable dt = DbHelperMySQL.Query(sql).Tables[0];
            var data = dt.ConvertToModel<t_news_talk>();
            return Json(Result.Success(data.Count, data), JsonRequestBehavior.AllowGet);
        }
        public ActionResult NewsTalk_Add(t_news_talk model, IEnumerable<HttpPostedFileBase> files)
        {
            string sql = string.Format("insert into t_news_talk(newsid,userid,time,question,del) values({0},{1},now(),'{2}',0);SELECT @@IDENTITY", model.newsid, Common.getUser().id, model.question);
            int talkid = Convert.ToInt32(DbHelperMySQL.GetSingle(sql));
            try
            {
                Common.Count_User_News_Question();
            }
            catch (Exception e)
            {

            }
            int result = 0;
            if (files != null && files.Count() != 0)
            {
                foreach (var f in files)
                {
                    string fileName = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "." + Guid.NewGuid() + Path.GetExtension(f.FileName);
                    string filePath = Path.Combine(Request.MapPath("~/Upload"), fileName);
                    f.SaveAs(filePath);
                    sql = string.Format("insert into t_files(filename,type,typekey,del) values('{0}',1,'{1}',0)", fileName, talkid);
                    int count = DbHelperMySQL.ExecuteSql(sql);
                    result += count;
                }
                return Json(Result.AutoResult(result == files.Count() ? 1 : 0), JsonRequestBehavior.AllowGet);
            }
            return Json(Result.AutoResult(talkid == 0 ? 0 : 1), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 消息
        public ActionResult Message()
        {
            return View();
        }
        public ActionResult Message_GetList()
        {
            List<t_user_message> messages = new List<t_user_message>();
            string sql = string.Format("select t.*,t1.name xzqhname from t_user_xzqh t left join t_xzqh t1 on t.xzqhcode=t1.code where t.userid={0} and t.newcount>0", Common.getUser().id);
            DataTable dt = DbHelperMySQL.Query(sql).Tables[0];
            var data1 = dt.ConvertToModel<t_user_xzqh>();
            foreach (var item in data1)
            {
                t_user_message m = new t_user_message()
                {
                    content = item.xzqhname + "发布了" + item.newcount + "条新闻",
                    time = item.time,
                };
                messages.Add(m);
            };
            sql = string.Format("select t.time,t1.title newstitle from t_news_talk t left join t_news t1 on t.newsid=t1.id  where t.userid={0} and t.read=0", Common.getUser().id);
            dt = DbHelperMySQL.Query(sql).Tables[0];
            var data2 = dt.ConvertToModel<t_news_talk>();
            foreach (var item in data2)
            {
                t_user_message m = new t_user_message()
                {
                    content = "回复：《" + item.newstitle + "》",
                    time = item.time,
                };
                messages.Add(m);
            };
            return Json(Result.Success(messages.Count, messages), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Message_Read()
        {
            int count = 0;
            string sql = string.Format("update t_user_xzqh t set t.newcount=0 where t.userid={0}", Common.getUser().id);
            count += DbHelperMySQL.ExecuteSql(sql);
            sql = string.Format("update t_news_talk t set t.read=1 where t.userid={0} and t.read=0", Common.getUser().id);
            count += DbHelperMySQL.ExecuteSql(sql);
            return Json(Result.AutoResult(count > 0 ? 1 : 0), JsonRequestBehavior.AllowGet);
        }
        #endregion

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

        #region 排行榜
        public ActionResult MyRank()
        {
            return View();
        }
        public ActionResult MyRank_GetList()
        {
            string sql = "select * from t_user t order by t.count_scan desc";
            DataTable dt1 = DbHelperMySQL.Query(sql).Tables[0];
            var data1 = dt1.ConvertToModel<t_user>();
            sql = "select * from t_user t order by t.count_question desc";
            DataTable dt2 = DbHelperMySQL.Query(sql).Tables[0];
            var data2 = dt2.ConvertToModel<t_user>();
            sql = "select * from t_user t order by t.count_duration desc";
            DataTable dt3 = DbHelperMySQL.Query(sql).Tables[0];
            var data3 = dt3.ConvertToModel<t_user>();
            return Json(Result.Success(3, new { scan = data1, question = data2, duration = data3 }), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 村镇关注
        public ActionResult MyXzqh()
        {
            return View();
        }
        public ActionResult MyXzqh_Add(List<string> list)
        {
            t_user user = Common.getUser();
            string sql = string.Format("delete from t_user_xzqh where userid={0}", user.id);
            int count = DbHelperMySQL.ExecuteSql(sql);
            int result = 0;
            foreach (var l in list)
            {
                sql = string.Format("insert into t_user_xzqh(userid,xzqhcode,newcount,time) values({0},'{1}',0,now())", user.id, l);
                count = DbHelperMySQL.ExecuteSql(sql);
                result++;
            }
            return Json(Result.AutoResult(result == list.Count ? 1 : 0), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 我的提问
        public ActionResult MyQuestion()
        {
            return View();
        }
        public ActionResult MyQuestion_GetListByPage(SearchModel model)
        {
            string sql1 = string.Format("select t.*,t1.title newstitle,(select count(*) from t_files tt where tt.del=0 and tt.type=1 and tt.typekey=t.id) filecount from t_news_talk t left join t_news t1 on t.newsid = t1.id where t.del=0 and t.userid={0} order by t.id desc limit {1}", Common.getUser().id, Util.getPage(model.page, model.limit));
            string sql2 = string.Format("select count(*) from t_news_talk t where t.del=0 and t.userid={0}", Common.getUser().id);
            DataTable dt = DbHelperMySQL.Query(sql1).Tables[0];
            var data = dt.ConvertToModel<t_news_talk>();
            int count = Convert.ToInt32(DbHelperMySQL.GetSingle(sql2));
            return Json(Result.Success(count, data), JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}