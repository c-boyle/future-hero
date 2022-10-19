using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEffects : MonoBehaviour
{
    private HashSet<AudioEchoFilter> echoFilters = new HashSet<AudioEchoFilter>();
    public float echoDelay = 200f;
    public float echoDecayRatio = 0.5f;
    public float echoDryMix = 1.0f;
    public float echoWetMix = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        var audioSources = FindObjectsOfType<AudioSource>();
        foreach (var audioSource in audioSources) {
            AudioEchoFilter echoFilter = audioSource.gameObject.AddComponent<AudioEchoFilter>();
            echoFilter.enabled = false;
            SetEcho(echoFilter);
            echoFilters.Add(echoFilter);
        }
    }

    private void SetEcho(AudioEchoFilter echoFilter) {
        echoFilter.delay = echoDelay;
        echoFilter.decayRatio = echoDecayRatio;
        echoFilter.dryMix = echoDryMix;
        echoFilter.wetMix = echoWetMix;
    }

    public void SetEnabled(bool enable) {
        foreach (var filter in echoFilters) {
            filter.enabled = enable;
        }
    }

}
