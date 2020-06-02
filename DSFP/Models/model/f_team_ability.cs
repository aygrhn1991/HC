using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DSFP.Models.model
{
    public class f_team_ability
    {
        private int _id;
        private string _name;
        private int? _del;
        public int id { set { _id = value; } get { return _id; } }
        public string name { set { _name = value; } get { return _name; } }
        public int? del { set { _del = value; } get { return _del; } }
    }
}