using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxScript : MonoBehaviour
{
    public float acceleration = 1f;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
            animator.Play("Fox_Jump_InAir");
        else if (Input.GetKey(KeyCode.DownArrow))
            animator.Play("Fox_Falling");
        else if (Input.GetKey(KeyCode.RightArrow))
            animator.Play("Fox_Run_Right");
        else if (Input.GetKey(KeyCode.LeftArrow))
            animator.Play("Fox_Run_Left");

        acceleration += 0.01f;
        transform.Translate(Vector3.forward * acceleration * Time.deltaTime);
    }
}
