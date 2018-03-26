using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    [SerializeField]
    private Transform target;
    [SerializeField]
    private float speed;

    void FixedUpdate()
    {
        if (target != null)
        {
            transform.LookAt(target);
            transform.Translate(Vector3.left * speed);
        }
    }
}
