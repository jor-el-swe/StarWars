using System;
using UnityEngine;

public class Thruster : MonoBehaviour
{ 
    //public bool isActive;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


    }

    private void OnEnable()
    {
        GetComponent<ParticleSystem>().Play();
    }

    private void OnDisable()
    {
        GetComponent<ParticleSystem>().Stop();
    }
}
