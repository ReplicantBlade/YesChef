using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightOnMouseHover : MonoBehaviour
{
    [SerializeField] public GameObject highlightTarget;

    private Renderer myRenderer;
    private Color startcolor;
    private void Start()
    {
        myRenderer = highlightTarget == null ? transform.GetComponent<Renderer>() : highlightTarget.transform.GetComponent<Renderer>();
        startcolor = myRenderer.material.color;
    }
    void OnMouseEnter()
    {
        myRenderer.material.color = Color.yellow;
    }
    void OnMouseExit()
    {
        myRenderer.material.color = startcolor;
    }
}
