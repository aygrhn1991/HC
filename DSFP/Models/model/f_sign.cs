namespace DSFP.Models.model
{
    public partial class f_sign
    {
        private int _id;
        private int? _poorid;
        private string _name;
        private string _number;
        private string _time;
        private string _datestart;
        private string _dateend;
        private float? _weight;
        private float? _price;
        private int? _del;

        public int id { set { _id = value; } get { return _id; } }
        public int? poorid { set { _poorid = value; } get { return _poorid; } }
        public string name { get => _name; set => _name = value; }
        public string number { get => _number; set => _number = value; }
        public string time { get => _time; set => _time = value; }
        public string datestart { get => _datestart; set => _datestart = value; }
        public string dateend { get => _dateend; set => _dateend = value; }
        public float? weight { set { _weight = value; } get { return _weight; } }
        public float? price { set { _price = value; } get { return _price; } }
        public int? del { set { _del = value; } get { return _del; } }
        private string _poorname;
        public string poorname { get => _poorname; set => _poorname = value; }

    }
}

