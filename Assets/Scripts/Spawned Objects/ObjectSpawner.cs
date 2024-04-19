using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Creates a SpawnedObject if an event from <see cref="ObjectEventSystem.cs"/> was called.
/// Depends on <see cref="ObjectEventSystem.cs"/> to function.
/// </summary>
public class ObjectSpawner : MonoBehaviour
{
    [Serializable]
    public class ObjectMeshDada
    {
        public Mesh ObjectMesh;
        public int ObjectID;
    }

    [SerializeField]
    private SpawnedObject _objectPrefab;
    [SerializeField]
    private ObjectMeshDada[] _meshData;
    private Dictionary<int,Mesh> _loadedMeshData = new();

    private void Awake()
    {
        foreach (var objectMeshData in _meshData)
        {
            _loadedMeshData.Add(objectMeshData.ObjectID, objectMeshData.ObjectMesh);
        }
    }

    private void OnEnable()
    {
        ObjectEventSystem.Current.OnSpawnObject.AddListener(SpawnNewObject);
    }

    private void OnDisable()
    {
        ObjectEventSystem.Current.OnSpawnObject.RemoveListener(SpawnNewObject);
    }

    private void SpawnNewObject(SpawnedObjectData data)
    {
        if(_loadedMeshData.TryGetValue(data.object_id, out var mesh))
        {
            SpawnedObject newObject = Instantiate(_objectPrefab);
            newObject.Init(data, mesh);
        }
        else
        {
            Debug.LogError($"[ObjectSpawner] Mesh with ID: {data.object_id} not found", this);
        }

    }
}
