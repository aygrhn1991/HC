namespace CWGK.Models.model
{
    public class t_spider_cwgk
    {
        private int _id;
        private string _title;
        private string _time;
        private string _author;
        private string _content;

        public int id { get => _id; set => _id = value; }
        public string title { get => _title; set => _title = value; }
        public string time { get => _time; set => _time = value; }
        public string author { get => _author; set => _author = value; }
        public string content { get => _content; set => _content = value; }
    }
}