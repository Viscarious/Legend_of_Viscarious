using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject[] enemySpawnPoints;
    [SerializeField] private GameObject[] powerUpSpawnPoints;
    [SerializeField] private GameObject tanker;
    [SerializeField] private GameObject ranger;
    [SerializeField] private GameObject soldier;
    [SerializeField] private GameObject arrow;
    [SerializeField] private Text levelText;
    [SerializeField] private GameObject healthPowerUp;
    [SerializeField] private GameObject speedPowerUp;
    [SerializeField] private int maxPowerUps = 3;
    [SerializeField] private Text endgameText;
    [SerializeField] private int maxLevel = 10;

    private bool gameOver = false;
    private int currentLevel;
    ///enemies spawn 1 second after each other
    private float generatedSpawnTime = 1.0f;
    private float currentSpawnTime = 0.0f;
    private float powerUpSpawnTime = 5.0f;
    private float currentPowerUpSpawnTime = 0.0f;
    private int powerUpSpawned = 0;
    

    private List<EnemyHealth> currentEnemies = new List<EnemyHealth>();
    private List<EnemyHealth> killedEnemies = new List<EnemyHealth>();
    private GameObject[] enemiesArray = new GameObject[3];
    private GameObject[] powerupsArray = new GameObject[2];

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != null)
        {
            Destroy(gameObject);
        }

        Assert.IsNotNull(tanker);
        Assert.IsNotNull(ranger);
        Assert.IsNotNull(soldier);
        Assert.IsNotNull(healthPowerUp);
        Assert.IsNotNull(speedPowerUp);
    }

    // Use this for initialization
    void Start () {
        endgameText.GetComponent<Text>().enabled = false;
        currentLevel = 1;
        levelText.text = "Level " + currentLevel;

        enemiesArray[0] = soldier;
        enemiesArray[1] = ranger;
        enemiesArray[2] = tanker;

        powerupsArray[0] = healthPowerUp;
        powerupsArray[1] = speedPowerUp;

        StartCoroutine(SpawnEnemies());
        StartCoroutine(SpawnPowerUps());
    }
	
	// Update is called once per frame
	void Update () {
        currentSpawnTime += Time.deltaTime;
        currentPowerUpSpawnTime += Time.deltaTime;
	}

    public void PlayerHit(int currentHP)
    {
        if(currentHP > 0)
        {
            gameOver = false;
        }
        else
        {
            gameOver = true;
            StartCoroutine(EndGame("Game Over"));
        }
    }

    /// <summary>
    /// Spawn enemies
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnEnemies()
    {
        if(currentSpawnTime > generatedSpawnTime)
        {
            currentSpawnTime = 0;

            if(currentEnemies.Count <= currentLevel)
            {
                int randomSpawnPoint = Random.Range(0, enemySpawnPoints.Length);
                GameObject spawnPoint = enemySpawnPoints[randomSpawnPoint];

                int randomEnemy = Random.Range(0, enemiesArray.Length);
                GameObject newEnemy = Instantiate(enemiesArray[randomEnemy]);

                newEnemy.transform.position = spawnPoint.transform.position;
            }

            if (killedEnemies.Count == (currentLevel + 1) && currentLevel != maxLevel)
            {
                currentEnemies.Clear();
                killedEnemies.Clear();

                yield return new WaitForSeconds(3.0f);
                currentLevel++;
                levelText.text = "Level " + currentLevel.ToString();
            }

            if(killedEnemies.Count == maxLevel)
            {
                StartCoroutine(EndGame("Victory"));
            }
        }

        yield return null;
        StartCoroutine(SpawnEnemies());
    }

    /// <summary>
    /// Spawn power ups
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnPowerUps()
    {
        if(currentPowerUpSpawnTime > powerUpSpawnTime)
        {
            currentPowerUpSpawnTime = 0; 

            if (powerUpSpawned < maxPowerUps)
            {
                int randomSpawnPoint = Random.Range(0, powerUpSpawnPoints.Length);
                GameObject spawnPoint = powerUpSpawnPoints[randomSpawnPoint];

                int randomPowerUp = Random.Range(0, powerupsArray.Length);
                GameObject newPowerUp = Instantiate(powerupsArray[randomPowerUp]);

                //Vector3 newPosition = spawnPoint.transform.position;
                //newPowerUp.transform.position = new Vector3(newPosition.x, 1.5f, newPosition.z);
                newPowerUp.transform.position = spawnPoint.transform.position;
            }
        }

        yield return null;
        StartCoroutine(SpawnPowerUps());
    }

    public void RegisterEnemy(EnemyHealth enemy)
    {
        currentEnemies.Add(enemy);
    }

    public void KilledEnemy(EnemyHealth killedEnemy)
    {
        killedEnemies.Add(killedEnemy);
    }

    public void RegisterPowerUp()
    {
        powerUpSpawned++;
    }

    public void DeregisterPowerUp()
    {
        powerUpSpawned--;
    }

    IEnumerator EndGame(string outcome)
    {
        endgameText.text = outcome;
        endgameText.GetComponent<Text>().enabled = true;

        yield return new WaitForSeconds(3.0f);

        SceneManager.LoadScene("GameMenu");
    }

    public bool GameOver
    {
        get
        {
            return gameOver;
        }
    }

    public GameObject Player
    {
        get
        {
            return player;
        }
    }

    public GameObject Arrow
    {
        get
        {
            return arrow;
        }
    }
}
