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

        CreateAttributes();

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
        Debug.Log($"Scale: {scaleFactor} BB: {_boundingBox.size}");
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
        foreach (var attribute in Data.attributes)
        {
            var label = _labelPool.GetLabel();
            label.transform.SetParent(this.transform);
            label.Show(attribute);

            _labels.Add(label);
        }
    }

    private void DestroyAttributes()
    {
        foreach (var label in _labels)
        {
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
        }
        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, _targetScale, Time.deltaTime * LERP_SPEED);
        }
    }
}
