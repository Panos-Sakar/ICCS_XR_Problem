using System.Collections.Generic;
using UnityEngine;

public class BoundingBoxDebug : MonoBehaviour
{
    private Bounds _totalBoundSize;

    private void OnValidate()
    {
        var meshRenderers = gameObject.GetComponentsInChildren<MeshRenderer>();
        var boundsCenter = Vector3.zero;
        
        foreach (var meshRenderer in meshRenderers)
        {
            boundsCenter += meshRenderer.bounds.center;
        }

        boundsCenter /= meshRenderers.Length;
        _totalBoundSize = new Bounds(boundsCenter, Vector3.zero);

        foreach (var meshRenderer in meshRenderers)
        {
            _totalBoundSize.Encapsulate(meshRenderer.bounds);
        }
    }

    private void OnDrawGizmosSelected()
    {
        var color = Gizmos.color;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(_totalBoundSize.center, _totalBoundSize.size);
        Gizmos.color = color;
    }
}
