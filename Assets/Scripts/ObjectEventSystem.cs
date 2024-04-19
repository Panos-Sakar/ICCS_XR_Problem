using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A singleton class that holds events for creating and destroying Objects
/// Also a convenient place to check for an exit command (Escape Key)
/// </summary>
public class ObjectEventSystem : MonoBehaviour
{
    public static ObjectEventSystem Current;


    public UnityEvent<SpawnedObjectData> OnSpawnObject { get; private set; } = new();
    public UnityEvent<int> OnSpawnedObjectDestroyed { get; private set; } = new();
    public float NextSpawnTime { get; set; }

    private void Awake()
    {
        //Only one object with this component is allowed
        if(Current != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Current = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}