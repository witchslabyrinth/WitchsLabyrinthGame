﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternPuzzle : MonoBehaviour
{
    [SerializeField]
    private GameObject patCamera;

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
        _currentCube = initalCube;
        patternCubes[_currentCube].Select();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!ACubeIsAnimating())
                CurrentCube = Mathf.Clamp(CurrentCube - 1, 0, 4);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (!ACubeIsAnimating())
            {
                patternCubes[CurrentCube].Rotate(Direction.BACKWARD);
                StartCoroutine(CheckSolved());
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!ACubeIsAnimating())
                CurrentCube = Mathf.Clamp(CurrentCube + 1, 0, 4);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (!ACubeIsAnimating())
            {
                SwapCubes(CurrentCube, CurrentCube - 1);

                // Current cube index changed, set it directly so select and deslect aren't called
                _currentCube = CurrentCube - 1;

                StartCoroutine(CheckSolved());
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (!ACubeIsAnimating())
            {
                patternCubes[CurrentCube].Rotate(Direction.FORWARD);
                StartCoroutine(CheckSolved());
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (!ACubeIsAnimating())
            {
                SwapCubes(CurrentCube, CurrentCube + 1);
                _currentCube = CurrentCube + 1;

                StartCoroutine(CheckSolved());
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!ACubeIsAnimating())
            {
                // Restore control to player actor
                Actor actor = PlayerController.Instance.GetPlayer();
                actor.Enable();

                // Restore actor swapping
                PlayerController.Instance.canSwap = true;

                patCamera.SetActive(false);
                this.enabled = false;

                patternCubes[_currentCube].Deselect();
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

    private void Solved()
    {
        Debug.Log("Solved");
    }
}
