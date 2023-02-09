using System;
using System.Collections.Generic;
using UnityEngine;

public class ChefController : MonoBehaviour
{
    [SerializeField] public float speed = 20f;
    private List<Transform> _movementPositionList = new();
    public bool ChefIsBusy { get; set; }
    
    private void Update()
    {
        UpdateMovementOrder();

        if (!HaveTaskToDo()) return;
        var target = GetTargetToGo();
        MoveTowards(target);
        if (IsChefReachedTheTarget(target) && !ChefIsBusy)
        {
            ChefTaskIsDone();
        }
    }

    private void UpdateMovementOrder()
    {
        if (!ChefIsBusy)
            _movementPositionList = new List<Transform>(GameManager.Instance.GetOrderList());
    }

    private bool HaveTaskToDo()
    {
        return _movementPositionList.Count > 0;
    }

    private void ChefTaskIsDone()
    {
        GameManager.Instance.OrderAccomplish();
    }

    private Transform GetTargetToGo()
    {
        return _movementPositionList[0];
    }

    private void MoveTowards(Transform target)
    {
        if (target == null) return;

        var direction = (target.position - transform.position).normalized;
        direction.y = 0;
        transform.position += speed * Time.deltaTime * direction;
        transform.LookAt(target);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }

    private bool IsChefReachedTheTarget(Transform target)
    {
        var distance = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(target.position.x, 0, target.position.z));
        return distance <= 0.5f;
    }
}
