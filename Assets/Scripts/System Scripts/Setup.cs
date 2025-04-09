using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//I made this script only for the purpose of dealing with listeners
public class Setup : MonoBehaviour
{
    public PowerUpManager powerUpManager;
    public Spaceship spaceship;
    public ShieldVisual shieldVisual;

    // Start is called before the first frame update
    void Start()
    {
        //Here I am using the PowerUp Manager to listen for a powerup to be collected and then act
        powerUpManager.OnShieldCollected.AddListener(spaceship.ActivateShield);
        powerUpManager.OnShieldCollected.AddListener(shieldVisual.PlayShieldEffect);
        powerUpManager.OnSpeedBoostCollected.AddListener(spaceship.ActivateSpeedBoost);

        powerUpManager.OnShieldCollected.AddListener(() =>
        {
            powerUpManager.OnShieldCollected.RemoveListener(shieldVisual.PlayShieldEffect);
        });
    }
}