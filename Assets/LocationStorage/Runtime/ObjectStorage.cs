using System;
using UnityEngine;
using System.Collections.Generic;
using Action = Unity.Plastic.Antlr3.Runtime.Misc.Action;

[CreateAssetMenu(fileName = "ObjectStorage", menuName = "ScriptableObjects/ObjectStorage", order = 1)]
public class ObjectStorage : ScriptableObject
{
    [Header("prefab database")] [SerializeField]
    private List<GameObject> prefabDatabase;

    [SerializeField] private bool useVirtualPlaceholders = true;

    [Header("spawn prefab")] [SerializeField]
    private GameObject prefab;

    [SerializeField] private Dictionary<string, VisitObject> objectStorage;

    public Action<int, int> OnVisitItemProgress;
    public Action OnGameWon;
    public Action OnGameStarted;
    
    private int visitedObjectsCounter = 0;
    private int spawnIndex = 0;
    public void AddVirtualObject(Vector3 position)
    {
        if (objectStorage == null)
        {
            objectStorage = new Dictionary<string, VisitObject>();
        }
        
        GameObject viewObject = GameObject.Instantiate(prefabDatabase[spawnIndex++ % prefabDatabase.Count], position, Quaternion.identity);
        
        VisitObject visitObject = viewObject.AddComponent<VisitObject>();
        visitObject.OnColliderTriggerEvent += OnVisitObject;

        // Store the camera transform with its timestamp in the dictionary
        Debug.Log($"Save object: {viewObject.name} at position: {position}");
        objectStorage.Add(viewObject.name, visitObject);
        
        OnVisitItemProgress?.Invoke(visitedObjectsCounter, objectStorage.Count);
    }

    public void SavePosition(string key, Vector3 position)
    {
        if (objectStorage == null)
        {
            objectStorage = new Dictionary<string, VisitObject>();
        }
        
        GameObject viewObject = GameObject.Instantiate(prefab, position, Quaternion.identity);
        viewObject.gameObject.name = key;

        VisitObject visitObject = viewObject.GetComponent<VisitObject>();
        visitObject.OnColliderTriggerEvent += OnVisitObject;

        // Store the camera transform with its timestamp in the dictionary
        Debug.Log($"Save object: {key} at position: {position}");
        objectStorage.Add(key, visitObject);
        
        OnVisitItemProgress?.Invoke(visitedObjectsCounter, objectStorage.Count);
    }

    public void OnVisitObject(VisitObject obj)
    {
        visitedObjectsCounter += 1;
        obj.DisableCollider();
        
        OnVisitItemProgress?.Invoke(visitedObjectsCounter, objectStorage.Count);
        if (visitedObjectsCounter == objectStorage.Count)
        {
            Debug.Log("You win");
            OnGameWon?.Invoke();
        }
    }

    public void OnReplayStarted()
    {
        visitedObjectsCounter = 0;
        foreach (VisitObject obj in objectStorage.Values)
        {
            obj.EnableCollider();
        }
        
        OnVisitItemProgress?.Invoke(visitedObjectsCounter, objectStorage.Count);
        OnGameStarted?.Invoke();
    }

}
