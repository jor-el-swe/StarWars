using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyByWire : MonoBehaviour
{
    public GameHandler GameHandler;
    
    public Thruster t1, t2, t3, t4;
    
   
    
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


            if (Input.GetAxis("FwdBck") < 0)
            {
                Debug.Log("moving down");
                t1.enabled = true;
                t2.enabled = true;
                t3.enabled = false;
                t4.enabled = false;
            }
            else if (Input.GetAxis("FwdBck") > 0)
            {
                Debug.Log("moving up");
                t1.enabled = false;
                t2.enabled = false;
                t3.enabled = true;
                t4.enabled = true;
            }
            else if (Input.GetAxis("StrafeShip") < 0)
            {
                Debug.Log("moving left");
                t1.enabled = false;
                t2.enabled = true;
                t3.enabled = false;
                t4.enabled = true;
                ;
            }
            else if (Input.GetAxis("StrafeShip") > 0)
            {
                Debug.Log("moving right");
                t1.enabled = true;
                t2.enabled = false;
                t3.enabled = true;
                t4.enabled = false;
                ;
            }
            else if (Input.GetAxis("YawShip") < 0)
            {
                Debug.Log("yawing counter clockwise");
                t1.enabled = false;
                t2.enabled = true;
                t3.enabled = true;
                t4.enabled = false;
            }
            else if (Input.GetAxis("YawShip") > 0)
            {
                Debug.Log("yawing clockwise");
                t1.enabled = true;
                t2.enabled = false;
                t3.enabled = false;
                t4.enabled = true;
            }
            else
            {
                t1.enabled = Input.GetButton("Thruster1");
                t2.enabled = Input.GetButton("Thruster2");
                t3.enabled = Input.GetButton("Thruster3");
                t4.enabled = Input.GetButton("Thruster4");
            }
        }
        else
        {
            t1.enabled = false;
            t2.enabled = false;
            t3.enabled = false;
            t4.enabled = false; 
        }

        //.... continye the same way with the rest of the axes
        //.... SOOO much if/else. must be a smarter way to do that.
    }
}
