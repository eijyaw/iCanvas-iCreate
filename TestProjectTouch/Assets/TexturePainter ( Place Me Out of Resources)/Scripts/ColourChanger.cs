using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;
using TouchScript;
using TouchScript.Gestures;
using TouchScript.Pointers;
public enum TimerTouch_BrushMode { PAINT, DECAL };
public class ColourChanger : MonoBehaviour
{
    public GameObject brushCursor, brushContainer; //The cursor that overlaps the model and our container for the brushes painted
    public Camera sceneCamera, canvasCam;  //The camera that looks at the model, and the camera that looks at the canvas.
    public Sprite cursorPaint, cursorDecal; // Cursor for the differen functions 
    public RenderTexture canvasTexture; // Render Texture that looks at our Base Texture and the painted brushes
    public Material baseMaterial; // The material of our base texture (Were we will save the painted texture)
    private TouchPaintAction tPA;
    public bool drawing;
    public int pointerPool;
    public int materialPool;
    //public GameObject brushC;
    public GameObject cameraRotatecontroller;
    private CameraControlTouch cTouch;

    TimerTouch_BrushMode mode; //Our painter mode (Paint brushes or decals)
    float brushSize = 0.2f; //The size of our brush
    Color brushColor; //The selected color
    int brushCounter = 0, MAX_BRUSH_COUNT = 50000; //To avoid having millions of brushes
    bool saving = false; //Flag to check if we are saving the texture

    //public Material testmaterial;
    Color[] colourList;
    int colourCount;
    public float countDownTimer;
    public Image[] colourcounter;
    bool countingDown;
    float timeRemains;
    public Text timerText;

    private void Awake()
    {
        tPA = new TouchPaintAction();
        cTouch = cameraRotatecontroller.GetComponent<CameraControlTouch>();

        TouchManager.Instance.PointersPressed += DrawTest;

        TouchManager.Instance.PointersUpdated += UpdateBrushCursor;
        TouchManager.Instance.PointersReleased += StopAction;

        colourList = new Color[3];
        colourList[0] = Color.red;
        colourList[1] = Color.black;
        colourList[2] = Color.green;
        colourCount = 0;
        countingDown = true;
        timeRemains = countDownTimer;
        //testmaterial.color = colourList[colourCount];
        colourcounter[0].color = colourList[colourCount];
        colourcounter[1].color = colourList[colourCount + 1];
    }

    private void OnEnable()
    {
        tPA.Enable();
    }

    private void OnDisable()
    {
        tPA.Disable();
    }

    //private void Start()
    //{
    //    tPA.Touch.InteractAction.started += ContextMenu => StartTouch(ContextMenu);
    //    tPA.Touch.InteractAction.canceled += ContextMenu => EndTouch(ContextMenu);
    //}

    //void StartTouch(InputAction.CallbackContext cTxt)
    //{
    //    Debug.Log("TouchStart" + tPA.Touch.TouchPos.ReadValue<Vector2>());
    //}
    //void EndTouch(InputAction.CallbackContext cTxt)
    //{
    //    Debug.Log("TouchEnd" + tPA.Touch.TouchPos.ReadValue<Vector2>());
    //}
    void Update()
    {
        /*brushColor = brushC.GetComponent<ColorSelector>().GetColor();  *///Updates our painted color with the selected color
        brushColor = colourList[colourCount];

        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("BrushColour" + brushColor);

        }
        //TestAction(tPA.Touch.InteractAction);

        //tPA.Touch.InteractAction.started += ContextMenu => DoAction();
        //tPA.Touch.InteractAction.performed += ContextMenu => DoAction();

        //UpdateBrushCursor();
        if (countingDown == true)
        {

            if (timeRemains > 0)
            {
                timeRemains -= Time.deltaTime;
                updateUITime(timeRemains);
            }
            else
            {
                colorChange();
                timeRemains = countDownTimer;
            }
        }
        colourcounter[0].color = colourList[colourCount];
    }
    void colorChange()
    {
        //colourCount += 1;
        //colourcounter[1].color = colourList[colourCount + 1]
        colourCount++;
        colourCount = (colourCount >= colourList.Length) ? 0 : colourCount;

        if (colourCount == colourList.Length - 1)
        {

            colourcounter[1].color = colourList[0];
        }

        if (colourCount < colourList.Length - 1)
        {
            colourcounter[1].color = colourList[colourCount + 1];
        }

        if (colourCount > colourList.Length - 1)
        {
            colourCount = 0;
        }
    }
    //void TestAction(InputAction EntryTP)
    //{
    //    EntryTP.started += ContextMenu => DoAction();
    //    EntryTP.performed += ContextMenu => DoAction();

    void updateUITime(float currentTime)
    {
        currentTime += 1;
        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    //}
    //The main action, instantiates a brush or decal entity at the clicked position on the UV map
    void DrawTest(object obj, PointerEventArgs eventArgs)
    {
        drawing = true;

    }

    void DoAction()
    {

        if (saving)

            return;
        Vector3 uvWorldPosition = Vector3.zero;
        if (HitTestUVPosition(ref uvWorldPosition))
        {
            GameObject brushObj;

            if (mode == TimerTouch_BrushMode.PAINT)
            {
                brushObj = (GameObject)Instantiate(Resources.Load("TexturePainter-Instances/BrushEntity")); //Paint a brush
                brushObj.GetComponent<SpriteRenderer>().color = brushColor; //Set the brush color
            }
            else
            {
                brushObj = (GameObject)Instantiate(Resources.Load("TexturePainter-Instances/DecalEntity")); //Paint a decal
            }

            brushColor.a = brushSize * 2.0f; // Brushes have alpha to have a merging effect when painted over.
            brushObj.transform.parent = brushContainer.transform; //Add the brush to our container to be wiped later
            brushObj.transform.localPosition = uvWorldPosition; //The position of the brush (in the UVMap)
            brushObj.transform.localScale = Vector3.one * brushSize;//The size of the brush
        }
        brushCounter++; //Add to the max brushes
        if (brushCounter >= MAX_BRUSH_COUNT)
        { //If we reach the max brushes available, flatten the texture and clear the brushes
            brushCursor.SetActive(false);
            saving = true;
            Invoke("SaveTexture", 0.1f);

        }
        Debug.Log("Drawing");
    }
    //To update at realtime the painting cursor on the mesh
    void StopAction(object obj, PointerEventArgs eventArgs)
    {
        drawing = false;

    }


    void UpdateBrushCursor(object obj, PointerEventArgs eventArgs)
    {
        foreach (var pointer in eventArgs.Pointers)
        {
            Vector3 uvWorldPosition = Vector3.zero;
            if (HitTestUVPosition(ref uvWorldPosition) && !saving)
            {
                brushCursor.SetActive(true);
                brushCursor.transform.position = uvWorldPosition + brushContainer.transform.position;
            }
            else
            {
                brushCursor.SetActive(false);
            }
            if (cTouch.drawToggle == true && drawing == true)
            {
                DoAction();
            }


            //if(canvasCam.pixelRect.Contains(pointer.Position) /*&& (int)Display.RelativeMouseAt(pointer.Position).z == canvasCam.targetDisplay*/)
            //{
            //    Debug.Log(canvasCam);
            //}
        }

    }
    //Returns the position on the texuremap according to a hit in the mesh collider
    bool HitTestUVPosition(ref Vector3 uvWorldPosition)
    {
        RaycastHit hit;
        Vector3 cursorPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f);
        Ray cursorRay = sceneCamera.ScreenPointToRay(cursorPos);
        if (Physics.Raycast(cursorRay, out hit, 500))
        {
            MeshCollider meshCollider = hit.collider as MeshCollider;


            if (meshCollider == null || meshCollider.sharedMesh == null)
                return false;
            Vector2 pixelUV = new Vector2(hit.textureCoord.x, hit.textureCoord.y);
            uvWorldPosition.x = pixelUV.x - canvasCam.orthographicSize;//To center the UV on X
            uvWorldPosition.y = pixelUV.y - canvasCam.orthographicSize;//To center the UV on Y
            uvWorldPosition.z = 0.0f;
            if (hit.collider)
            {
                Debug.Log(hit.collider);
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
        //StartCoroutine ("SaveTextureToFile"); //Do you want to save the texture? This is your method!
        Invoke("ShowCursor", 0.1f);
    }
    //Show again the user cursor (To avoid saving it to the texture)
    void ShowCursor()
    {
        saving = false;
    }

    ////////////////// PUBLIC METHODS //////////////////

    public void SetBrushMode(TimerTouch_BrushMode brushMode)
    { //Sets if we are painting or placing decals
        mode = brushMode;
        brushCursor.GetComponent<SpriteRenderer>().sprite = brushMode == TimerTouch_BrushMode.PAINT ? cursorPaint : cursorDecal;
    }
    public void SetBrushSize(float newBrushSize)
    { //Sets the size of the cursor brush or decal
        brushSize = newBrushSize;
        brushCursor.transform.localScale = Vector3.one * brushSize;
    }

    ////////////////// OPTIONAL METHODS //////////////////

#if !UNITY_WEBPLAYER
    IEnumerator SaveTextureToFile(Texture2D savedTexture)
    {
        brushCounter = 0;
        string fullPath = System.IO.Directory.GetCurrentDirectory() + "\\UserCanvas\\";
        System.DateTime date = System.DateTime.Now;
        string fileName = "CanvasTexture.png";
        if (!System.IO.Directory.Exists(fullPath))
            System.IO.Directory.CreateDirectory(fullPath);
        var bytes = savedTexture.EncodeToPNG();
        System.IO.File.WriteAllBytes(fullPath + fileName, bytes);
        Debug.Log("<color=orange>Saved Successfully!</color>" + fullPath + fileName);
        yield return null;
    }
#endif
}
