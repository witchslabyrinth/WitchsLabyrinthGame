using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    [SerializeField]
    private CameraEntity main;

    // Start is called before the first frame update
    void Awake()
    {
        // Default main camera to player's actor cam
        if(!main) 
        {
            Actor player = PlayerController.Instance.GetPlayer();
            main = player.actorCamera;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public CameraEntity GetMainCamera()
    {
        return main;
    }
}
