using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using Object = System.Object;

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

    //UI handling
    public TextMeshProUGUI welcomeText;
    public TextMeshProUGUI flyingText;
    public TextMeshProUGUI landingText;
    public TextMeshProUGUI successText;
    public TextMeshProUGUI crashText;
    
    
    private Vector2 startPosition;
    private float startRotation;
    public float maxLandingDistance = 3.0f;
    public float maxLandingVelocity = 0.3f;
    public float maxSpaceDistance = 15.0f;
    
    public Rigidbody2D spaceShipRB;
    public GameObject landingZone;

    public Collider2D thruster1;
    public Collider2D thruster2;
    public Collider2D landingLeg1Collider;
    public Collider2D landingLeg2Collider;
    public Collider2D landingZoneCollider;
    public Collider2D spaceStationCollider;
    
    //game music
    public AudioSource mainTheme;
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
                crashText.enabled = false;
                successText.enabled = false;
                welcomeText.enabled = true;
                
                //reset spaceship position
                spaceShipRB.position = startPosition;
                spaceShipRB.rotation = startRotation;
                spaceShipRB.angularVelocity = 0f;
                    
                if (Input.GetKey(KeyCode.Space))
                {
                    currentState = gameState.Flying;
                }
                spaceShipRB.velocity = new Vector2(0,0);
                break;
            
                case gameState.Flying:
                //Debug.Log("state:flying");
                

                landingText.enabled = false;
                welcomeText.enabled = false;
                flyingText.enabled = true;
                
                //land if we are close enough and slow enough and correctly aligned
                if (Vector2.Distance(spaceShipRB.position, landingZone.transform.position) < maxLandingDistance)
                {
                    currentState = gameState.Landing;
                    //display landing message
                }
                
                //crash if we hit the space station too hard
                TestIfCrashed();
                TestIfLostInSpace();
                
                break;
            

            case gameState.Landing:
                //Debug.Log("state:landing");
                flyingText.enabled = false;
                landingText.enabled = true;

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
                landingText.enabled = false;
                successText.enabled = true;
                
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
                flyingText.enabled = false;
                landingText.enabled = false;
                crashText.enabled = true;
                
                if (Input.GetKey(KeyCode.Space))
                {
                    _fadeOnce = true;
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
