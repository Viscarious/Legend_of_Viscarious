using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPowerUp : MonoBehaviour {

    [SerializeField] private float rotationSpeed = 20.0f;
    [SerializeField] private int healthToAdd = 25;

    private GameObject player;
    private PlayerController playerController;
    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;

    // Use this for initialization
    void Start () {

        player = GameManager.instance.Player;
        playerController = player.GetComponent<PlayerController>();
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        GameManager.instance.RegisterPowerUp();

    }
	
	// Update is called once per frame
	void Update () {
        this.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            audioSource.PlayOneShot(audioSource.clip);
            playerController.SpeedPowerUp();

            spriteRenderer.enabled = false;

            GameManager.instance.DeregisterPowerUp();

            StartCoroutine(DestroyPowerUp());
        }
    }

    IEnumerator DestroyPowerUp()
    {
        yield return new WaitForSeconds(2.0f);

        Destroy(this.gameObject);
    }
}
