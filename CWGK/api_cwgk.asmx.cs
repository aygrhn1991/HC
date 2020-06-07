using CWGK_SPIDER.spiders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace CWGK
{
    /// <summary>
    /// api_cwgk 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class api_cwgk : System.Web.Services.WebService
    {

        [WebMethod]
        public string Spider()
        {
            try
            {
                zwgk.start();
                bjtj.start();
                bjgs.start();
                bslc.start();
                return "0";
            }
            catch (Exception e)
            {
                return e.Message.ToString();
            }

        }
    }
}
