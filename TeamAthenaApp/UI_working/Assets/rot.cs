using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rot : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate the object around its local X axis at 1 degree per second
       // transform.Rotate(Vector3.right * Time.deltaTime);

        // ...also rotate around the World's Y axis
        transform.Rotate(new Vector3(0f,0f,1f));
    }
}
