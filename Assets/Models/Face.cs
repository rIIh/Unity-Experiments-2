using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;

class Face {
    public Point p1 { get; protected set; }
    public Point p2 { get; protected set; }
    public Point p3 { get; protected set; }
    public bool subdivided;

    public Face(Point[] ps) {
        p1 = ps[0];
        p2 = ps[1];
        p3 = ps[2];
    }

    public Vector3[] getVertices() {
        Vector3[] vertices = new Vector3[3];
        vertices[0] = new Vector3(p1.X, p1.Y, p1.Z);
        vertices[1] = new Vector3(p2.X, p2.Y, p2.Z);
        vertices[2] = new Vector3(p3.X, p3.Y, p3.Z);
        return vertices;
    }

    public Face(Point p1, Point p2, Point p3) {
        this.p1 = p1;
        this.p2 = p2;
        this.p3 = p3;
    }

    public bool EqualsTo(Face face) {
        Point[] facePoints1 = new Point[3] {
            p1, p2, p3
        };
        Point[] facePoints2 = new Point[3] {
            face.p1, face.p2, face.p3
        };

        bool bool1 = false;
        bool bool2 = false;
        bool bool3 = false;
        int i1 = new int();
        int i2 = new int();

        for (int i = 0; i < 3; i++) {
            if (facePoints1[0] == facePoints2[i]) {
                bool1 = true;
                i1 = i;
                break;
            }
        }
        for (int i = 0; i < 3; i++) {
            if (facePoints1[1] == facePoints2[i] && i != i1) {
                bool2 = true;
                i2 = i;
                break;
            }
        }
        for (int i = 0; i < 3; i++) {
            if (facePoints1[2] == facePoints2[i] && i != i1 && i != i2) {
                bool3 = true;
                break;
            }
        }

        if (bool1 & bool2 & bool3) {
            return true;
        }
        return false;

    }

    public Face[] Subdivide(int n) {
        if (n < 2) return null;
        subdivided = true;

        Point[] side12 = new Point[n + 1];
        side12[0] = p1;
        side12[n] = p2;

        Point[] side13 = new Point[n + 1];
        side13[0] = p1;
        side13[n] = p3;

        Point[,] span = new Point[n + 1, n];

        for (int i = 1; i < n; i++) {
            side12[i] = new Point(
                    p1.X + (i / (float)n) * (p2.X - p1.X),
                    p1.Y + (i / (float)n) * (p2.Y - p1.Y),
                    p1.Z + (i / (float)n) * (p2.Z - p1.Z)
                );
            side13[i] = new Point(
                    p1.X + (i / (float)n) * (p3.X - p1.X),
                    p1.Y + (i / (float)n) * (p3.Y - p1.Y),
                    p1.Z + (i / (float)n) * (p3.Z - p1.Z)
                );
        }

        for (int i = 0; i <n + 1; i++) {
            for (int j = 1; j < n - i; j++){
                span[i,j] = new Point(
                    side12[n - i].X + (j /  ((float)n - i)) * (side13[n - i].X - side12[n - i].X),
                    side12[n - i].Y + (j /  ((float)n - i)) * (side13[n - i].Y - side12[n - i].Y),
                    side12[n - i].Z + (j /  ((float)n - i)) * (side13[n - i].Z - side12[n - i].Z)
                );
            }
        }

        Face[] faces = new Face[n*n];
        Point[] ps = new Point[3];
        int nfaces = 0;

        //top four subfaces
        ps[0] = side12[0];
        ps[1] = side13[1];
        ps[2] = side12[1];
        faces[nfaces] = new Face(ps);
        nfaces++;

        ps[0] = side12[1];
        ps[1] = side12[2];
        ps[2] = span[n - 2, 1];
        faces[nfaces] = new Face(ps);
        nfaces++;

        ps[0] = side12[1];
        ps[1] = span[n - 2, 1];
        ps[2] = side13[1];
        faces[nfaces] = new Face(ps);
        nfaces++;

        ps[0] = side13[1];
        ps[1] = side13[2];
        ps[2] = span[n - 2, 1];
        faces[nfaces] = new Face(ps);
        nfaces++;

        //the rest of the subfaces
        for (int i = 2; i < n; i++) {
            //side 2
            ps[0] = side12[i];
            ps[1] = side12[i + 1];
            ps[2] = span[n - i - 1, 1];
            faces[nfaces] = new Face(ps);

            nfaces++;
            ps[0] = side12[i];
            ps[1] = span[n - i, 1];
            ps[2] = span[n - i - 1, 1];
            faces[nfaces] = new Face(ps);
            nfaces++;
            //center
            for (int j = 1; j < n - i + 1; j++)
            {
                ps[0] = span[i - 2, j + 1];
                ps[1] = span[i - 2, j];
                ps[2] = span[i - 1, j];
                faces[nfaces] = new Face(ps);
                nfaces++;
                if (i > 2)
                {
                    ps[0] = span[i - 2, j + 1];
                    ps[1] = span[i - 2, j];
                    ps[2] = span[i - 3, j + 1];
                    faces[nfaces] = new Face(ps);
                    nfaces++;
                }
            }
            //side 2
            ps[0] = side13[i]; ps[1] = side13[i + 1]; ps[2] = span[n - i - 1, i];
            faces[nfaces] = new Face(ps); nfaces++;
            ps[0] = side13[i]; ps[1] = span[n - i, i - 1]; ps[2] = span[n - i - 1, i];
            faces[nfaces] = new Face(ps); nfaces++;
        }

        //some crap here
        //orient according to a given point
        for (int i = 0; i < nfaces; i++) {
            faces[i].Orient(faces[i].p1);
        }

        return faces;
    }

    //Some flippy-vippy things
    public void Flip() {
        var temp = p2;
        p2 = p3;
        p3 = temp;
    }

    Point GetNormal() {
        Point normal = new Point();
        Point v1 = new Point();
        Point v2 = new Point();

        v1.X = p2.X - p1.X;
        v1.Y = p2.Y - p1.Y;
        v1.Z = p2.Z - p1.Z;

        v2.X = p3.X - p2.X;
        v2.Y = p3.Y - p2.Y;
        v2.Z = p3.Z - p2.Z;

        normal.X = v1.Y*v2.Z - v1.Z*v2.Y;
        normal.Y = v1.Z*v2.X - v1.X*v2.Z;
        normal.Z = v1.X*v2.Y - v1.Y*v2.X;

        return normal;
    }

    void Orient(Point p) {
        if (get_point_side(p) > 0)
        {
            Flip();
        }
    }

    int get_point_side(Point p) {
        float side = dot_product_1p(GetNormal(), p) - get_d();
        if (side < 0) { return 1; }
        if (side > 0) { return -1; }
        return 0;
    }

    float get_d() {
        float d = -dot_product_1p(GetNormal(), p1);
        return d;
    }

    float dot_product_1p(Point v1, Point p2) {
        return (v1.X *p2.X)+(v1.Y *p2.Y)+(v1.Z *p2.Z);
    }
}
