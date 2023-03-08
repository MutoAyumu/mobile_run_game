using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class CaneraFilter : MonoBehaviour
{
    [SerializeField] Material[] _filters;

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if(_filters.Length == 0) return;

        foreach(var filter in _filters)
            Graphics.Blit(src, dest, filter);
    }
}
