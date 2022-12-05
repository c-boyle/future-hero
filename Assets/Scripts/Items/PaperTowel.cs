using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperTowel : Item
{
    private int maxDries = 3;
    private int numDries;
    private Vector3 orginalPaperScale;
    [SerializeField] private GameObject paper;
    [SerializeField] private AudioSource tearingSound;

    void Awake() {
        numDries = maxDries;
        orginalPaperScale = paper.transform.localScale;
    }

    public bool HasUsesLeft() {
      return numDries > 0;
    }

    public bool Use() {
        if (numDries <= 0) return false;
        Shrink();
        tearingSound.Play();
        return true;

    }

    private void Shrink() {
        float xDiff = orginalPaperScale.x / (maxDries + 1);
        float zDiff = orginalPaperScale.z / (maxDries + 1);

        Transform paperTransform = paper.transform;

        numDries--;
        if (numDries <= 0) {
            paper.transform.localScale = new Vector3(0, 0, 0);
        } else {
            float newX = paperTransform.localScale.x - xDiff;
            float newZ = paperTransform.localScale.z - zDiff;

            paper.transform.localScale = new Vector3(newX, paperTransform.localScale.y, newZ);
        }
    }
}
