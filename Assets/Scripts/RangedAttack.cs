using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : MonoBehaviour {

    private const float ATTACK_RANGE = 10.0f;
    private const float TIME_BETWEEN_ATTACKS = 1.0f;

    //TODO: Make into serializable fields?
    [SerializeField] private float attackRange;
    [SerializeField] private float timeBetweenAttacks;
    [SerializeField] private float timeFromLastDamage;
    [SerializeField] private Transform arrowSpawnLocation;

    private Animator animator;
    private GameObject player;
    private bool playerInRange;
    private EnemyHealth enemyHealth;
    private GameObject arrow;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameManager.instance.Player;
        enemyHealth = GetComponent<EnemyHealth>();
        arrow = GameManager.instance.Arrow;
        StartCoroutine(Attack());
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyHealth.IsAlive)
        {
            RotateTowards(player.transform);

            if (Vector3.Distance(this.transform.position, player.transform.position) < attackRange)
            {
                playerInRange = true;
            }
            else
            {
                playerInRange = false;
            }

            animator.SetBool("PlayerInRange", playerInRange);
        }
    }

    /// <summary>
    /// Enemy attack Coroutine
    /// </summary>
    /// <returns></returns>
    IEnumerator Attack()
    {
        if (playerInRange && !GameManager.instance.GameOver && enemyHealth.IsAlive && enemyHealth.IsRecovered)
        {
            animator.Play("Primary Attack");
            yield return new WaitForSeconds(timeBetweenAttacks);
        }

        yield return null;
        StartCoroutine(Attack());
    }

    private void RotateTowards(Transform player)
    {
        Vector3 direction = (player.position - this.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10.0f);
    }

    /// <summary>
    /// Fire an arrow at the player
    /// </summary>
    public void FireArrow()
    {
        //1. create an arrow
        GameObject newArrow = Instantiate(arrow) as GameObject;
        //2. set the position to a specific location
        newArrow.transform.position = arrowSpawnLocation.position;
        //3. set the orientation
        newArrow.transform.rotation = transform.rotation;
        //4. apply a velocity to it.
        newArrow.GetComponent<Rigidbody>().velocity = transform.forward * 25.0f;
    }

    public void EnemyBeginAttack()
    {
       
    }

    public void EnemyEndAttack()
    {
        
    }
}
