using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gaze : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public float top_AngleLimit = -90f;
    public float bottom_AngleLimit = 90f;
    float xRotation = 0f;
    float yRotation = 0f;   

    // Start is called before the first frame update
    void Start()
    {
        // locking the cursor at the middle screen
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // inputs
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotation around the x axis 
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, top_AngleLimit, bottom_AngleLimit);
        yRotation += mouseX;

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}
