using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightMovement : MonoBehaviour
{
    public float MovementSpeed = 1f;

    Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        startPos = this.transform.position;

        transform.position = startPos;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * MovementSpeed * Time.deltaTime);

        if (transform.position.x > 10)
        {
        this.transform.position = startPos;
        }
        
        //Vector3 startPos = new Vector3(10, transform.position.y, transform.position.z);

    }
}
