using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //Variable for the falling down speed of the enemy
    public float speed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Function to move the enemy only downwards at a constant speed
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        //Pac-Man effect once again, this time vetically to ensure that if the enemy reaches the bottom of the screen, it teleports to the top
        if (transform.position.y < -6.2f)
        {
            //Added a random range on the X so that the position of the teleported enemy is unpredictable
            transform.position = new Vector3(Random.Range(-9.5f, 9.5f), 6.2f, 0);
        }
    }
}
