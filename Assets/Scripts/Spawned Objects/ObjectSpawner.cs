using System;
using System.Collections.Generic;
using UnityEngine;


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
        EventCaller.OnSpawnObject.AddListener(SpawnNewObject);
    }

    private void OnDisable()
    {
        EventCaller.OnSpawnObject.RemoveListener(SpawnNewObject);
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
            Debug.LogError($"Mesh with ID: {data.object_id} not found");
        }

    }
}
