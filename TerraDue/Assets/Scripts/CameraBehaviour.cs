using UnityEngine;
using UnityEngine.Networking;

public class CameraBehaviour : NetworkBehaviour
{

    static private float CAMERA_ROTATION_X = 30 * Mathf.Deg2Rad;
    static private Vector3 CAMERA_DIRECTION_VECTOR = new Vector3(0, -Mathf.Sin(CAMERA_ROTATION_X), Mathf.Cos(CAMERA_ROTATION_X));
    static private float WASD_FACTOR = 0.2f;
    static private float WHEEL_FACTOR = 2.0f;

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
            WASD_FACTOR *= -1;
            CAMERA_DIRECTION_VECTOR.z *= -1;
            Camera.main.transform.Rotate(new Vector3(0, 1, 0), 180);
        }
        // Move camera to default orientation and position
        Camera.main.transform.Rotate(new Vector3(1, 0, 0), Mathf.Rad2Deg * CAMERA_ROTATION_X);
        PositionCamera();
    }

    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        if (Input.GetKeyDown("q") && GetComponent<PlayerBehaviour>().IsAlive())
        {
            PositionCamera();
            return;
        }
        if (Input.GetKey("w"))
        {
            TranslateCamera(new Vector3(0, 0, WASD_FACTOR));
        }
        if (Input.GetKey("a"))
        {
            TranslateCamera(new Vector3(-WASD_FACTOR, 0, 0));
        }
        if (Input.GetKey("s"))
        {
            TranslateCamera(new Vector3(0, 0, -WASD_FACTOR));
        }
        if (Input.GetKey("d"))
        {
            TranslateCamera(new Vector3(WASD_FACTOR, 0, 0));
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            TranslateCamera(-WHEEL_FACTOR * CAMERA_DIRECTION_VECTOR);
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            TranslateCamera(WHEEL_FACTOR * CAMERA_DIRECTION_VECTOR);
        }
    }

    private void PositionCamera()
    {
        Camera.main.transform.position = transform.position - 8 * CAMERA_DIRECTION_VECTOR;
    }

    private void TranslateCamera(Vector3 translation)
    {
        var position = Camera.main.transform.position;
        Camera.main.transform.position = position + translation;
    }
}