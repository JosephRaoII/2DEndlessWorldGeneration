using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSCamera : MonoBehaviour
{
    public float panSpeed = 20f; // Speed at which the camera pans
    public float panBorderThickness = 10f; // Distance from screen edges that triggers panning
    public float zoomSpeed = 20f; // Speed at which the camera zooms
    public float minZoomSize = 5f; // Minimum field of view for zooming in
    public float maxZoomSize = 25f; // Maximum field of view for zooming out


    void Update()
    {
        // Handle keyboard movement
        HandleKeyboardMovement();

        // Handle mouse movement
        HandleMouseMovement();

        // Handle zooming
        HandleZoom();
    }

    void HandleKeyboardMovement()
    {
        // Get the input axis values for horizontal and vertical movement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Calculate the desired translation based on input and pan speed
        Vector3 translation = new Vector3(horizontal, vertical, 0f) * panSpeed * Time.deltaTime;

        // Apply the translation to the camera's position
        transform.Translate(translation, Space.World);
    }

    void HandleMouseMovement()
    {
        // Get the mouse position in screen coordinates
        Vector3 mousePosition = Input.mousePosition;

        // Get the camera's viewport dimensions
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Calculate the normalized mouse position
        float normalizedX = mousePosition.x / screenWidth;
        float normalizedY = mousePosition.y / screenHeight;

        // Calculate the movement direction based on the mouse position
        float horizontal = 0f;
        float vertical = 0f;

        if (normalizedX < panBorderThickness / screenWidth)
        {
            horizontal = -1f;
        }
        else if (normalizedX > 1f - panBorderThickness / screenWidth)
        {
            horizontal = 1f;
        }

        if (normalizedY < panBorderThickness / screenHeight)
        {
            vertical = -1f;
        }
        else if (normalizedY > 1f - panBorderThickness / screenHeight)
        {
            vertical = 1f;
        }

        // Calculate the desired translation based on input and pan speed
        Vector3 translation = new Vector3(horizontal, vertical, 0f) * panSpeed * Time.deltaTime;

        // Apply the translation to the camera's position
        transform.Translate(translation, Space.World);
    }

    void HandleZoom()
    {
        // Get the scroll wheel input value
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        // Calculate the desired zoom amount based on input and zoom speed
        float zoomAmount = scrollInput * zoomSpeed * Time.deltaTime;

        // Calculate the new orthographic size for the camera
        float newSize = GetComponent<Camera>().orthographicSize - zoomAmount;

        // Clamp the new size within the min and max zoom size values
        newSize = Mathf.Clamp(newSize, minZoomSize, maxZoomSize);

        // Apply the new size to the camera
        GetComponent<Camera>().orthographicSize = newSize;
    }
}
