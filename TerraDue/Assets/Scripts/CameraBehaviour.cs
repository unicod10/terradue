using UnityEngine;
using UnityEngine.Networking;

public class CameraBehaviour : NetworkBehaviour
{
    static private float CAMERA_DISTANCE = 12;
    static private float CAMERA_ROTATION_X = 50;
    static private float PAN_SPEED = 20;
    static private float PAN_BORDER = 10;
    static private float WHEEL_FACTOR = 45;
    static private float[] CLAMP_DISTANCE = new float[] { 110, 90 };
    private float cameraDistance;
    private float sign;

    private void Start()
    {
        if (isLocalPlayer)
        {
            sign = gameObject.tag == "Human" ? +1 : -1;
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
            TranslateCamera(Camera.main.transform.position, new Vector3(sign * -PAN_SPEED * Time.deltaTime, 0, 0));
        }
        if (Input.GetKey("s") || Input.mousePosition.y <= PAN_BORDER)
        {
            TranslateCamera(Camera.main.transform.position, new Vector3(sign * +PAN_SPEED * Time.deltaTime, 0, 0));
        }
        if (Input.GetKey("a") || Input.mousePosition.x <= PAN_BORDER)
        {
            TranslateCamera(Camera.main.transform.position, new Vector3(0, 0, sign * -PAN_SPEED * Time.deltaTime));
        }
        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - PAN_BORDER)
        {
            TranslateCamera(Camera.main.transform.position, new Vector3(0, 0, sign * +PAN_SPEED * Time.deltaTime));
        }
        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            var scroll = Input.GetAxis("Mouse ScrollWheel");
            float delta = -Mathf.Sign(scroll) * WHEEL_FACTOR * Time.deltaTime;
            float oldDist = cameraDistance;
            cameraDistance = Mathf.Clamp(oldDist + delta, 5, 25);
            var trx = CalcTrxDelta(cameraDistance - oldDist);
            TranslateCamera(Camera.main.transform.position, trx);
        }
        if (Input.GetKeyDown("c"))
        {
            PositionCamera();
        }
    }

    public void PositionCamera()
    {
        var rotationY = transform.tag == "Human" ? -90 : +90;
        Camera.main.transform.rotation = Quaternion.Euler(CAMERA_ROTATION_X, rotationY, 0);
        var translation = CalcTrxDelta(cameraDistance);
        TranslateCamera(transform.position, translation);
    }

    private Vector3 CalcTrxDelta(float delta)
    {
        var translation = new Vector3(0, 0);
        translation.x = sign * delta * Mathf.Cos(Mathf.Deg2Rad * CAMERA_ROTATION_X);
        translation.y = delta * Mathf.Sin(Mathf.Deg2Rad * CAMERA_ROTATION_X);
        return translation;
    }

    private void TranslateCamera(Vector3 origin, Vector3 translation)
    {
        var position = origin + translation;
        position.x = Mathf.Clamp(position.x, -CLAMP_DISTANCE[0], CLAMP_DISTANCE[0]);
        position.z = Mathf.Clamp(position.z, -CLAMP_DISTANCE[1], CLAMP_DISTANCE[1]);
        Camera.main.transform.position = position;
    }
}