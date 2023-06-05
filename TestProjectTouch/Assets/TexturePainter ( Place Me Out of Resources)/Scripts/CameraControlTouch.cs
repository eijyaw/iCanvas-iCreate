using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TouchScript;
using TouchScript.Gestures;
using TouchScript.Pointers;

public class CameraControlTouch : MonoBehaviour
{
    [SerializeField] private Camera touchCam;
    [SerializeField] public Transform targetObj;
    [SerializeField] public GameObject paintObj;

    public bool drawToggle;
    private Vector3 prevPos;
    public float posY;
    public float YAdjustment;
    // Start is called before the first frame update
    void Start()
    {
        drawToggle = true;
    
        TouchManager.Instance.PointersPressed += CameraRotate;
        touchCam.transform.position = new Vector3(targetObj.position.x, targetObj.position.y+posY, targetObj.position.z-10);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            drawToggle = !drawToggle;

        }
        if (drawToggle == false)
        {

        
            if (Input.GetMouseButtonDown(0)&& touchCam.pixelRect.Contains(Input.mousePosition))
            {
                prevPos = touchCam.ScreenToViewportPoint(Input.mousePosition);
            }

         if (Input.GetMouseButton(0) && touchCam.pixelRect.Contains(Input.mousePosition))
            {
            Vector3 direction = prevPos - touchCam.ScreenToViewportPoint(Input.mousePosition);

            touchCam.transform.position = targetObj.position;

            touchCam.transform.Rotate(new Vector3(1, 0, 0), direction.y * 180);
            touchCam.transform.Rotate(new Vector3(0, 1, 0), -direction.x * 180,Space.World);
            touchCam.transform.Translate(new Vector3(0, posY, -10));

            prevPos = touchCam.ScreenToViewportPoint(Input.mousePosition);
            }
        }
    }
    void CameraRotate(object obj, PointerEventArgs eventArgs)
    {

    }
}
