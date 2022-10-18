using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class ObjectHighlight : MonoBehaviour
{
    //[SerializeField] private float RimThickness;
    //[SerializeField] private float RimIntensity;
    //[SerializeField] private float Brightness;
    //[SerializeField] private float RimThickness;
    //[SerializeField] private float RimThickness;

    [SerializeField] [ReadOnly] private string HIGHLIGHT_SHADER_NAME = "Shader Graphs/FutureHighlightShader";
    [SerializeField] [MustBeAssigned] [Layer] private int _layer;

    private Renderer _highlightRenderer;
    private Material _highlightMaterial;

    void Start() {
        // copying the mesh of the game object
        GameObject _highlightObject = new GameObject("Highlighted Copy");
        var meshFilter = _highlightObject.AddComponent<MeshFilter>();
        meshFilter.sharedMesh = GetComponent<MeshFilter>().mesh;
        _highlightRenderer = _highlightObject.AddComponent<MeshRenderer>();

        _highlightObject.transform.parent = transform;
        _highlightObject.transform.localPosition = Vector3.zero;
        _highlightObject.transform.localEulerAngles = Vector3.zero;
        _highlightObject.transform.localScale = new Vector3(1.02f, 1.02f, 1.02f);

        // copying the material of the original game object but with a different shader
        _highlightMaterial = new Material(Shader.Find(HIGHLIGHT_SHADER_NAME));
        _highlightMaterial.SetColor("_Color", GetComponent<Renderer>().material.GetColor("_Color"));
        _highlightRenderer.material = _highlightMaterial;

        // set highLightObject layer to no post process layer
        _highlightObject.layer = _layer;

        // remove highlightObject cast shadow
        _highlightRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
    }
}
