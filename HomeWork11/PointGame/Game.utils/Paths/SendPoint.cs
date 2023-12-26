using System.Drawing;

namespace Game.utils.Paths
{
    public class SendPoint
    {
        public SendPoint(){}
        public SendPoint(Point point, byte a, byte r, byte g, byte b)
        {
            Point = point;
            A = a;
            R = r;
            G = g;
            B = b;
        }

        public Point Point { get; set; }
        public byte A { get; set; }
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }
    }
}
