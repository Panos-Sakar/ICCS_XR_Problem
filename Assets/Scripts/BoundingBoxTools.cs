using UnityEngine;

/// <summary>
/// Provides functions to calculate the bounding box of an objects based on all Mesh data and scale factors to resize an object to fit in another bounding box.
/// </summary>
public static class BoundBoxTools
{
    public static Bounds GetMeshBoundBox(GameObject gameObject)
    {
        var meshRenderers = gameObject.GetComponentsInChildren<MeshRenderer>();
        var boundsCenter = Vector3.zero;
        
        //Center is the average of all centers
        foreach (var meshRenderer in meshRenderers)
        {
            boundsCenter += meshRenderer.bounds.center;
        }
        boundsCenter /= meshRenderers.Length;
        
        //Calculate overall bounding box
        var boundBox = new Bounds(boundsCenter, Vector3.zero);
        foreach (var meshRenderer in meshRenderers)
        {
            boundBox.Encapsulate(meshRenderer.bounds);
        }

        return boundBox;
    }

    public static float CalculateScaleFactor(Bounds boundBox, float targetDimension)
    {
        //Find the max dimension of the bounding box
        var maxDimension = Mathf.Max(boundBox.size.x, boundBox.size.y);
        maxDimension = Mathf.Max(maxDimension, boundBox.size.z);
        
        //Do not divide by 0
        if(Mathf.Abs(maxDimension) <= Mathf.Epsilon)
        {
            return 1f;
        }
        
        //Return the scale amount that is required to shrink maxDimension to the targetDimension
        return targetDimension/maxDimension;
    }
}