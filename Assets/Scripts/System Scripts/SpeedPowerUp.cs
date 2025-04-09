using UnityEngine;

public class SpeedBoostPowerUp : MonoBehaviour
{
    public float powerUpWidth = 1f;
    public float powerUpHeight = 1f;

    public float playerHitboxWidth = 2f;
    public float playerHitboxHeight = 2f;

    public float fallSpeed = 2f;

    private GameObject player;
    private CollisionDetection collisionDetection;
    public PowerUpManager powerUpManager;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        collisionDetection = GetComponent<CollisionDetection>();

        powerUpManager = FindObjectOfType<PowerUpManager>();

    }

    void Update()
    {
        CheckPlayerPickup();

        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
    }

    void CheckPlayerPickup()
    {
        if (player == null || powerUpManager == null) return;

        if (collisionDetection.CheckCollision(
            transform.position.x, transform.position.y, powerUpWidth, powerUpHeight,
            player.transform.position.x, player.transform.position.y, playerHitboxWidth, playerHitboxHeight))
        {
            powerUpManager.TriggerSpeedBoost();
            Destroy(gameObject);
        }
    }
}
