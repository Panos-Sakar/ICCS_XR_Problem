using UnityEngine;


public class ObjectSpawner : MonoBehaviour
{
    [SerializeField]
    private SpawnedObject _objectPrefab;

    private void OnEnable()
    {
        EventCaller.OnSpawnObject.AddListener(SpawnNewObject);
    }

    private void SpawnNewObject(SpawnedObjectData data)
    {
        SpawnedObject newObject = Instantiate(_objectPrefab);
        newObject.Init(data);
    }
}
