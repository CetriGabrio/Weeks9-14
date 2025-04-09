using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


//This is definately the main script of the whole project
//It contains the plyer's spaceship behavior
public class Spaceship : MonoBehaviour
{
    //Size of the player
    //Since I am using a rectangles shape, I only need the width and height
    private float playerWidth = 1f;
    private float playerHeight = 2f;

    //I added these variables to adjust the hitbox of the enemies in relation to the player hitbox
    //At first they were incorrect, and the player would collide with the enemy in the most random position
    //Since I didn't want to modify the rectangle's hitbox, I simply added some offsets to adjust them first handed
    float enemyHitboxOffsetX = -1f;
    float enemyHitboxOffsetY = +0.5f;
    float enemyHitboxTrimRight = -1.5f;

    //Player lives
    private int lives = 3;

    //Movement speed for the player
    public float speed = 1f;
    //This variable baseSpeed is used for having a faster speed when the boost is collected
    private float baseSpeed;

    //Fire rate for the player
    private float baseFireRate = 1f;
    //Added a cooldown between laser so that the player cannot spam
    private float canFire = -1f;
    //Variable to detect if the fire rate has been modified
    private bool isFireRateBoosted = false;
    //This variable baseSpeed is used for having a higher fire rate when the boost is collected
    private float boostedFireRate = 0.5f;
    private float currentFireRate;

    //Reference to the prefabs
    public GameObject playerLaserPrefab;
    public GameObject enemy;

    //Referencing the collisions with the collision detection script
    private CollisionDetection collisionDetection;

    //Referencing the powerupManager for all the powerups
    private PowerUpManager powerUpManager;

    //Reference to the spawnManager
    private SpawnManager spawnManager;

    //////////////////////////////////////////////////////
    //////////////////UI
    /////////////////////////////////////////////////


    //Score of the player, obtained by destroying enemies, with the text to give visual feedback
    public int score = 0;
    public TextMeshProUGUI scoreText;

    //Images to showcase the hearts / lives of the player, both filled and empty
    public Image[] hearts;
    public Sprite fullHeartSprite;
    public Sprite emptyHeartSprite;

    //Reference to the shield powerup to visually show the shield 
    public bool isShielded = false;
    public ShieldVisual shieldVisual;

    //All the audio sources and clips for the music and effects
    public AudioSource audioSource;
    public AudioClip laserSoundClip;
    public AudioSource backgroundMusicSource;
    public AudioClip powerUpSoundClip;

    //Bool variable that says when game over is triggered
    private bool isGameOver = false;
    public TextMeshProUGUI gameOverText;

    // Start is called before the first frame update
    void Start()
    {
        //Reset player position at the start of the game to the centre of the screen
        transform.position = new Vector2(0, -2);

        collisionDetection = GetComponent<CollisionDetection>();
        powerUpManager = FindObjectOfType<PowerUpManager>();

        if (powerUpManager != null)
        {
            powerUpManager.OnShieldCollected.AddListener(ActivateShield);
            powerUpManager.OnSpeedBoostCollected.AddListener(ActivateSpeedBoost);
            powerUpManager.OnFireRateCollected.AddListener(ActivateFireRateBoost);
        }

        spawnManager = FindObjectOfType<SpawnManager>();

        shieldVisual = GetComponent<ShieldVisual>();

        baseSpeed = speed;
        currentFireRate = baseFireRate;

        UpdateScoreDisplay();
        UpdateHeartsDisplay();

        if (isGameOver)
            return;

        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(false);  
        }

    }

    // Update is called once per frame
    void Update()
    {
        SpaceshipMovement();

        ShootLaser();

        EnemyCollision();
    }

    //Created a new function for all the spaceship movement features
    void SpaceshipMovement()
    {
        //Variables for the player horizontal and vertical movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //Functions to allow the player to move on both horizontal and vertical axis at a realistic speed
        //Since it's a shoot em up game, I do not want the player to be able to rotate and point in any direction other than upwards
        transform.Translate(Vector3.right * horizontalInput * speed * Time.deltaTime);
        transform.Translate(Vector3.up * verticalInput * speed * Time.deltaTime);

        //Creating player bouds so that the spaceship is enclosed in the screen

        //This part is handling the vertical movement, so the player can only stay in the bottom side of the screen
        if (transform.position.y >= -1f)
        {
            transform.position = new Vector3(transform.position.x, -1f, 0);
        }
        else if (transform.position.y <= -4f)
        {
            transform.position = new Vector3(transform.position.x, -4f, 0);
        }

        //This part is handling the horizontal movement, implementing a pac man effect that allows the player to teleport on the other border of the screen
        if (transform.position.x > 9.5f)
        {
            transform.position = new Vector3(-9.5f, transform.position.y, 0);
        }
        else if (transform.position.x < -9.5f)
        {
            transform.position = new Vector3(9.5f, transform.position.y, 0);
        }

        /////////////////
        ///Use camera to wolrd point to handle different aspect ratios
        ////////////////
    }

    //Created a function to handle all the shooting mechanics to organize the code
    void ShootLaser()
    {
        if (Input.GetKey(KeyCode.Space) && Time.time > canFire) 
        {
            if (isFireRateBoosted)
            {
                Shoot();
                PlayLaserSound();
                canFire = Time.time + boostedFireRate;  
            }
            else
            {
                Shoot();  
                canFire = Time.time + baseFireRate;
                PlayLaserSound();
            }
        }
    }


    void Shoot()
    {
        Instantiate(playerLaserPrefab, transform.position + new Vector3(0, 1f, 0), Quaternion.identity);
    }

    void PlayLaserSound()
    {
        
        if (audioSource != null && laserSoundClip != null)
        {
            audioSource.PlayOneShot(laserSoundClip);
        }
    }

    void EnemyCollision()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            //Get enemy's dimensions - both height and width
            float enemyWidth = enemy.transform.localScale.x;
            float enemyHeight = enemy.transform.localScale.y;


            if (collisionDetection.CheckCollision(
                    transform.position.x, transform.position.y, playerWidth, playerHeight,
                    enemy.transform.position.x + enemyHitboxOffsetX, enemy.transform.position.y + enemyHitboxOffsetY, enemyWidth + enemyHitboxTrimRight, enemyHeight))
            {
                if (isShielded)
                {
                    DeactivateShield();
                    Destroy(enemy);
                    Debug.Log("Shield absorbed the hit");
                }
                else
                {

                    lives--;

                    UpdateHeartsDisplay();

                    if (lives < 1)
                    {
                        GameOver();
                    }
                    else
                    {
                        Destroy(enemy);
                    }
                }
            }
        }
    }

    public void ActivateShield()
    {
        isShielded = true;
        //Debug.Log("Shield On");

        PlayPowerUpSound();
    }

    public void DeactivateShield()
    {
        if (isShielded)
        {
            isShielded = false;
            Destroy(enemy);
            IncreaseScore(1);
            //Debug.Log("Shield Off");
        }
        else
        {
            //Debug.Log("Damage");
        }
    }

    public void ActivateSpeedBoost()
    {
        StopAllCoroutines();
        StartCoroutine(SpeedBoostCoroutine());
        PlayPowerUpSound();
    }

    IEnumerator SpeedBoostCoroutine()
    {
        speed = baseSpeed * 2f;
        yield return new WaitForSeconds(5f);
        speed = baseSpeed;
    }

    public void ActivateFireRateBoost()
    {
        if (isFireRateBoosted) return;

        isFireRateBoosted = true;
        currentFireRate = boostedFireRate;  
        canFire = -1f;  

        StartCoroutine(ResetFireRateAfterSeconds(5f));
        PlayPowerUpSound(); ;

    }

    IEnumerator ResetFireRateAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        isFireRateBoosted = false;
        currentFireRate = baseFireRate;  
        canFire = Time.time + currentFireRate;  
    }

    public void IncreaseScore(int points)
    {
        score += points;
        UpdateScoreDisplay();
    }

    void UpdateScoreDisplay()
    {
        {
            if (scoreText != null)
            {
                scoreText.text = "Score: " + score;
            }
        }
    }

    void UpdateHeartsDisplay()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < lives)
            {
                hearts[i].sprite = fullHeartSprite; 
            }
            else
            {
                hearts[i].sprite = emptyHeartSprite;
            }
        }
    }

    void PlayPowerUpSound()
    {
        if (audioSource != null && powerUpSoundClip != null)
        {
            audioSource.PlayOneShot(powerUpSoundClip);
        }
    }

    void GameOver()
    {
        isGameOver = true;

        if (backgroundMusicSource != null && backgroundMusicSource.isPlaying)
        {
            backgroundMusicSource.Stop();
        }

        Time.timeScale = 0f;  

        gameObject.SetActive(false);

        if (powerUpManager != null)
        {
            powerUpManager.OnShieldCollected.RemoveListener(ActivateShield);
            powerUpManager.OnSpeedBoostCollected.RemoveListener(ActivateSpeedBoost);
            powerUpManager.OnFireRateCollected.RemoveListener(ActivateFireRateBoost);
        }

        if (gameOverText != null)
        {
            gameOverText.text = "GAME OVER"; 
            gameOverText.gameObject.SetActive(true); 
        }
    }
}