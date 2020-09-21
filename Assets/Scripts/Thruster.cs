using System;

using UnityEngine;
using UnityEngine.UIElements;
using Object = System.Object;

public class Thruster : MonoBehaviour
{
    public float forceMagnitude = 1f;
    private Rigidbody2D _rb2D;
    private ParticleSystem _particleSystem;


    // Start is called before the first frame update
    void Start()
    {
        _rb2D = this.transform.GetComponent<Rigidbody2D>();
        _particleSystem = GetComponent<ParticleSystem>();
        _particleSystem.Play();
    }

    // Update is called once per frame, if component is enabled
    void FixedUpdate()
    { 
        _rb2D.AddForce(-this.transform.up * this.forceMagnitude);

        //blow fire from thrusters
        var main = _particleSystem.main;
        main.startLifetimeMultiplier = this.forceMagnitude * 2f;
    }

    private void OnEnable()
    {
        _particleSystem.Play();
    }

    private void OnDisable()
    {
        _particleSystem.Stop();
    }
}
