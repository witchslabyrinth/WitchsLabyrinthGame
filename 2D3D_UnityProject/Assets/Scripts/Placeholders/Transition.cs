using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    public GameObject player;

    public float distanceToTransition;

    private Vector3 startPosition;

    public int sceneIndex;

    void Start()
    {
        // track player start pos
        startPosition = player.transform.position;
    }

    void Update()
    {
        // Get distance from start point (ignoring change in z)
        Vector3 distanceFromStart = player.transform.position - startPosition;
        distanceFromStart.z = 0;

        // Load scene if player has moved far enough
        if(distanceFromStart.magnitude >= distanceToTransition) {
            SceneManager.LoadScene(sceneIndex);
        }
    }
}
