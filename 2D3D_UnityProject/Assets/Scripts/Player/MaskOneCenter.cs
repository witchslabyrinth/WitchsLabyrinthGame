using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MaskOneCenter : MonoBehaviour
{
    [SerializeField]
    private Material material;

    private void LateUpdate()
    {
        material.SetVector("_MaskOnePosition", transform.position);
    }
}
