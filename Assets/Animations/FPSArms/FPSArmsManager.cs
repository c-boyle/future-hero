using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class FPSArmsManager : MonoBehaviour
{
    [SerializeField] public bool isWatchShown = false;
    [SerializeField] [MustBeAssigned] Animator animator;

    void Update()
    {
        animator.SetBool("isWatchShown", isWatchShown);
    }
}
