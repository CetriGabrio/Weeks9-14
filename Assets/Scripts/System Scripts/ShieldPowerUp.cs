using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPowerUp : MonoBehaviour
{

    public float powerUpWidth = 1f;
    public float powerUpHeight = 1f;

    public float playerHitboxWidth = 2f;
    public float playerHitboxHeight = 2f;

    public float speed = 2f;

    private GameObject player;
    private CollisionDetection collisionDetection;
    public PowerUpManager powerUpManager;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        collisionDetection = GetComponent<CollisionDetection>();

        powerUpManager = FindObjectOfType<PowerUpManager>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerPickup();

        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }

    void CheckPlayerPickup()
    {
        if (player == null || powerUpManager == null) return;

        if (collisionDetection.CheckCollision(
            transform.position.x, transform.position.y, powerUpWidth, powerUpHeight,
            player.transform.position.x, player.transform.position.y, playerHitboxWidth, playerHitboxHeight))
        {
            powerUpManager.TriggerShield();
            Destroy(gameObject);
        }
    }
}
