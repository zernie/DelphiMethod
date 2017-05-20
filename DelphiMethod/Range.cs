namespace DelphiMethod
{
    public struct Range
    {
        public double Start;
        public double End;

        public Range(double start, double end)
        {
            Start = start;
            End = end;
        }

        public bool Includes(double value)
        {
            return Start <= value && value <= End;
        }

        public override string ToString()
        {
            return $"{Start}...{End}";
        }
    }
}