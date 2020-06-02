namespace CWGK.Models.model
{
    public partial class t_user
    {
        private int _id;
        private string _name;
        private string _phone;
        private string _key;
        private int? _count_scan;
        private int? _count_question;
        private int? _count_duration;
        private int? _del;
        public int id { set { _id = value; } get { return _id; } }
        public string name { set { _name = value; } get { return _name; } }
        public string phone { set { _phone = value; } get { return _phone; } }
        public string key { set { _key = value; } get { return _key; } }
        public int? count_scan { set { _count_scan = value; } get { return _count_scan; } }
        public int? count_question { set { _count_question = value; } get { return _count_question; } }
        public int? count_duration { set { _count_duration = value; } get { return _count_duration; } }
        public int? del { set { _del = value; } get { return _del; } }
        private string _xzqhname;
        public string xzqhname { set { _xzqhname = value; } get { return _xzqhname; } }
    }
}

