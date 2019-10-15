using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Transform nextCheckpoint;

    private void OnTriggerEnter(Collider other)
    {
        Agent agent = other.GetComponent<Agent>();

        if (agent || agent.nextCheckpoint != transform)
            return;
        agent.CheckpointReached(nextCheckpoint);



    }
}
