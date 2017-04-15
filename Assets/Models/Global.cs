
using System;
using Boo.Lang;

class Global
{
    public static Global _Global { get; protected set; }
    public List<Segment> segments { get; protected set; }
    public List<Face> faces { get; protected set; }
    public List<Point> CenterPoints { get; protected set; }
    public Point[] PentagonPoints;
    public Icosahedron icosahedron;

    public Global()
    {
        PentagonPoints = new Point[12];
        segments = new List<Segment>();
        faces = new List<Face>();
    }

    public void AddCenterPoint(Point p)
    {
        CenterPoints.Add(p);
    }

    public void ClearSegments()
    {
        ClearSG(); //TOFIX: 31 instead of 30
        ClearSG();

        ClearFC();
    }

    void ClearSG()
    {
        var i = 0;

        while (true)
        {
            if (i == segments.Count)
            {
                break;
            }

            var j = i + 1;
            while (true)
            {
                if (j == segments.Count)
                {
                    i++;
                    break;
                }

                if (segments[i].EqualsTo(segments[j]))
                {
                    segments.RemoveAt(i);
                }
                else
                {
                    j++;
                }
            }
        }
    }

    void ClearFC()
    {
        var i = 0;

        while (true)
        {
            if (i == faces.Count)
            {
                break;
            }

            var j = i + 1;
            while (true)
            {
                if (j == faces.Count)
                {
                    i++;
                    break;
                }

                if (faces[i].EqualsTo(faces[j]))
                {
                    faces.RemoveAt(j);
                    
                }
                else
                {
                    j++;
                }
            }
        }
    }
}
