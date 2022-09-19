using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gizmo : MonoBehaviour
{
    [SerializeField] MeshRenderer _renderer;
    BoxCollider _collider;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
    }

    public void Toggle()
    {
        _renderer.enabled ^= true;
        _collider.enabled ^= true;
    }    
}
