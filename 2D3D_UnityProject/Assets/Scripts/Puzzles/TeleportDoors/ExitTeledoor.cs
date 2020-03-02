using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitTeledoor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //Exit if collided with non-player object
        Actor objectCollided = other.GetComponent<Actor>();
        if (objectCollided == null || objectCollided != PlayerController.Instance.GetPlayer())
        {
            return;
        }
        //otherwise continue
        SceneLoader.LoadScene(SCENE_ID.FINAL);
    }
}
