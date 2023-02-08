using System.Collections;
using System.Collections.Generic;
using UnityEditor.XR;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ChefController : MonoBehaviour
{
    [SerializeField] public float speed = 20f;

    private static List<Transform> movementPositionList = new();
    private bool chefIsBusy { get;  set; } = false;
    void Awake()
    {

    }

    void Update()
    {
        UpdateMovementOrder();

        if (HaveTaskToDo())
        {
            Transform target = GetTargetToGo();
            MoveTowards(target);
            if (IsChefReachedTheTarget(target))
            {
                ChefTaskIsDone();
            }
        }
    }

    private void UpdateMovementOrder()
    {
        if (!chefIsBusy)
            movementPositionList = new List<Transform>(GameManager.Instance.GetOrderList());
    }

    private bool HaveTaskToDo()
    {
        return movementPositionList.Count > 0;
    }

    private void ChefTaskIsDone()
    {
        GameManager.Instance.OrderAccomplish();
    }

    Transform GetTargetToGo()
    {
        return movementPositionList[0];
    }

    private void MoveTowards(Transform target)
    {
        if (target == null) return;

        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0;
        transform.position += speed * Time.deltaTime * direction;
        transform.LookAt(target);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }

    private bool IsChefReachedTheTarget(Transform target)
    {
        float distance = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(target.position.x, 0, target.position.z));
        if (distance <= 0.5f)
        {
            return true;
        }
        else return false;
    }
}
