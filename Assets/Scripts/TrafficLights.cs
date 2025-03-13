using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TrafficLights : MonoBehaviour
{
    public UnityEvent OnTimerHasFinished;
    public float timerLength = 3;
    public float t;

    void Update()
    {
        t += Time.deltaTime;
        if (t > timerLength)
        {
            OnTimerHasFinished.Invoke();
            t = 0;
        }
    }
}
