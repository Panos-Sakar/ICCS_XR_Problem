using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class SpawnedObject : MonoBehaviour
{
    public enum MovementMode
    {
        Transform,
        RigidBody,
    }

    public SpawnedObjectData Data { get; private set; }
    
    [Header("Movement")]
    [SerializeField]
    private MovementMode _movementMode;
    [SerializeField]
    public float _velocityScale = 1f;

    [Header("Y Offset")]
    [SerializeField]
    private float _yOffset = 1f;
    [SerializeField]
    public bool _isAdditive = false;
    
    private Vector3 _startPosition;
    private MeshFilter _meshFilter;

    public void Init(SpawnedObjectData data, Mesh objectMesh)
    {
        Data = data;
        gameObject.name = $"Object-{Data.object_id}";

        //Conversion of Up axis happens here
        _startPosition = new Vector3((float)Data.x, (float)Data.z, (float)Data.y);
        //if _isAdditive is equal to true, keep original Y value and add _yOffset. Otherwise use _yOffset.
        _startPosition =  Vector3.up * (_isAdditive? _yOffset + _startPosition.y : _yOffset);
        transform.position = _startPosition;

        _meshFilter = GetComponent<MeshFilter>();
        _meshFilter.mesh = objectMesh;

        if(_movementMode == MovementMode.RigidBody)
        {
            var rigidBody = gameObject.AddComponent<Rigidbody>();
            rigidBody.useGravity = false;
            rigidBody.velocity = transform.forward * (float)(Data.velocity * _velocityScale);
        }

#if VISUALIZE_CONTOUR_POINTS
        var lineRenderer = GetComponent<LineRenderer>();
        if(lineRenderer)
        {
            GetComponent<MeshRenderer>().enabled = false;
            lineRenderer.positionCount = Data.contour_points.Count;
            lineRenderer.useWorldSpace = false;
            lineRenderer.loop = true;
            
            int i = 0;
            foreach (var contourPoint in Data.contour_points)
            {
                lineRenderer.SetPosition(i, new Vector3((float)contourPoint.x, 0, (float)contourPoint.y));
                i++;
            }
        }
#endif
    
    }

    private void Update()
    {
        if(transform.position.z - _startPosition.z >= 2f)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void FixedUpdate()
    {
        if(_movementMode != MovementMode.Transform) return;

        transform.Translate(transform.forward * (float)(Data.velocity * _velocityScale));
    }
}
