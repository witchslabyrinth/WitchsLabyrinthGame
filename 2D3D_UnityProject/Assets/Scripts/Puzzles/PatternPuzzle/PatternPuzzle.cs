using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternPuzzle : MonoBehaviour
{
    [SerializeField]
    private List<PatternCube> patternCubes;

    [SerializeField]
    private GameObject patternCanvas;

    [SerializeField]
    [Tooltip("The cube that will start selected when the scene loads. 0 for leftmost cube, 4 for rightmost cube.")]
    [Range(0, 4)]
    private int initalCube;

    private int _currentCube;
    private int CurrentCube
    {
        get => _currentCube;
        set
        {
            patternCubes[_currentCube].Deselect();
            _currentCube = value;
            patternCubes[_currentCube].Select();
        }
    }

    private void Start()
    {
        CurrentCube = initalCube;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            patternCubes[CurrentCube].Rotate(Direction.BACKWARD);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            CurrentCube = Mathf.Clamp(CurrentCube - 1, 0, 4);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            patternCubes[CurrentCube].Rotate(Direction.FORWARD);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            CurrentCube = Mathf.Clamp(CurrentCube + 1, 0, 4);
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            // Restore control to player actor
            Actor actor = PlayerController.Instance.GetPlayer();
            actor.Enable();
            CameraController.Instance.SetMainCamera(actor.actorCamera);

            // Restore actor swapping
            PlayerController.Instance.canSwap = true;
            
            // Disable puzzle
            this.enabled = false;

            patternCubes[_currentCube].Deselect();
        }
    }

    private void OnEnable()
    {
        CurrentCube = _currentCube;

        patternCanvas.SetActive(true);
    }

    public enum Direction
    {
        FORWARD = 1,
        BACKWARD = -1
    }

    private void OnDisable()
    {
        patternCanvas.SetActive(false);
    }
}
