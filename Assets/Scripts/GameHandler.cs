using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;



public class GameHandler : MonoBehaviour
{
    public enum gameState {
        Init,
        Start,
        Flying, 
        Landing,
        GameOver, 
        Crashed
    }
    public gameState currentState = gameState.Init;
    private bool _hasRestarted = false;

    //UI handling
    [SerializeField] HUDController hudController = null;
    public GameObject mainCamera = null;
    
    //Spaceship collision handling
    [SerializeField] SpaceShipController spaceshipController = null;
    
    //game music
    [SerializeField]  AudioSource mainTheme = null;
    private bool _fadeOnce = true;

    
    // Start is called before the first frame update
    void Start()
    {

        mainTheme.volume = 0;
        mainTheme.Play();
        if (_fadeOnce)
        {
            StartCoroutine(FadeAudioSource.StartFade(mainTheme, 2f, 0.5f));
            _fadeOnce = false;
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case gameState.Init:
                
                if (mainCamera.GetComponent<LookAtTransform>().zoomedIn)
                {
                    currentState = gameState.Start;
                    //show start message
                    hudController.ShowStartMessage();
                }

                break;
            case gameState.Start:

                if (_fadeOnce)
                {
                    StartCoroutine(FadeAudioSource.StartFade(mainTheme, 2f, 0.5f));
                    _fadeOnce = false;
                }
                
                //reset spaceship position
                spaceshipController.ResetSpaceshipStartPosition();


                if (Input.GetKey(KeyCode.Space) || _hasRestarted)
                {
                    currentState = gameState.Flying;
                    hudController.ShowFlyingMessage();
                }
                break;
            
            case gameState.Flying:

                //enter landing state if we are close enough
                if (spaceshipController.IsApproachingLanding())
                {
                    currentState = gameState.Landing;
                    hudController.ShowLandingMessage();
                }

                //crash if we hit the space station too hard
                if (spaceshipController.HasCrashed())
                {
                    currentState = gameState.Crashed;
                    hudController.ShowCrashedMessage();
                    _fadeOnce = true;
                }
                
                //crash if we get too far from spacestation
                if (spaceshipController.IsLostInSpace())
                {
                    currentState = gameState.Crashed;
                    hudController.ShowCrashedMessage();
                    _fadeOnce = true;
                }
                
                break;
            

            case gameState.Landing:
                
                if (!spaceshipController.IsApproachingLanding())
                {
                    currentState = gameState.Flying;
                    hudController.ShowFlyingMessage();
                    break;
                }

                //crash if we hit the space station too hard
                if (spaceshipController.HasCrashed())
                {
                    currentState = gameState.Crashed;
                    hudController.ShowCrashedMessage();
                    _fadeOnce = true;
                }
                
                //land if we are close enough and slow enough and correctly aligned
                if (spaceshipController.IsLanding())
                {
                    currentState = gameState.GameOver;
                    hudController.ShowGameOverMessage();
                    _fadeOnce = true;
                }
                break;
            
            case gameState.GameOver:
                if (_fadeOnce)
                {
                    StartCoroutine(FadeAudioSource.StartFade(mainTheme, 1f, 0f));
                    _fadeOnce = false;
                }
  
                spaceshipController.StopSpaceship();
                
                if (Input.GetKey(KeyCode.Space))
                {
                    //show start message
                    hudController.ShowStartMessage();
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
                
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    //show start message
                    hudController.ShowStartMessage();
                    _fadeOnce = true;
                    _hasRestarted = true;
                    currentState = gameState.Start;
                }
                break;
            
            default:
                break;
        }
        
    }
    
}
