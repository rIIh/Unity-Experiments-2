using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug : MonoBehaviour {
    [Range(1, 20)]
    [SerializeField]
    int SubDivide = 1;
    [SerializeField]
    bool Spherize = false;
    [SerializeField]
    float Radius = 1;
    public Material sprite;
    public Material defaultMaterial;
    public Vector3 translator;
    Global global;
    Dictionary<Face, GameObject> faceGo = new Dictionary<Face, GameObject>();
    public GameObject pointgo ;

    void Start() {
        global = new Global();
        Icosahedron icosahedron = new Icosahedron(global, SubDivide, Spherize, Radius);

        foreach (var point in icosahedron.HexagonPoints)
        {
            VisualizePoint(point);
            UnityEngine.Debug.Log(point);
        }

        gameObject.transform.Translate(translator);
    }

    void RenderSegments(Icosahedron icos) {
        GameObject SegmentParent = new GameObject("SegmentParent");
        SegmentParent.transform.SetParent(gameObject.transform);
        foreach (var seg in icos.global.segments)
        {
            GameObject segment = new GameObject("edge");
            segment.transform.SetParent(SegmentParent.transform);
            var lr = segment.AddComponent<LineRenderer>();
            lr.SetPosition(0, new Vector3(seg.p1.X, seg.p1.Y, seg.p1.Z));
            lr.SetPosition(1, new Vector3(seg.p2.X, seg.p2.Y, seg.p2.Z));
            lr.SetWidth(0.04f, 0.04f);
            lr.useWorldSpace = false;
            lr.material = sprite;
        }
    }

    void RenderSubFaces(Icosahedron icos) {
        GameObject FaceParent = new GameObject("SubFaceParent");
        FaceParent.transform.SetParent(gameObject.transform);

        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < icos.subdividemp * icos.subdividemp; j++)
            {
                var verticies = icos.subFaces[i][j].getVertices();

                GameObject face = new GameObject("face");
                face.transform.SetParent(FaceParent.transform);
                faceGo.Add(icos.subFaces[i][j], face);

                var meshrenderer = face.AddComponent<MeshRenderer>();
                var meshfilter = face.AddComponent<MeshFilter>();
                var mesh = new Mesh();

                meshfilter.mesh = mesh;
                mesh.vertices = verticies;
                mesh.RecalculateNormals();

                mesh.triangles = new int[3]
                {
                    0, 1, 2,
                };

                mesh.RecalculateNormals();

                meshrenderer.material = defaultMaterial;
                //face.transform.localScale = 0.95f * Vector3.one;
            }
        }

    }

    void  VisualizePoint(Point point) {

        if (point != null)
        {
            GameObject pointGO = Instantiate(pointgo, new Vector3(point.X, point.Y, point.Z), Quaternion.identity, gameObject.transform);
            pointGO.transform.LookAt(Vector3.zero);
        }
    }

    void RenderFaces(Icosahedron icos) {
        GameObject FaceParent = new GameObject("FaceParent");
        FaceParent.transform.SetParent(gameObject.transform);

        foreach (var xface in icos.global.faces)
        {
            if (xface.subdivided)
            {
                RenderSubFaces(icos);
                return;
            }

            GameObject face = new GameObject("face");
            face.transform.SetParent(FaceParent.transform);
            var meshrenderer = face.AddComponent<MeshRenderer>();
            var meshfilter = face.AddComponent<MeshFilter>();
            var mesh = new Mesh();
            meshfilter.mesh = mesh;
            mesh.vertices = xface.getVertices();
            mesh.triangles = new int[6] {0, 1, 2, 2,1,0};

            mesh.RecalculateNormals();

            meshrenderer.material = defaultMaterial;
            face.transform.localScale = 0.95f * Vector3.one;
        }
    }

    void RenderHexes(Icosahedron icosahedron) {
        GameObject FaceParent = new GameObject("FaceParent");
        FaceParent.transform.SetParent(gameObject.transform);

        foreach (var hex in icosahedron.Hexagons)
        {
            GameObject hexGO = new GameObject("hex");
            hexGO.transform.SetParent(FaceParent.transform);
            var meshrenderer = hexGO.AddComponent<MeshRenderer>();
            var meshfilter = hexGO.AddComponent<MeshFilter>();
            var mesh = new Mesh();
            meshfilter.mesh = mesh;
            mesh.vertices = hex.GetVertices();
            mesh.triangles = new int[12] { 0, 1, 2,
                                          2, 3, 4,
                                          4, 5, 0,
                                          1, 3, 5
            };

            mesh.RecalculateNormals();

            meshrenderer.material = defaultMaterial;
            hexGO.transform.localScale = 0.95f * Vector3.one;
        }
    }

    void Update() {
        //RandomFaceFlip();
        Rotating();
    }

    Vector3 Rotator;
    void Rotating() {
        Rotator.y += 10 * Time.deltaTime;
        if (Rotator.y >= 360)
        {
            Rotator.y = 0;
        }
        gameObject.transform.rotation = Quaternion.Euler(Rotator);
    }

    //Flipping things
    void RandomFaceFlip() {
        //Random flipping
        var face = global.icosahedron.subFaces[Random.Range(0, 20)][Random.Range(0, 4)];
        face.Flip();
        var mesh = faceGo[face].GetComponent<MeshFilter>().mesh;
        mesh.vertices = face.getVertices();

        faceGo[face].GetComponent<MeshFilter>().mesh.RecalculateNormals();
    }

    //Accordingly to the name
    void FlipMesh(Face face) {
        face.Flip();
        var mesh = faceGo[face].GetComponent<MeshFilter>().mesh;
        mesh.vertices = face.getVertices();
        faceGo[face].GetComponent<MeshFilter>().mesh.RecalculateNormals();
    }
}
