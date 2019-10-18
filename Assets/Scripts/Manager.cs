﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Manager instance;
    private void Awake() { instance = this; }

    public int populationSize = 50;
    public int runPopulationSize = 2;
    public float trainingDuration = 10;

    public int[] layer;

    public int mutationRate = 8;

    public GameObject agentPrefab;
    public Transform agentGroup;

    List<Agent> agents = new List<Agent>();
    Agent agent;

    public Transform startPoints;
    List<Transform> startPos = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < startPoints.childCount - 1; i++)
        {
            startPos.Add(startPoints.GetChild(i));
        }

        StartCoroutine(InitCoroutine());
    }

    IEnumerator InitCoroutine()
    {
        NewGeneration();
        InitNeuralNetworkViewer();
        Load();
        Focus();

        yield return new WaitForSeconds(trainingDuration);
        StartCoroutine(Loop());
    }

    IEnumerator Loop()
    {

        NewGeneration();
        Focus();

        yield return new WaitForSeconds(trainingDuration);
        StartCoroutine(Loop());
    }

    void NewGeneration()
    {
        AddOrRemoveAgent();
        agents.Sort();
        Mutate();
        ResetAgent();
        SetColor();
    }
    
    void AddOrRemoveAgent()
    {
        if(ModeManager.instance.mode == ModeManager.Mode.Training)  //TRAINING MODE
        {
            if (agents.Count == populationSize)
                return;

            int dif = populationSize - agents.Count;

            if (dif > 0)
                for (int i = 0; i < dif; i++)
                {
                    AddAgent();
                }
            else if (dif < 0)
                for (int i = 0; i < -dif; i++)
                {
                    RemoveAgent();
                }
        }
        else if (ModeManager.instance.mode == ModeManager.Mode.Run)  //RUN MODE
        {
            if (agents.Count == runPopulationSize)
                return;

            int dif = runPopulationSize - agents.Count;
            if (dif > 0)
                for (int i = 0; i < dif; i++)
                {
                    AddAgent();
                }
            else if (dif < 0)
                for (int i = 0; i < -dif; i++)
                {
                    RemoveAgent();
                }
        }
    }


    
    void AddAgent()
    {
        if (ModeManager.instance.mode == ModeManager.Mode.Training)  //TRAINING MODE
        {
            agent = (Instantiate(agentPrefab, Vector3.zero, Quaternion.identity, agentGroup)).GetComponent<Agent>();
            agent.net = new NeuralNetwork(layer);
            agents.Add(agent);
        }
        else if (ModeManager.instance.mode == ModeManager.Mode.Run)    //RUN MODE
        {
            for (int i = 0; i < runPopulationSize; i++)
            {
                agent = (Instantiate(agentPrefab, startPos[i].position, Quaternion.identity, agentGroup)).GetComponent<Agent>();
                agent.net = new NeuralNetwork(layer);
                agents.Add(agent);
                //agents[i].gameObject.transform.position = startPos[i].position;
            }
        }
    }

    void RemoveAgent()
    {
        Destroy(agents[agents.Count-1].gameObject);
        agents.RemoveAt(agents.Count - 1);
    }

    private void Mutate()
    {
        for (int i = agents.Count/2; i < agents.Count; i++)
        {
            agents[i].net.CopyNet(agents[i-(agents.Count/2)].net);
            agents[i].net.Mutate(mutationRate);
        }
    }

    void ResetAgent()
    {
        for (int i = 0; i < agents.Count; i++)
        {
            if (ModeManager.instance.mode == ModeManager.Mode.Training)   //TRAINING MODE
                agents[i].ResetAgent(Vector3.zero);
            else if (ModeManager.instance.mode == ModeManager.Mode.Run)  //RUN MODE
                agents[i].ResetAgent(startPos[i].position);
        }
    }

    public void Refocus()
    {
        agents.Sort();
        Focus();
    }

    void Focus()
    {
        NeuralNetworkViewer.instance.agent = agents[0];
        NeuralNetworkViewer.instance.RefreshAxon();

        CameraControler.instance.target = agents[0].transform;
    }

    public void Load()
    {
        Data data = DataManager.instance.Load();

        if (data != null)
            for (int i = 0; i < agents.Count; i++)
            {
                agents[i].net = data.nets[i];
            }

        StopAllCoroutines();
        StartCoroutine(Loop());
    }

    [ContextMenu("Save")]
    public void Save()
    {
        List<NeuralNetwork> nets = new List<NeuralNetwork>();

        for (int i = 0; i < agents.Count; i++)
        {
            nets.Add(agents[i].net);
        }

        DataManager.instance.Save(nets);
    }

    void SetColor()
    {
        agents[0].SetFirstColor();
        if (ModeManager.instance.mode == ModeManager.Mode.Training)          //TRAINING MODE
        {
            for (int i = 1; i < populationSize; i++)
                agents[i].SetDefaultColor();

            for (int i = populationSize / 2; i < populationSize; i++)
                agents[i].SetMutateColor();
        }
        else if(ModeManager.instance.mode == ModeManager.Mode.Run)      //RUN MODE
        {
            for (int i = 1; i < runPopulationSize; i++)
                agents[i].SetDefaultColor();

            for (int i = runPopulationSize / 2; i < runPopulationSize; i++)
                agents[i].SetMutateColor();
        }

    }

    public void Restart()
    {
        StopAllCoroutines();
        StartCoroutine(Loop());
    }

    public void ResetNets()
    {
        for (int i = 0; i < agents.Count; i++)
        {
            agents[i].net = new NeuralNetwork(agent.net.layers);
        }
        Restart();
    }

    public void ResetMode()
    {
        ResetAgent();
        Load();
    }

    void InitNeuralNetworkViewer()
    {
        NeuralNetworkViewer.instance.Init(agents[0]);
    }

    public int AgentPosition(Agent _agent)
    {
        agents.Sort();
        for (int i = 0; i < agents.Count; i++)
        {
            if (_agent == agents[i])
            {
                return i;
            }
        }
        return 0;
    }
}
