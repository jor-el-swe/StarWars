using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTransform : MonoBehaviour
{
   // private Quaternion _rotation;
    

    public Transform SpaceShip;
    private Camera _cam;

    
    // Start is called before the first frame update
    void Start()
    {
       _cam = transform.GetComponent<Camera>();
       _cam.fieldOfView = 200;
       StartCoroutine( StartFade(_cam, 5f, 60) );
    }

    void Awake()
    {
        
      //  _rotation = transform.rotation;
    }

    private void Update()
    {
        transform.LookAt(SpaceShip);
    }

    void LateUpdate()
    {
        
       // transform.rotation = _rotation;
    }
    

    public static IEnumerator StartFade(Camera cam, float duration, float targetFOV)
    {
        float currentTime = 0;
        float start = 200; 

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            cam.fieldOfView = Mathf.Lerp(start, targetFOV, currentTime / duration);
            yield return null;
        }
        yield break;
    }

}