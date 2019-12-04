using UnityEngine;
using System.Collections;

public class ControlCamera : MonoBehaviour //this script controls the facing of the camera with the mouse position
{

    public float HorizontalSpeed = 0.0f; //H sensitivity
    public float VerticalSpeed = 0.0f;   //V Sensitivity

    private float yaw = 0.0f; //turning on vertical axis (moving left-right)
    private float pitch = 0.0f; //turning on horizontal axis (moving up-down)

    void Start()
    {
        //Cursor.visible = false; //make cursor invisible
    }

    void Update()
    {
        yaw += HorizontalSpeed * Input.GetAxis("Mouse X"); //updates H facing based on mouse position
        pitch -= VerticalSpeed * Input.GetAxis("Mouse Y"); //updates V facing based on mouse position

        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
    }
}