using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PowerUpManager : MonoBehaviour
{
    public UnityEvent OnShieldCollected;
    public UnityEvent OnSpeedBoostCollected;
    public UnityEvent OnFireRateCollected;

    public Spaceship spaceship;


    // Start is called before the first frame update
    void Start()
    {
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

    private void Awake()
    {
        if (OnShieldCollected == null)
            OnShieldCollected = new UnityEvent();

        if (OnSpeedBoostCollected == null)
            OnSpeedBoostCollected = new UnityEvent();

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
