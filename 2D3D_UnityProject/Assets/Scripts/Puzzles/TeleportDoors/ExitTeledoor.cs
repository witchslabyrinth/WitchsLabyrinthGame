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
        if (other.GetComponent<PlayerController>() == null)
        {
            return;
        }
        //otherwise continue
        SceneManager.LoadScene("Scenes/Ukiyo-e Environment");
    }
}
