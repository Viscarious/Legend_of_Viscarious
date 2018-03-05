using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private const float MOVE_SPEED = 3.0f;
    private const float RUN_MULTIPLIER = 2.0f;
    private const float ROTATION_SPEED = 5.0f;
    private const float RUN_POWERUP_TIME = 5.0f;

    private float moveSpeed = MOVE_SPEED;
    private float runMultiplier = RUN_MULTIPLIER;
    private float rotationSpeed = ROTATION_SPEED;
    private float runPowerUpTime = RUN_POWERUP_TIME;
    [SerializeField] private LayerMask layerMask;

    private CharacterController charController;
    private Animator animator;
    private Vector3 currentLookTarget = Vector3.zero;
    private BoxCollider[] swordColliders;

    private GameObject fireTrail;
    private ParticleSystem fireTrailParticles;

    private bool isRunning = false;

	// Use this for initialization
	void Start ()
    {
        charController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        swordColliders = GetComponentsInChildren<BoxCollider>();
        fireTrail = GameObject.FindWithTag("Fire") as GameObject;
        fireTrail.SetActive(false);
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!GameManager.instance.GameOver)
        {
            Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            
            //TODO: Need a better way to handle player movement when attacking
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("DoubleChop") && !animator.GetCurrentAnimatorStateInfo(0).IsName("SpinAttack"))
            {
                if (isRunning || Input.GetKey(KeyCode.LeftShift))
                {
                    charController.SimpleMove(moveDirection * moveSpeed * runMultiplier);
                }
                else
                {
                    charController.SimpleMove(moveDirection * moveSpeed);
                }

                if (moveDirection == Vector3.zero)
                {
                    animator.SetBool("IsWalking", false);
                    animator.SetBool("IsRunning", false);
                }
                else
                {
                    if (isRunning || Input.GetKey(KeyCode.LeftShift))
                    {
                        animator.SetBool("IsRunning", true);
                        animator.SetBool("IsWalking", false);
                    }
                    else
                    {
                        animator.SetBool("IsWalking", true);
                        animator.SetBool("IsRunning", false);
                    }
                }

                if (Input.GetMouseButtonDown(0))
                {
                    animator.Play("DoubleChop");
                }

                if (Input.GetMouseButtonDown(1))
                {
                    animator.Play("SpinAttack");
                }
            }
        }
	}

    /// <summary>
    /// Similar to update, but used for physics objects
    /// </summary>
    private void FixedUpdate()
    {
        if (!GameManager.instance.GameOver)
        {
            RaycastHit raycastHit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //Debug.DrawRay(ray.origin, ray.direction * 500, Color.blue);

            if (Physics.Raycast(ray, out raycastHit, 500, layerMask, QueryTriggerInteraction.Ignore))
            {
                if (raycastHit.point != currentLookTarget)
                {
                    currentLookTarget = raycastHit.point;
                }

                // Find the point where the player should now look
                Vector3 targetPosition = new Vector3(raycastHit.point.x, this.transform.position.y, raycastHit.point.z);
                //Identify the new rotation direction
                Quaternion rotation = Quaternion.LookRotation(targetPosition - this.transform.position);
                //Rotate the character
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, rotation, Time.deltaTime * rotationSpeed);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void BeginAttack()
    {
        foreach(var weapon in swordColliders)
        {
            weapon.enabled = true;
        }
    }

    public void EndAttack()
    {
        foreach(var weapon in swordColliders)
        {
            weapon.enabled = false;
        }
    }

    public void SpeedPowerUp()
    {
        StartCoroutine(FireTrail());
    }

    IEnumerator FireTrail()
    {
        isRunning = true;
        fireTrail.SetActive(true);
        
        yield return new WaitForSeconds(runPowerUpTime);

        isRunning = false;

        fireTrailParticles = fireTrail.GetComponent<ParticleSystem>();
        var em = fireTrailParticles.emission;
        em.enabled = false;

        yield return new WaitForSeconds(3.0f);
        em.enabled = true;

        fireTrail.SetActive(false);
    }

}
