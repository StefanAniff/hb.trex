namespace Trex.Server.Reports
{
    public class BooleanSourceObject
    {
        private readonly bool[] _values = new[] {true, false};

        public bool[] Values
        {
            get { return _values; }
        }
    }
}