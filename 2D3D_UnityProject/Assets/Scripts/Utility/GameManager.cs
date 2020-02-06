using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Shows/hides mouse cursor
    /// </summary>
    /// <param name="active"></param>
    public static void SetCursorActive(bool active)
    {
        Cursor.lockState = active ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
