using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;



public class GameHandler : MonoBehaviour
{
    public enum gameState {
        Start,
        Flying, 
        Landing,
        GameOver, 
        Crashed
    }
    public gameState currentState = gameState.Start;
    private bool _hasRestarted = false;

    //UI handling
    [SerializeField] HUDController hudController = null;
    
    
    private Vector2 startPosition;
    private float startRotation;
    private const float maxLandingDistance = 3.0f;
    private const float maxLandingVelocity = 0.3f;
    private const float maxSpaceDistance = 15.0f;
    
    
    //move this logic to its respective components
    [SerializeField]  Rigidbody2D spaceShipRB = null;
    [SerializeField]  GameObject landingZone = null;

    
    [SerializeField] Collider2D thruster1 = null;
    [SerializeField] Collider2D thruster2 = null;
    [SerializeField] Collider2D landingLeg1Collider = null;
    [SerializeField] Collider2D landingLeg2Collider = null;
    [SerializeField] Collider2D landingZoneCollider = null;
    [SerializeField] Collider2D spaceStationCollider = null;
    
    //game music
    [SerializeField]  AudioSource mainTheme = null;
    private bool _fadeOnce = true;

    
    // Start is called before the first frame update
    void Start()
    {
        startPosition = spaceShipRB.position;
        startRotation = spaceShipRB.rotation;
        mainTheme.volume = 0;
        mainTheme.Play();

    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case gameState.Start:

                if (_fadeOnce)
                {
                    StartCoroutine(FadeAudioSource.StartFade(mainTheme, 2f, 0.5f));
                    _fadeOnce = false;
                }

                //Debug.Log("state:start");
                hudController.ShowStartMessage();
                
                //reset spaceship position
                spaceShipRB.position = startPosition;
                spaceShipRB.rotation = startRotation;
                spaceShipRB.angularVelocity = 0f;
                    
                if (Input.GetKey(KeyCode.Space) || _hasRestarted)
                {
                    currentState = gameState.Flying;
                }
                spaceShipRB.velocity = new Vector2(0,0);
                break;
            
                case gameState.Flying:
                //Debug.Log("state:flying");
                hudController.ShowFlyingMessage();
                
                //enter landing state if we are close enough
                if (Vector2.Distance(spaceShipRB.position, landingZone.transform.position) < maxLandingDistance)
                {
                    currentState = gameState.Landing;
                }
                
                //crash if we hit the space station too hard
                TestIfCrashed();
                TestIfLostInSpace();
                
                break;
            

            case gameState.Landing:
                //Debug.Log("state:landing");
                hudController.ShowLandingMessage();

                if (Vector2.Distance(spaceShipRB.position, landingZone.transform.position) > maxLandingDistance)
                {
                    currentState = gameState.Flying;
                    break;
                }

                //crash if we hit the space station too hard
                TestIfCrashed();
                
                //land if we are close enough and slow enough and correctly aligned
                if (landingLeg1Collider.Distance(landingZoneCollider).isOverlapped &&
                    landingLeg2Collider.Distance(landingZoneCollider).isOverlapped
                    )
                {
                    if (spaceShipRB.velocity.magnitude < maxLandingVelocity)
                    {
                        currentState = gameState.GameOver;
                        _fadeOnce = true;
                    }
                    else
                    {
                        currentState = gameState.Crashed;
                        _fadeOnce = true;
                    }
                }
                break;
            
            case gameState.GameOver:
                if (_fadeOnce)
                {
                    StartCoroutine(FadeAudioSource.StartFade(mainTheme, 1f, 0f));
                    _fadeOnce = false;
                }
                //Debug.Log("state:game over");
                hudController.ShowGameOverMessage();
                
                spaceShipRB.velocity = new Vector2(0,0);
                
                if (Input.GetKey(KeyCode.Space))
                {
                    currentState = gameState.Start;
                    _fadeOnce = true;
                }
                //show game over message
                break;
            
            case gameState.Crashed:
                if (_fadeOnce)
                {
                    StartCoroutine(FadeAudioSource.StartFade(mainTheme, 1f, 0f));
                    _fadeOnce = false;
                }
                //Debug.Log("state:crashed");
                
                //show game over message
                hudController.ShowCrashedMessage();
                
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    _fadeOnce = true;
                    _hasRestarted = true;
                    currentState = gameState.Start;
                }
                break;
            
            default:
                break;
        }
        
    }

    void TestIfCrashed()
    {
        if (landingLeg1Collider.Distance(spaceStationCollider).isOverlapped ||
            landingLeg2Collider.Distance(spaceStationCollider).isOverlapped ||
            thruster1.Distance(spaceStationCollider).isOverlapped ||
            thruster2.Distance(spaceStationCollider).isOverlapped 
        )
        {
            if (spaceShipRB.velocity.magnitude > maxLandingVelocity)
            {
                currentState = gameState.Crashed;
                _fadeOnce = true;
 
            }
        }
    }

    void TestIfLostInSpace()
    {
        if (Vector2.Distance(spaceShipRB.position, landingZone.transform.position) > maxSpaceDistance)
        {
            currentState = gameState.Crashed;
            _fadeOnce = true;
        }
    }
}
