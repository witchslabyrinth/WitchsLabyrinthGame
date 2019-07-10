using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    protected GameObject player;
    protected CharacterController controller;
    public GameObject ghostCamera;

    public float speed = 10f;
    public float jumpSpeed = 20f;
    public float rotateSpeed = 90f;
    public float gravity = 9.8f;
    
    void Start()
    {
        player = this.gameObject;
        controller = this.GetComponent<CharacterController>();

    }


    void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(x * speed, 0, z * speed).normalized;
        controller.Move(transform.TransformDirection(movement));
    }
}
