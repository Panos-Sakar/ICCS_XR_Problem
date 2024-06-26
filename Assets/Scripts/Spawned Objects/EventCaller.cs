using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Reads JSON files from Streaming Assets folder in "<see cref="_streamingAssetsSubfolder"/>".
/// Every <see cref="_spawnInterval"/> Calls an event with the contents of one of the files as parameter.
/// </summary>
public class EventCaller : MonoBehaviour
{
    [SerializeField]
    private string _streamingAssetsSubfolder;
    [SerializeField]
    private float _spawnInterval = 5f;
    
    private string _assetsFolder;
    private List<SpawnedObjectData> _spawnedObjectData = new();
    private float _timeSinceLastSpawn;
    
    private void Awake()
    {
        _assetsFolder = Path.Combine(Application.streamingAssetsPath, _streamingAssetsSubfolder);
        if(Directory.Exists(_assetsFolder))
        {
            foreach (var assetPath in Directory.EnumerateFiles(_assetsFolder))
            {
                //Filter out Json files
                if(assetPath.EndsWith(".json") == false) continue;

                //Try to read file contents before adding the new asset to list
                SpawnedObjectData itemContent;
                try
                {
                    itemContent = JsonUtility.FromJson<SpawnedObjectData>(File.ReadAllText(assetPath));
                }
                catch (System.Exception)
                {
                    //If any error is occurred, Ignore the file
                    Debug.LogWarning($"[EventCaller] Could not load asset at: {assetPath}", this);
                    continue;
                }

                //Blindly adds element on list. Does not check for duplicates. 
                //Other struct should be used if duplicate check is required
                _spawnedObjectData.Add(itemContent);
                Debug.Log($"[EventCaller] Found asset: '{assetPath}' object_id: '{itemContent.object_id}'", this);
            }
        }

        ObjectEventSystem.Current.NextSpawnTime = 0;
    }

    private void Update()
    {
        _timeSinceLastSpawn += Time.deltaTime;

        if(_timeSinceLastSpawn >= _spawnInterval)
        {
            _timeSinceLastSpawn = 0;
            var randomObject = _spawnedObjectData[Random.Range(0, _spawnedObjectData.Count)];
            ObjectEventSystem.Current.OnSpawnObject.Invoke(randomObject);
        }

        ObjectEventSystem.Current.NextSpawnTime = _spawnInterval - _timeSinceLastSpawn;
    }
}
