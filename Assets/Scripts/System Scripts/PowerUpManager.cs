using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//I created this manager script solely to handle all the elements in common between the various power ups 
public class PowerUpManager : MonoBehaviour
{
    //I used events for these powerps as they are easy enough to be called, and because I was constrained to use events for this assignment
    //Here are the event variables for the 3 different power ups
    public UnityEvent OnShieldCollected;
    public UnityEvent OnSpeedBoostCollected;
    public UnityEvent OnFireRateCollected;

    public Spaceship spaceship;


    // Start is called before the first frame update
    void Start()
    {
        //Here I am simply adding the listeners for each power up so I can call them in the other scripts
        if (spaceship != null)
        {
            OnShieldCollected.AddListener(spaceship.ActivateShield);
            OnSpeedBoostCollected.AddListener(spaceship.ActivateSpeedBoost);
            OnFireRateCollected.AddListener(spaceship.ActivateFireRateBoost);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //This function is necessary for the events to happen, as it is calling the event whenever the intended powerup is picked up
    private void Awake()
    {
        //Event 1 for the shield powerup
        if (OnShieldCollected == null)
            OnShieldCollected = new UnityEvent();

        //Event 2 for the speed boost
        if (OnSpeedBoostCollected == null)
            OnSpeedBoostCollected = new UnityEvent();

        //Event 3 for the fire rate boost
        if (OnFireRateCollected == null)
            OnFireRateCollected = new UnityEvent();
    }

    public void TriggerShield()
    {
        OnShieldCollected.Invoke();
    }

    public void TriggerSpeedBoost()
    {
        OnSpeedBoostCollected.Invoke();
    }

    public void TriggerFireRate()
    {
        OnFireRateCollected.Invoke();
    }

}
