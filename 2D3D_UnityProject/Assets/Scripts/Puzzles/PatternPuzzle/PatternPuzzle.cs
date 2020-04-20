using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class PatternPuzzle : MonoBehaviour
{
   [Header("Wwise")]
    /// <summary>
    /// Set Wwise variables
    /// </summary>
    /// <param name="paused">Set Wwise variables for sounds here</param>
    public AK.Wwise.Event Block_Move;
    public AK.Wwise.Event Block_Interact;
    public AK.Wwise.Event Block_Win;
   
   
   
   
   
    [SerializeField]
    private List<PatternCube> patternCubes;

    [SerializeField]
    private GameObject patternCanvas;

    [SerializeField]
    [Tooltip("The cube that will start selected when the scene loads. 0 for leftmost cube, 4 for rightmost cube.")]
    [Range(0, 4)]
    private int initalCube;

    [SerializeField]
    private AnimationCurve winAnimationCurve;
    [SerializeField]
    private float winAnimationJumpTime;
    [SerializeField]
    private float winAnimationTimeInBetween;

    public AK.Wwise.Event puzzleSolved;

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

    /// <summary>
    /// Indicates whether player has solved the puzzle
    /// </summary>
    public bool solved = false;


    // TODO: find a better place for this
    // [SerializeField]
    // private YarnProgram scene3;
    [SerializeField]
    private CameraEntity scene3Cam;

    private void Start()
    {
        _currentCube = initalCube;
        patternCubes[_currentCube].Select();


        // DialogueRunner dialogueRunner = FindObjectOfType<Yarn.Unity.DialogueRunner>();
        // dialogueRunner.Add(scene3);
    }

    private void Update()
    {
        if (!ACubeIsAnimating() && !solved)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                Block_Move.Post(gameObject); //Wwise
                if (CurrentCube - 1 >= 0)
                    CurrentCube--;
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                Block_Move.Post(gameObject); //Wwise
                patternCubes[CurrentCube].Rotate(Direction.BACKWARD);
                StartCoroutine(CheckSolved());
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                Block_Move.Post(gameObject); //Wwise
                if (CurrentCube + 1 < patternCubes.Count)
                    CurrentCube++;
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                Block_Move.Post(gameObject); //Wwise
                SwapCubes(CurrentCube, CurrentCube - 1);

                // Current cube index changed, set it directly so select and deslect aren't called
                _currentCube = CurrentCube - 1;

                StartCoroutine(CheckSolved());
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                Block_Move.Post(gameObject); //Wwise
                patternCubes[CurrentCube].Rotate(Direction.FORWARD);
                StartCoroutine(CheckSolved());
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                Block_Move.Post(gameObject); //Wwise 
                SwapCubes(CurrentCube, CurrentCube + 1);
                _currentCube = CurrentCube + 1;

                StartCoroutine(CheckSolved());
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                Exit();
            }
        }
    }

    private void SwapCubes(int cubeIndexOne, int cubeIndexTwo)
    {
        // Tell cubes to animate
        Vector3 cubeOnePosition = patternCubes[cubeIndexOne].gameObject.transform.localPosition;
        Vector3 cubeTwoPosition = patternCubes[cubeIndexTwo].gameObject.transform.localPosition;
        // Ignore Y value so they don't raise or lower
        patternCubes[cubeIndexOne].Translate(cubeOnePosition, new Vector3(cubeTwoPosition.x, cubeOnePosition.y, cubeTwoPosition.z));
        patternCubes[cubeIndexTwo].Translate(cubeTwoPosition, new Vector3(cubeOnePosition.x, cubeTwoPosition.y, cubeOnePosition.z));

        // Swap cubes in patternCubes
        PatternCube temp = patternCubes[cubeIndexOne];
        patternCubes[cubeIndexOne] = patternCubes[cubeIndexTwo];
        patternCubes[cubeIndexTwo] = temp;
    }

    private void OnEnable()
    {
        _currentCube = initalCube;

        StartCoroutine(WaitThenSelect());
        //patternCanvas.SetActive(true);
    }

    private IEnumerator WaitThenSelect()
    {
        while (ACubeIsAnimating())
            yield return null;

        patternCubes[_currentCube].Select();
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

    private IEnumerator CheckSolved()
    {
        bool ready = false;
        while (!ready)
        {
            ready = !ACubeIsAnimating();
            yield return null;
        }

        bool correct = true;
        for (int i = 0; i < patternCubes.Count; i++)
        {
            if (!patternCubes[i].Correct(i))
                correct = false;
        }

        if (correct)
            Solved();
    }

    private bool ACubeIsAnimating()
    {
        bool animating = false;
        foreach (PatternCube pc in patternCubes)
        {
            if (pc.animating)
            {
                animating = true;
                break;
            }
        }
        return animating;
    }

    private void Exit()
    {
        // Restore control to player actor
        Actor player = PlayerController.Instance.GetPlayer();
        player.Enable();
        CameraController.Instance.SetMainCamera(player.actorCamera);

        // Restore actor swapping
        PlayerController.Instance.canSwap = true;

        // Disable puzzle and deselect cube
        this.enabled = false;
        patternCubes[_currentCube].Deselect();
    }

    private void Solved()
    {
        Debug.Log("Solved");
        puzzleSolved.Post(gameObject); //Wwise
        solved = true;
        patternCubes[_currentCube].Deselect();
        StartCoroutine(WinAnimation());

        FindObjectOfType<DialogueRunner>().StartDialogue("Switcher-3&5");
        CameraController.Instance.SetMainCamera(scene3Cam);
    }

    private IEnumerator WinAnimation()
    {
        while (ACubeIsAnimating())
          
            yield return null;

        for (int i = 0; i < patternCubes.Count; i++)
        {
            patternCubes[i].Jump(winAnimationCurve, winAnimationJumpTime);
            yield return new WaitForSeconds(winAnimationTimeInBetween);
        }
    }
}
