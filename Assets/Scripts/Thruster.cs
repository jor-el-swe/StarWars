using System;

using UnityEngine;
using UnityEngine.UIElements;
using Object = System.Object;

public class Thruster : MonoBehaviour
{
    public float forceMagnitude = 1f;
    private Rigidbody2D _rb2D;
    public AudioSource thrusterSound;

    // Start is called before the first frame update
    void Start()
    {
        _rb2D = this.transform.GetComponent<Rigidbody2D>();
        thrusterSound.volume = 0;
        thrusterSound.Play();
    }

    // Update is called once per frame, if component is enabled
    void FixedUpdate()
    { 
        _rb2D.AddForce(-this.transform.up * this.forceMagnitude);

    }

    private void OnEnable()
    {
        GetComponent<ParticleSystem>().Play();
        StartCoroutine(FadeAudioSource.StartFade(thrusterSound, 0.1f, 0.6f));

    }

    private void OnDisable()
    {
        GetComponent<ParticleSystem>().Stop();
        StartCoroutine(FadeAudioSource.StartFade(thrusterSound, 0.1f, 0f));

    }
}
