using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cigarette : Item
{
    private bool SafePosition() {
        // At the moment there is no safe place to leave the cigarette
        return false;
    }
}
