using System;
using UnityEngine;

public class Kirby_Animation : MonoBehaviour
{
    internal bool isGrounded;
    private Animator animator;

    internal void Move(Vector3 vector3)
    {
        throw new NotImplementedException();
    }

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            animator.SetBool("isWalking", true);
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            animator.SetBool("isWalking", false);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            animator.SetBool("isClapping", true);
        }
        else if (Input.GetKeyUp(KeyCode.H))
        {
            animator.SetBool("isClapping", false);
        }
    }
}
