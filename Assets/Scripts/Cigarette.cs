using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cigarette : MonoBehaviour
{
    public void MoveTo(Vector3 newPosition, Vector3 newRotation) {
        transform.rotation = Quaternion.Euler(newRotation);
        MoveTo(newPosition);
    }

    public void MoveTo(Vector3 newPosition) {
        transform.position = newPosition;
        if (!SafePosition()) {
            LevelTimer.Instance.SetSecondsLeft(10);
        }
    }

    private bool SafePosition() {
        // At the moment there is no safe place to leave the cigarette
        return false;
    }
}
