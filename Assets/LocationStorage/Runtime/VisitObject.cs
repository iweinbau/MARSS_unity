using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class VisitObject : MonoBehaviour
{
    [SerializeField] private BoxCollider m_boxCollider;
    [SerializeField] public Action OnColliderTriggerEvent;

    public void EnableCollider()
    {
        m_boxCollider.enabled = true;
    }

    public void DisableCollider()
    {
        m_boxCollider.enabled = false;
    }

    private void Start()
    {
        m_boxCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "MainCamera")
        {
            OnColliderTriggerEvent?.Invoke();
        }
    }
}
