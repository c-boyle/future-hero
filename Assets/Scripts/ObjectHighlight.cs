using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class ObjectHighlight : MonoBehaviour
{
    [SerializeField] [ReadOnly] private string HIGHLIGHT_SHADER_NAME = "Shader Graphs/FutureHighlightShader";
    [SerializeField] [MustBeAssigned] [Layer] private int _layer;

    private Renderer _highlightRenderer;
    private Material _highlightMaterial;

    void Start() {
        transform.gameObject.layer = _layer;

        /*
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
        _highlightObject.transform.localScale = new Vector3(1, 1, 1);

        // copying the material of the original game object
        _highlightMaterial = new Material(Shader.Find(HIGHLIGHT_SHADER_NAME));
        _highlightMaterial.SetColor("_Color", GetComponent<Renderer>().material.GetColor("_Color"));
        _highlightRenderer.material = _highlightMaterial;

        // set highLightObject layer to no post process layer
        _highlightObject.layer = _layer;

        // remove highlightObject cast shadow
        _highlightRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        */
    }
}
