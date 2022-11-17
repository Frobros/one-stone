using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform Target;
    public float speed;

    private void Awake()
    {
        Vector3 targetPosition = Target.position;
        targetPosition.z = transform.position.z;
        transform.position = targetPosition;
    }
    private void Update()
    {
        if (Target != null)
        {
            Vector3 targetPosition = Target.position;
            targetPosition.z = transform.position.z;
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * speed);
        }
    }
}
