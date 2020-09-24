using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDController : MonoBehaviour
{
    //UI handling
    //create a new UI component
    //make that aware of the gamestate
    
    
    //findobjectoftype(hud)/gamemanager)
    //getcomponentsinchildren
    
    
    
    
    //add text for showing current speed and target speed
    
    [SerializeField] TextMeshProUGUI welcomeText = null;
    [SerializeField] TextMeshProUGUI flyingText = null;
    [SerializeField] TextMeshProUGUI landingText = null;
    [SerializeField] TextMeshProUGUI successText = null;
    [SerializeField] TextMeshProUGUI crashText = null;


    public void ShowStartMessage()
    {
        crashText.enabled = false;
        successText.enabled = false;
        welcomeText.enabled = true;
        landingText.enabled = false;
    }

    public void ShowFlyingMessage()
    {
        landingText.enabled = false;
        welcomeText.enabled = false;
        flyingText.enabled = true;
    }

    public void ShowLandingMessage()
    {
        flyingText.enabled = false;
        landingText.enabled = true;
    }

    public void ShowGameOverMessage()
    {
        landingText.enabled = false;
        successText.enabled = true;
    }

    public void ShowCrashedMessage()
    {
        flyingText.enabled = false;
        landingText.enabled = false;
        crashText.enabled = true;
    }
}
