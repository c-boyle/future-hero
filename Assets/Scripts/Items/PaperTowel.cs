using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperTowel : Item
{
    public bool isProper = true;
    private int maxDries = 3;
    private int numDries;
    private Vector3 orginalPaperScale;
    [SerializeField] private GameObject[] papers;
    [SerializeField] private AudioSource tearingSound;

    void Awake() {
        numDries = maxDries;
        orginalPaperScale = papers[0].transform.localScale;
    }

    public bool HasUsesLeft() {
      return numDries > 0;
    }

    public bool Use() {
        if (numDries <= 0) return false;
    
        if(isProper) Tear();
        else Shrink();
        
        tearingSound.Play();

        numDries--;
        return true;

    }

    private void Tear() {
        papers[numDries-1].SetActive(false);
    }

    private void Shrink() {
        float xDiff = orginalPaperScale.x / (maxDries + 1);
        float zDiff = orginalPaperScale.z / (maxDries + 1);
        GameObject paper = papers[0];

        Transform paperTransform = paper.transform;

        if (numDries <= 0) {
            paper.transform.localScale = new Vector3(0, 0, 0);
        } else {
            float newX = paperTransform.localScale.x - xDiff;
            float newZ = paperTransform.localScale.z - zDiff;

            paper.transform.localScale = new Vector3(newX, paperTransform.localScale.y, newZ);
        }
    }
}
