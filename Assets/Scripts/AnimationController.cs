using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator animator;
    public Movement movementScript;  // Public reference to Movement script

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Check if movementScript is assigned and use its isMoving property
        if (movementScript != null)
        {
            SetWalking(movementScript.isMoving);
        }
    }

    public void SetWalking(bool isWalking)
    {
        animator.SetBool("isWalking", isWalking);
    }

    public void SetShooting(bool isShooting)
    {
        animator.SetBool("isShooting", isShooting);
    }

    public void SetReloading(bool isReloading)
    {
        animator.SetBool("isReloading", isReloading);
    }
}
