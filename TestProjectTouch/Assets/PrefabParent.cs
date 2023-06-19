using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabParent : MonoBehaviour
{
    public GameObject parentBrush;
    public GameObject PainterObj;
    public PaintByNumberScript01 PaintRef;
    SpriteRenderer ownsprite;
    // Start is called before the first frame update
    void Start()
    {
        parentBrush = GameObject.FindGameObjectWithTag("BrushContanner");
        PainterObj = GameObject.FindGameObjectWithTag("PainterOBJ");
        PaintRef = PainterObj.GetComponent<PaintByNumberScript01>();
        ownsprite = GetComponent<SpriteRenderer>();
        ownsprite.color = PaintRef.pBNcolour;
        parentTest();
    }
    void parentTest()
    {
        Vector3 vecZero =  Vector3.zero;
        //gameObject.transform.localPosition = vecZero;
        this.gameObject.transform.SetParent(parentBrush.transform);
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
