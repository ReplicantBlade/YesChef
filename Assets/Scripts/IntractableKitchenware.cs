using UnityEngine;

public class IntractableKitchenware : MonoBehaviour
{
    [SerializeField] public GameObject highlightTarget;
    [SerializeField] public Transform chefStandPosition;

    private Renderer _highlightTargetRenderer;
    private Color _highlightTargetStartColor;
    private void Start()
    {
        _highlightTargetRenderer = highlightTarget == null ? transform.GetComponent<Renderer>() : highlightTarget.transform.GetComponent<Renderer>();
        _highlightTargetStartColor = _highlightTargetRenderer.material.color;
    }
    private void OnMouseEnter()
    {
        _highlightTargetRenderer.material.color = Color.yellow;
    }
    private void OnMouseExit()
    {
        _highlightTargetRenderer.material.color = _highlightTargetStartColor;
    }

    private void OnMouseDown()
    {
        SendOrder();
        TriggerMyCollider();
    }
    private void SendOrder()
    {
        GameManager.Instance.MovementOrder(chefStandPosition);
    }

    private void TriggerMyCollider()
    {
        var myBoxCollider = transform.GetComponent<BoxCollider>();
        myBoxCollider.enabled = false;
        myBoxCollider.enabled = true;
    }
}
