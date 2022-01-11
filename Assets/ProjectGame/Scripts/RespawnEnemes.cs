using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnEnemes : MonoBehaviour
{
    public GameObject[] enemyPrefab;
    private Transform[] respawn;
    private bool spawn = false;

    void Start()
    {
        respawn = new Transform[transform.childCount];
        int i = 0;
        foreach (Transform point in transform)
            respawn[i++] = point;

        StartCoroutine(RespawnLoop());
    }

    IEnumerator RespawnLoop()
    {
        yield return new WaitForSeconds(3f);
        
        while(!spawn)
        {
            yield return null;
            Transform resp = respawn[Random.Range(0, respawn.Length)];
            spawn = !resp.GetComponent<CheckVisible>().onScreen;
            if (spawn)
                Instantiate(enemyPrefab[Random.Range(0, enemyPrefab.Length)], resp.position, Quaternion.identity); 
        }
        spawn = false;
        StartCoroutine(RespawnLoop());
    }
}
