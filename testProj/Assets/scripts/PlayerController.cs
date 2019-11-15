using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    protected GameObject player;
    protected CharacterController controller;
    public GameObject ghostCamera;

    public float speed = 0.5f;
    public float jumpSpeed = 100f;
    public float gravity = .98f;

    private bool constrainedX = false;
    private bool constrainedY = false;
    private bool constrainedZ = false;

    void Start () {
        player = this.gameObject;
        controller = this.GetComponent<CharacterController> ();
    }

    void Update () {
        float x = 0, z = 0, y = 0;
        if (constrainedX) {
            if (Input.GetKey ("d")) {
                z = 1f;
            } else if (Input.GetKey ("a")) {
                z = -1f;
            }
        } else {
            if (Input.GetKey ("d")) {
                x = 1f;
            } else if (Input.GetKey ("a")) {
                x = -1f;
            }
            if (Input.GetKey ("w") && !constrainedZ) {
                z = 1f;
            } else if (Input.GetKey ("s") && !constrainedZ) {
                z = -1f;
            }
        }

        if (controller.isGrounded && !constrainedY) {
            if (Input.GetKeyDown ("space")) {
                y = jumpSpeed;
            }
        }

        y -= gravity * Time.deltaTime;

        Vector3 movement = new Vector3 (x * speed, y, z * speed);

        if (constrainedY || constrainedX || constrainedZ) {
            controller.Move (movement * Time.deltaTime);
        } else {
            controller.Move (transform.TransformDirection (movement * Time.deltaTime));
        }
    }

    public void constrainX (bool set) {
        constrainedX = set;
    }

    public void constrainY (bool set) {
        constrainedY = set;
    }

    public void constrainZ (bool set) {
        constrainedZ = set;
    }
}