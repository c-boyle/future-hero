using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class FPSArmsRightManager : MonoBehaviour
{
    [SerializeField] public bool isSprinting = false;
    [SerializeField] [MustBeAssigned] Animator animator;

    void Update()
    {
        animator.SetBool("isSprinting", isSprinting);
    }

}
