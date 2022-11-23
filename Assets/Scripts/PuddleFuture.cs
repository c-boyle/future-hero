using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuddleFuture : MonoBehaviour
{
    private bool erased = false;

    public void Erase() {
        gameObject.SetActive(false);
        erased = true;
    }

    public void Happens() {
        if (!erased) gameObject.SetActive(true);
    }
}
