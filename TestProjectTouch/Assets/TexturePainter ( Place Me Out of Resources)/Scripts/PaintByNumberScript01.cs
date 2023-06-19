using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using TouchScript;
using TouchScript.Gestures;
using TouchScript.Pointers;
using System.Collections.Generic;
using Photon.Pun;
public enum PainterByNumbers_BrushMode { PAINT, DECAL };
public class PaintByNumberScript01 : MonoBehaviour
{

    public GameObject[] BCList;
    public GameObject brushCursor, brushContainer, crayon; //The cursor that overlaps the model and our container for the brushes painted
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
    public SpriteMask spriteMaskPass;
    public GameObject CurrentObj;
    public GameObject CurrentSpiteObj;
    public bool ButtonSelect;
    public GameObject ColouredWheel;
    public ColorPaletteController cWheelPicker;
    public Text pText;
    public Material savedMat;
    public Vector3 mousePosStorage;
    public GameObject brushObj;
    Vector2 SwitchV2;
    public int sMode;
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
        switch (sMode)
        {
            case 0:
                SwitchV2 = Vector2.up;
                break;
            case 1:
                SwitchV2 = Vector2.down;
                break;
            case 2:
                SwitchV2 = Vector2.left;
                break;
            case 3:
                SwitchV2 = Vector2.right;
                break;
            case 4:
                SwitchV2 = Vector2.zero;
                break;
        }
        if (Input.touchCount > 0)
        {
            Touch t1 = Input.GetTouch(0);
            Vector2 t1pos = sceneCamera.ScreenToWorldPoint(t1.position);
            Vector2 t1pos2 = sceneCamera.ViewportToWorldPoint(t1.position);
            //DoAction(t1.position);
            PhotonView photonView = PhotonView.Get(this);


            photonView.RPC("PassPosData", RpcTarget.All,t1.deltaPosition);
            photonView.RPC("TrackMouse", RpcTarget.All, mousePosStorage);
            photonView.RPC("DoAction", RpcTarget.All, mousePosStorage);
            //DoAction(mousePosStorage);
            //photonView.RPC("DoAction", RpcTarget.All);
            photonView.RPC("UpdateBrushCursor", RpcTarget.All, mousePosStorage);
            if (t1.phase == UnityEngine.TouchPhase.Ended)
            {

                brushCursor.SetActive(false);
                //SaveTexture();
                photonView.RPC("SaveDelayCall", RpcTarget.All,0.2f);
                //saving = true;
                //Invoke("SaveTexture", 0.1f);
            }


        }
        //Vector2 camrect = sceneCamera.transform.InverseTransformPoint(Input.mousePosition);
        int playercount = PhotonNetwork.PlayerList.Length;

        if (Input.GetMouseButton(0) /*&& sceneCamera.rect.Contains(camrect)*/)
        {
            PhotonView photonView = PhotonView.Get(this);
            mousePosStorage = new Vector2(Input.mousePosition.x, Input.mousePosition.y);


            photonView.RPC("TrackMouse", RpcTarget.All, mousePosStorage);
            photonView.RPC("DoAction", RpcTarget.All, mousePosStorage);

            //DoAction(mousePosStorage);
            //photonView.RPC("DoAction", RpcTarget.All);
            photonView.RPC("UpdateBrushCursor", RpcTarget.All, mousePosStorage);
        }
        //{
        //    PhotonView photonView = PhotonView.Get(this);
        //    mousePosStorage = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 2f);
  

        //    photonView.RPC("TrackMouse", RpcTarget.All, mousePosStorage);
        //    photonView.RPC("DoAction", RpcTarget.All);
        //}
        if (Input.GetKey(KeyCode.X))
        {
            brushCursor.SetActive(false);
            Invoke("SaveTexture", 0.2f);
        }
        if (Input.GetKeyUp(KeyCode.V))
        {
            sMode += 1;
           
            if(sMode >= 5)
            {
                sMode = 0;
            }
      
        }
        if (ButtonSelect == false)
        {
            pBNcolour = cWheelPicker.SelectedColor;
        }
        
    }
    void StopAction(object obj, PointerEventArgs eventArgs)
    {
        SaveTexture();
        drawing = false;

    }
    [PunRPC]
    void SaveDelayCall(float delaytime)
    {
        brushCursor.SetActive(false);
        StartCoroutine("SaveTxTimer",delaytime);
    }
    [PunRPC]
    void TrackMouse(Vector3 TouchTrack)
    {
        //pText.text = TouchTrack.ToString();
    }
    [PunRPC]
    void SetMask(int f)
    {
        SMContainer[f].GetComponent<SpriteMask>().enabled = true;
        //pText.text = SMContainer[f].ToString();
    }
    [PunRPC]
    void PassPosData(Vector2 inputData)
    {
        mousePosStorage = inputData;
        //pText.text = SMContainer[f].ToString();
    }
    [PunRPC]
    public void ChangeColour(int h)
    {
        pBNcolour = pBNcolourList[h];
        //pText.text = "Colour";
        crayon.GetComponent<SpriteRenderer>().color = pBNcolourList[h];
        brushCursor.GetComponent<SpriteRenderer>().color = pBNcolourList[h];
    }
    [PunRPC]
    void DoAction(Vector3 touchPos)
    {
        PhotonView photonView = PhotonView.Get(this);
        if (saving)

            return;

       Vector3 uvWorldPosition = Vector3.zero;
       
        if (HitTestUVPosition(ref uvWorldPosition))
        {

            GameObject brushObjC;
            
            if (mode == PainterByNumbers_BrushMode.PAINT)
            {


                //brushObjC = PhotonNetwork.Instantiate(brushObj.name,touchPos,Quaternion.identity); //Paint a brush
                brushObjC = PhotonNetwork.Instantiate(brushObj.name,touchPos,Quaternion.identity);
                brushObjC.transform.parent = brushContainer.transform;
                pBNcolour.a = brushSize;
                //pText.text = pBNcolour.ToString();

            }
            else
            {
                brushObjC = (GameObject)Instantiate(Resources.Load("TexturePainter-Instances/DecalEntity")); //Paint a decal
            }
            brushObjC.transform.parent = brushContainer.transform; //Add the brush to our container to be wiped later
            /*brushObjC.transform.localPosition*/
            brushObjC.transform.localPosition =   uvWorldPosition; //The position of the brush (in the UVMap)
            /*brushObjC.transform.localScale*/
            brushObjC.transform.localScale = Vector3.one * brushSize;//The size of the brush
            

            //if (brushObjC == null)
            //    return;

             /** 2.0f;*/ // Brushes have alpha to have a merging effect when painted over.

            

        }
        brushCounter++; //Add to the max brushes
        if (brushCounter >= MAX_BRUSH_COUNT)
        { //If we reach the max brushes available, flatten the texture and clear the brushes
            brushCursor.SetActive(false);
            saving = true;
            Invoke("SaveTexture", 0.1f);

        }
   
    }
    public void ChangeColourB(int n)
    {
        if(ButtonSelect == true)
        {
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("ChangeColour", RpcTarget.All, n);
            //pBNcolour = pBNcolourList[n];
            //crayon.GetComponent<SpriteRenderer>().color = pBNcolourList[n];
            //brushCursor.GetComponent<SpriteRenderer>().color = pBNcolourList[n];

        }
        
    }
    void DrawTest(object obj, PointerEventArgs eventArgs)
    {
        drawing = true;

    }
    [PunRPC]
    void UpdateBrushCursor( Vector3 touchPos)
    {
        if (HitTestUVPosition(ref touchPos) && !saving)
        {
            brushCursor.SetActive(true);
            brushCursor.transform.position = touchPos + brushContainer.transform.position;
        }
        else
        {
            brushCursor.SetActive(false);
        }

    }
    [PunRPC]
    bool HitTestUVPosition(ref Vector3 uvWorldPosition)
    {
        
        RaycastHit hit;
        Vector3 cursorPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f);
        Ray cursorRay = sceneCamera.ScreenPointToRay(mousePosStorage);
        pText.text = cursorRay.origin.ToString();
        if (Physics.Raycast(cursorRay, out hit, 1000))
        {
         

                MeshCollider meshCollider = hit.collider as MeshCollider;
            //Sprite spriteOBJTest = hit.collider as Sprite;

            if (meshCollider == null || meshCollider.sharedMesh == null)
                return false;
            Vector2 pixelUV = new Vector2(hit.textureCoord.x, hit.textureCoord.y);
            uvWorldPosition.x = pixelUV.x - canvasCam.orthographicSize;//To center the UV on X
            uvWorldPosition.y = pixelUV.y - canvasCam.orthographicSize;//To center the UV on Y

            RaycastHit2D hit2D = Physics2D.Raycast(uvWorldPosition, SwitchV2);
            //pText.text = uvWorldPosition.ToString();
            //Debug.Log(hit2D.collider.ToString());

            if (hit2D.collider != null)
            {
                CurrentSpiteObj = hit2D.collider.gameObject;
                //pText.text = CurrentSpiteObj.ToString();
                for (int f = 0; f < SMContainer.Length; f++)
                {
                    if(CurrentSpiteObj == SMContainer[f] )
                    {
                        SMContainer[f].GetComponent<SpriteMask>().enabled = true;
                        PhotonView photonView = PhotonView.Get(this);
                        SpriteMask spmask;
                        spmask = SMContainer[f].GetComponent<SpriteMask>();
                        //spmask.enabled = true;
                        //pText.text = spmask.ToString();
                        photonView.RPC("SetMask", RpcTarget.All, f);

                    }
                    else
                    {
                        SMContainer[f].GetComponent<PolygonCollider2D>().enabled = false;
                    }
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
    [PunRPC]
    void SaveTexture()
    {
        pText.text = "saving";
        brushCounter = 0;
        //brushCursor.SetActive(false);
        System.DateTime date = System.DateTime.Now;
        RenderTexture.active = canvasTexture;
        Texture2D tex = new Texture2D(canvasTexture.width, canvasTexture.height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, canvasTexture.width, canvasTexture.height), 0, 0);
        tex.Apply();
        RenderTexture.active = null;
        baseMaterial.mainTexture = tex; //Put the painted texture as the base
        savedMat = baseMaterial;
        foreach (Transform child in brushContainer.transform)
        {//Clear brushes
            //PhotonView photonView = PhotonView.Get(this);
            //PhotonNetwork.Destroy(child.gameObject);
            Destroy(child.gameObject);
        }
        for (int f = 0; f < SMContainer.Length; f++)
        {
          
          
                SMContainer[f].GetComponent<PolygonCollider2D>().enabled = true;
            SMContainer[f].GetComponent<SpriteMask>().enabled = false;
            //pText.text = "Colliders Enabled";
        }
        CurrentSpiteObj = null;


        //StartCoroutine ("SaveTextureToFile"); //Do you want to save the texture? This is your method!
        Invoke("ShowCursor", 0.1f);
    }
    //Show again the user cursor (To avoid saving it to the texture)
    void ShowCursor()
    {
        ////pText.text = "Saving";
        saving = false;
    }

    IEnumerator SaveTxTimer(float inputTime)
    {
        yield return new WaitForSeconds(inputTime);
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("SaveTexture", RpcTarget.All);
    }
}
