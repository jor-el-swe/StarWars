using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyByWire : MonoBehaviour
{
    
    //[SerializeField] instead of public
    
    private const int LIMIT_ANGULAR_VELOCITY = 10;
    public GameHandler GameHandler;
   
    //user input
    private float _hAxis, _vAxis, _yawAxis;
    
    //audio
    public AudioSource thrusterSound;
    private bool _soundEffectPlaying = false;
    private bool _playSound = true;

    private Rigidbody2D _spaceshipRb;

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
        _spaceshipRb = GetComponent<Rigidbody2D>();
        
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
                _playSound = true;
            }
            else
            {
                _playSound = false;
            }

            if (_vAxis < 0)
            {
                 Debug.Log("moving down");
                 ActivateThrusters(_upThrusters, 1);
            }
            if (_vAxis > 0)
            {
                 Debug.Log("moving up");
                 ActivateThrusters(_bottomThrusters, 1);
            }
            if (_hAxis < 0)
            {
                 Debug.Log("moving left");
                 ActivateThrusters(_rightThrusters, 1);
            }
            if (_hAxis > 0)
            {
                 Debug.Log("moving right");
                 ActivateThrusters(_leftThrusters, 1);
            }
            if (_yawAxis > 0)
            {
                 Debug.Log("yawing counter clockwise");
                 ActivateThrusters(_clockwiseThrusters, 1);
            }
            if (_yawAxis < 0)
            {
                 Debug.Log("yawing clockwise");
                 ActivateThrusters(_counterClockwiseThrusters, 1);
            }

            if (Input.GetKey(KeyCode.Space))
            {
                //TODO
                //make the brakes work  using the thrusters instead
                DecayVelocity();
            }
        }
        else
        {
            _playSound = false;
        }

        //manage the in-game music        
        if (_playSound) 
        {
            if (!_soundEffectPlaying)
            {
                StartCoroutine(FadeAudioSource.StartFade(thrusterSound, 0.1f, 0.3f));
                _soundEffectPlaying = true;
            }
        }
        else
        {
            if (_soundEffectPlaying)
            { 
                StartCoroutine(FadeAudioSource.StartFade(thrusterSound, 0.1f, 0.0f));
                _soundEffectPlaying = false;
            }
        }
        
    }

  void DecayVelocity()
  {
      if (Math.Abs(_spaceshipRb.angularVelocity) > LIMIT_ANGULAR_VELOCITY)
      {
          //first check angular velocity, and apply counter/clockwise thrusters
          if (_spaceshipRb.angularVelocity > 0)
          {
              ActivateThrusters(_clockwiseThrusters, 2);

          }
          else if (_spaceshipRb.angularVelocity < 0)
          {
              ActivateThrusters(_counterClockwiseThrusters, 2);
          }
      }
      else
      {
          _spaceshipRb.angularVelocity = 0;
      }
  
                
      //then check velocity in x/y and apply thrusters in that direction
      if (_spaceshipRb.velocity.magnitude > 0.1)
      {
          var localDir = this.transform.InverseTransformDirection(_spaceshipRb.velocity);
          if (localDir.x > 0)
          {
              ActivateThrusters(_rightThrusters, 1);  
          }
      
          if (localDir.x < 0)
          {
              ActivateThrusters(_leftThrusters, 1);          
          }
      
          if (localDir.y > 0)
          {
              ActivateThrusters(_upThrusters, 1);       
          }
      
          if (localDir.y < 0)
          {
              ActivateThrusters(_bottomThrusters, 1);           
          }
      }
      else
      {
          _spaceshipRb.velocity = new Vector2(0,0);
      }

  }
  
  //maybe improve implementation later to take axes as parameter
  void ActivateThrusters(List<Thruster> inputThrusters, int value)
  {
      foreach (var thruster in inputThrusters)
      {
          thruster.forceMagnitude+=value;
      }
  }
  
}
