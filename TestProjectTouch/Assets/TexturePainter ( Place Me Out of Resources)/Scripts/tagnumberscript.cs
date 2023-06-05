using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tagnumberscript : MonoBehaviour
{

    public int tagNo;
    public GameObject spriteMC;
    

    public void Activate()
    {
        spriteMC.SetActive(true);
    }

    public void DEActivate()
    {
        spriteMC.SetActive(false);
        Debug.Log("Deactive");
    }
}
