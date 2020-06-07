using DSFP.Models;
using DSFP.Models.model;
using Maticsoft.DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DSFP.Controllers
{
    public class DataController : Controller
    {
        public ActionResult AddSign()
        {
            int start_id = 10000;

            string sql_poor = "select * from f_poor t where t.id>=24";
            DataTable dt_poor = DbHelperMySQL.Query(sql_poor).Tables[0];
            List<f_poor> data_poor = dt_poor.ConvertToModel<f_poor>().ToList();
            Random random = new Random();
            foreach (var poor in data_poor)
            {
                string sql_sign_insert = string.Format("insert into f_sign(id,poorid,name,number,time,datestart,dateend,weight,price,del) " +
                    "values({0},{1},'{2}','{3}','{4}','{5}','{6}',{7},{8},0)",
                    start_id, poor.id, poor.name + "2020扶贫签约", "20200607" + start_id, DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd"), random.Next(10, 100) * 50, 3, 0);
                int sql_sign_insert_result = DbHelperMySQL.ExecuteSql(sql_sign_insert);
                start_id++;
            }
            return Json("ok", JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddActivity()
        {
            int start_id = 10000;

            string sql_team = "select * from f_team t where t.del=0 and t.id>=10000 and t.id%2=0";
            DataTable dt_team = DbHelperMySQL.Query(sql_team).Tables[0];
            List<f_team> data_team = dt_team.ConvertToModel<f_team>().ToList();

            foreach (var team in data_team)
            {
                string sql_activity_insert = string.Format("insert into f_activity(id,name,number,teamid,abilityid,time,state,result,suggest,del) " +
                    "values({0},'{1}','{2}',{3},{4},'{5}',{6},'{7}','{8}',0)",
                    start_id, team.name.Replace("帮扶小组", "帮扶活动"), "20200607" + start_id, team.id, team.abilityid, GetRandomDate(), 1, "帮扶活动顺利完成", "无", 0);
                int sql_activity_insert_result = DbHelperMySQL.ExecuteSql(sql_activity_insert);

                string sql_poor = "select * from f_poor t where t.id>=24";
                DataTable dt_poor = DbHelperMySQL.Query(sql_poor).Tables[0];
                List<f_poor> data_poor = dt_poor.ConvertToModel<f_poor>().ToList();
                Random random = new Random();
                for (int i = 0; i < 5; i++)
                {
                    int r = random.Next(0, data_poor.Count);
                    string sql_activity_poor_insert = string.Format("insert into f_activity_poor(activityid,poorid,profit) values({0},{1},{2})", start_id, data_poor[r].id, random.Next(10, 20) * 50);
                    int sql_activity_poor_insert_result = DbHelperMySQL.ExecuteSql(sql_activity_poor_insert);
                }
                start_id++;

            }
            return Json("ok", JsonRequestBehavior.AllowGet);
        }

        private string GetRandomDate()
        {
            Random random = new Random();
            int year1 = 2019;
            int year2 = 2020;
            int month1 = random.Next(7, 13);
            int month2 = random.Next(1, 7);
            int date = random.Next(1, 28);
            return random.Next(1, 3) == 1 ? (year1 + "-" + month1 + "-" + date) : (year2 + "-" + month2 + "-" + date);
        }
        public ActionResult AddPoor()
        {
            string sql_xzqh = "select * from t_xzqh t where t.pcode!='0'";
            DataTable dt_xzqh = DbHelperMySQL.Query(sql_xzqh).Tables[0];
            List<t_xzqh> data_xzqh = dt_xzqh.ConvertToModel<t_xzqh>().ToList();

            List<string> names = new List<string>();
            string name3 = "龚星宇姜伟毅蒋嘉澍何修然梁伟诚谢宏才刘德义林乐邦黎建柏康宇文苏乐康田明煦刘翰采钱宇寰张星火许华池周玉山丁嘉慕方俊名姜英发苏刚豪彭浩歌邹英华石波光沈力勤张良哲漕金鑫张子晋赵高驰武涵育薛开诚余温瑜李涵蓄王伟祺宋浩初陆子明韩睿思史奇胜史承志郭炎彬陈成周钱正平徐兴言廖华池范阳冰文鸿晖林天泽钱波光陆宇达蔡烨煜史智勇廖才艺曾兴运彭和昶江雅达赖成双康和惬陈鹏云崔文虹赖乐人邵天纵卢星光彭明亮康国兴吕宇荫苏高丽许和韵孙乐康邱昊穹贾景焕秦文滨宋永嘉熊嘉赐徐欣荣戴子琪罗意蕴汪开畅孔向阳杨弘懿孟开诚罗成荫蔡飞掣侯泰河梁锐泽李高翰吕乐游程阳焱顾濮存吕俊远黄开朗常志诚郭思淼毛元基王泰华贺元良萧皓轩杜俊郎郑烨烁叶文星赵修杰阎泰华戴坚诚罗心远白兴业陆天元赖祺祥邓正浩曾鸿达曹阳旭石恺歌马文敏蒋勇锐蔡阳曦何凯捷方和悌杨俊远武鸿朗汤锐锋龚博易汤安康侯正平贾鸿朗夏宏浚赖智渊董经国范昂然曹弘壮廖英纵汪正谊朱勇捷陆阳焱漕成双李俊爽邵涵忍夏兴怀钱宏硕程雅志孔鸿晖叶浩言吴承颜马承颜陆永康赵康伯魏乐湛钱向笛冯德运卢建同崔志尚贾晓博龙博容吕涵畅孟学真杜星宇汪乐志沈伟茂乔睿聪刘文宣赵兴思薛高达董高原秦睿诚孙俊杰乔智勇邹建白刘博厚汤飞章汪炫明汤安翔漕茂材江煜祺何建义陆光誉郝光赫余巍昂赵坚秉文德运贾浦泽高天纵江浩歌杜弘益潘子实文浩浩汪俊良郝高畅钟星驰蔡星华康阳舒萧新立顾欣然王文宣高毅然段雨星王伟志万英耀戴建修史华奥白坚诚姜弘盛薛黎昕乔同甫";
            string name2 = "何铠吴凯韩弓吴梁田仓曹钧王州董列崔佺萧棋宋金夏彬卢曾梁鹏汪策任鲛漕攀苏固龙曹周慕孙生邱韩田毅马达郝沧贺晋赖弩王偿罗问钱生郭端钱兴苏真钱忠方段史钢谢部谢义王龄吴镰卢征冯杰杜健彭勰梁彪熊瑎任苓谢俟秦弛毛帅贺修龙广卢清任涵谢擎邹晁魏耘董曾郝会姜巡杨昮乔沅冯迈田船贾宽秦耘萧宙梁鸿石骢宋勘谭擎吕江金密余促任骏杜亮孔诤万琦乔参黄瑎罗仁张促王寒傅馗马轼朱铠叶黎朱金武助乔嘉曾乔方仕于峙邹密徐固傅清史伸王孺黄富钟峙谭达曾凌袁安邱佐徐固龙宸毛谊孙强韩子乔发漕钢石欧黎沅石密卢峰孙圣熊腾廖楠马源阎征韩泉苏盛萧筠高濮戴矢钟竹彭邦王金戴群赖综王舱白策田勇彭林尹盛杜丕田昱万谦杨骏邵仝蔡宣卢剑林真贾参汤民董霆梁侃萧书梁御邱生龙中郭群钱鲲李社漕筠龙葆郑莱陆炜龚生萧劻郑晸任亭薛汗田俣蒋星夏瑎赖羚朱澔林促石生邵琩于泰蔡鸿朱智贺谦张浩赵祚尹先漕发杨林漕俯邱森武琛周滕阎玄潘弢蔡穆龙元孙帝贾煜于骞汪昱张沧万魏叶乒龚浩姜璥潘部姜宽程茂";
            for (int i = 0; i < 200; i++)
            {
                names.Add(name3.Substring(i * 3, 3));
                names.Add(name2.Substring(i * 2, 2));
            }
            Random random = new Random();
            foreach (var item in names)
            {
                int r = random.Next(0, data_xzqh.Count);
                int population = random.Next(2, 5);
                int square = random.Next(8, 20) * 5;
                string sql_poor_insert = string.Format("insert into f_poor(name,xzqhcode,population,square,del) values('{0}','{1}',{2},{3},0)", item, data_xzqh[r].code, population, square);
                int sql_poor_insert_result = DbHelperMySQL.ExecuteSql(sql_poor_insert);
            }
            return Json("ok", JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddTeam()
        {
            int start_id = 10000;

            string sql_ability = "select * from f_team_ability t where t.del=0";
            DataTable dt_ability = DbHelperMySQL.Query(sql_ability).Tables[0];
            List<f_team_ability> data_ability = dt_ability.ConvertToModel<f_team_ability>().ToList();

            string sql_xzqh = "select * from t_xzqh t where t.pcode!='0'";
            DataTable dt_xzqh = DbHelperMySQL.Query(sql_xzqh).Tables[0];
            List<t_xzqh> data_xzqh = dt_xzqh.ConvertToModel<t_xzqh>().ToList();
            foreach (var xzqh in data_xzqh)
            {
                Random random = new Random();
                int r1 = random.Next(0, data_ability.Count);
                int r2 = random.Next(0, data_ability.Count);
                for (int i = 0; i < data_ability.Count; i++)
                {
                    if (i == r1 || i == r2)
                    {
                        string name = xzqh.name + data_ability[i].name + "帮扶小组";
                        string sql_user = "select * from t_user t where t.name like '%" + xzqh.name + "%'";
                        DataTable dt_user = DbHelperMySQL.Query(sql_user).Tables[0];
                        List<t_user> data_user = dt_user.ConvertToModel<t_user>().ToList();
                        if (data_user.Count != 0)
                        {
                            string sql_team_insert = string.Format("insert into f_team(id,name,abilityid,leaderid,del) values({0},'{1}',{2},{3},0)", start_id, name, data_ability[i].id, data_user[0].id);
                            int sql_team_insert_result = DbHelperMySQL.ExecuteSql(sql_team_insert);
                            foreach (var user in data_user)
                            {
                                string sql_team_user_insert = string.Format("insert into f_team_user(teamid,userid) values({0},{1})", start_id, user.id);
                                int sql_team_user_insert_result = DbHelperMySQL.ExecuteSql(sql_team_user_insert);
                            }
                            start_id++;
                        }
                    }
                }
            }
            return Json("ok", JsonRequestBehavior.AllowGet);
        }
    }
}