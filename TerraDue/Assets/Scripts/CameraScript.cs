using UnityEngine;
using UnityEngine.Networking;

public class CameraScript : NetworkBehaviour {

    static private float CAMERA_ROTATION_X = 60;
    static private Vector3 CAMERA_POSITION_OFFSET = new Vector3(0, 10f, -4f);
    static private float WASD_FACTOR = 0.2f;
    static private float WHEEL_FACTOR = 0.1f;

    private void Start()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        // Show aliens from back too
        if (tag == "Alien")
        {
            WASD_FACTOR *= -1;
            CAMERA_POSITION_OFFSET.z *= -1;
            Camera.main.transform.rotation = Quaternion.identity;
            Camera.main.transform.Rotate(new Vector3(0, 1, 0), 180);
            Camera.main.transform.Rotate(new Vector3(1, 0, 0), CAMERA_ROTATION_X);
        }
        // Move camera to default position
        PositionCamera();
    }

    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        if(Input.GetKeyDown("q"))
        {
            PositionCamera();
            return;
        }
        if(Input.GetKey("w"))
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
            TranslateCamera(WHEEL_FACTOR * CAMERA_POSITION_OFFSET);
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            TranslateCamera(-WHEEL_FACTOR * CAMERA_POSITION_OFFSET);
        }
    }

    private void PositionCamera()
    {
        var position = transform.position;
        position.y += CAMERA_POSITION_OFFSET.y;
        position.z += CAMERA_POSITION_OFFSET.z;
        Camera.main.transform.position = position;
    }

    private void TranslateCamera(Vector3 translation)
    {
        var position = Camera.main.transform.position;
        position.x += translation.x;
        position.y += translation.y;
        position.z += translation.z;
        Camera.main.transform.position = position;
    }
}
