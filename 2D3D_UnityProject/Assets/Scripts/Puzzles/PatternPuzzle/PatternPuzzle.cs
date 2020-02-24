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

    [SerializeField]
    private AnimationCurve winAnimationCurve;
    [SerializeField]
    private float winAnimationJumpTime;
    [SerializeField]
    private float winAnimationTimeInBetween;

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
        _currentCube = initalCube;
        patternCubes[_currentCube].Select();
    }

    private void Update()
    {
        if (!ACubeIsAnimating())
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (CurrentCube - 1 >= 0)
                    CurrentCube--;
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                patternCubes[CurrentCube].Rotate(Direction.BACKWARD);
                StartCoroutine(CheckSolved());
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (CurrentCube + 1 < patternCubes.Count)
                    CurrentCube++;
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                SwapCubes(CurrentCube, CurrentCube - 1);

                // Current cube index changed, set it directly so select and deslect aren't called
                _currentCube = CurrentCube - 1;

                StartCoroutine(CheckSolved());
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                patternCubes[CurrentCube].Rotate(Direction.FORWARD);
                StartCoroutine(CheckSolved());
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                SwapCubes(CurrentCube, CurrentCube + 1);
                _currentCube = CurrentCube + 1;

                StartCoroutine(CheckSolved());
            }
            if (Input.GetKeyDown(KeyCode.Escape))
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
        patternCubes[_currentCube].Select();

        //patternCanvas.SetActive(true);
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
        Exit();
        StartCoroutine(WinAnimation());
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
