using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

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
    
    private Vector2 startPosition;
    private float startRotation;
    public float maxLandingDistance = 3.0f;
    public float maxLandingVelocity = 0.3f;

    public Rigidbody2D spaceShipRB;
    public GameObject landingZone;
    
    public Collider2D landingLeg1Collider;
    public Collider2D landingLeg2Collider;
    public Collider2D landingZoneCollider;
    
    // Start is called before the first frame update
    void Start()
    {
        startPosition = spaceShipRB.position;
        startRotation = spaceShipRB.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case gameState.Start:
                Debug.Log("state:start");
                //reset spaceship position
                spaceShipRB.position = startPosition;
                spaceShipRB.rotation = startRotation;
                
                if (Input.GetKey(KeyCode.Space))
                {
                    currentState = gameState.Flying;
                }
                spaceShipRB.velocity = new Vector2(0,0);
                break;
            case gameState.Flying:
                Debug.Log("state:flying");
                //land if we are close enough and slow enough and correctly aligned
                if (Vector2.Distance(spaceShipRB.position, landingZone.transform.position) < maxLandingDistance)
                {
                    currentState = gameState.Landing;
                    //display landing message
                }
                break;

            case gameState.Landing:
                //Debug.Log("state:landing");

                if (Vector2.Distance(spaceShipRB.position, landingZone.transform.position) > maxLandingDistance)
                {
                    currentState = gameState.Flying;
                    //display flying message
                }

               
                //land if we are close enough and slow enough and correctly aligned
                if (landingLeg1Collider.Distance(landingZoneCollider).isOverlapped &&
                    landingLeg2Collider.Distance(landingZoneCollider).isOverlapped
                    )
                {
                    if (spaceShipRB.velocity.magnitude < maxLandingVelocity)
                    {
                        currentState = gameState.GameOver;
                    }
                    else
                    {
                        currentState = gameState.Crashed;
                    }
                        
                }
                break;
            
            case gameState.GameOver:
                Debug.Log("state:game over");
                spaceShipRB.velocity = new Vector2(0,0);
                
                if (Input.GetKey(KeyCode.Space))
                {
                    currentState = gameState.Start;
                }
                //show game over message
                break;
            
            case gameState.Crashed:
                Debug.Log("state:crashed");
                if (Input.GetKey(KeyCode.Space))
                {
                    currentState = gameState.Start;
                }
                //show game over message
                break;
            
            default:
                break;
        }
        
    }
}
