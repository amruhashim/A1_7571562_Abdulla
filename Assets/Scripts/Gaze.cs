using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gaze : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public float top_AngleLimit = -90f;
    public float bottom_AngleLimit = 90f;
    public float collisionOffset = 0.2f; // Offset to avoid clipping
    public Transform playerBody; // Reference to the player's body for rotation

    float xRotation = 0f;
    float yRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, top_AngleLimit, bottom_AngleLimit);
        yRotation += mouseX;

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);

        PreventCameraClipping();
    }

    private void PreventCameraClipping()
    {
        // Check if the camera is close to a wall
        RaycastHit hit;
        if (Physics.Raycast(playerBody.position, transform.position - playerBody.position, out hit, Vector3.Distance(playerBody.position, transform.position)))
        {
            // If the raycast hits a wall, adjust the camera's position
            if (hit.collider != null)
            {
                transform.position = hit.point - (transform.forward * collisionOffset);
            }
        }
    }
}
