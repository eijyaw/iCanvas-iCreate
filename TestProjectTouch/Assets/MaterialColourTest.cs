using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialColourTest : MonoBehaviour
{
    public Material SlotA;
    public Material SlotB;
    public Material SlotC;
   // public GameObject TestObject;
    int ColourList;
    int ColourListB;
    int ColourListC;
   // private bool Parented;

    private void Start()
    {
        ColourList = 0;
        ColourListB = 0;
        ColourListC = 0;
        SlotA.color = Color.white;
      //  Parented = false;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            Changecolour();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            ChangecolourB();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            ChangecolourC();
        }

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    if(Parented == false)
        //    {
        //        TestObject.transform.SetParent(this.gameObject.transform,false);
                
        //        print("Parented");
        //    }
        //    else
        //    {
        //        TestObject.transform.SetParent(null);
        //    }
            
        //}
    }
    void Changecolour()
    {
        ColourList += 1;
        if(ColourList > 2)
        {
            ColourList = 0;
        }
        switch (ColourList)
        {
            case 0:
                SlotA.color = Color.white;
                break;
            case 1:
                SlotA.color = Color.red;
                break;
            case 2:
                SlotA.color = Color.green;
                break;
        }
        
    }
    void ChangecolourB()
    {
        ColourListB += 1;
        if (ColourListB > 2)
        {
            ColourListB = 0;
        }
        switch (ColourListB)
        {
            case 0:
                SlotB.color = Color.white;
                break;
            case 1:
                SlotB.color = Color.red;
                break;
            case 2:
                SlotB.color = Color.green;
                break;
        }

    }
    void ChangecolourC()
    {
        ColourListC += 1;
        if (ColourListC > 2)
        {
            ColourListC = 0;
        }
        switch (ColourListC)
        {
            case 0:
                SlotC.color = Color.white;
                break;
            case 1:
                SlotC.color = Color.red;
                break;
            case 2:
                SlotC.color = Color.green;
                break;
        }

    }
}
