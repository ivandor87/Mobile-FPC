using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyToTime : MonoBehaviour
{
    public float delayDestroy = 0.5f;
    void Update()
    {
        if ((delayDestroy -= Time.deltaTime) <= 0f)
            Destroy(gameObject);
    }
}
