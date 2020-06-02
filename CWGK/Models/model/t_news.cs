namespace CWGK.Models.model
{
    public partial class t_news
    {
        private int _id;
        private string _xzqhcode;
        private int? _typeid;
        private int? _adminid;
        private string _title;
        private string _time;
        private string _content;
        private int? _scan;
        private int? _state;
        private int? _del;
        public int id { set { _id = value; } get { return _id; } }
        public string xzqhcode { set { _xzqhcode = value; } get { return _xzqhcode; } }
        public int? typeid { set { _typeid = value; } get { return _typeid; } }
        public int? adminid { get => _adminid; set => _adminid = value; }
        public string title { set { _title = value; } get { return _title; } }
        public string time { set { _time = value; } get { return _time; } }
        public string content { set { _content = value; } get { return _content; } }
        public int? scan { set { _scan = value; } get { return _scan; } }
        public int? state { set { _state = value; } get { return _state; } }
        public int? del { set { _del = value; } get { return _del; } }
        private string _xzqhname;
        private string _typename;
        private string _adminname;
        public string xzqhname { set { _xzqhname = value; } get { return _xzqhname; } }
        public string typename { get => _typename; set => _typename = value; }
        public string adminname { get => _adminname; set => _adminname = value; }

    }
}

