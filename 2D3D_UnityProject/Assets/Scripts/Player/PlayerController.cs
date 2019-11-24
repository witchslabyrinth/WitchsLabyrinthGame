using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
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

    ///    CAN PROBABLY DISCARD NEXT SECTION IN REFACTOR    ///

    /// <summary>
    /// is the player within talking distance of an npc
    /// </summary>
    private bool inDialogueZone;

    /// <summary>
    /// the id of the npc in range of the player
    /// </summary>
    private int dialoguePartner;

    /// <summary>
    /// reference to the orb the player is trying to find
    /// </summary>
    public GameObject orb;

    private bool inZodiacZone;

    ///    CAN PROBABLY DISCARD NEXT SECTION IN REFACTOR - END    ///

    // this is probably all bad, but should work for tomorrow's demo
    private ZodiacPuzzle zodiacPuzzle;

    private GameObject zodiacCam;

    public GameObject mainCam;
    // end of bad stuff

    void Start()
    {
        player = this.gameObject;
        controller = this.GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        float x = 0, z = 0, y = 0;

        // Used for animation
        movingType = 0;

        // Get movement based on camera perspective
        if (constrainedX)
        {
            if (Input.GetKey("d"))
            {
                z = 1f;
                movingType = 1f;
            }
            else if (Input.GetKey("a"))
            {
                z = -1f;
                movingType = 1f;
            }
        }
        else
        {
            if (Input.GetKey("d"))
            {
                x = 1f;
                movingType = 1f;
            }
            else if (Input.GetKey("a"))
            {
                x = -1f;
                movingType = 1f;
            }
            if (Input.GetKey("w") && !constrainedZ)
            {
                z = 1f;
                movingType = 1f;
            }
            else if (Input.GetKey("s") && !constrainedZ)
            {
                z = -1f;
                movingType = 1f;
            }
        }
        if (Mathf.Abs(x) > 0 || Mathf.Abs(z) > 0)
        {
            dir.Set(x, z);
        }

        // Apply gravity if airborne, or allow jump if grounded
        if (controller.isGrounded && !constrainedY)
        {
            if (Input.GetKeyDown("space"))
            {
                y = jumpSpeed;
            }
        }
        y -= gravity * Time.deltaTime;

        // Apply resulting movement based on camera perpective
        Vector3 movement = new Vector3(x * speed, y, z * speed);

        // 2D movement
        if (constrainedY || constrainedX || constrainedZ)
        {
            controller.Move(movement * Time.deltaTime);
        }
        // 3D movement (relative to camera facing direction)
        else
        {
            controller.Move(transform.TransformDirection(movement * Time.deltaTime));
        }

        // Set animations based on movement
        UpdateAnims(dir.x, dir.y, movingType);
        
        ///    CAN PROBABLY DISCARD NEXT SECTION IN REFACTOR    ///

        // Checks if player is near interactable
        CheckInteraction();
        
        // if (Input.GetKeyDown(KeyCode.E))
        // {
        //     if (inDialogueZone)
        //     {
        //         if (dialoguePartner == 0)
        //             orb.SetActive(true);
        //         LiarGameManager.Instance().CheckOrb(dialoguePartner);
        //     }
        // }

        ///    CAN PROBABLY DISCARD NEXT SECTION IN REFACTOR - END    ///
    }

    /// <summary>
    /// Checks if player is near interactable and pressing interact button
    /// </summary>
    private void CheckInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (inDialogueZone)
            {
                LiarGameManager.Instance().StartConversation(dialoguePartner);
                ghostCamera.GetComponent<PerspectiveCameraControl>().enabled = false;
                this.enabled = false;
            }
            else if (inZodiacZone)
            {
                zodiacPuzzle.enabled = true;
                zodiacCam.SetActive(true);
                mainCam.SetActive(false);
                this.enabled = false;
            }
        }
    }

    public void constrainX(bool set)
    {
        constrainedX = set;
    }

    public void constrainY(bool set)
    {
        constrainedY = set;
    }

    public void constrainZ(bool set)
    {
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

    ///    CAN PROBABLY DISCARD NEXT SECTION IN REFACTOR    ///

    /// <summary>
    /// called by OnTriggerEnter and OnTriggerExit of npc zones. Determines whether the player can talk or not
    /// </summary>
    /// <param name="withinZone">true on enter, false on exit</param>
    /// <param name="partner">ID of npc</param>
    public void SetInDialogueZone(bool withinZone, int partner)
    {
        inDialogueZone = withinZone;
        dialoguePartner = partner;
    }

     ///    CAN PROBABLY DISCARD NEXT SECTION IN REFACTOR - END    ///

     public void SetInZodiacZone(bool withinZone, ZodiacPuzzle zodPuz, GameObject zodCam)
     {
         inZodiacZone = withinZone;
         zodiacPuzzle = zodPuz;
         zodiacCam = zodCam;
     }
}