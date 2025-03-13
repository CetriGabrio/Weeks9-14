using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventsDemo : MonoBehaviour
{
    public RectTransform banana;

    public UnityEvent OnTimerHasFinished;
    public float timerLength = 3;
    public float t;

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        if(t > timerLength)
        {
            OnTimerHasFinished.Invoke();
            t = 0;
        }
    }

    public void MouseJustEnteredImage()
    {
        Debug.Log("The mouse just entered in the image!");
        banana.localScale = Vector3.one * 1.3f;
    }

    public void MouseJustLeftImage()
    {
        Debug.Log("The mouse just left the image!");
        banana.localScale = Vector3.one;

    }

}
