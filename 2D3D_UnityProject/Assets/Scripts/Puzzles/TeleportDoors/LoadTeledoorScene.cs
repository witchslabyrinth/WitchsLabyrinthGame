using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadTeledoorScene : MonoBehaviour
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
        if (other.TryGetComponent<Actor>(out Actor actor) != PlayerController.Instance.GetActor())
        {
            return;
        }
        //otherwise continue
        SceneManager.LoadScene("Scenes/Puzzles/TeledoorPuzzle");
    }
}
