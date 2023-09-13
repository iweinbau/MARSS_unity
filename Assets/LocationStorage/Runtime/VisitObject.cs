using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class VisitObject : MonoBehaviour
{
    [SerializeField] private Collider collider;
    [SerializeField] public Action<VisitObject> OnColliderTriggerEvent;

    public void EnableCollider()
    {
        collider.enabled = true;
    }

    public void DisableCollider()
    {
        collider.enabled = false;
    }

    private void Awake()
    {
        collider = GetComponent<Collider>();
    }

    private void Start()
    {
        collider.enabled = false;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "MainCamera")
        {
            OnColliderTriggerEvent?.Invoke(this);
        }
    }
}
