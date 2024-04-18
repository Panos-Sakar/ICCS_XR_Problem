using UnityEngine;
using MockGetRequest;
using System.Collections.Generic;

public class DynamicObject : MonoBehaviour
{
    public DynamicObjectData Data { get; private set; }
    
    private LabelPool _labelPool;

    private List<Label> _labels;

    public void Init(DynamicObjectData data, LabelPool labelPool)
    {
        Data = data;
        _labelPool = labelPool;
        _labels = new List<Label>();

        transform.position = data.position;
        foreach (var attribute in data.attributes)
        {
            var label = _labelPool.GetLabel();
            label.transform.SetParent(this.transform);
            label.Show(attribute);

            _labels.Add(label);
        }
    }
}
