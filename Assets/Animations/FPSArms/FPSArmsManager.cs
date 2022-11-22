using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class FPSArmsManager : MonoBehaviour
{
    [SerializeField] public bool isWatchShown = false;
    [SerializeField] public bool isSprinting = false;
    [SerializeField] public bool isMidAir = false;
    [SerializeField] [MustBeAssigned] Animator animator;

    void Update()
    {
        animator.SetBool("isWatchShown", isWatchShown);
        animator.SetBool("isSprinting", isSprinting);
        animator.SetBool("isMidAir", isMidAir);
    }

    public void StartJump() {
        // Only queue the jump animation if the current state is idle
        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextState = animator.GetNextAnimatorStateInfo(0);
        if (currentState.IsName("Idle") || nextState.IsName("Idle"))
            animator.SetTrigger("startJump");
    }
}
