using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setup : MonoBehaviour
{
    public PowerUpManager powerUpManager;
    public Spaceship spaceship;
    public ShieldVisual shieldVisual;

    // Start is called before the first frame update
    void Start()
    {
        powerUpManager.OnShieldCollected.AddListener(spaceship.ActivateShield);
        powerUpManager.OnShieldCollected.AddListener(shieldVisual.PlayShieldEffect);
        powerUpManager.OnSpeedBoostCollected.AddListener(spaceship.ActivateSpeedBoost);

        powerUpManager.OnShieldCollected.AddListener(() =>
        {
            powerUpManager.OnShieldCollected.RemoveListener(shieldVisual.PlayShieldEffect);
        });
    }
}