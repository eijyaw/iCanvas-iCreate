using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionAnimationScript : MonoBehaviour
{
    Collider objCollider;
    Animator objAnimator;
    public bool touchCheck;
    public bool interactable;
    public float countDownTimer;
    // Start is called before the first frame update
    void Start()
    {
        objAnimator = GetComponent<Animator>();
        objCollider = GetComponent<Collider>();
        objAnimator.Play("Normal");
    }

    // Update is called once per frame
    void Update()
    {
        if(touchCheck == false)
        {
            objAnimator.Play("Normal");
        }
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
