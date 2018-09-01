using System;
using System.Collections.Generic;

class Pentagon : Hexagon {
    public Pentagon(Point center, Icosahedron icos, List<Point> points = null) {
        this.points = new Point[5];
        CenterPoint = center;
        icosahedron = icos;

        Init(points);
        UnityEngine.Debug.Log(this.points.Length);
    }
    public Pentagon(Point center, Icosahedron icos, Point[] points = null) {
        this.points = new Point[5];
        CenterPoint = center;
        icosahedron = icos;

        Init(points);
        UnityEngine.Debug.Log(this.points.Length);
    }

    private void Init(List<Point> allPoints) {
        Point[] neighboursList = FindLinkPoints(allPoints);
        for (int i = 0; i < points.Length; i++)
        {
            if (neighboursList[i] != null)
                points[i] = neighboursList[i];
        }
    }

    private void Init(Point[] allPoints) {
        List<Point> neighboursList = FindLinkPoints(allPoints);
        for (int i = 0; i < points.Length; i++)
        {
            if (neighboursList[i] != null)
                points[i] = neighboursList[i];
        }
    }
}
