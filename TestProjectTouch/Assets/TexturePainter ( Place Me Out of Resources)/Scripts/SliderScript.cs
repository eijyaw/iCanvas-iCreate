using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    public Material TestMatColourA, TestMatColourB, TestMatColourC;
    private float hUeA, hUeB,hUeC;
    public Slider m1_SliderHue, m2_SliderHue, m3_SliderHue;
    // Start is called before the first frame update
    void Start()
    {
        m1_SliderHue.maxValue = 1;
        m1_SliderHue.minValue = 0;


        m2_SliderHue.maxValue = 1;
        m2_SliderHue.minValue = 0;

        m3_SliderHue.maxValue = 1;
        m3_SliderHue.minValue = 0;

        m1_SliderHue.value = 0;
        m2_SliderHue.value = 0.25f;
        m1_SliderHue.value = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {

        ColourControl();
       
    }

    void ColourControl()
    {
        hUeA = m1_SliderHue.value;
        hUeB = m2_SliderHue.value;
        hUeC = m3_SliderHue.value;



        TestMatColourA.color = Color.HSVToRGB(hUeA, 0.75f, 0.75f);
        TestMatColourB.color = Color.HSVToRGB(hUeB, 0.75f, 0.75f);
        TestMatColourC.color = Color.HSVToRGB(hUeC, 0.75f, 0.75f);
    }
}
