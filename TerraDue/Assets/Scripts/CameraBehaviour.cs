using UnityEngine;
using UnityEngine.Networking;

public class CameraBehaviour : NetworkBehaviour
{

    static private float CAMERA_ROTATION_X = 20;
    static private float CAMERA_DISTANCE_XZ = 4;
    static private float CAMERA_DISTANCE_Y = 3.4f;

    private void Start()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        PositionCamera();
    }

    void Update()
    {
        if (isLocalPlayer)
        {
            PositionCamera();
        }
    }

    private void PositionCamera()
    {
        // Rotate and position according to player
        Camera.main.transform.rotation = transform.rotation * Quaternion.Euler(Vector3.right * CAMERA_ROTATION_X);
        Camera.main.transform.position = transform.position - CAMERA_DISTANCE_XZ * transform.forward + new Vector3(0, CAMERA_DISTANCE_Y);
    }
}