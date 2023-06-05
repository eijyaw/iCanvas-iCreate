using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnimationScript : MonoBehaviour
{
    public Animator efreetAnimator;
    public bool touched;
    
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(touched == true)
        {
            efreetAnimator.Play("Slap");
            touched = false;
        }
        else
        {
            efreetAnimator.Play("Idle");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        touched = true;
        Debug.Log("Ow");
    }
}
