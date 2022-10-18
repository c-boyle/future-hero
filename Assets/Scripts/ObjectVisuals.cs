using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class ObjectVisuals : MonoBehaviour
{
    [SerializeField] [MustBeAssigned] private LayerMask _noPostProcessLayer;
    [SerializeField] [MustBeAssigned] private Material _highlightMaterial;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var gameObject in GetComponentsInChildren<GameObject>()) {
            GameObject highlightObject = GameObject.Instantiate(gameObject);

            // set highLightObject shader to highlight shader
            highlightObject.GetComponent<MeshRenderer>().material = _highlightMaterial;

            // set highLightObject layer to no post process layer
            highlightObject.layer = _noPostProcessLayer;

            // remove highlightObject cast shadow
            highlightObject.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

            // remove visibility for original game object
            gameObject.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
