using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class holo : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject pan;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(
                Camera.main.transform.position,
                Camera.main.transform.forward,
                out hitInfo,
                20.0f,
                Physics.DefaultRaycastLayers))
        {
            // If the Raycast has succeeded and hit a hologram
            // hitInfo's point represents the position being gazed at
            // hitInfo's collider GameObject represents the hologram being gazed at
        }
    }
    public void on_off() {
        if (pan.activeSelf){
            pan.SetActive(false);
        }
        else
        {

            pan.SetActive(true);
        }
    }
}
