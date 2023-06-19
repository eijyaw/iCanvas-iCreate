using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InteractionAnimationScript : MonoBehaviour
{
    SphereCollider objCollider;
    Animator objAnimator;
    public bool touchCheck;
    public bool interactable;
    public float countDownTimer;
    public Text threeDeeText;
    public Camera tDTouchCam;
    // Start is called before the first frame update
    void Start()
    {
        objAnimator = GetComponent<Animator>();
        objCollider = GetComponent<SphereCollider>();
        objAnimator.Play("Normal");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount >= 1)
        {
            // The pos of the touch on the screen
            Vector2 vTouchPos = Input.GetTouch(0).position;

            // The ray to the touched object in the world
            Ray ray = tDTouchCam.ScreenPointToRay(vTouchPos);

            // Your raycast handling
            RaycastHit vHit;
            if (Physics.Raycast(ray.origin, ray.direction, out vHit))
            {
                if(vHit.collider.gameObject.tag == "THING")
                {
                    objAnimator.Play("Active");
                }
            }
        }
        threeDeeText.text = objCollider.enabled.ToString();
    }
    public void ActiveAnimation()
    {
        objAnimator.Play("Active");
        objCollider.enabled = false;
        countDownTimer = objAnimator.GetCurrentAnimatorStateInfo(0).length;
        StartCoroutine(CDown(countDownTimer));
     
    }
    IEnumerator CDown(float countdown)
    {
        yield return new WaitForSeconds(countdown);
        objCollider.enabled = true;

    }
}
