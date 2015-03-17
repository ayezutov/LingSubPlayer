namespace LingSubPlayer.Common.Subtitles
{
    public struct Rect
    {
        private static readonly Rect empty = new Rect(0,0,0,0);
        public static Rect Empty
        {
            get { return empty; }
        }

        public Rect(double x1, double x2, double y1, double y2) : this()
        {
            X1 = x1;
            X2 = x2;
            Y1 = y1;
            Y2 = y2;
        }

        public double X1 { get; private set; }

        public double X2 { get; private set; }

        public double Y1 { get; private set; }

        public double Y2 { get; private set; }

        public bool Equals(Rect other)
        {
            return X1.Equals(other.X1) && X2.Equals(other.X2) && Y1.Equals(other.Y1) && Y2.Equals(other.Y2);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Rect && Equals((Rect) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X1.GetHashCode();
                hashCode = (hashCode*397) ^ X2.GetHashCode();
                hashCode = (hashCode*397) ^ Y1.GetHashCode();
                hashCode = (hashCode*397) ^ Y2.GetHashCode();
                return hashCode;
            }
        }
    }
}