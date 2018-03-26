using UnityEngine;

public class CameraMovement : MonoBehaviour {

    //for camera Movement
    public Vector3 offset;
    public Transform target;
    public float smoothSpeed = 0.125f;

    private void FixedUpdate()
    {
        Vector3 desiredPos = target.position + offset;
        Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed);
        //Transform cameraTf = cameraEasy.transform;
        transform.position = smoothedPos;
    }

}
