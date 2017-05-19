namespace DelphiMethod
{
    public struct Range
    {
        public decimal Start;
        public decimal End;

        public Range(decimal start, decimal end)
        {
            Start = start;
            End = end;
        }

        public bool Includes(decimal value)
        {
            return Start <= value && value <= End;
        }

        public override string ToString()
        {
            return $"{Start} ... {End}";
        }
    }
}