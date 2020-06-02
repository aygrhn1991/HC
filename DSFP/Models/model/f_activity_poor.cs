namespace DSFP.Models.model
{
    public partial class f_activity_poor
    {
        private int _activityid;
        private int _poorid;
        private float _profit;

        public int activityid { set { _activityid = value; } get { return _activityid; } }
        public int poorid { set { _poorid = value; } get { return _poorid; } }
        public float profit { get => _profit; set => _profit = value; }
    }
}

