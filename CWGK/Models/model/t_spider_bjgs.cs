namespace CWGK.Models.model
{
    public class t_spider_bjgs
    {
        private int _id;
        private string _rECEIVE_NUMBER;
        private string _oRG_NAME;
        private string _aPPLY_SUBJECT;
        private string _fINISH_TIME;
        private string _sTATE;

        public int id { get => _id; set => _id = value; }
        public string RECEIVE_NUMBER { get => _rECEIVE_NUMBER; set => _rECEIVE_NUMBER = value; }
        public string ORG_NAME { get => _oRG_NAME; set => _oRG_NAME = value; }
        public string APPLY_SUBJECT { get => _aPPLY_SUBJECT; set => _aPPLY_SUBJECT = value; }
        public string FINISH_TIME { get => _fINISH_TIME; set => _fINISH_TIME = value; }
        public string STATE { get => _sTATE; set => _sTATE = value; }
    }
}