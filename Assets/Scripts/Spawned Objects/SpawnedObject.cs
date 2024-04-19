using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This component sets up the object and moves it trough space.
/// All other necessary components that are required are created by this script except the MeshRenderer and MeshFilter to draw the object.
/// </summary>
[RequireComponent(typeof(MeshFilter))]
public class SpawnedObject : MonoBehaviour
{
    public enum MovementMode
    {
        None,
        Transform,
        RigidBody,
    }

    public SpawnedObjectData Data { get; private set; }
    public Vector3 StartPosition {get; private set;}
    //Cashed value to show on the debug inspector
    public double velocity {get; private set;}
    
    [Header("Movement")]
    [SerializeField]
    private MovementMode _movementMode;
    [SerializeField]
    private float _velocityScale = 1f;

    [Header("Y Offset")]
    [SerializeField]
    private float _yOffset = 1f;
    [SerializeField]
    private bool _isAdditive = false;
    
    private MeshFilter _meshFilter;

    public void Init(SpawnedObjectData data, Mesh objectMesh)
    {
        Data = data;
        gameObject.name = $"Object-{Data.object_id}";

        //Conversion of Up axis happens here
        StartPosition = new Vector3((float)Data.x, (float)Data.z, (float)Data.y);
        //Add _yOffset to start position. If _isAdditive is false, the original y position should be removed first.
        StartPosition +=  Vector3.up * (_isAdditive? _yOffset : _yOffset - StartPosition.y);
        transform.position = StartPosition;

        velocity = Data.velocity;

        _meshFilter = GetComponent<MeshFilter>();
        _meshFilter.mesh = objectMesh;

        //Add a rigid body if MovementMode requires it
        if(_movementMode == MovementMode.RigidBody)
        {
            var rigidBody = gameObject.AddComponent<Rigidbody>();
            rigidBody.useGravity = false;
            rigidBody.velocity = transform.forward * (float)(velocity * _velocityScale);
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
        if(transform.position.z - StartPosition.z >= 2f)
        {
            Destroy(gameObject);
            return;
        }

        if(_movementMode != MovementMode.Transform) return;

        transform.Translate(transform.forward * (float)(velocity * _velocityScale) * Time.deltaTime);
    }

    private void OnDestroy()
    {
        ObjectEventSystem.Current.OnSpawnedObjectDestroyed.Invoke(Data.object_id);
    }
}
