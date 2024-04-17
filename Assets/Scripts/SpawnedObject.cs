using UnityEngine;

public class SpawnedObject : MonoBehaviour
{
    public SpawnedObjectData Data { get; private set; }

    public void Init(SpawnedObjectData data)
    {
        Data = data;
    }
}
