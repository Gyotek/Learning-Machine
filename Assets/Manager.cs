using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public int populationSize = 50;
    public float trainingDuration = 30;

    public int[] layer;

    public int mutationRate = 8;

    public GameObject agentPrefab;
    public Transform agentGroup;

    List<Agent> agents = new List<Agent>();
    Agent agent;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(InitCoroutine());
    }

    IEnumerator InitCoroutine()
    {
        NewGeneration();
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
        if (agents.Count == populationSize)
            return;

        int dif = populationSize - agents.Count;

        if(dif > 0)
            for (int i = 0; i < dif; i++)
            {
                AddAgent();
            }
        else if(dif < 0)
            for (int i = 0; i < -dif; i++)
            {
                RemoveAgent();
            }
    }

    void AddAgent()
    {
        agent = (Instantiate(agentPrefab, Vector3.zero, Quaternion.identity, agentGroup)).GetComponent<Agent>();
        agent.net = new NeuralNetwork(layer);
        agents.Add(agent);
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
            agents[i].ResetAgent();
        }
    }

    public void Refocus()
    {
        agents.Sort();
        Focus();
    }

    void Focus()
    {
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

        for (int i = 1; i < populationSize ; i++)
        {
            agents[i].SetDefaultColor();
        }

        for (int i = populationSize/2 ; i < populationSize; i++)
        {
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
}
