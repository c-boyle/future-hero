using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour
{
    public int ColourCode = 1;
    public UnityEvent OnCol;
    private void OnCollisionEnter(Collision col)
    {
        Debug.Log("Collision");
        OnCol.Invoke();
    }
}
