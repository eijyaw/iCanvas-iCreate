using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonColourScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Material[] testMatColourArray;
    private int currentMaterial;
    public GameObject cameraRotate;
    private CameraControlTouch cTouch;
    private int colourType;
    public GameObject currentColourMarker;
    public GameObject[] buttonsArray;
    Image imageColour;
    void Start()
    {
        cTouch = cameraRotate.GetComponent<CameraControlTouch>();
        testMatColourArray[0].color = Color.HSVToRGB(0, 1, 1);
        testMatColourArray[1].color = Color.HSVToRGB(1 / 9f, 1, 1);
        testMatColourArray[2].color = Color.HSVToRGB(2 / 9f, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        imageColour = currentColourMarker.GetComponent<Image>();
        imageColour.color = testMatColourArray[currentMaterial].color;


        
        Debug.Log(testMatColourArray[currentMaterial]);
    }
    public void ChangeColour(int i)
    {
        if(cTouch.drawToggle == true)
        {
            colourType = i;
            switch (colourType)
            {
                case 0:
                    testMatColourArray[currentMaterial].color = Color.HSVToRGB(0, 1, 1);
                    break;
                case 1:
                    testMatColourArray[currentMaterial].color = Color.HSVToRGB(1 / 9f, 1, 1);
                    break;
                case 2:
                    testMatColourArray[currentMaterial].color = Color.HSVToRGB(2 / 9f, 1, 1);
                    break;
                case 3:
                    testMatColourArray[currentMaterial].color = Color.HSVToRGB(3 / 9f, 1, 1);
                    break;
                case 4:
                    testMatColourArray[currentMaterial].color = Color.HSVToRGB(4 / 9f, 1, 1);
                    break;
                case 5:
                    testMatColourArray[currentMaterial].color = Color.HSVToRGB(5 / 9f, 1, 1);
                    break;
                case 6:
                    testMatColourArray[currentMaterial].color = Color.HSVToRGB(6 / 9f, 1, 1);
                    break;
                case 7:
                    testMatColourArray[currentMaterial].color = Color.HSVToRGB(7 / 9f, 1, 1);
                    break;
                case 8:
                    testMatColourArray[currentMaterial].color = Color.HSVToRGB(8 / 9f, 1, 1);
                    break;
            }
        }
        
        Debug.Log("Pressed");
    }

    public void ChangeMaterial(int m)
    {
        currentMaterial = m;
    }
}
