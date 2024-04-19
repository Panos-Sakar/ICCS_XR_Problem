using UnityEngine;

/// <summary>
/// Casts Rays to detect if a 'DynamicObject' was hit, and call the appropriate destroy method.
/// </summary>
public class MouseRaycaster : MonoBehaviour
{
    [SerializeField]
    private LayerMask _raycastMask;
    [SerializeField]
    private float _maxRaycastDistance = 100f;

    private void Update()
    {
        if (Input.GetMouseButtonUp(0)) CastAndDestroy();
    }

    private void CastAndDestroy()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray.origin, ray.direction, out hit, _maxRaycastDistance, _raycastMask))
        {
            var dynamicObject = hit.collider.gameObject.GetComponent<DynamicObject>();
            //Calls DestroyMe() instead of directly destroying the object because cleanup is required for DynamicObjects
            if(dynamicObject != null) dynamicObject.DestroyMe();
        }
    }
}