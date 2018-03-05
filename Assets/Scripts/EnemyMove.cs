using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

public class EnemyMove : MonoBehaviour {

    private Transform player;
    private NavMeshAgent nav;
    private Animator animator;
    private EnemyHealth enemyHealth;

    private void Awake()
    {
        
    }

    // Use this for initialization
    void Start ()
    {
        animator = this.GetComponent<Animator>();
        nav = this.GetComponent<NavMeshAgent>();
        enemyHealth = this.GetComponent<EnemyHealth>();
        player = GameManager.instance.Player.transform;
    }
	
	// Update is called once per frame
	void Update ()
    {
        //if the enemy is alive and it's not game over
        if (enemyHealth.IsAlive && !GameManager.instance.GameOver)
        {
            nav.SetDestination(player.position);
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Primary Attack"))
            {
                nav.isStopped = true;
            }
            else
            {
                nav.isStopped = false;
            }
        }
        //else if the enemy is still alive and it's game over
        else if(enemyHealth.IsAlive && GameManager.instance.GameOver)
        {
            nav.isStopped = true;
            animator.Play("Idle");
        }
        // otherwise the enemy is dead
        else
        {
            nav.isStopped = true;
        }
	}
}
