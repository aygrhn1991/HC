namespace DSFP.Models.model
{
    public partial class f_activity
    {
        private int _id;
        private string _name;
        private string _number;
        private int? _teamid;
        private int _abilityid;
        private string _time;
        private int? _state;
        private string _result;
        private string _suggest;
        private int? _del;

        public int id { set { _id = value; } get { return _id; } }
        public string name { set { _name = value; } get { return _name; } }
        public string number { get => _number; set => _number = value; }
        public int? teamid { set { _teamid = value; } get { return _teamid; } }
        public int abilityid { get => _abilityid; set => _abilityid = value; }
        public string time { get => _time; set => _time = value; }
        public int? state { set { _state = value; } get { return _state; } }
        public string result { get => _result; set => _result = value; }
        public string suggest { get => _suggest; set => _suggest = value; }
        public int? del { set { _del = value; } get { return _del; } }
        private string _teamname;
        private string _abilityname;
        public string teamname { get => _teamname; set => _teamname = value; }
        public string abilityname { get => _abilityname; set => _abilityname = value; }

    }
}

