using UnityEngine;
using UnityEngine.Events;
using System.Collections;

/// <summary>
/// This is not a functional weapon script. It just shows how to implement shooting and reloading with buttons system.
/// </summary>
public class WeaponExample : MonoBehaviour 
{
    private PlayerControl PCtrl { get { return PlayerControl.Instance; } }

    public FP_Input playerInput;
    public float bulletForce;


    public float shootRate = 0.15F;
    public float reloadTime = 1.0F;
    public int ammoCount = 15;

    public UnityEvent actionShoot;

    private int ammo;
    private float delay;
    private bool reloading;

	void Start () 
    {
        ammo = ammoCount;
        PCtrl.UpdateAmmo(ammoCount);
	}
	
	void Update () 
    {
        if (PCtrl.gameOver)
            return;

        if (playerInput.UseMobileInput)
        {
            if (playerInput.Shoot())                         //IF SHOOT BUTTON IS PRESSED (Replace your mouse input)
                if (Time.time > delay)
                    Shoot();

            if (playerInput.Reload())                        //IF RELOAD BUTTON WAS PRESSED (Replace your keyboard input)
                if (!reloading && ammoCount < ammo)
                {
                    PCtrl.ReloadSound();
                    StartCoroutine("Reload");
                }
        }
        else
        {
            if(Input.GetMouseButtonDown(0))
                if (Time.time > delay)
                    Shoot();

            if (Input.GetKeyDown(KeyCode.R))
                if (!reloading && ammoCount < ammo)
                {
                    PCtrl.ReloadSound();
                    StartCoroutine("Reload");
                }
        }
        
	}

    void Shoot()
    {
        if (ammoCount > 0)
        {
            //   Debug.Log("Shoot");
            actionShoot?.Invoke();
            Vector3 fwd = transform.TransformDirection(Vector3.forward);
            GameObject rigBullet = Instantiate(PCtrl.bulletPrefabs[PCtrl.bulletInd], transform.position, Quaternion.LookRotation(Vector3.Slerp(transform.position, fwd, 1f)));
            rigBullet.GetComponent<Rigidbody>().AddForce(fwd * bulletForce, ForceMode.Impulse);
            ammoCount--;
            PCtrl.UpdateAmmo(ammoCount);
        }
        else
            PCtrl.EmptySound();
        //    Debug.Log("Empty");

        delay = Time.time + shootRate;
    }

    IEnumerator Reload()
    {
        reloading = true;
     //   Debug.Log("Reloading");
        yield return new WaitForSeconds(reloadTime);
        ammoCount = ammo;
        PCtrl.UpdateAmmo(ammoCount);
        //   Debug.Log("Reloading Complete");
        reloading = false;
    }
}
