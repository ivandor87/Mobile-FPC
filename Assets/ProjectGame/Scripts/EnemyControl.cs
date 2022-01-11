using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyControl : MonoBehaviour
{
    private PlayerControl PCtrl { get { return PlayerControl.Instance; } }

    public int HP = 100;
    public int DamageValue = 50;
    private GameObject target;
    private NavMeshAgent agentEnemy;
    private Animator animator;
    private float periodRequest = 1f;
    static private string[] idAnim = { "Attack 01", "Attack 02" };
    static private int idLength = idAnim.Length;
    private bool dieEnemy = false;
    private Transform trans;
    private bool damage = false;

    void Start()
    {
        trans = transform;
        target = GameObject.Find("Player");
        agentEnemy = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agentEnemy.destination = target.transform.position;
        animator.SetBool("Run Forward", true);
        agentEnemy.avoidancePriority = (int)Random.Range(20,80);
    }

    private void Update()
    {
        if (!damage)
        {
            if ((periodRequest -= Time.deltaTime) <= 0)
            {
                periodRequest = 1f;
                agentEnemy.destination = target.transform.position;
                if (agentEnemy.remainingDistance <= agentEnemy.stoppingDistance)
                {
                    PCtrl.RoarSound();
                    animator.SetTrigger(idAnim[Random.Range(0, idLength)]);
                    animator.SetBool("Run Forward", false);
                    StartCoroutine(AttackLoop());

                }
                else
                {
                    animator.SetBool("Run Forward", true);
                }
            }
        }
        else if(dieEnemy)
        {
            if ((periodRequest -= Time.deltaTime) <= 0)
            {
                Instantiate(PCtrl.bonusCubes[Random.Range(0, PCtrl.bonusCubes.Length)], trans.position + Vector3.up, Quaternion.identity);
                Destroy(gameObject);
            }
        }
        else if(damage && IsAnimationPlaying("Take Damage"))
        {
            damage = false;
            animator.SetBool("Run Forward", true);
            agentEnemy.isStopped = false;
        }
    }

    IEnumerator AttackLoop()
    {
        yield return new WaitForSeconds(0.2f);
        if (CheckDestination())
            PCtrl.DamagePlayer();
    }

    private bool CheckDestination()
    {
        float distanceToTarget = Vector3.Distance(trans.position, target.transform.position);
        if (distanceToTarget < agentEnemy.stoppingDistance)
        {
            return true;
        }
        return false;
    }

    public bool IsAnimationPlaying(string animationName)
    {
        var animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (animatorStateInfo.IsName(animationName))
            return true;

        return false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Bullet") && !dieEnemy)
        {
            HP -= DamageValue;
            damage = true;
            agentEnemy.isStopped = true;
            animator.SetBool("Run Forward", false);  
            if (HP > 0)
            {
                animator.SetTrigger("Take Damage");
            }
            else
            {
                animator.SetTrigger("Die");
                agentEnemy.enabled = false;
                dieEnemy = true;
                periodRequest = 1.7f;
            }
        }
    }
}
