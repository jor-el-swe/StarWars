using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyByWire : MonoBehaviour
{
    public GameHandler GameHandler;
   
    //user input
    private float _hAxis, _vAxis, _yawAxis;
    
    //audio
    public AudioSource thrusterSound;
    private bool soundEffectPlaying = false;
    private bool playSound = true;
    


    public List<Thruster> thrusters;
    
    private List<Thruster> _leftThrusters = new List<Thruster>();
    private List<Thruster> _rightThrusters = new List<Thruster>();
    private List<Thruster> _bottomThrusters =new List<Thruster>();
    private List<Thruster> _upThrusters =new List<Thruster>();
    private List<Thruster> _clockwiseThrusters =new List<Thruster>();
    private List<Thruster> _counterClockwiseThrusters =new List<Thruster>();

    
    
    // Start is called before the first frame update
    void Start()
    {
        foreach (var thruster in this.thrusters)
        {
            thruster.enabled = true;
            if (thruster.transform.localPosition.x > 0)
            {
                this._rightThrusters.Add(thruster);
                if (thruster.transform.localPosition.y < 0)
                {
                    this._clockwiseThrusters.Add(thruster);
                }
                if (thruster.transform.localPosition.y > 0)
                {
                    this._counterClockwiseThrusters.Add(thruster);
                }
            }
            
            if (thruster.transform.localPosition.x < 0)
            {
                this._leftThrusters.Add(thruster);
                if (thruster.transform.localPosition.y > 0)
                {
                    this._clockwiseThrusters.Add(thruster);
                }
                if (thruster.transform.localPosition.y < 0)
                {
                    this._counterClockwiseThrusters.Add(thruster);
                }
            }
            
            if (thruster.transform.localPosition.y < 0)
            {
                this._bottomThrusters.Add(thruster);
            }
            
            if (thruster.transform.localPosition.y > 0)
            {
                this._upThrusters.Add(thruster);
            }
            
        }

        thrusterSound.volume = 0;
        thrusterSound.Play();
    }

    // Update is called once per frame
  void Update()
    {
        //reset all previous scaling of forces 
        foreach (var thruster in this.thrusters)
        {
            thruster.forceMagnitude = 0f;
        }
        
        if (GameHandler.currentState != GameHandler.gameState.GameOver &&
            GameHandler.currentState != GameHandler.gameState.Crashed)
        {
            //get user axis input
            _hAxis = Input.GetAxis("StrafeShip");
            _vAxis = Input.GetAxis("FwdBck"); 
            _yawAxis = Input.GetAxis("YawShip");

            if (_hAxis != 0 || _vAxis != 0 || _yawAxis != 0)
            {
                playSound = true;
            }
            else
            {
                playSound = false;
            }

            if (_vAxis < 0)
            {
                 Debug.Log("moving down");
                 foreach (var thruster in this._upThrusters)
                 {
                     thruster.forceMagnitude++;
                 }
            }
            if (_vAxis > 0)
            {
                 Debug.Log("moving up");
    
                 foreach (var thruster in this._bottomThrusters)
                 {
                     thruster.forceMagnitude++;
                 }
  
            }
            if (_hAxis < 0)
            {
                 Debug.Log("moving left");
                 foreach (var thruster in this._rightThrusters)
                 {
                     thruster.forceMagnitude++;
                 }
                        
            }
            if (_hAxis > 0)
            {
                 Debug.Log("moving right");
                 foreach (var thruster in this._leftThrusters)
                 {
                     thruster.forceMagnitude++;
                 }
                        
            }
            if (_yawAxis > 0)
            {
                 Debug.Log("yawing counter clockwise");
                 foreach (var thruster in this._clockwiseThrusters)
                 {
                     thruster.forceMagnitude++;
                 }
            }
            if (_yawAxis < 0)
            {
                 Debug.Log("yawing clockwise");
                 foreach (var thruster in this._counterClockwiseThrusters)
                 {
                     thruster.forceMagnitude++;
                 }
            }
        }
        else
        {
            playSound = false;
        }

        //manage the in-game music        
        if (playSound) 
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
        
    }
}
