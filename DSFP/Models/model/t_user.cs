namespace DSFP.Models.model
{
    public partial class t_user
    {
        private int _id;
        private string _name;
        private string _phone;
        private string _key;
        private int _abilityid;
        private string _position;
        private int? _del;

        public int id { set { _id = value; } get { return _id; } }
        public string name { set { _name = value; } get { return _name; } }
        public string phone { set { _phone = value; } get { return _phone; } }
        public string key { set { _key = value; } get { return _key; } }
        public int abilityid { set { _abilityid = value; } get { return _abilityid; } }
        public string position { set { _position = value; } get { return _position; } }
        public int? del { set { _del = value; } get { return _del; } }
        private string _abilityname;
        public string abilityname { get => _abilityname; set => _abilityname = value; }

    }
}

