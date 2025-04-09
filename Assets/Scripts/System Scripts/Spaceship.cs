using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Spaceship : MonoBehaviour
{

    public float speed = 1f;
    private float baseSpeed;

    public GameObject playerLaserPrefab;
    public GameObject enemy;

    private CollisionDetection collisionDetection;

    public float firerate = 1f;
    private float canFire = -1f;    
    private bool isFireRateBoosted = false;

    public float playerWidth = 1f;
    public float playerHeight = 2f;

    public int lives = 3;

    public int score = 0;
    public TextMeshProUGUI scoreText;

    float enemyHitboxOffsetX = -1f;
    float enemyHitboxOffsetY = +0.5f;
    float enemyHitboxTrimRight = -1.5f;

    private SpawnManager spawnManager;

    public bool isShielded = false;
    public ShieldVisual shieldVisual;

    // Start is called before the first frame update
    void Start()
    {
        //Reset player position at the start of the game to the centre of the screen
        transform.position = new Vector2(0, -2);

        collisionDetection = GetComponent<CollisionDetection>();

        spawnManager = FindObjectOfType<SpawnManager>();

        shieldVisual = GetComponent<ShieldVisual>();

        baseSpeed = speed;

        UpdateScoreDisplay();

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

    //Created a function t handle all the shooting mechanics to organize the code
    void ShootLaser()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (isFireRateBoosted)
            {
                Shoot();
            }
            else if (Time.time > canFire)
            {
                canFire = Time.time + firerate;
                Shoot();
            }
        }
    }

    void Shoot()
    {
        // Replace this with your laser instantiation logic
        Instantiate(playerLaserPrefab, transform.position + new Vector3(0, 1f, 0), Quaternion.identity);
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

                    if (lives < 1)
                    {
                        spawnManager.StopSpawning();
                        Destroy(this.gameObject);
                        Debug.Log("Game Over");
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
        Debug.Log("Shield On");
    }

    public void DeactivateShield()
    {
        if (isShielded)
        {
            isShielded = false;
            Destroy(enemy);
            IncreaseScore(1);
            Debug.Log("Shield Off");
        }
        else
        {
            Debug.Log("Damage");
        }
    }

    public void ActivateSpeedBoost()
    {
        StopAllCoroutines();
        StartCoroutine(SpeedBoostCoroutine());
    }

    IEnumerator SpeedBoostCoroutine()
    {
        speed = baseSpeed * 2f;
        Debug.Log("Speed boost on!");
        yield return new WaitForSeconds(5f);
        speed = baseSpeed;
        Debug.Log("Speed boost off.");
    }

    public void ActivateFireRateBoost()
    {
        if (isFireRateBoosted) return;

        Debug.Log("Fire rate boosted!");
        isFireRateBoosted = true;
        canFire = -1f;

        StartCoroutine(ResetFireRateAfterSeconds(5f));
    }

    IEnumerator ResetFireRateAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        isFireRateBoosted = false;
        canFire = Time.time + firerate;
        Debug.Log("Fire rate returned to normal.");
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
}