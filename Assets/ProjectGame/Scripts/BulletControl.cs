using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour
{
    private PlayerControl PCtrl { get { return PlayerControl.Instance; } }
    public GameObject animPrefab;
    public float lifeTime = 3f;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if ((lifeTime -= Time.deltaTime) <= 0f)
            Destroy(gameObject);
    }
    private void FixedUpdate()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        Ray ray = new Ray(transform.position, fwd);
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, fwd, out hitInfo, 1.2f))
        {
            Debug.DrawRay(transform.position, fwd * 1.2f, Color.blue);
            Rigidbody rig = GetComponent<Rigidbody>();
            rig.position = hitInfo.point;
            if (animPrefab)
                Instantiate(animPrefab, hitInfo.point - fwd*0.5f, Quaternion.LookRotation(Vector3.Slerp(transform.position, fwd, 1f)));
            PCtrl.BulletDestroySound();
            Destroy(gameObject);
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (animPrefab)
    //        Instantiate(animPrefab, transform.position, Quaternion.identity);
    //    PCtrl.BulletDestroySound();
    //    Destroy(gameObject);
    //}
}
