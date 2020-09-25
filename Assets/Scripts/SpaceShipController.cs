using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/*this script controls collisions and if spacehip is lost in space
*/

public class SpaceShipController : MonoBehaviour
{
    [SerializeField] private GameObject [] landingZones = null;
    [SerializeField] private Collider2D landingZoneCollider = null;
  
    //getcomponentsinchildren for these thrusters!!!
    [SerializeField] private Collider2D landingLeg1Collider = null;
    [SerializeField] private Collider2D landingLeg2Collider = null;
    
    
    private Vector2 _startPosition;
    private float _startRotation;
    public  float MaxLandingDistance = 3.0f;
    private const float MaxLandingVelocity = 0.4f;
    private const float MaxSpaceDistance = 15.0f;

    public TextMeshProUGUI speedText = null;
    public TextMeshProUGUI maxSpeedText = null;

    private Rigidbody2D _spaceShipRb;
    private bool _hasCrashed = false;
    
    
    public bool HasCrashed
    {
        get { return _hasCrashed; }
        set { _hasCrashed = value; }
    }
    
    // Start is called before the first frame update
    void Start()
    {        
        _spaceShipRb = GetComponent<Rigidbody2D>();
        
        _startPosition = _spaceShipRb.position;
        _startRotation = _spaceShipRb.rotation;

    }

    private void Update()
    {
        var currentSpeed = Math.Round(_spaceShipRb.velocity.magnitude * 100f);
        var maxLandingSpeed = Math.Round(MaxLandingVelocity * 100f);
        
        speedText.SetText( currentSpeed.ToString() );
        maxSpeedText.SetText(maxLandingSpeed.ToString());
    }

    public void ResetSpaceshipStartPosition()
    {
        _hasCrashed = false;
        _spaceShipRb.position = _startPosition;
        _spaceShipRb.rotation = _startRotation;
        _spaceShipRb.angularVelocity = 0f;
        _spaceShipRb.velocity = new Vector2(0,0);
    }

    public void StopSpaceship()
    {
        _spaceShipRb.velocity = new Vector2(0,0);
    }   
    
    public bool IsLostInSpace()
    {
        var isCloseEnough = false;
        
        foreach (var landingZone in landingZones)
        {
            if (Vector2.Distance(_spaceShipRb.position, landingZone.transform.position) < MaxSpaceDistance)
            {
                isCloseEnough = true;
            }
        }
 
        return !isCloseEnough;
    }

    public bool IsLanding()
    {

        return (landingLeg1Collider.Distance(landingZoneCollider).isOverlapped &&
                landingLeg2Collider.Distance(landingZoneCollider).isOverlapped &&
                _spaceShipRb.velocity.magnitude < MaxLandingVelocity &&
                !_hasCrashed);
        
    }

    public bool IsApproachingLanding()
    {
        var isCloseEnough = false;
        
        foreach (var landingZone in landingZones)
        {
            if (Vector2.Distance(_spaceShipRb.position, landingZone.transform.position) < MaxLandingDistance)
            {
                isCloseEnough = true;
            }
        }
 
        return isCloseEnough;
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        _hasCrashed = _spaceShipRb.velocity.magnitude > MaxLandingVelocity;
    }
}
