namespace DSFP.Models.model
{
    public partial class f_team
    {
        private int _id;
        private string _name;
        private int _abilityid;
        private int? _leaderid;
        private int? _del;

        public int id { set { _id = value; } get { return _id; } }
        public string name { set { _name = value; } get { return _name; } }
        public int abilityid { set { _abilityid = value; } get { return _abilityid; } }
        public int? leaderid { set { _leaderid = value; } get { return _leaderid; } }
        public int? del { set { _del = value; } get { return _del; } }
        private string _abilityname;
        private string _leadername;

        public string abilityname { get => _abilityname; set => _abilityname = value; }
        public string leadername { get => _leadername; set => _leadername = value; }

    }
}

