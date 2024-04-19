using UnityEngine;
using MockGetRequest;
using System.Collections.Generic;

public class DynamicObjectSpawner : MonoBehaviour
{
    [SerializeField]
    private MockHandler _mocHandler;
    [SerializeField]
    private LabelPool _labelPool;
    [SerializeField]
    private ModelObjectDatabase _modelDatabase;

    private Dictionary<string, DynamicObject> _spawnedObjects = new();

    private void OnEnable()
    {
        ObjectEventSystem.Current.OnSpawnedObjectDestroyed.AddListener(SpawnNewObject);
    }

    private void OnDisable()
    {
        ObjectEventSystem.Current.OnSpawnedObjectDestroyed.RemoveListener(SpawnNewObject);
    }

    private void SpawnNewObject(int objectID)
    {
        _mocHandler.Create(objectID).OnSuccess(OnSuccessfulResponse).GET();
    }

    private void OnSuccessfulResponse(DynamicObjectData data)
    {
        if(_spawnedObjects.ContainsKey(data.model_name))
        {
            Debug.Log($"[DynamicObjectSpawner] Model with name: {data.model_name} already spawned. Ignoring...", this);
            return;
        }
        
        var newModel = Instantiate(_modelDatabase.GetModelByName(data.model_name));

        var dynamicObject =  newModel.AddComponent<DynamicObject>();
        dynamicObject.Init(data, _labelPool).OnDestroyRequest.AddListener(OnDestroyRequest);

        _spawnedObjects.Add(data.model_name, dynamicObject);
    }

    private void OnDestroyRequest(DynamicObject obj)
    {
        if(_spawnedObjects.ContainsKey(obj.Data.model_name))
        {
            _spawnedObjects.Remove(obj.Data.model_name);
        }
        
        obj.OnDestroyRequest.RemoveAllListeners();
        Destroy(obj.gameObject);
    }
}
