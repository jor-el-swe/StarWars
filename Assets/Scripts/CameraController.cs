using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    public GameObject targetObject;


    private Vector3 _offset;
    // Start is called before the first frame update
    void Start()
    {
        transform.LookAt(targetObject.transform);
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(targetObject.transform);
    }
}
