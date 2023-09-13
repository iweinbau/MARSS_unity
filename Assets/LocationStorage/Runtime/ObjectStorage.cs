using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ObjectStorage", menuName = "ScriptableObjects/ObjectStorage", order = 1)]
public class ObjectStorage: ScriptableObject
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private Dictionary<string, VisitObject> objectStorage;

    private int visitedObjectsCounter = 0;

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
    }

    public void OnVisitObject(VisitObject obj)
    {
        visitedObjectsCounter++;
        obj.DisableCollider();
        if (visitedObjectsCounter == objectStorage.Count)
        {
            Debug.Log("You win");
        }
    }

    public void OnReplayStarted()
    {
        visitedObjectsCounter = 0;
        foreach (VisitObject obj in objectStorage.Values)
        {
            obj.EnableCollider();
        }
    }

}
