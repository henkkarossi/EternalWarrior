using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject target;
    public float followDistance;

    void FixedUpdate()
    {
        FollowTarget(target);
    }

    void FollowTarget(GameObject target)
    {
        float speed = target.GetComponent<Player>().walkSpeed;
        transform.Translate(Vector3.forward * Time.deltaTime * speed * (target.transform.position.z - transform.position.z));
    }
}
