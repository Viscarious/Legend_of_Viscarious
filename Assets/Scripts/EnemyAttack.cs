using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {

    private const float ATTACK_RANGE = 3.0f;
    private const float TIME_BETWEEN_ATTACKS = 1.0f;

    //TODO: Make into serializable fields?
    [SerializeField] private float attackRange;
    [SerializeField] private float timeBetweenAttacks;

    private Animator animator;
    private GameObject player;
    private bool playerInRange;
    private BoxCollider[] weaponColliders;
    private EnemyHealth enemyHealth;
    
    // Use this for initialization
    void Start ()
    {
        animator = GetComponent<Animator>();
        weaponColliders = GetComponentsInChildren<BoxCollider>();
        player = GameManager.instance.Player;
        enemyHealth = GetComponent<EnemyHealth>();
        StartCoroutine(Attack());
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (enemyHealth.IsAlive)
        {
            if (Vector3.Distance(this.transform.position, player.transform.position) < attackRange)
            {
                playerInRange = true;
            }
            else
            {
                playerInRange = false;
            }
        }
	}

    /// <summary>
    /// Enemy attack Coroutine
    /// </summary>
    /// <returns></returns>
    IEnumerator Attack()
    {
        if(playerInRange && !GameManager.instance.GameOver && enemyHealth.IsAlive && enemyHealth.IsRecovered)
        {
            animator.Play("Primary Attack");
            yield return new WaitForSeconds(timeBetweenAttacks);
        }

        yield return null;
        StartCoroutine(Attack());
    }

    public void EnemyBeginAttack()
    {
        foreach(var weapon in weaponColliders)
        {
            weapon.enabled = true;
        }
    }

    public void EnemyEndAttack()
    {
        foreach (var weapon in weaponColliders)
        {
            weapon.enabled = false;
        }
    }
}
