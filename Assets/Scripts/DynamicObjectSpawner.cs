using UnityEngine;

public class DynamicObjectSpawner : MonoBehaviour
{
    private void OnEnable()
    {
        SpawnedObject.OnSpawnedObjectDestroyed.AddListener(SpawnNewObject);
    }

    private void OnDisable()
    {
        SpawnedObject.OnSpawnedObjectDestroyed.RemoveListener(SpawnNewObject);
    }

    private void SpawnNewObject(int objectID)
    {
        Debug.LogWarning($"Loading: {objectID}");
    }
}
