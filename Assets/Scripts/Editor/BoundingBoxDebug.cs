using System.Collections.Generic;
using UnityEngine;

public class BoundingBoxDebug : MonoBehaviour
{
    private void OnDrawGizmosSelected()
    {
        var totalBoundSize = BoundBoxTools.GetMeshBoundBox(gameObject);

        var color = Gizmos.color;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(totalBoundSize.center, totalBoundSize.size);
        Gizmos.color = color;
    }
}
