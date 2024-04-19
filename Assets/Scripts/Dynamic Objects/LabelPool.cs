using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Creates and manages 3D Labels and provides them to any script than needs one.
/// </summary>
public class LabelPool : MonoBehaviour
{
    [SerializeField]
    private Label _labelPrefab;
    [SerializeField]
    private int _capacity = 10;

    private Queue<Label> _availableLabels = new();
    private List<Label> _inUseLabel = new(); 

    private void Start()
    {
        for (int i = 0; i < _capacity; i++)
        {
            CreateLabel();
        }
    }

    private void CreateLabel()
    {
        var label = Instantiate(_labelPrefab);
        label.Init(this);
        label.transform.SetParent(this.transform);
        _availableLabels.Enqueue(label);
    }

    public Label GetLabel()
    {
        if(_availableLabels.Count <= 0)
        {
            Debug.LogWarning($"[LabelPool] Capacity reached! Creating a new one", this);
            CreateLabel();
        }
        
        var label = _availableLabels.Dequeue();
        label.ResetLabel();
        _inUseLabel.Add(label);

        return label;
    }

    public void ReturnLabel(Label label)
    {
        if(_inUseLabel.Contains(label))
        {
            _inUseLabel.Remove(label);
            _availableLabels.Enqueue(label);
        }
        else
        {
            Debug.LogWarning($"[LabelPool] Trying to return unknown label", label.gameObject);
        }
    }

    private void OnDestroy()
    {
        _inUseLabel.Clear();
        _availableLabels.Clear();
    }

    public void LabelDestroyed(Label label)
    {
        //If a script destroyed a label, remove it from the list to avoid null refs
        if(_inUseLabel.Contains(label))
        {
            Debug.LogWarning($"[LabelPool] Please do not destroy pooled objects! Call the ReturnToPool() function instead!", this);
            _inUseLabel.Remove(label);
        }
        
        //Queue should be recreated here to avoid null refs
        if(_availableLabels.Contains(label))
        {
            Debug.LogWarning($"[LabelPool] Label in available queue destroyed. Dequeue can return null object", this);
        }
    }
}