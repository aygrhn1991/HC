namespace DSFP.Models.model
{
    public partial class f_poor
    {
        private int _id;
        private string _name;
        private string _xzqhcode;
        private int? _population;
        private int? _del;

        public int id { set { _id = value; } get { return _id; } }
        public string name { set { _name = value; } get { return _name; } }
        public string xzqhcode { set { _xzqhcode = value; } get { return _xzqhcode; } }
        public int? population { set { _population = value; } get { return _population; } }
        public int? del { set { _del = value; } get { return _del; } }
        private string _xzqhname;
        private float _profit;

        public string xzqhname { get => _xzqhname; set => _xzqhname = value; }
        public float profit { get => _profit; set => _profit = value; }

    }
}

