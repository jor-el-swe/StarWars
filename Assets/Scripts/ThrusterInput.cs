using UnityEngine;

public class ThrusterInput : MonoBehaviour
{
    public string buttonName;
    
    public Thruster thruster;
  // Start is called before the first frame update
    void Start()
    {
        thruster = GetComponent<Thruster>();
    }

    // Update is called once per frame
    void Update()
    {
       thruster.enabled = Input.GetButton(this.buttonName);
    }
}
