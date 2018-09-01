using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Point {
    private float x;
    private float y;
    private float z;

    public float X
    {
        get { return x; }
        set { x = value; }
    }
    public float Y
    {
        get { return y; }
        set { y = value; }
    }
    public float Z
    {
        get { return z; }
        set { z = value; }
    }


    public Point(float x = 0, float y = 0, float z = 0) {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public float DistanceTo(Point point) {
        var dist =  (x - point.X) * (x - point.X) +
                    (y - point.Y) * (y - point.Y) +
                    (z - point.Z) * (z - point.Z);
        return dist;
    }

    public bool EqualsTo(Point point) {
        if (point == null) {
            UnityEngine.Debug.LogError("Target point is null");
            return false;
        }
        if (X == point.X && y == point.Y && z == point.Z) {
            return true;
        }
        if (Math.Abs(X - point.X) < 0.0001f && Math.Abs(Y - point.Y) < 0.0001f && Math.Abs(Z - point.Z) < 0.0001f) {
            return true;
        }
        return false;
    }

    public bool EqualsToAny(List<Point> points) {
        if (points == null) {
            UnityEngine.Debug.LogError("List is null");
            return false;
        }

        bool temp = false;
        for (int j = 0; j < points.Count; j++) {
            if (points[j] != null && EqualsTo(points[j]))
            {
                temp = true;
                break;
            }
        }
        return temp;
    }

    public bool EqualsToAny(Point[] points) {
        if (points == null) {
            UnityEngine.Debug.LogError("Array is null");
            return false;
        }

        bool temp = false;
        foreach (var tpoint in points) {
            if (EqualsTo(tpoint)) {
                temp = true;
                break;
            }
        }
        return temp;
    }

    public List<Point> RemovePointFrom(List<Point> points) {
        foreach (var point in points) {
            if (EqualsTo(point)) {
                points.Remove(point);
                break;
            }
        }
        return points;
    }

    public Point[] GetClosestPoints(List<Point> allPoints, int subdivLevel, float customMagnitude = 0) {
        Point[] points = new Point[6];
        var i = 0;
        float distance;
        if (customMagnitude == 0) {
            distance = 4 / (subdivLevel * subdivLevel);
        }
        else {
            distance = customMagnitude;
        }
        foreach (var point in allPoints) {
            var magnitude = (X - point.X) * (X - point.X) +
                            (Y - point.Y) * (Y - point.Y) +
                            (Z - point.Z) * (Z - point.Z);
            if (magnitude <= distance && magnitude > 0.001) {
                UnityEngine.Debug.Log(Mathf.Sqrt(magnitude) + " <= " + Mathf.Sqrt(distance));
                points[i] = point;
                i++;
            }
            if(i == 6) break;
        }
        return points;
    }
    public Point[] GetClosestPoints(Point[] allPoints, int subdivLevel, float customMagnitude = 0) {
        Point[] points = new Point[6];
        var i = 0;
        float distance;
        if (customMagnitude == 0) {
            distance = 4/(subdivLevel*subdivLevel);
        }
        else {
            distance = customMagnitude;
        }
        foreach (var point in allPoints) {
            //Yeah do reuse they told
            var magnitude =    (X - point.X)*(X - point.X) +
                                (Y - point.Y)*(Y - point.Y) +
                                (Z - point.Z)*(Z - point.Z);

            if ( magnitude <= distance && magnitude > 0.001 ) {
                UnityEngine.Debug.Log(Mathf.Sqrt(magnitude) + " <= " + Mathf.Sqrt(distance));

                points[i] = point;
                i++;
            }
        }
        return points;
    }
}
