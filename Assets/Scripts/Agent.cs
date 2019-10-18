using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Agent : MonoBehaviour, IComparable<Agent>
{
    public NeuralNetwork net;
    public CarControler carControler;
    public float fitnesss;
    public float rayRange = 1;

    public LayerMask layerMask;

    float[] inputs;

    public Transform nextCheckpoint;
    public float nextCheckpointDist;
    public float distanceTraveled = 0;
    public float lastDistCheckpoint;

    public Rigidbody rb;

    public Material firstMat;
    public Material mutateMat;
    public Material defaultMat;

    public Renderer render;
    public Renderer posRenderer;

    private void Start()
    {
        Init();
    }

    private void FixedUpdate()
    {
        InputUpdate();
        OutputUpdate();

        UpdateFitness();

        lastDistCheckpoint = distCheckpoint;
    }

    float DistCheckPoint()
    {
        return ClosestDistToPointOnLigne(nextCheckpoint.position, nextCheckpoint.position + nextCheckpoint.forward, transform.position);
    }

    float ClosestDistToPointOnLigne(Vector3 vA,Vector3 vB, Vector3 vPoint)
    {
        return (vA + (vB - vA).normalized*Vector3.Dot((vB - vA).normalized, vPoint - vA)- vPoint).magnitude;
    }

    Vector3 v;
    float d;
    float t;
    Vector3 ClosestPointToPointOnLigne(Vector3 vA, Vector3 vB, Vector3 vPoint)
    {
        v = (vB - vA).normalized;
        d = Vector3.Distance(vA, vB);
        t = Vector3.Dot(v, vPoint - vA);

        if (t <= 0)
            return vA;

        if (t >= d)
            return vB;

        return vA + v * t;
    }

    public void ResetAgent(Vector3 spawnPos)
    {
        fitnesss = 0;
        transform.position = spawnPos;
        transform.rotation = Quaternion.identity;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        distanceTraveled = 0;

        Init();
    }

    private void Init()
    {
        inputs = new float[net.layers[0]];

        nextCheckpoint = CheckpointManager.instance.firstCheckpoint;
        nextCheckpointDist = (transform.position - nextCheckpoint.position).magnitude;
    }

    public void CheckpointReached(Transform _nextCheckpoint)
    {
        distanceTraveled += nextCheckpointDist;
        nextCheckpoint = _nextCheckpoint;
        nextCheckpointDist = (transform.position - nextCheckpoint.position).magnitude;
        lastDistCheckpoint = DistCheckPoint();
    }

    float distCheckpoint;
    Vector3 pos;
    float angleZ;
    void InputUpdate()
    {
        pos = transform.position;

        inputs[0] = RaySensor(pos + Vector3.up * 0.2f, transform.forward, 32f);
        inputs[1] = RaySensor(pos + Vector3.up * 0.2f, transform.right, 12f);
        inputs[2] = RaySensor(pos + Vector3.up * 0.2f, -transform.right, 12f);
        inputs[3] = RaySensor(pos + Vector3.up * 0.2f, transform.right + transform.forward, 16f);
        inputs[4] = RaySensor(pos + Vector3.up * 0.2f, -transform.right + transform.forward, 16f);

        inputs[5] = 1 - (float)Math.Tanh(rb.velocity.magnitude / 20);
        inputs[6] = (float)Math.Tanh(rb.angularVelocity.y * 0.1f);
        inputs[7] = (float)Math.Tanh(Manager.instance.AgentPosition(this));
        inputs[8] = RaySensor(pos + Vector3.up * 0.2f, -transform.forward, 16f);
        inputs[9] = RaySensor(pos + Vector3.up * 0.2f, transform.right + -transform.forward, 16f);
        inputs[10] = RaySensor(pos + Vector3.up * 0.2f, -transform.right + -transform.forward, 16f);

        /*
        distCheckpoint = DistCheckPoint();
        inputs[6] = (float)Math.Tanh((lastDistCheckpoint - distCheckpoint)*5);
        */
        //inputs[7] = (float)Math.Tanh(rb.angularVelocity.z * 0.1f);
        /*
        inputs[8] = (float)Math.Tanh(transform.InverseTransformDirection(ClosestPointToPointOnLigne(nextCheckpoint.position, nextCheckpoint.position + nextCheckpoint.forward, transform.position)).x);
        inputs[9] = (float)Math.Tanh(transform.InverseTransformDirection(ClosestPointToPointOnLigne(nextCheckpoint.position, nextCheckpoint.position + nextCheckpoint.forward, transform.position)).z);
        */

        /*
        angleZ = transform.eulerAngles.z > 180 ? transform.eulerAngles.z - 360 : transform.eulerAngles.z;
        inputs[11] = (float)Math.Tanh(angleZ / 90f);
        */
    }

    public void OutputUpdate()
    {
        net.FeedForward(inputs);

        carControler.horizontalInput = net.neurons[net.layers.Length - 1][0];
        carControler.verticalInput = net.neurons[net.layers.Length - 1][1];
    }

    void UpdateFitness()
    {
        SetFitness(distanceTraveled + (nextCheckpointDist - (transform.position - nextCheckpoint.position).magnitude));// * /*(float)Math.Tanh(*/Manager.instance.AgentPosition(this));
    }

    void SetFitness(float _newFitness)
    {
        if (fitnesss < _newFitness)
            fitnesss = _newFitness;
    }

    RaycastHit hit;
    float RaySensor(Vector3 pos, Vector3 direction, float length)
    {
        if (!Physics.Raycast(pos, direction, out hit, length, layerMask))
        {
            Debug.DrawRay(pos, direction * hit.distance, Color.red);
            return 0;
        }
        
        Debug.DrawRay(pos, direction * hit.distance, Color.yellow);
        return (rayRange * length - hit.distance) / (rayRange * length);
    }

    public int CompareTo(Agent other)
    {
        if (fitnesss < other.fitnesss)
            return 1;
        else if (fitnesss > other.fitnesss)
            return -1;
        else
            return 0;
    }

    public void SetDefaultColor()
    {
        render.material = defaultMat;
        posRenderer.material = defaultMat;
    }

    public void SetMutateColor()
    {
        render.material = mutateMat;
        posRenderer.material = mutateMat;
    }

    public void SetFirstColor()
    {
        render.material = firstMat;
        posRenderer.material = firstMat;
    }
}
