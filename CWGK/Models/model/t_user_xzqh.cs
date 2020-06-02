namespace CWGK.Models.model
{
    public partial class t_user_xzqh
    {
        private int _userid;
        private string _xzqhcode;
        private int _newcount;
        private string _time;

        public int userid { set { _userid = value; } get { return _userid; } }
        public string xzqhcode { set { _xzqhcode = value; } get { return _xzqhcode; } }
        public int newcount { get => _newcount; set => _newcount = value; }
        public string time { get => _time; set => _time = value; }
        private string _xzqhname;
        public string xzqhname { get => _xzqhname; set => _xzqhname = value; }
    }
}

