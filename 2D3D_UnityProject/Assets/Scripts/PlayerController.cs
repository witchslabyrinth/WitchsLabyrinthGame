﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(CharacterController))]
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

    private Animator anim;
    private float movingType;
    private Vector2 dir;

    void Start () {
        player = this.gameObject;
        controller = this.GetComponent<CharacterController> ();
        anim = GetComponent<Animator>();
    }

    void Update () {
        float x = 0, z = 0, y = 0;
        movingType = 0;

        if (constrainedX) {
            if (Input.GetKey ("d")) {
                z = 1f;
                movingType = 1f;
            } else if (Input.GetKey ("a")) {
                z = -1f;
                movingType = 1f;
            }
        } else {
            if (Input.GetKey ("d")) {
                x = 1f;
                movingType = 1f;
            } else if (Input.GetKey ("a")) {
                x = -1f;
                movingType = 1f;
            }
            if (Input.GetKey ("w") && !constrainedZ) {
                z = 1f;
                movingType = 1f;
            } else if (Input.GetKey ("s") && !constrainedZ) {
                z = -1f;
                movingType = 1f;
            }
        }
        if (Mathf.Abs(x) > 0 || Mathf.Abs(z) > 0)
        {
            dir.Set(x, z);
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

        UpdateAnims(dir.x, dir.y, movingType);
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

    /// <summary>
    /// Update animation parameters
    /// </summary>
    /// <param name="xdir">looking left or right, from -1 to 1</param>
    /// <param name="ydir">looking down or up, from -1 to 1</param>
    /// <param name="moveType">0 - Standing, 1 - Walking, 3 - Running</param>
    private void UpdateAnims(float xdir, float ydir, float moveType)
    {
        anim.SetFloat("MoveX", xdir);
        anim.SetFloat("MoveY", ydir);
        anim.SetFloat("Speed", moveType);
    }
}