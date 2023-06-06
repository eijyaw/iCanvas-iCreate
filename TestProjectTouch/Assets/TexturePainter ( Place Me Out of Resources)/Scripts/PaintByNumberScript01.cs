using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using TouchScript;
using TouchScript.Gestures;
using TouchScript.Pointers;
using System.Collections.Generic;
public enum PainterByNumbers_BrushMode { PAINT, DECAL };
public class PaintByNumberScript01 : MonoBehaviour
{

    public GameObject[] BCList;
    public GameObject brushCursor, brushContainer; //The cursor that overlaps the model and our container for the brushes painted
    public Camera sceneCamera,canvasCam;
  //The camera that looks at the model, and the camera that looks at the canvas.
    public Sprite cursorPaint, cursorDecal; // Cursor for the differen functions 
    public RenderTexture canvasTexture; // Render Texture that looks at our Base Texture and the painted brushes
    public RenderTexture[] txtList;
    public Material baseMaterial; // The material of our base texture (Were we will save the painted texture)
    private TouchPaintAction tPA;
    public bool drawing;
    public int pointerPool;
    public int materialPool;
    //public GameObject brushC;
    public GameObject cameraRotatecontroller;
    private CameraControlTouch cTouch;
    public GameObject[] objArray;
    //public Sprite[] spritemaskList;
    public GameObject[] SMContainer;
   // public SpriteMask spriteMaskPass;
    public GameObject CurrentObj;
    public bool ButtonSelect;
    public GameObject ColouredWheel;
    public ColorPaletteController cWheelPicker;
    public Text pText;

    PainterByNumbers_BrushMode mode; //Our painter mode (Paint brushes or decals)
    float brushSize = 0.5f; //The size of our brush
    int brushCounter = 0, MAX_BRUSH_COUNT = 50000; //To avoid having millions of brushes
    bool saving = false; //Flag to check if we are saving the texture

    public Color pBNcolour;
    public Color[] pBNcolourList;
    public Button[] colourbuttonList;
    


    // Start is called before the first frame update
    void Awake()
    {
        tPA = new TouchPaintAction();
        cTouch = cameraRotatecontroller.GetComponent<CameraControlTouch>();
        cWheelPicker = ColouredWheel.GetComponent<ColorPaletteController>();
        //TouchManager.Instance.PointersPressed += DrawTest;
        for(int c = 0;c < pBNcolourList.Length; c++)
        {
            pBNcolourList[c] = colourbuttonList[c].GetComponent<Image>().color;
        }
        //TouchManager.Instance.PointersUpdated += UpdateBrushCursor;
        //TouchManager.Instance.PointersReleased += StopAction;
      
        if(ButtonSelect == true)
        {
            pBNcolour = pBNcolourList[0];
        }
        else
        {
            pBNcolour = cWheelPicker.SelectedColor;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch t1 = Input.GetTouch(0);
           
          
                DoAction(t1.position);
               
            if(t1.phase == UnityEngine.TouchPhase.Ended)
            {
                pText.text = "TestDebug";
                
                for (int f = 0; f < objArray.Length; f++)
                {
                        objArray[f].GetComponent<MeshCollider>().enabled = true;
                    
                }
                SaveTexture();
            }
                //for (int f = 0; f < objArray.Length; f++)
                //{
                //    objArray[f].GetComponent<MeshCollider>().enabled = true;

                //}
            
        }
       
            
        
        if(ButtonSelect == false)
        {
            pBNcolour = cWheelPicker.SelectedColor;
        }
        
    }
    void StopAction(object obj, PointerEventArgs eventArgs)
    {
        SaveTexture();
        drawing = false;

    }
    void DoAction(Vector3 touchPos)
    {

        if (saving)

            return;

        //Vector3 uvWorldPosition = Vector3.zero;
        if (HitTestUVPosition(ref touchPos))
        {
            GameObject brushObj;

            if (mode == PainterByNumbers_BrushMode.PAINT)
            {
        
               
                brushObj = (GameObject)Instantiate(Resources.Load("TexturePainter-Instances/BrushEntity")); //Paint a brush
                brushObj.GetComponent<SpriteRenderer>().color = pBNcolour; //Set the brush color
                
            }
            else
            {
                brushObj = (GameObject)Instantiate(Resources.Load("TexturePainter-Instances/DecalEntity")); //Paint a decal
            }

            pBNcolour.a = brushSize; /** 2.0f;*/ // Brushes have alpha to have a merging effect when painted over.
            brushObj.transform.parent = brushContainer.transform; //Add the brush to our container to be wiped later
            brushObj.transform.localPosition = touchPos; //The position of the brush (in the UVMap)
            brushObj.transform.localScale = Vector3.one * brushSize;//The size of the brush
        }
        brushCounter++; //Add to the max brushes
        if (brushCounter >= MAX_BRUSH_COUNT)
        { //If we reach the max brushes available, flatten the texture and clear the brushes
            brushCursor.SetActive(false);
            saving = true;
            Invoke("SaveTexture", 0.1f);

        }
   
    }
    public void ChangeColour(int n)
    {
        if(ButtonSelect == true)
        {
            pBNcolour = pBNcolourList[n];
        }
        
    }
    void DrawTest(object obj, PointerEventArgs eventArgs)
    {
        drawing = true;

    }
    void UpdateBrushCursor(object obj, PointerEventArgs eventArgs)
    {
       if(Input.touchCount>0)
        {
            
            //Vector3 uvWorldPosition = Vector3.zero;
            //if (HitTestUVPosition(ref uvWorldPosition) && !saving)
            //{
            //    brushCursor.SetActive(true);
            //    brushCursor.transform.position = uvWorldPosition + brushContainer.transform.position;
            //}
           
           


            //if(canvasCam.pixelRect.Contains(pointer.Position) /*&& (int)Display.RelativeMouseAt(pointer.Position).z == canvasCam.targetDisplay*/)
            //{
            //    Debug.Log(canvasCam);
            //}
        }

    }
    bool HitTestUVPosition(ref Vector3 uvWorldPosition)
    {

        RaycastHit hit;
        Vector3 cursorPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f);
        Ray cursorRay = sceneCamera.ScreenPointToRay(cursorPos);
        if (Physics.Raycast(cursorRay, out hit, 1000))
        {
            CurrentObj = hit.collider.gameObject;
            
            //sMHolders[0] = CurrentObj;

            for (int m = 0; m < objArray.Length; m++)
            {
                if (CurrentObj == objArray[m])
                {
                    SMContainer[m].GetComponent<SpriteMask>().enabled = true;
                    pText.text = SMContainer[m].ToString();
                }
                
             }

                MeshCollider meshCollider = hit.collider as MeshCollider;
            //Sprite spriteOBJTest = hit.collider as Sprite;

            if (meshCollider == null || meshCollider.sharedMesh == null)
                return false;
            Vector2 pixelUV = new Vector2(hit.textureCoord.x, hit.textureCoord.y);
            uvWorldPosition.x = pixelUV.x - canvasCam.orthographicSize;//To center the UV on X
            uvWorldPosition.y = pixelUV.y - canvasCam.orthographicSize;//To center the UV on Y
     

            //if (hit.collider)
            //{
            //    Debug.Log(hit.collider);
            //}

            for (int f = 0; f < objArray.Length; f++)
            {
                
                if (objArray[f] == CurrentObj)
                {
                    objArray[f].GetComponent<MeshCollider>().enabled = true;
                    
                }
                else
                {
                    objArray[f].GetComponent<MeshCollider>().enabled = false;
                }
            }




            return true;
        }
        else
        {
            return false;
        }

    }
    //Sets the base material with a our canvas texture, then removes all our brushes
    void SaveTexture()
    {
      
        brushCounter = 0;
        System.DateTime date = System.DateTime.Now;
        RenderTexture.active = canvasTexture;
        Texture2D tex = new Texture2D(canvasTexture.width, canvasTexture.height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, canvasTexture.width, canvasTexture.height), 0, 0);
        tex.Apply();
        RenderTexture.active = null;
        baseMaterial.mainTexture = tex; //Put the painted texture as the base
        foreach (Transform child in brushContainer.transform)
        {//Clear brushes
            Destroy(child.gameObject);
        }
        for (int m = 0; m < objArray.Length; m++)
        {
            
                SMContainer[m].GetComponent<SpriteMask>().enabled = false;
            

        }
        //StartCoroutine ("SaveTextureToFile"); //Do you want to save the texture? This is your method!
        Invoke("ShowCursor", 0.1f);
    }
    //Show again the user cursor (To avoid saving it to the texture)
    void ShowCursor()
    {
        saving = false;
    }
}
