namespace DSFP.Models.model
{
    public partial class f_files
    {
        private int _id;
        private string _filename;
        private int? _type;
        private string _typekey;
        private int? _del;
        public int id { set { _id = value; } get { return _id; } }
        public string filename { set { _filename = value; } get { return _filename; } }
        public int? type { set { _type = value; } get { return _type; } }
        public string typekey { set { _typekey = value; } get { return _typekey; } }
        public int? del { set { _del = value; } get { return _del; } }

    }
}

