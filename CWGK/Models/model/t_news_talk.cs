namespace CWGK.Models.model
{
    public partial class t_news_talk
    {
        private int _id;
        private int? _newsid;
        private int? _userid;
        private int? _adminid;
        private int? _type;
        private string _time;
        private string _question;
        private string _answer;
        private int _read;
        private int? _del;

        public int id { set { _id = value; } get { return _id; } }
        public int? newsid { set { _newsid = value; } get { return _newsid; } }
        public int? userid { set { _userid = value; } get { return _userid; } }
        public int? adminid { set { _adminid = value; } get { return _adminid; } }
        public int? type { set { _type = value; } get { return _type; } }
        public string time { set { _time = value; } get { return _time; } }
        public string question { get => _question; set => _question = value; }
        public string answer { get => _answer; set => _answer = value; }
        public int read { get => _read; set => _read = value; }
        public int? del { set { _del = value; } get { return _del; } }
        private string _newstitle;
        private string _xzqhname;
        private string _typename;
        private string _adminname;
        private string _username;
        private int _filecount;

        public string newstitle { get => _newstitle; set => _newstitle = value; }
        public string xzqhname { get => _xzqhname; set => _xzqhname = value; }
        public string typename { get => _typename; set => _typename = value; }
        public string adminname { get => _adminname; set => _adminname = value; }
        public string username { get => _username; set => _username = value; }
        public int filecount { get => _filecount; set => _filecount = value; }

    }
}

