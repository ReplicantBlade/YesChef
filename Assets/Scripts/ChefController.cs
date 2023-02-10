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
        if (ReferenceEquals(target, null)) return;

        var transform1 = transform;
        var position = transform1.position;
        var direction = (target.position - position).normalized;
        direction.y = 0;
        position += speed * Time.deltaTime * direction;
        transform1.position = position;
        Transform transform2;
        (transform2 = transform).LookAt(target);
        transform2.eulerAngles = new Vector3(0, transform2.eulerAngles.y, 0);
    }

    private bool IsChefReachedTheTarget(Transform target)
    {
        var transform1 = transform;
        var position = transform1.position;
        var position1 = target.position;
        var distance = Vector3.Distance(new Vector3(position.x, 0, position.z), new Vector3(position1.x, 0, position1.z));
        return distance <= 0.5f;
    }
}
