using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyByWire : MonoBehaviour
{
    public GameHandler GameHandler;
    
    public Thruster t1, t2, t3, t4;

    private bool t1Active, t2Active, t3Active, t4Active;
    private float _hAxis, _vAxis, _yawAxis;

    private bool soundEffectPlaying = false;
    public AudioSource thrusterSound;

    
   
    
    // Start is called before the first frame update
    void Start()
    {
        thrusterSound.volume = 0;
        thrusterSound.Play();
 
    }

    // Update is called once per frame
  void Update()
    {
        if (GameHandler.currentState != GameHandler.gameState.GameOver &&
            GameHandler.currentState != GameHandler.gameState.Crashed)
        {
            t1Active = Input.GetButton("Thruster1");
            t2Active = Input.GetButton("Thruster2");
            t3Active = Input.GetButton("Thruster3");
            t4Active = Input.GetButton("Thruster4");
            
            //reset all previous scaling of forces (previous diagonal moves)
            t1.forceMagnitude = 1;
            t2.forceMagnitude = 1;
            t3.forceMagnitude = 1;
            t4.forceMagnitude = 1;

            _hAxis = Input.GetAxis("StrafeShip");
            _vAxis = Input.GetAxis("FwdBck"); 
            _yawAxis = Input.GetAxis("YawShip");
    
           
             if (_vAxis < 0)
             {
                 Debug.Log("moving down");
                 t1Active = true;
                 t2Active = true;
                 
                 //counteract the rotation when going on a diagonal
                 if (_hAxis > 0)
                 {
                     t1.forceMagnitude = 2 * _hAxis;
                 }
                 if (_hAxis < 0)
                 {
                     t2.forceMagnitude = -2 *_hAxis;
                 }
             }
             if (_vAxis > 0)
             {
                 Debug.Log("moving up");
                 t3Active = true;
                 t4Active = true;
                 
                 //counteract the rotation when going on a diagonal
                 if (_hAxis > 0)
                 {
                     t3.forceMagnitude = 2 * _hAxis;
                 }
                 if (_hAxis < 0)
                 {
                     t4.forceMagnitude = -2 *_hAxis;
                 }
             }
             if (_hAxis < 0)
             {
                 Debug.Log("moving left");
                 t2Active = true;
                 t4Active = true;
                        
             }
             if (_hAxis > 0)
             {
                 Debug.Log("moving right");
                 t1Active = true;
                 t3Active = true;
                        
             }
             if (_yawAxis < 0)
             {
                 Debug.Log("yawing counter clockwise");
                 t2Active = true; 
                 t3Active = true;
             }
             if (_yawAxis > 0)
             {
                 Debug.Log("yawing clockwise");
                 t1Active = true;
                 t4Active = true;
             }

        }
        else
        {
            t1Active = false;
            t2Active = false;
            t3Active = false;
            t4Active = false;
        }

        if (t1Active || t2Active || t3Active || t4Active) 
        {
            if (!soundEffectPlaying)
            {
                StartCoroutine(FadeAudioSource.StartFade(thrusterSound, 0.1f, 0.3f));
                soundEffectPlaying = true;
            }
        }
        else
        {
            if (soundEffectPlaying)
            { 
                StartCoroutine(FadeAudioSource.StartFade(thrusterSound, 0.1f, 0.0f));
                soundEffectPlaying = false;
            }
        }

        t1.enabled = t1Active;
        t2.enabled = t2Active;
        t3.enabled = t3Active;
        t4.enabled = t4Active;
    }
}
