using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*this script controls collisions and if spacehip is lost in space
*/

public class SpaceShipController : MonoBehaviour
{
    [SerializeField] private  GameObject landingZone = null;
    [SerializeField] private Collider2D landingZoneCollider = null;
    [SerializeField] private Collider2D spaceStationCollider = null;


    //getcomponentsinchildren for these thrusters!!!
    [SerializeField] private Collider2D thruster1 = null;
    [SerializeField] private Collider2D thruster2 = null;
    [SerializeField] private Collider2D landingLeg1Collider = null;
    [SerializeField] private Collider2D landingLeg2Collider = null;
    
    
    private Vector2 _startPosition;
    private float _startRotation;
    private const float MaxLandingDistance = 3.0f;
    private const float MaxLandingVelocity = 0.3f;
    private const float MaxSpaceDistance = 15.0f;

    private Rigidbody2D _spaceShipRb;
    
    // Start is called before the first frame update
    void Start()
    {        
        _spaceShipRb = GetComponent<Rigidbody2D>();
        
        _startPosition = _spaceShipRb.position;
        _startRotation = _spaceShipRb.rotation;

    }
    
    public void ResetSpaceshipStartPosition()
    {
        _spaceShipRb.position = _startPosition;
        _spaceShipRb.rotation = _startRotation;
        _spaceShipRb.angularVelocity = 0f;
        _spaceShipRb.velocity = new Vector2(0,0);
    }

    public void StopSpaceship()
    {
        _spaceShipRb.velocity = new Vector2(0,0);
    }   
    
    public bool HasCrashed()
    {
        if (landingLeg1Collider.Distance(spaceStationCollider).isOverlapped ||
            landingLeg2Collider.Distance(spaceStationCollider).isOverlapped ||
            thruster1.Distance(spaceStationCollider).isOverlapped ||
            thruster2.Distance(spaceStationCollider).isOverlapped 
        )
        {
            if (_spaceShipRb.velocity.magnitude > MaxLandingVelocity)
            {
                return true;
            }
        }
        return false;
    }

    public bool IsLostInSpace()
    {
        if (Vector2.Distance(_spaceShipRb.position, landingZone.transform.position) > MaxSpaceDistance)
        {
            return true;
        }
        return false;
    }

    public bool IsLanding()
    {

        return (landingLeg1Collider.Distance(landingZoneCollider).isOverlapped &&
                landingLeg2Collider.Distance(landingZoneCollider).isOverlapped &&
                _spaceShipRb.velocity.magnitude < MaxLandingVelocity);
        
    }

    public bool IsApproachingLanding()
    {
        return (Vector2.Distance(_spaceShipRb.position, landingZone.transform.position) < MaxLandingDistance);

    }
}
