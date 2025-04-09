using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

//I made this script to deal with the visual part of the shield powerup
public class ShieldVisual : MonoBehaviour
{
    //variables for the shield visual effect
    public GameObject visualEffect;
    public Transform playerTransform;
    private GameObject activeShieldEffect;

    //when the shield is activated
    public void PlayShieldEffect()
    {
        activeShieldEffect = Instantiate(visualEffect, playerTransform.position, Quaternion.identity); //instantiate it on the player
        activeShieldEffect.transform.SetParent(playerTransform); //set the spaceship as the parent so the sheild always follows the player
        activeShieldEffect.transform.localPosition = new Vector3(0, 0, 0);
    }

    //when the shield is no more in use
    public void DeactivateShieldEffect()
    {
        if (activeShieldEffect != null)
        {
            Destroy(activeShieldEffect); //destroy it
            //Debug.Log("Shield visual effect deactivated.");
        }
    }
}