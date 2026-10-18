// Sarah Hannah Langdon
// 2026-04-10
// Advanced Unity Programming
// https://www.youtube.com/watch?v=f473C43s8nE

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    // sensitivity
    public float sensX;
    public float sensY;

    public Transform orientation;

    // rotation of camera
    float xRotation;
    float yRotation;

    // Cursor is locked in the middle of screen and invisible
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // Get Mouse update
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;

        xRotation -= mouseY;

        // Clamps the rotation to up and down
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // rotate cam and orientation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }

}
