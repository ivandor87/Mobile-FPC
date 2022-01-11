using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class CheckVisible : MonoBehaviour
{
    public bool onScreen = false;

    private void OnBecameVisible() 
    { 
        onScreen = true; 
    }

    private void OnBecameInvisible() 
    { 
        onScreen = false; 
    }
}
