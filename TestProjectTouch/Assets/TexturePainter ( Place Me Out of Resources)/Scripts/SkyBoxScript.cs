using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBoxScript : MonoBehaviour
{
    public Material[] skyboxMaterials;
    public int materialCount;
    // Start is called before the first frame update
    void Start()
    {
        materialCount = 0;
        RenderSettings.skybox = skyboxMaterials[materialCount];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            materialCount += 1;
            if (materialCount <= skyboxMaterials.Length)
            {
                materialCount = 0;
            }
        }
    }
}
