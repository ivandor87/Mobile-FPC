using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    public GameObject bloodEffect;
    public Text textHP;
    public Text textAMMO;
    public Text textGameOver;
    public Text textRed;
    public Text textGreen;
    public Text textYellow;
    public int HP = 100;
    public int DamageValue = 5;
    public AudioClip soundShoot;
    public AudioClip bulletDestroy;
    public AudioClip soundBonus;
    public AudioClip soundHit;
    public AudioClip soundRoar;
    public AudioClip soundEmpty;
    public AudioClip soundReload;
    public GameObject[] bulletPrefabs;
    public GameObject[] bonusCubes;
    public int bulletInd { get; set; }
    private float delayRestart = 2f;
    public bool gameOver = false;
    private AudioSource audioSource;
    private int redScore=0;
    private int greenScore=0;
    private int yellowScore=0;
    private FP_Controller fpCtrl;

    public static PlayerControl Instance;
    void Awake()
    {
        Application.targetFrameRate = 60;
        if (Instance) Destroy(gameObject);
        else
        {
            Instance = this;
        }

        textHP.text = "HEALTH: " + HP;

#if UNITY_EDITOR || UNITY_STANDALONE
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
#endif
        bulletInd = Random.Range(0, bulletPrefabs.Length);
    }

    private void Start()
    {
        fpCtrl = GetComponent<FP_Controller>();
        audioSource = GetComponent<AudioSource>();
        textRed.text = "RED:         " + redScore;
        textGreen.text = "GREEN:    " + greenScore;
        textYellow.text = "YELLOW:  " + yellowScore;
    }

#if UNITY_EDITOR || UNITY_STANDALONE
    private void Update()
    {
        if (!gameOver)
        {         
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
#endif
    IEnumerator BloodEffectOff()
    {
        yield return new WaitForSeconds(0.3f);
         if(!gameOver)   
            bloodEffect.SetActive(false);
    }

    public void UpdateAmmo(int a)
    {
        textAMMO.text = "AMMO: " + a;
    }
    public void ShootSound()
    {
        audioSource.PlayOneShot(soundShoot);
    }
    public void EmptySound()
    {
        audioSource.PlayOneShot(soundEmpty);
    }
    public void ReloadSound()
    {
        audioSource.PlayOneShot(soundReload);
    }

    public void RoarSound()
    {
        audioSource.PlayOneShot(soundRoar);
    }
    public void BulletDestroySound()
    {
        audioSource.PlayOneShot(bulletDestroy);
    }
    public void DamagePlayer()
    {
        if (gameOver)
            return;

        audioSource.PlayOneShot(soundHit);
        HP -= DamageValue;
        bloodEffect.SetActive(true);
        textHP.text = "HEALTH: " + HP;
        if (HP <= 0)
        {
            textGameOver.gameObject.SetActive(true);
            gameOver = true;
            fpCtrl.canControl = false;
            Invoke("RestartLevel", 2f);
        }
        else
            StartCoroutine(BloodEffectOff());
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "BonusRed(Clone)")
        {
            bulletInd = 0;
            audioSource.PlayOneShot(soundBonus);
            Destroy(other.gameObject);
            redScore++;
            textRed.text = "RED:         " + redScore;
        }
        else if (other.gameObject.name == "BonusYellow(Clone)")
        {
            bulletInd = 1;
            audioSource.PlayOneShot(soundBonus);
            Destroy(other.gameObject);
            yellowScore++;
            textYellow.text = "YELLOW:  " + yellowScore;
        }
        else if (other.gameObject.name == "BonusGreen(Clone)")
        {
            bulletInd = 2;
            audioSource.PlayOneShot(soundBonus);
            Destroy(other.gameObject);
            greenScore++;
            textGreen.text = "GREEN:    " + greenScore;
        }
    }
}
