using System;

public class Segment
{
    public Point p1;
    public Point p2;

    public Segment(Point p1, Point p2) {
        this.p1 = p1;
        this.p2 = p2;
    }

    public bool EqualsTo(Segment seg2) {
        if ((this.p1 == seg2.p1 && this.p2 == seg2.p2) || (this.p1 == seg2.p2 && this.p2 == seg2.p1)) {
            return true;
        }
        return false;
    }
}
