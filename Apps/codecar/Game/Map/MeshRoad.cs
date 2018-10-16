using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class MeshRoad : MonoBehaviour
{
    public GameObject objGame;
    public Camera mainCam;
    private Mesh mesh;
    MeshRenderer meshRender;
    Material mat;
    private Vector3[] vertices;
    private int[] triangles;
    public List<Vector3> listPoint;
    public float roadWidth = 2f;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        listPoint = new List<Vector3>();
        mesh = GetComponent<MeshFilter>().mesh;
        meshRender = GetComponent<MeshRenderer>();

        string strshader = "Custom/ShaderRoad";
        Material mat = new Material(Shader.Find(strshader));

        string pic = ResMap.IMAGE_TILE;
        Texture2D tex = (Texture2D)Resources.Load(pic);
        mat.SetTexture("_MainTex", tex);
        meshRender.material = mat;

    }
    // Use this for initialization
    void Start()
    {

        // int count = 4;
        // for (int i = 0; i < count; i++)
        // {
        //     AddPoint(new Vector3(roadWidth * i, 0, 0));
        // }


        // Draw();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void AddPoint(Vector3 vec)
    {
        if (listPoint == null)
        {
            Debug.Log("AddPoint:listPoint=null");
            return;
        }

        listPoint.Add(vec);


    }

    Vector3[] GetverticeOfPoint(Vector2 pt)
    {
        int count = 4;
        float z = 0f;
        Vector3[] v = new Vector3[count];

        //left_bottom
        v[0] = new Vector3(pt.x - roadWidth / 2, pt.y - roadWidth / 2, z);

        //right_bottom
        v[1] = new Vector3(pt.x + roadWidth / 2, pt.y - roadWidth / 2, z);
        //top_left
        v[2] = new Vector3(pt.x - roadWidth / 2, pt.y + roadWidth / 2, z);
        //top_right
        v[3] = new Vector3(pt.x + roadWidth / 2, pt.y + roadWidth / 2, z);

        // for (int i = 0; i < count; i++)
        // {
        //     Vector2 a = points[i + 0].ToXZVector2();
        //     Vector2 b = (connectEnds && i == segments - 1) ? points[0].ToXZVector2() : points[i + 1].ToXZVector2();

        //     bool flip = (a.x > b.x);// ? theta[i] : -theta[i];

        //     Vector3 rght = flip ? new Vector3(0, 0, -1) : new Vector3(0, 0, 1);
        //     Vector3 lft = flip ? new Vector3(0, 0, 1) : new Vector3(0, 0, -1);

        //     theta[i] = RoadMath.AngleRadian(a, b);

        //     // seg a
        //     v.Add(points[i] + rght * roadWidth);
        //     v.Add(points[i] + lft * roadWidth);
        //     // seg b
        //     int u = (connectEnds && i == segments - 1) ? 0 : i + 1;
        //     v.Add(points[u] + rght * roadWidth);
        //     v.Add(points[u] + lft * roadWidth);

        //     // apply angular rotation to points
        //     int l = v.Count - 4;

        //     v[l + 0] = v[l + 0].RotateAroundPoint(points[i + 0], -theta[i]);
        //     v[l + 1] = v[l + 1].RotateAroundPoint(points[i + 0], -theta[i]);

        //     v[l + 2] = v[l + 2].RotateAroundPoint(points[u], -theta[i]);
        //     v[l + 3] = v[l + 3].RotateAroundPoint(points[u], -theta[i]);

        //     t.AddRange(new int[6]{
        //         tri_index + 2,
        //         tri_index + 1,
        //         tri_index + 0,

        //         tri_index + 2,
        //         tri_index + 3,
        //         tri_index + 1
        //         });

        //     tri_index += 4;
        // }

        return v;
    }
    public void Draw()
    {

        if (listPoint == null)
        {
            return;
        }

        mesh.Clear();
        int count = listPoint.Count;
        vertices = new Vector3[count * 4];
        triangles = new int[count * 6];
        Vector2[] uvs = new Vector2[count * 4];
        int tri_index = 0;
        for (int i = 0; i < listPoint.Count; i++)
        {
            Vector3[] v = GetverticeOfPoint(listPoint[i]);

            for (int j = 0; j < 4; j++)
            {
                vertices[i * 4 + j] = v[j];
            }


            //纹理坐标
            {
                //left_bottom 
                uvs[i * 4 + 0] = new Vector2(0f, 0f);

                //right_bottom
                uvs[i * 4 + 1] = new Vector2(1f, 0f);
                //top_left
                uvs[i * 4 + 2] = new Vector2(0f, 1f);
                //top_right
                uvs[i * 4 + 3] = new Vector2(1f, 1f);

            }

            int idx = 0;
            //三角型1
            {
                //top_left
                idx = i * 6 + 0;
                triangles[idx] = tri_index + 2;
                //right_bottom
                idx = i * 6 + 1;
                triangles[idx] = tri_index + 1;
                //left_bottom
                idx = i * 6 + 2;
                triangles[idx] = tri_index + 0;
            }

            //三角型2
            {
                //top_left
                idx = i * 6 + 3;
                triangles[idx] = tri_index + 2;
                //top_right
                idx = i * 6 + 4;
                triangles[idx] = tri_index + 3;
                //bottom_right
                idx = i * 6 + 5;
                triangles[idx] = tri_index + 1;
            }



            tri_index += 4;
        }



        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
    }
}
