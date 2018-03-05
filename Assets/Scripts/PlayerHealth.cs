using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

    private const int STARTING_HEALTH = 100;
    private const float TIME_SINCE_LAST_HIT = 1.0f;
    private const int ENEMY_DAMAGE = 25;

    [SerializeField] Slider healthSlider;

    private float timer;
    private int startingHealth = STARTING_HEALTH;
    private float timeSinceLastHit = TIME_SINCE_LAST_HIT;

    private CharacterController characterController;
    private AudioSource audioSource;
    private Animator anim;
    private ParticleSystem blood;
    private int currentHealth;

    private void Awake()
    {
        Assert.IsNotNull(healthSlider);
    }

    // Use this for initialization
    void Start () {

        characterController = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        blood = GetComponentInChildren<ParticleSystem>();
        currentHealth = startingHealth;
        healthSlider.value = currentHealth;
        timer = 0.0f;
    }
	
	// Update is called once per frame
	void Update () {

        timer += Time.deltaTime;
	}

    private void OnTriggerEnter(Collider other)
    {
        if(timer >= timeSinceLastHit && !GameManager.instance.GameOver)
        {
            if(other.tag == "Weapon")
            {
                TakeHit(ENEMY_DAMAGE);
                timer = 0;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void TakeHit(int damage)
    {
        currentHealth -= damage;
        healthSlider.value = currentHealth;
        blood.Play();

       if (currentHealth > 0)
        {
            GameManager.instance.PlayerHit(currentHealth);
            audioSource.PlayOneShot(audioSource.clip);
            anim.Play("Injured");
        }

       else if(currentHealth <= 0)
        {
            GameManager.instance.PlayerHit(currentHealth);
            anim.SetTrigger("HeroDie");
            audioSource.PlayOneShot(audioSource.clip);
            characterController.enabled = false;
        }
    }

    public void AddHealth(int healthToAdd)
    {
        currentHealth += healthToAdd;
        if(currentHealth > startingHealth)
        {
            currentHealth = startingHealth;
        }
        healthSlider.value = currentHealth;
    }

    public int CurrentHealth
    {
        get
        {
            return currentHealth;
        }
    }
}
