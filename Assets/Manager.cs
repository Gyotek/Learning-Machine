using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public int populationSize = 50;
    public float trainingDuration = 30;

    public int[] layer;

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
        agents.Sort();
        AddOrRemoveAgent();
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

    void Focus()
    {
        CameraControler.instance.target = agents[0].transform;
    }
}
