using System;
using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;

class Hexagon
{
    public Point[] points;

    public Point CenterPoint;

    internal Icosahedron icosahedron;

    bool pentagon;

    public Hexagon(Point center, Icosahedron icos, List<Point> points = null )
    {
        this.points = new Point[6];
        CenterPoint = center;
        icosahedron = icos;

        Init(points);
        UnityEngine.Debug.Log(this.points.Length);

    }

    public Hexagon()
    {
        
    }

    void Init(List<Point> allPoints)
    {
        Point[] neighboursList = FindLinkPoints(allPoints);
        for (int i = 0; i < this.points.Length; i++)
        {
            if(neighboursList[i] != null)
            points[i] = neighboursList[i];
            UnityEngine.Debug.Log(points[2]);
        }
    }

    internal Point[] FindLinkPoints(List<Point> allPoints)
    {
        Point[] neighboursList = new Point[6];
        float distanceToN = icosahedron.distanceBtwSphPoints + 10000000 / icosahedron.subdividemp ;
        UnityEngine.Debug.Log(distanceToN);
        neighboursList = CenterPoint.GetClosestPoints(allPoints, icosahedron.subdividemp, distanceToN);

        return neighboursList;
    }

    internal List<Point> FindLinkPoints(Point[] allPoints)
    {
        List<Point> neighboursList = new List<Point>();
        float distanceToN = icosahedron.distanceBtwSphPoints + 200 / icosahedron.subdividemp;
        neighboursList.AddRange(CenterPoint.GetClosestPoints(allPoints, icosahedron.subdividemp, distanceToN));

        return neighboursList;
    }

    Segment[] CreateSegments()
    {
        return new Segment[2];
        for (int i = 0; i < 2; i++)
        {
            
        }
    }

    public Vector3[] GetVertices()
    {
        Vector3[] vertices = new Vector3[6];
        for (int i = 0; i < points.Length; i++)
        {
            if (points[i] != null)
            {
                vertices[i] = new Vector3(points[i].X, points[i].Y, points[i].Z);
            }
            else
            {
                vertices[i] = Vector3.zero;
            }
        } 
        return vertices;
    }
}

