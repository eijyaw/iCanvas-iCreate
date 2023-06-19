using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : MonoBehaviour
{
    public GameObject[] TestScenes;
    // Start is called before the first frame update

    
    public void PaintScene()
    {
        TestScenes[0].SetActive(false);
        TestScenes[1].SetActive(true);
        
    }

    public void ThreeDeeScene()
    {
        TestScenes[0].SetActive(true);
        TestScenes[1].SetActive(false);
    }
}
