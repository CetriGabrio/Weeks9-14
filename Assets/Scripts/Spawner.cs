using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject SquareBuild;
    [SerializeField] GameObject CircleBuild;
    [SerializeField] GameObject TriangleBuild;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;

        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(SquareBuild, pos, Quaternion.identity);
            //Destroy(SquareBuild, 5f);
        }

        if (Input.GetMouseButtonDown(1))
        {
            Instantiate(CircleBuild, pos, Quaternion.identity);
            //Destroy(SquareBuild, 5f);
        }

        if (Input.GetMouseButtonDown(2))
        {
            Instantiate(TriangleBuild, pos, Quaternion.identity);
            //Destroy(SquareBuild, 5f);
        }
    }
}
