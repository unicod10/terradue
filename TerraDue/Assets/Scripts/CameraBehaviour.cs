using UnityEngine;
using UnityEngine.Networking;

public class CameraBehaviour : NetworkBehaviour
{

    static private int CAMERA_DISTANCE = 10;
    static private float CAMERA_ROTATION_X = 30 * Mathf.Deg2Rad;
    static private Vector3 CAMERA_DIRECTION_VECTOR = new Vector3(0, -Mathf.Sin(CAMERA_ROTATION_X), Mathf.Cos(CAMERA_ROTATION_X));

    private void Start()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        Camera.main.transform.rotation = Quaternion.identity;
        // Show aliens from back too
        if (tag == "Alien")
        {
            Camera.main.transform.Rotate(new Vector3(0, 1, 0), 180);
        }
        // Rotate camera and move to default position
        Camera.main.transform.Rotate(new Vector3(1, 0, 0), Mathf.Rad2Deg * CAMERA_ROTATION_X);
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
        Camera.main.transform.position = transform.position - CAMERA_DISTANCE * CAMERA_DIRECTION_VECTOR;
    }
}