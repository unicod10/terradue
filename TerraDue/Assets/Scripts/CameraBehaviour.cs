using UnityEngine;
using UnityEngine.Networking;

public class CameraBehaviour : NetworkBehaviour
{

    static private float CAMERA_ROTATION_X = 20;
    static private float CAMERA_DISTANCE_XZ = 5.0f;
    static private float CAMERA_DISTANCE_Y = 4.0f;

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
            if(!GetComponent<MoveToPoint>().IsMoving())
            {
                if (Input.GetKeyDown("e"))
                {
                    transform.rotation *= Quaternion.AngleAxis(+90, new Vector3(0, 1));
                }
                if (Input.GetKeyDown("q"))
                {
                    transform.rotation *= Quaternion.AngleAxis(-90, new Vector3(0, 1));
                }
                if (Input.GetKeyDown("w"))
                {
                    transform.rotation *= Quaternion.AngleAxis(180, new Vector3(0, 1));
                }
            }
            else
            {
                if (Input.GetKeyDown("space"))
                {
                    GetComponent<MoveToPoint>().StopMovement();
                }
            }
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