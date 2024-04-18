using UnityEngine;
using MockGetRequest;
using System.Collections.Generic;

public class DynamicObject : MonoBehaviour
{
    private const float LERP_SPEED = 10;

    public DynamicObjectData Data { get; private set; }
    
    private LabelPool _labelPool;
    private List<Label> _labels;
    private Vector3 _targetScale;
    private bool _lerpToTargetScale;
    private Bounds _boundingBox;

    public void Init(DynamicObjectData data, LabelPool labelPool)
    {
        Data = data;
        _labelPool = labelPool;
        _labels = new List<Label>();

        //Move object to position
        transform.position = Data.position;

        CreateAttributes();
    }

    private void Start()
    {
        //Calculate the desired scale
        _boundingBox = BoundBoxTools.GetMeshBoundBox(this.gameObject);
        var scaleFactor = BoundBoxTools.CalculateScaleFactor(_boundingBox, 0.5f);
        Debug.Log($"Scale: {scaleFactor} BB: {_boundingBox.size}");
        _targetScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);

        //Scale down model to then lerp to the desired scale
        transform.localScale = Vector3.zero;
        _lerpToTargetScale = true;
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
