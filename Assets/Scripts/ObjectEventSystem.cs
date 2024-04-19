using UnityEngine;
using UnityEngine.Events;

public class ObjectEventSystem : MonoBehaviour
{
    public static ObjectEventSystem Current;


    public UnityEvent<SpawnedObjectData> OnSpawnObject { get; private set; } = new();
    public UnityEvent<int> OnSpawnedObjectDestroyed { get; private set; } = new();
    public float NextSpawnTime { get; set; }

    private void Awake()
    {
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