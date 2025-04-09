using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//I made this script to deal with the visual part of the shield powerup
public class ShieldVisual : MonoBehaviour
{
    public GameObject visualEffect;
    public Transform playerTransform;
    private GameObject activeShieldEffect;

    public void PlayShieldEffect()
    {
        activeShieldEffect = Instantiate(visualEffect, playerTransform.position, Quaternion.identity);
        activeShieldEffect.transform.SetParent(playerTransform);
        activeShieldEffect.transform.localPosition = new Vector3(0, 0, 0);
    }


    public void DeactivateShieldEffect()
    {
        if (activeShieldEffect != null)
        {
            Destroy(activeShieldEffect); 
            Debug.Log("Shield visual effect deactivated.");
        }
    }
}