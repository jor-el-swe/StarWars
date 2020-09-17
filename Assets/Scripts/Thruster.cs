using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;
using Object = System.Object;

public class Thruster : MonoBehaviour
{
    public float forceMagnitude = 1f;
    private Rigidbody2D _rb2D;

    // Start is called before the first frame update
    void Start()
    {
        _rb2D = this.transform.GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame, if component is enabled
    void FixedUpdate()
    { 
        _rb2D.AddForce(-this.transform.up * this.forceMagnitude);
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
