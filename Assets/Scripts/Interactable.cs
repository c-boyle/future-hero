using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private PathMovement trainMovement;
    [SerializeField] private Animator animator;
    private bool leverUp = true;

    public void Interact()
    {
        trainMovement.SwitchPath();
        leverUp = !leverUp;
        animator.SetBool("lever_up", leverUp);
        Debug.Log("interacted + " + leverUp);
    }
}
