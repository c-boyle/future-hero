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
}
