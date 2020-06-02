namespace CWGK.Models.model
{
    public partial class t_news_type
    {
        private int _id;
        private string _name;
        private int? _del;
        public int id { set { _id = value; } get { return _id; } }
        public string name { set { _name = value; } get { return _name; } }
        public int? del { set { _del = value; } get { return _del; } }

    }
}

