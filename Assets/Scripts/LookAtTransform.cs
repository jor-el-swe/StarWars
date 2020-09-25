using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTransform : MonoBehaviour
{
    

    public Transform SpaceShip;
    private Camera _cam;

    private bool _zoomedIn = false;
    
    public bool zoomedIn
    {
        get { return _zoomedIn; }
    }
    
    // Start is called before the first frame update
    void Start()
    {
       _cam = transform.GetComponent<Camera>();
        _cam.fieldOfView = 200;
       StartCoroutine( StartFade(_cam, 3f, 60) );
    }

    private void Update()
    {
        transform.LookAt(SpaceShip);
    }
    
    private IEnumerator StartFade(Camera cam, float duration, float targetFOV)
    {
        float currentTime = 0;
        const float start = 200; 

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            cam.fieldOfView = Mathf.Lerp(start, targetFOV, currentTime / duration);
            yield return null;
        }

        _zoomedIn = true;
        
        yield break;
    }

}