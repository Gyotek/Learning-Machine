using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    public Transform nextChexkpoint;


    public void CheckpointReached(Transform _nextCheckpoint)
    {
        nextChexkpoint = _nextCheckpoint;
    }
}
