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
                
                //show start message
                //sendmessage() can be used to invoke method also
                hudController.ShowStartMessage();
                
                
                //reset spaceship position
                spaceshipController.ResetSpaceshipStartPosition();


                if (Input.GetKey(KeyCode.Space) || _hasRestarted)
                {
                    currentState = gameState.Flying;
                }
                break;
            
            case gameState.Flying:

                hudController.ShowFlyingMessage();
                
                //enter landing state if we are close enough
                if (spaceshipController.IsApproachingLanding())
                {
                    currentState = gameState.Landing;
                }

                //crash if we hit the space station too hard
                if (spaceshipController.HasCrashed)
                {
                    currentState = gameState.Crashed;
                    _fadeOnce = true;
                }
                
                //crash if we get too far from spacestation
                if (spaceshipController.IsLostInSpace())
                {
                    currentState = gameState.Crashed;
                    _fadeOnce = true;
                }
                
                break;
            

            case gameState.Landing:
                hudController.ShowLandingMessage();

                if (!spaceshipController.IsApproachingLanding())
                {
                    currentState = gameState.Flying;
                    break;
                }

                //crash if we hit the space station too hard
                if (spaceshipController.HasCrashed)
                {
                    currentState = gameState.Crashed;
                    _fadeOnce = true;
                }
                
                //land if we are close enough and slow enough and correctly aligned
                if (spaceshipController.IsLanding())
                {
                    currentState = gameState.GameOver;
                    _fadeOnce = true;
                }
                break;
            
            case gameState.GameOver:
                if (_fadeOnce)
                {
                    StartCoroutine(FadeAudioSource.StartFade(mainTheme, 1f, 0f));
                    _fadeOnce = false;
                }
  
                hudController.ShowGameOverMessage();
                spaceshipController.StopSpaceship();
                
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
    
}
