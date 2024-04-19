using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Reads and displays the "NextSpawnTime" reading.
/// Depends on <see cref="ObjectEventSystem.cs"/> to function.
/// </summary>
public class DebugCanvasManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _label;
    [SerializeField]
    private string _prefix;
    [SerializeField]
    private float _updateInterval = 0.01f;

    private float _timer;

    private void Update()
    {
        _timer += Time.deltaTime;

        if(_timer > _updateInterval)
        {
            _timer = 0;
            _label.text = $"{_prefix}{ObjectEventSystem.Current.NextSpawnTime:00.00}";
        }
    }
}
