using System.Collections.Generic;
using UnityEngine;

public class LabelPool : MonoBehaviour
{
    [SerializeField]
    private Label _labelPrefab;
    public Label GetLabel()
    {
        var label = Instantiate(_labelPrefab);
        label.Init(this);
        return label;
    }

    public void ReturnLabel(Label label)
    {
        Destroy(label.gameObject);
    }

    public void LabelDestroyed(Label label)
    {
        Debug.LogWarning($"Please do not destroy pooled objects! Call the ReturnToPool() function instead!");
    }
}