using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuffianFuture : MonoBehaviour
{
    [SerializeField] private GameObject puddleFuture;
    [SerializeField] private GameObject fireFuture;

    public void EraseFuture() {
        gameObject.SetActive(false);
        if (!puddleFuture.activeSelf && !fireFuture.activeSelf) LevelTimer.Instance.EndLevel(true);
    }
}
