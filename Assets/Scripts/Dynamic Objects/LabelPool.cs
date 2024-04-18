using System.Collections.Generic;
using UnityEngine;

public class LabelPool : MonoBehaviour
{
    public Label GetLabel()
    {
        var obj = new GameObject();
        var label = obj.AddComponent<Label>();
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