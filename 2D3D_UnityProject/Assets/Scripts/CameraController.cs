﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour {

    public GameObject ghostCamera;
    public GameObject player;
    protected Camera myCam;

    protected Vector3 rightRotation = new Vector3 (0f, -90f, 0f);
    protected Vector3 topRotation = new Vector3 (90f, 0f, 0f);
    protected Vector3 backRotation = new Vector3 (0f, 0f, 0f);

    public float orthoOffset = 2.0f;
    public float headway = 2.0f;

    protected enum CameraViews {
        RIGHT,
        TOP,
        BACK,
        PERSPECTIVE,
    }
    protected CameraViews currentView;

    void Start () {
        this.transform.SetParent (ghostCamera.transform);
        myCam = this.GetComponent<Camera> ();
        currentView = CameraViews.PERSPECTIVE;
    }

    // Update is called once per frame
    void Update () {
        switch (currentView) {
        case CameraViews.PERSPECTIVE:
            PerspectiveUpdate ();
            break;
        case CameraViews.RIGHT:
            OrthographicUpdate(new Vector3(headway, 4f, headway));
            break;
        case CameraViews.TOP:
            OrthographicUpdate(new Vector3(0, 20f, 2f));
            break;
        case CameraViews.BACK:
            OrthographicUpdate(new Vector3(headway, 4f, -orthoOffset));
            break;
        }
    }

    /******  SET VARIOUS CAMERA VIEWS ******/
    public void SetToPerspective () {
        currentView = CameraViews.PERSPECTIVE;
        myCam.orthographic = false;
        this.transform.SetParent (ghostCamera.transform);
    }

    public void SetToRightOrtho () {
        currentView = CameraViews.RIGHT;
        myCam.orthographic = true;
        this.transform.eulerAngles = rightRotation;
    }

    public void SetToTopOrtho () {
        currentView = CameraViews.TOP;
        myCam.orthographic = true;
        this.transform.eulerAngles = topRotation;
    }

    public void SetToBackOrtho () {
        currentView = CameraViews.BACK;
        myCam.orthographic = true;
        this.transform.eulerAngles = backRotation;
    }

    /******  UPDATE VARIOUS CAMERA VIEWS ******/
    private void PerspectiveUpdate () {
        this.transform.position = ghostCamera.transform.position;
        this.transform.rotation = ghostCamera.transform.rotation;
    }

    private void OrthographicUpdate(Vector3 cameraOffset)
    {
        transform.position = player.transform.position + cameraOffset;
    }
}