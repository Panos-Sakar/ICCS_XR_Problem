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
}