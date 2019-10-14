using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : CameraControler
{
    public override void Init()
    {
        Destroy(target.GetComponent<Agent>());
    }
}
