using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    public CarController carController;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        carController.horizontalInput = Input.GetAxis("Horizontal");
        carController.verticalInput = Input.GetAxis("Vertical");
    }
}
