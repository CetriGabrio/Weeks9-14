using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PowerUpManager : MonoBehaviour
{
    public UnityEvent OnShieldCollected;

    public UnityEvent OnSpeedBoostCollected;

    // Start is called before the first frame update
    void Start()
    {
        
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
    }

    public void TriggerShield()
    {
        //Debug.Log("Shield Collected");
        OnShieldCollected.Invoke();
    }
    public void TriggerSpeedBoost()
    {
        //Debug.Log("Speed Boost collected!");
        OnSpeedBoostCollected.Invoke();
    }

}
