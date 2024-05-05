using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private const string ZoomAxisName = "Mouse ScrollWheel";
    private const float MinZoom = 5f;
    private const float MaxZoom = 50f;
    private const string MouseXAxisName = "Mouse X";
    private const string MouseYAxisName = "Mouse Y";
    private const int MiddleMouseButton = 2;

    [SerializeField] private float _moveSpeed = 5f; 
    [SerializeField] private Terrain _terrain;
    [SerializeField] private float _zoomSpeed = 5f;
    [SerializeField] private float _rotationSpeed = 2f;

    private void Update()
    {
        Vector3 currentPosition = transform.position;

        HandleMovement(ref currentPosition);
        ClampCameraPosition(ref currentPosition);
        HandleZoom();
        HandleRotation();

        transform.position = currentPosition;
    }

    private void HandleMovement(ref Vector3 position)
    {
        Vector3 forwardDirection = transform.forward;

        forwardDirection.y = 0f;
        forwardDirection.Normalize();

        if (Input.GetKey(KeyCode.W))
        {
            position += forwardDirection * _moveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            position -= forwardDirection * _moveSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            position -= transform.right * _moveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            position += transform.right * _moveSpeed * Time.deltaTime;
        }
    }


    private void ClampCameraPosition(ref Vector3 position)
    {
        float terrainWidth = _terrain.terrainData.size.x;
        float terrainLength = _terrain.terrainData.size.z;

        position.x = Mathf.Clamp(position.x, 0, terrainWidth);
        position.z = Mathf.Clamp(position.z, 0, terrainLength);
    }
    private void HandleZoom()
    {
        float zoomAmount = Input.GetAxis(ZoomAxisName) * _zoomSpeed;
        Camera.main.fieldOfView -= zoomAmount;
        Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, MinZoom, MaxZoom);
    }

    private void HandleRotation()
    {
        if (Input.GetMouseButton(MiddleMouseButton)) 
        {
            float mouseX = Input.GetAxis(MouseXAxisName);
            float mouseY = Input.GetAxis(MouseYAxisName);

            transform.Rotate(Vector3.up, mouseX * _rotationSpeed, Space.World);
            transform.Rotate(Vector3.left, mouseY * _rotationSpeed, Space.Self);
        }
    }
}
