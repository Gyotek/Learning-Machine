using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    public CarControler carControler;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        carControler.horizontalInput = Input.GetAxis("Horizontal");
        carControler.verticalInput = Input.GetAxis("Vertical");
    }
}
