using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.Sprites;


//This is definately the main script of the whole project
//It contains the plyer's spaceship behavior and the interactions with all of the main game elements
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
    private float baseFireRate = 0.7f;
    //Added a cooldown between laser so that the player cannot spam
    private float canFire = -1f;
    //Variable to detect if the fire rate has been modified
    private bool isFireRateBoosted = false;
    //This variable baseSpeed is used for having a higher fire rate when the boost is collected
    private float boostedFireRate = 0.3f;
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

        //Looking for the object that have those components attached to them so that the player can interact with both collisions and powerups
        collisionDetection = GetComponent<CollisionDetection>();
        powerUpManager = FindObjectOfType<PowerUpManager>();

        //check if the powerupManager was found, and if it was preform the events
        //The listeners here are essential to detect the events and perform them rather than constantly checking for them
        if (powerUpManager != null)
        {
            powerUpManager.OnShieldCollected.AddListener(ActivateShield); //Activate the shield powerup
            powerUpManager.OnSpeedBoostCollected.AddListener(ActivateSpeedBoost); //Activate the speed boost
            powerUpManager.OnFireRateCollected.AddListener(ActivateFireRateBoost); //Activate the fire rate boost
        }

        //Reference to the spawnmanager for powerups
        spawnManager = FindObjectOfType<SpawnManager>();

        //reference to the shieldvisual component to show the shiel when the powerup is collected
        shieldVisual = GetComponent<ShieldVisual>();

        //Storing the current speed and fire rate as the base ones
        //By doing so, they can be used to provide the boost to those qualities
        baseSpeed = speed;
        currentFireRate = baseFireRate;

        //Updating the UI every time an enemy is destroyed and/or the player is hit
        UpdateScoreDisplay();
        UpdateHeartsDisplay();

        //This part is to check whether the game ended or not
        //If it did end, then it can skip to the next part and show the text
        //If the game is still going, hide the game over text message
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
        //I divided the main basic functions of the spaceship to keep the code more organized and avoid filling the update section with messy code
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
    }

    //Created a function to handle all the shooting mechanics to organize the code
    void ShootLaser()
    {
        //Technically this would allow the player to shoot everytime the space bar is pressed, allowing them to spam by holding down the key
        if (Input.GetKey(KeyCode.Space) && Time.time > canFire)  //For this reason I added a cooldown in between shots 
        {
            if (isFireRateBoosted) //checking if the poweriup has been collected
            {
                Shoot();
                canFire = Time.time + boostedFireRate; //faster fire rate
                PlayLaserSound();
            }
            else
            {
                Shoot();  
                canFire = Time.time + baseFireRate; //base fire rate
                PlayLaserSound();
            }
        }
    }


    void Shoot()
    {
        //Shooting a laser on the spaceship postition
        //Howevr I added an offset on the Y-Axis so that the lasers are spawning from the front of the spaceship and not inside
        Instantiate(playerLaserPrefab, transform.position + new Vector3(0, 1f, 0), Quaternion.identity);
    }

    //This is just a function to play a laser sound averytime a laser is instantiated
    void PlayLaserSound()
    {
        
        if (audioSource != null && laserSoundClip != null)
        {
            audioSource.PlayOneShot(laserSoundClip);
        }
    }

    //The function holding the collision between the player and the enemy
    //I wanted to make so that if the player and the enemy collide with eachother, the player takes 1 life of damage and the enemy gets destroyed
    void EnemyCollision()
    {
        //Previously I had a simple enemy = GameObject.FindGameObjectsWithTag("Enemy");
        //However, this meethod only counted as the first enemy spawned, meaning that the others where "Tagless"
        //This created issues with the hitboxes not being attached to the enemy
        //Therefore, I updated it to a list method so that every enemy spawned is detected as enemy using the assigned Tag
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        //Once again, since I am dealing with multiple enemies, I want the following to apply to every single one of them
        //So I am basically calling for the single enemy inside the list of enemies multiple times 
        foreach (GameObject enemy in enemies)
        {
            //Get enemy's dimensions - both height and width
            //Localscale gives more precision for the hitbox
            float enemyWidth = enemy.transform.localScale.x;
            float enemyHeight = enemy.transform.localScale.y;

            //Check if the laser collides with the enemy using the rectangle-based collision detection
            //Since I can not use colliders, rectangles are the easiest shape to code through logic
            if (collisionDetection.CheckCollision(
                    transform.position.x, transform.position.y, playerWidth, playerHeight,
                    enemy.transform.position.x + enemyHitboxOffsetX, enemy.transform.position.y + enemyHitboxOffsetY, enemyWidth + enemyHitboxTrimRight, enemyHeight))
            {
                //If the player shield is active, than the player takes no damage for that hit, but the shield is destroyed
                if (isShielded)
                {
                    DeactivateShield();
                    Destroy(enemy);
                    Debug.Log("Shield absorbed the hit");
                }
                else
                {

                    lives--; //otherwise the player loses 1 life

                    UpdateHeartsDisplay();//If this happens, then remove 1 heart from the player UI

                    //If the player's lives reach 0 it's game over
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

    //When a powerup is collected
    public void ActivateShield()
    {
        isShielded = true;
        //Debug.Log("Shield On");

        PlayPowerUpSound(); //play the sound
    }

    public void DeactivateShield()
    {
        if (isShielded)
        {
            isShielded = false;
            Destroy(enemy);
            IncreaseScore(1); //if the shield gets destroyed by an enemy, destroy both the enemy and the shield, but still add 1 point to the score
            //Debug.Log("Shield Off");
        }
    }

    //What happens when a speed boost is picked up?
    public void ActivateSpeedBoost()
    {
        StopAllCoroutines(); //Stop all the active coroutines to avoid an overlapping of effects
        StartCoroutine(SpeedBoostCoroutine());//After stopping the previous ones, start this speed boost coroutine
        PlayPowerUpSound(); //Play the power up sound as audio feedback
        //This method is perfect to avoid effects to mix together, providing an incredible amount of speed, adding the duration, etc.
    }

    IEnumerator SpeedBoostCoroutine()
    {
        speed = baseSpeed * 2f; //the speedboost provides double the base speed to the player
        yield return new WaitForSeconds(5f); //it lasts 5 seconds
        speed = baseSpeed; //lastly reset the speed to the base value
    }

    //What happens when the FireRateBoost boost is picked up?
    public void ActivateFireRateBoost()
    {
        if (isFireRateBoosted) return; //Avoid powerups to overlap eachother once again

        isFireRateBoosted = true; //detect whetehr the powerup is active or not
        currentFireRate = boostedFireRate; //change the fire rate to the faster version
        canFire = -1f;  

        StartCoroutine(ResetFireRateAfterSeconds(5f)); //Boost lasts 5 seconds before going back to normal
        PlayPowerUpSound(); //play the sound for audio feedback

    }

    //After 5 seconds, fire rate goes back to the original value with the original cooldown value between shots
    IEnumerator ResetFireRateAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        isFireRateBoosted = false;
        currentFireRate = baseFireRate;  
        canFire = Time.time + currentFireRate;  
    }

    //whenevr a point is gained, the score is increased by the determned amount
    public void IncreaseScore(int points)
    {
        score += points;
        UpdateScoreDisplay();
    }

    //Updating the UI score with the text every time a point is gained
    void UpdateScoreDisplay()
    {
        {
            if (scoreText != null)
            {
                scoreText.text = "Score: " + score;
            }
        }
    }

    //Updating the UI lives whenever the player is hit by an enemy
    void UpdateHeartsDisplay()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < lives)
            {
                hearts[i].sprite = fullHeartSprite; //if  life is higher than 1, show full heart
            }
            else
            {
                hearts[i].sprite = emptyHeartSprite; //if life is lower than 1, show empty heart
            }
        }
    }

    //Player the powerup sound every time a powerup is collected
    void PlayPowerUpSound()
    {
        if (audioSource != null && powerUpSoundClip != null)
        {
            audioSource.PlayOneShot(powerUpSoundClip);
        }
    }

    //When the player loses, show the game over screen
    void GameOver()
    {
        isGameOver = true;

        if (backgroundMusicSource != null && backgroundMusicSource.isPlaying) //if the music is still playing
        {
            backgroundMusicSource.Stop(); //stop iy
        }

        Time.timeScale = 0f; //stop / freeze every moving item

        gameObject.SetActive(false);

        //Remove all the listeners so they cannot take action anymore
        if (powerUpManager != null)
        {
            powerUpManager.OnShieldCollected.RemoveListener(ActivateShield);
            powerUpManager.OnSpeedBoostCollected.RemoveListener(ActivateSpeedBoost);
            powerUpManager.OnFireRateCollected.RemoveListener(ActivateFireRateBoost);
        }

        if (gameOverText != null) //Only if the game is over
        {
            gameOverText.text = "GAME OVER";  //Display the Game Over text
            gameOverText.gameObject.SetActive(true); //otherwise is set to flase and counted as hidden
        }
    }
}