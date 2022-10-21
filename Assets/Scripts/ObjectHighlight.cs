using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class ObjectHighlight : MonoBehaviour
{
    [SerializeField] [MustBeAssigned] [Layer] private int _layer;
    [SerializeField] [MustBeAssigned] private Material _highlightMaterial;

    private Renderer _objectRenderer;
    private Renderer _highlightRenderer;

    void Start() {
        _objectRenderer = GetComponent<Renderer>();

        // copying the mesh of the game object
        GameObject _highlightObject = new GameObject("Highlighted Copy");

        // copying mesh
        _highlightObject.AddComponent<MeshFilter>().sharedMesh = GetComponent<MeshFilter>().mesh;
        
        // attaching renderer component
        _highlightRenderer = _highlightObject.AddComponent<MeshRenderer>();

        // setting transform
        _highlightObject.transform.parent = transform;
        _highlightObject.transform.localPosition = Vector3.zero;
        _highlightObject.transform.localEulerAngles = Vector3.zero;
        _highlightObject.transform.localScale = new Vector3(1.05f, 1.05f, 1.05f);

        // copying the material of the original game object
        //Material _highlightMaterial = new Material(Shader.Find("Shader Graphs/HighlightShader"));
        //_highlightMaterial.SetColor("_Color", _objectRenderer.material.GetColor("_Color"));
        _highlightRenderer.material = _highlightMaterial;

        // set highLightObject layer to no post process layer
        _highlightObject.layer = _layer;

        // remove highlightObject cast shadow
        _highlightRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
    }

    void Update() {
        float progress = Shader.GetGlobalFloat("_Progress");
        if (progress > 0.65) {
            _highlightRenderer.enabled = true;
            _objectRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
        } else {
            _highlightRenderer.enabled = false;
            _objectRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        }
    }
}
