using UnityEngine;
using MockGetRequest;
using System.Collections.Generic;
using UnityEngine.Events;

public class DynamicObject : MonoBehaviour
{
    private const float LERP_SPEED = 10;

    public UnityEvent<DynamicObject> OnDestroyRequest = new();

    public DynamicObjectData Data { get; private set; }
    
    private LabelPool _labelPool;
    private List<Label> _labels;
    private Vector3 _targetScale;
    private bool _lerpToTargetScale;
    private Bounds _boundingBox;

    public DynamicObject Init(DynamicObjectData data, LabelPool labelPool)
    {
        Data = data;
        _labelPool = labelPool;
        _labels = new List<Label>();

        //Move object to position
        transform.position = Data.position;

        gameObject.layer = LayerMask.NameToLayer("DynamicObjects");

        return this;
    }

    /// <summary>
    /// Does not directly destroy this GameObject
    /// </summary>
    public void DestroyMe()
    {
        OnDestroyRequest.Invoke(this);
        _lerpToTargetScale = false;
        OnDestroyRequest.RemoveAllListeners();
    }

    private void Start()
    {
        //Calculate the desired scale
        _boundingBox = BoundBoxTools.GetMeshBoundBox(this.gameObject);
        var scaleFactor = BoundBoxTools.CalculateScaleFactor(_boundingBox, 0.5f);
        _targetScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);

        var boxCollider =gameObject.AddComponent<BoxCollider>();
        boxCollider.center = _boundingBox.center - transform.localPosition;
        boxCollider.size = _boundingBox.size;

        //Scale down model to then lerp to the desired scale
        transform.localScale = Vector3.zero;
        _lerpToTargetScale = true;
    }

    private void OnDestroy()
    {
        DestroyAttributes();
    }

    private void CreateAttributes()
    {
        var angle  = 0f;
        //Calculate angle step on the unit circle based on number of labels
        var nextAngle = 2*Mathf.PI/Data.attributes.Count;

        foreach (var attribute in Data.attributes)
        {
            //Calculate coordinates around the circle with perimeter 0.3f
            var perimeter = 0.4f;
            var x = Mathf.Cos(angle)*perimeter;
            var y = Mathf.Sin(angle)*perimeter;
            Vector3 offset = new Vector3(x, 0f , y);

            var label = _labelPool.GetLabel();
            label.Show(attribute).Follow(this.transform, offset);

            _labels.Add(label);
            angle += nextAngle;
        }
    }

    private void DestroyAttributes()
    {
        foreach (var label in _labels)
        {
            if(label == null) continue;
            label.ReturnToPool();
        }
        _labels.Clear();

    }

    private void Update()
    {
        if(_lerpToTargetScale) LerpScale();
    }

    private void LerpScale()
    {
        if(Mathf.Abs(_targetScale.x - transform.localScale.x) < 0.001f)
        {
            _lerpToTargetScale = false;
            transform.localScale = _targetScale;
            CreateAttributes();
        }
        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, _targetScale, Time.deltaTime * LERP_SPEED);
        }
    }
}
