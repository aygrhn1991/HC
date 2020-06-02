using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWGK.Models
{
    public static class Util
    {
        public static string getPage(int page, int limit)
        {
            return string.Format("{0},{1}", (page - 1) * limit, limit);
        }
    }
}