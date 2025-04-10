using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alpha1 : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            //Color Thing = new Color();

            spriteRenderer.color = Color.blue;
        }
    }
}
