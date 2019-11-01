using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MaskTwoCenter : MonoBehaviour
{
    [SerializeField]
    private Material material;

    private void LateUpdate()
    {
        material.SetVector("_MaskTwoPosition", transform.position);
    }
}
