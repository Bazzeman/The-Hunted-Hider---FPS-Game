using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCam : MonoBehaviour
{
    public Transform player;
    public float mouseX;
    public float mouseY;
    public float rotationX;
    public float sensitivity = 85F;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        transform.localRotation = Quaternion.Euler(Mathf.Clamp(rotationX -= mouseY, -90F, 90F), 0, 0);
        player.Rotate(Vector3.up * mouseX);
    }
}
