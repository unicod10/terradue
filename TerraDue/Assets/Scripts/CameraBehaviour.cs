using UnityEngine;
using UnityEngine.Networking;

public class CameraBehaviour : NetworkBehaviour
{

    static private float CAMERA_DISTANCE = 15;
    static private float CAMERA_ROTATION_X = 60;
    static private float PAN_SPEED = 15;
    static private float PAN_BORDER = 10;
    static private float WHEEL_FACTOR = 2;
    private float cameraDistance;

    private void Start()
    {
        if (isLocalPlayer)
        {
            cameraDistance = CAMERA_DISTANCE;
            PositionCamera();
        }
    }

    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - PAN_BORDER)
        {
            Camera.main.transform.position += new Vector3(-PAN_SPEED * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey("s") || Input.mousePosition.y <= PAN_BORDER)
        {
            Camera.main.transform.position += new Vector3(+PAN_SPEED * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey("a") || Input.mousePosition.x <= PAN_BORDER)
        {
            Camera.main.transform.position += new Vector3(0, 0, -PAN_SPEED * Time.deltaTime);
        }
        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - PAN_BORDER)
        {
            Camera.main.transform.position += new Vector3(0, 0, +PAN_SPEED * Time.deltaTime);
        }
        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            var scroll = Input.GetAxis("Mouse ScrollWheel");
            cameraDistance += -Mathf.Sign(scroll) * WHEEL_FACTOR;
            var trx = CalcTranslation(-Mathf.Sign(scroll) * WHEEL_FACTOR);
            TranslateCamera(Camera.main.transform.position, trx);
        }
        if (Input.GetKeyDown("c"))
        {
            PositionCamera();
        }
    }

    public void PositionCamera()
    {
        // Rotate to RTS camera
        var rotationY = transform.tag == "Human" ? -90 : +90;
        Camera.main.transform.rotation = Quaternion.Euler(CAMERA_ROTATION_X, rotationY, 0);

        // Center the player
        var translation = CalcTranslation(cameraDistance);
        TranslateCamera(transform.position, translation);
    }

    private Vector3 CalcTranslation(float delta)
    {
        var translation = new Vector3(0, 0);
        translation.x = delta * Mathf.Cos(Mathf.Deg2Rad * CAMERA_ROTATION_X);
        translation.y = delta * Mathf.Sin(Mathf.Deg2Rad * CAMERA_ROTATION_X);
        return translation;
    }

    private void TranslateCamera(Vector3 origin, Vector3 translation)
    {
        Camera.main.transform.position = origin + translation;
    }
}