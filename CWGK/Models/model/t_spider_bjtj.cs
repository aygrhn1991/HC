namespace CWGK.Models.model
{
    public class t_spider_bjtj
    {
        private int _id;
        private string _oRG_NAME;
        private string _tOTALNUM;
        private string _iNHALLNUM;
        private string _iNHALLRATE;
        private string _iSONLINENUM;
        private string _iSONLINE_PER;

        public int id { get => _id; set => _id = value; }
        public string ORG_NAME { get => _oRG_NAME; set => _oRG_NAME = value; }
        public string TOTALNUM { get => _tOTALNUM; set => _tOTALNUM = value; }
        public string INHALLNUM { get => _iNHALLNUM; set => _iNHALLNUM = value; }
        public string INHALLRATE { get => _iNHALLRATE; set => _iNHALLRATE = value; }
        public string ISONLINENUM { get => _iSONLINENUM; set => _iSONLINENUM = value; }
        public string ISONLINE_PER { get => _iSONLINE_PER; set => _iSONLINE_PER = value; }
    }
}