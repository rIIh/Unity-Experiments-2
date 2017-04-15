
using System;
using System.Collections.Generic;
using System.Linq;

class Icosahedron
{
    public int subdividemp;
    public bool spherize;
    public float radius = 1;

    Point[] vertices = new Point[12];
    List<Point> tempVertPoints; 
    float tao = 1.61803399f;
    public Face[] [] subFaces = new Face[20] [];

    public float distanceBtwSphPoints;

    public Global global;

    public List<Point> HexagonPoints;
    public List<Segment> HexagonSegments;
    public List<Hexagon> Hexagons;
    public Point[] PentagonCenters = new Point[12];
    public List<Pentagon> Pentagons;


    public Icosahedron(Global global, int subdividemp = 1, bool spherize = false, float radius = 1)
    {
        global.icosahedron = this;

        tempVertPoints = new List<Point>();
        vertices[0] = new Point(1, tao, 0);
        vertices[1] = new Point(-1, tao, 0);
        vertices[2] = new Point(1, -tao, 0);
        vertices[3] = new Point(-1, -tao, 0);

        vertices[4] = new Point(0, 1, tao); 
        vertices[5] = new Point(0, -1, tao);
        vertices[6] = new Point(0, 1, -tao);
        vertices[7] = new Point(0, -1, -tao);

        vertices[8] = new Point(tao, 0, 1);
        vertices[9] = new Point(-tao, 0, 1);
        vertices[10] = new Point(tao, 0, -1);
        vertices[11] = new Point(-tao, 0, -1);
        tempVertPoints.AddRange(tempVertPoints);
        this.subdividemp = subdividemp;
        this.global = global;
        for (int i = 0; i < 12; i++)
        {
            Create_Segments(vertices[i]);
        }
        CreateFacesBySegments();
        
        global.ClearSegments();

        var k = 0;
        foreach (var face in global.faces)
        {
            // UnityEngine.Debug.Log(face);
            subFaces[k] = face.Subdivide(this.subdividemp);
            k++;
        }

        return;
        this.radius = radius;
        this.spherize = spherize;
        if(spherize) Spherize();
        for (int i = 0; i < 12; i++)
        {
         //   UnityEngine.Debug.Log(global.PentagonPoints[i]);
        }

        CreateHexagons();
    }

    void CreateFacesBySegments()
    {
        foreach (var segment in global.segments)
        {
            Point[] p3s = new Point[2];
            Point[] p3s1 = GetClosestPoints(segment.p1);
            Point[] p3s2 = GetClosestPoints(segment.p2);

            var k = 0;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (p3s1[i] == p3s2[j])
                    {
                        p3s[k] = p3s1[i];
                        k++;
                    }
                }
            }
            for (int i = 0; i < 2; i++)
            {

                Face face = new Face(segment.p1, segment.p2, p3s[i]);
                global.faces.Add(face);
            }

        }
    }

    public List<Point> GetAllPoints()
    {

        List<Point> points = new List<Point>();

        if (subdividemp == 1)
        {
            points.AddRange(vertices);
            return points;
        }

        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < subdividemp*subdividemp; j++)
            {
                var p1 = subFaces[i][j].p1;
                var p2 = subFaces[i][j].p2;
                var p3 = subFaces[i][j].p3;
                if (!points.Contains(p1)) points.Add(p1);
                if (!points.Contains(p2)) points.Add(p2);
                if (!points.Contains(p3)) points.Add(p3);
            }
        }

        var k = 0;
        while (true)
        {
            if(k == points.Count) break;
            var n = k + 1;
            while (true)
            {
                if (n == points.Count)
                {
                    k++;
                    break;
                }

                if (points[k].EqualsTo(points[n]))
                {
                    points.RemoveAt(n);

                }
                else
                {
                    n++;
                }
            }
        }

        return points;
    }

    Point[] GetClosestPoints(Point p)
    {
        Point[] points = new Point[5];
        var i = 0;
        foreach (var point in vertices)
        {
            var magnitude = (p.X - point.X)*(p.X - point.X) + (p.Y - point.Y)*(p.Y - point.Y) + (p.Z - point.Z)*(p.Z - point.Z);
            if (
                magnitude <= 4 && magnitude > 0.00001

                )

            {
                points[i] = point;
                i++;
            }
        }
        return points;
    }

    void Create_Segments(Point point)
    {
        Point[] points = GetClosestPoints(point);
        foreach (var neighbour in points)
        {
            Segment segment = new Segment(point, neighbour);
            if (global.segments.Contains(segment) || neighbour == null)
            {
                continue;
            }
            global.segments.Add(segment);
        }


    }

    void Spherize()
    {
        var k = 0;
        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < subdividemp*subdividemp; j++)
            {
                var p1 = subFaces[i][j].p1;
                var p2 = subFaces[i][j].p2;
                var p3 = subFaces[i][j].p3;

                var mag1 = (float)Math.Sqrt(p1.X * p1.X + p1.Y * p1.Y + p1.Z * p1.Z);
                var mag2 = (float)Math.Sqrt(p2.X * p2.X + p2.Y * p2.Y + p2.Z * p2.Z);
                var mag3 = (float)Math.Sqrt(p3.X * p3.X + p3.Y * p3.Y + p3.Z * p3.Z);
                // UnityEngine.Debug.Log(mag1 + ", " + mag2 + ", " + mag3);

                p1.X *= radius / mag1; p1.Y *= radius / mag1; p1.Z *= radius / mag1;
                
               
                p2.X *= radius / mag2; p2.Y *= radius / mag2; p2.Z *= radius / mag2;
                
           
                p3.X *= radius / mag3; p3.Y *= radius / mag3; p3.Z *= radius / mag3;

                if(j == 0 && i == 0)
                distanceBtwSphPoints = p1.DistanceTo(p2);
            }
        }
    }


        void CreateHexagons()
        {
        HexagonPoints = new List<Point>();
        HexagonSegments = new List<Segment>();
        Hexagons = new List<Hexagon>();
        PentagonCenters = GetRadiusizedVertices();
        List<Point> allPoints = GetAllPoints();

        foreach (var pentagonCenter in PentagonCenters)
        {
            Pentagon pent = new Pentagon(pentagonCenter, this, allPoints);
            Hexagons.Add(pent);
            HexagonPoints.AddRange(pent.points);
        }
        
        
        foreach (var point in allPoints)
        {
            if (!point.EqualsToAny(PentagonCenters) && !point.EqualsToAny(HexagonPoints))
            {
                Hexagon hex = new Hexagon(point, this, allPoints);
                Hexagons.Add(hex);
                HexagonPoints.AddRange(hex.points);
                UnityEngine.Debug.Log(hex.points[5]);
            }
        }
    }

    Point[] GetRadiusizedVertices()
    {
        Point[] newVertices = new Point[12];
        
        for (int i = 0; i < 12; i++)
        {
            Point vertice = vertices[i];
            float mag = (float)Math.Sqrt(vertice.X * vertice.X + vertice.Y * vertice.Y + vertice.Z * vertice.Z);
            newVertices[i] = new Point(vertice.X * radius / mag, vertice.Y * radius / mag, vertice.Z * radius / mag);
        }
        return newVertices;
    }
}

