using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWGK.Models
{
    public class SearchModel
    {
        public int page { get; set; }
        public int limit { get; set; }
        public string string1 { get; set; }
        public string string2 { get; set; }
        public int number1 { get; set; }
        public int number2 { get; set; }
        public DateTime datetime1 { get; set; }
        public DateTime datetime2 { get; set; }
    }
}