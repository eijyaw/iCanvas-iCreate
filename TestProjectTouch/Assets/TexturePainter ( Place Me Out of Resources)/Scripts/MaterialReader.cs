using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialReader : MonoBehaviour
{
    [SerializeField] Material mat;
    public Camera cam;
    int triangleIdx;
    Mesh mesh;
    int subMeshesNr;
    int matI = -1;
    Renderer rend;
    MeshCollider meshCollider;
    Ray ray;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
