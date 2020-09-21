using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyByWire : MonoBehaviour
{
    public GameHandler GameHandler;
    
    public Thruster t1, t2, t3, t4;

    private bool t1Active, t2Active, t3Active, t4Active;
    
   
    
    // Start is called before the first frame update
    void Start()
    {
 
    }

    // Update is called once per frame
    void Update()
    {
        if (GameHandler.currentState != GameHandler.gameState.GameOver &&
            GameHandler.currentState != GameHandler.gameState.Crashed)
        {
            t1Active = false;
            t2Active = false; 
            t3Active = false;
            t4Active = false;
    
            //next: refactor by removing all "else" to enable multiple commands
            if (Input.GetAxis("FwdBck") < 0)
            {
                Debug.Log("moving down");
                t1Active = true;
                t2Active = true;
            }
            else if (Input.GetAxis("FwdBck") > 0)
            {
                Debug.Log("moving up");
                t3Active = true;
                t4Active = true;
            }
            else if (Input.GetAxis("StrafeShip") < 0)
            {
                Debug.Log("moving left");
                t2Active = true;
                t4Active = true;
                
            }
            else if (Input.GetAxis("StrafeShip") > 0)
            {
                Debug.Log("moving right");
                t1Active = true;
                t3Active = true;
                
            }
            else if (Input.GetAxis("YawShip") < 0)
            {
                Debug.Log("yawing counter clockwise");
                t2Active = true; 
                t3Active = true;
            }
            else if (Input.GetAxis("YawShip") > 0)
            {
                Debug.Log("yawing clockwise");
                t1Active = true;
                t4Active = true;
            }
            else
            {
                //next: move this section to the top, to avoid the "else"
                t1Active = Input.GetButton("Thruster1");
                t2Active = Input.GetButton("Thruster2");
                t3Active = Input.GetButton("Thruster3");
                t4Active = Input.GetButton("Thruster4");
            }
        }

        t1.enabled = t1Active;
        t2.enabled = t2Active;
        t3.enabled = t3Active;
        t4.enabled = t4Active;
    }
}
