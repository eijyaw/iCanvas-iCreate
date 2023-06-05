using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActingModelScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera EnvCam;
    public GameObject targetObj;

    void Update()
    {
        EnvRay();
    }

    // Update is called once per frame
    bool EnvRay()
    {
        RaycastHit hitObj;
        Vector3 cursorPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f);
        Ray cursorRay = EnvCam.ScreenPointToRay(cursorPos);
        if (Physics.Raycast(cursorRay, out hitObj, 1000))
        {
            if (hitObj.collider != null)
            {
                InteractionAnimationScript IScript = hitObj.collider.GetComponent<InteractionAnimationScript>();
                if (IScript != null)
                {
                    IScript.ActiveAnimation(); 
                    Debug.Log(hitObj.collider);
                }
               
            }
            return true;
        }
        else
        {
            return false;
        }
    }
}
