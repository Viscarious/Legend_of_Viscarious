using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour {

    private const int ENEMY_STARTING_HEALTH = 50;
    private const float ENEMY_TIME_SINCE_LAST_HIT = 1.0f;
    private const int PLAYER_DAMAGE = 25;
    private const float DISSAPEAR_SPEED = 2.0f;

    private float timer = 0.0f;
    [SerializeField] private int startingHealth;
    private float timeSinceLastHit = ENEMY_TIME_SINCE_LAST_HIT;
    private float dissapearSpeed = DISSAPEAR_SPEED;

    private AudioSource audioSource;
    private Animator anim;
    private int currentHealth;
    private NavMeshAgent nav;
    private Rigidbody rigidBody;
    private CapsuleCollider capsuleCollider;
    private ParticleSystem blood;
    private bool isAlive;
    private bool isRecovered;
    private bool dissapearEnemy;

    private void Awake()
    {
        //potentially movie this to start() of this causes problems
        
    }

    // Use this for initialization
    void Start ()
    {
        GameManager.instance.RegisterEnemy(this);
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        rigidBody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        blood = GetComponentInChildren<ParticleSystem>();
        isAlive = true;
        isRecovered = true;
        currentHealth = startingHealth;
    }

    // Update is called once per frame
    void Update()
    {

        timer += Time.deltaTime;

        if (dissapearEnemy)
        {
            transform.Translate(-Vector3.up * dissapearSpeed * Time.deltaTime);
        }

        if (timer >= timeSinceLastHit)
        {
            isRecovered = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (timer >= timeSinceLastHit && !GameManager.instance.GameOver && isAlive)
        {
            if (other.tag == "PlayerWeapon")
            {
                TakeHit(PLAYER_DAMAGE);
                isRecovered = false;
                timer = 0.0f;
            }
        }
    }

    private void TakeHit(int damage)
    {
        currentHealth -= damage;
        
        blood.Play();

        if (currentHealth > 0)
        {
            audioSource.PlayOneShot(audioSource.clip);
            anim.Play("Injured");

            StartCoroutine(PauseAnimation());
        }
        else if (currentHealth <= 0)
        {
            isAlive = false;
            audioSource.PlayOneShot(audioSource.clip);
            anim.SetTrigger("EnemyDie");

            capsuleCollider.enabled = false;
            nav.isStopped = true;
            rigidBody.isKinematic = true;

            GameManager.instance.KilledEnemy(this);

            StartCoroutine(RemoveEnemy());
        }
    }

    IEnumerator PauseAnimation()
    {
        yield return new WaitForSeconds(0.1f);
        anim.speed = 0;
        nav.isStopped = true;
        yield return new WaitForSeconds(0.1f);
        anim.speed = 1;
        nav.isStopped = false;
    }

    public bool IsRecovered
    {
        get
        {
            return isRecovered;
        }
    }

    public bool IsAlive
    {
        get
        {
            return isAlive;
        }
    }

    /// <summary>
    /// Once a enemy dies they dissolve into the ground and are destroyed
    /// </summary>
    /// <returns></returns>
    IEnumerator RemoveEnemy()
    {
        yield return new WaitForSeconds(4.0f);

        dissapearEnemy = true;

        yield return new WaitForSeconds(2.0f);

        Destroy(this.gameObject);
    }
}
