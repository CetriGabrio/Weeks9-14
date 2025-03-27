using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightMovement : MonoBehaviour
{
    public float MovementSpeed = 1f;

    Vector3 startPos;
    Vector3 trailStartPos;

    [Range(0,1)]
    public float t;
    public AnimationCurve Curve;

    public TrailRenderer Trail;

    // Start is called before the first frame update
    void Start()
    {
        startPos = this.transform.position;
        trailStartPos = this.transform.position;

        transform.position = startPos;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * MovementSpeed * Time.deltaTime);

        //startPos.y = startPos.y * t;

        if (transform.position.x > 10)
        {

            this.transform.position = startPos;

            Trail.enabled = false;
        }

       //if (transform.position.x < 10)
        //{
            //this.transform.position = trailStartPos;

            //Trail.enabled = true;
        //}
    }
}
