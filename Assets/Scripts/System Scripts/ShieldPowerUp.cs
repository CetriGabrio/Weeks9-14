using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is a script dedicated solely to the Shield boost power up
//It's atatched to the prefab of the power up and it rules its behaviour

public class ShieldPowerUp : MonoBehaviour
{
    //As ususal, detecting the width and height of the rectangular shape
    public float powerUpWidth = 1f;
    public float powerUpHeight = 1f;

    //Detecting the width and height of the player (a rectangular shape too)
    public float playerHitboxWidth = 2f;
    public float playerHitboxHeight = 2f;

    //The falling speed for the power up from the top of the screen
    public float fallSpeed = 2f;

    //The various variables to attach this to
    private GameObject player;
    private CollisionDetection collisionDetection;
    public PowerUpManager powerUpManager;

    void Start()
    {
        //Since I want the power up to be collected and only interact with the player, I used tags
        //The player, being the only element tagged as "Player" would be the only eligeble element that could interact with the powerup
        player = GameObject.FindWithTag("Player");
        collisionDetection = GetComponent<CollisionDetection>();

        //Not being able to attach gameobjects from the hierarchy to a prefab, the best solution was to look for the intended element directly
        powerUpManager = FindObjectOfType<PowerUpManager>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerPickup();

        //Simple movement to make the power up fall downwards at a constant speed from the top of the screen
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
    }

    void CheckPlayerPickup()
    {
        if (player == null || powerUpManager == null) return;

        //Detecting the actual collision between the player hitbox and the powerup hitbox
        //Therefore, the code will only go through if the collision is correctly detected
        if (collisionDetection.CheckCollision(
            transform.position.x, transform.position.y, powerUpWidth, powerUpHeight,
            player.transform.position.x, player.transform.position.y, playerHitboxWidth, playerHitboxHeight))
        {
            //Triggering the powerUp event which is stored in the PowerUp manager script
            powerUpManager.TriggerShield();

            //Once collected, destroy this gameobject
            Destroy(gameObject);
        }
    }
}
