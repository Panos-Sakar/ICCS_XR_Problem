using UnityEngine;

public class MouseRaycaster : MonoBehaviour
{
    [SerializeField]
    private LayerMask _raycastMask;
    [SerializeField]
    private float _maxRaycastDistance = 100f;
    [SerializeField]
    private bool _drawDebugRay;

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

        if(_drawDebugRay)
        {
            var distance = hit.distance < 0.01? _maxRaycastDistance : hit.distance;
            Debug.DrawRay(ray.origin, ray.direction * distance, Color.red);
        }
    }
}