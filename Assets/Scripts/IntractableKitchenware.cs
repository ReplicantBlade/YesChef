using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntractableKitchenware : MonoBehaviour
{
    [SerializeField] public GameObject highlightTarget;
    [SerializeField] public Transform chefStandPosition;

    private Renderer highlightTargetRenderer;
    private Color highlightTargetStartcolor;
    private void Start()
    {
        highlightTargetRenderer = highlightTarget == null ? transform.GetComponent<Renderer>() : highlightTarget.transform.GetComponent<Renderer>();
        highlightTargetStartcolor = highlightTargetRenderer.material.color;
    }
    private void OnMouseEnter()
    {
        highlightTargetRenderer.material.color = Color.yellow;
    }
    private void OnMouseExit()
    {
        highlightTargetRenderer.material.color = highlightTargetStartcolor;
    }

    private void OnMouseDown()
    {
        SendOrder();
    }
    private void SendOrder()
    {
        GameManager.Instance.MovementOrder(chefStandPosition);
    }
}
