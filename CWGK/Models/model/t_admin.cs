namespace CWGK.Models.model
{
    public partial class t_admin
    {
        private int _id;
        private int? _level;
        private string _xzqhcode;
        private string _account;
        private string _password;
        private string _name;
        private string _phone;
        private int? _count_publish;
        private int? _count_answer;
        private int? _del;
        public int id { set { _id = value; } get { return _id; } }
        public int? level { set { _level = value; } get { return _level; } }
        public string xzqhcode { set { _xzqhcode = value; } get { return _xzqhcode; } }
        public string account { set { _account = value; } get { return _account; } }
        public string password { set { _password = value; } get { return _password; } }
        public string name { set { _name = value; } get { return _name; } }
        public string phone { set { _phone = value; } get { return _phone; } }
        public int? count_publish { set { _count_publish = value; } get { return _count_publish; } }
        public int? count_answer { set { _count_answer = value; } get { return _count_answer; } }
        public int? del { set { _del = value; } get { return _del; } }
        private string _xzqhname;
        public string xzqhname { set { _xzqhname = value; } get { return _xzqhname; } }

    }
}

