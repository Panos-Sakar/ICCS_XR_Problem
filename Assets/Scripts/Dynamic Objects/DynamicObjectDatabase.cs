using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Create a scriptable object from this class to use as a database to dynamically spawn objects based on Object Names
/// </summary>
[CreateAssetMenu(fileName = "ModelDatabase", menuName = "ICCS/Create Model Database", order = 1)]
public class ModelObjectDatabase : ScriptableObject
{
    [Serializable]
    public class ModelDada
    {
        public GameObject Object;
        public string Name;
    }

    [SerializeField]
    private ModelDada[] _models;

    private Dictionary<string,GameObject> _availableModels;

    public GameObject GetModelByName(string objectName)
    {
        if(_availableModels == null) CreateDictionary();

        if(_availableModels.TryGetValue(objectName, out var dynamicObject))
        {
            return dynamicObject;
        }
        
        Debug.LogError($"[DynamicObjectDatabase] No Model with name '{objectName}' found in database", this);
        return null;
    }

    private void CreateDictionary()
    {
        _availableModels = new Dictionary<string,GameObject>();

        foreach (var model in _models)
        {
            if(_availableModels.TryAdd(model.Name, model.Object) == false)
            {
                Debug.LogError($"[DynamicObjectDatabase]  Duplicate key: {model.Name}", this);
            }
        }
    }
}