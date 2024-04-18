using UnityEngine;

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
            if(dynamicObject != null) dynamicObject.DestroyMe();
        }
    }
}