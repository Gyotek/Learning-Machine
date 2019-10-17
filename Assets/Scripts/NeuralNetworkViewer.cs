using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NeuralNetworkViewer : MonoBehaviour
{
    public static NeuralNetworkViewer instance;

    public Gradient colorGradient;

    public Transform viewerGroup;

    public float decalX = 50;
    public float decalY = -20f;

    public RectTransform neuronPrefab;
    RectTransform neuronInstance;
    Image[][] neurons;
    Text[][] neuronsValue;

    public RectTransform axonPrefab;
    RectTransform axonInstance;
    Image[][][] axons;

    public Text fitness;

    int x;
    int y;
    int z;

    Agent agent;

    private void Awake()
    {
        instance = this;
    }

    public void Init (Agent _agent)
    {
        agent = _agent;

        CreateViewer(agent.net);
    }

    void CreateViewer(NeuralNetwork net)
    {
        for (x = viewerGroup.childCount - 1; x >= 0; x--)
        {
            DestroyImmediate(viewerGroup.GetChild(x).gameObject);
        }

        InitNeurons(net);
        InitAxons(net);
    }

    void InitNeurons(NeuralNetwork net)
    {
        neurons = new Image[net.neurons.Length][];
        neuronsValue = new Text[net.neurons.Length][];

        for (x = 0; x < net.neurons.Length; x++)
        {
            neurons[x] = new Image[net.neurons[x].Length];
            neuronsValue[x] = new Text[net.neurons[x].Length];

            for (y = 0; y < net.neurons[x].Length; y++)
            {
                neuronInstance = Instantiate(neuronPrefab, Vector3.zero, Quaternion.identity, viewerGroup);
                neuronInstance.anchoredPosition = new Vector2(x * decalX, y * decalY);

                neurons[x][y] = neuronInstance.GetComponent<Image>();
                neuronsValue[x][y] = neuronInstance.GetChild(0).GetComponent<Text>();
            }
        }
    }

    float angle;
    float posX;
    float posY;
    float midPosX;
    float midPosY;
    void InitAxons(NeuralNetwork net)
    {
        axons = new Image[net.axons.Length][][];

        for (x = 0; x < net.axons.Length; x++)
        {
            axons[x] = new Image[net.axons[x].Length][];

            for (y = 0; y < net.axons[x].Length; y++)
            {
                axons[x][y] = new Image[net.axons[x][y].Length];

                for (z = 0; z < net.axons[x][y].Length; z++)
                {
                    midPosX = decalX * (x + 0.5f);
                    midPosY = (y - z) * decalY;


                    posX = x * decalX;
                    posY = y * decalY;
                    angle = Mathf.Atan2((y -z)* decalY, posX) * Mathf.Rad2Deg;

                    axonInstance = Instantiate(axonPrefab, Vector3.zero, Quaternion.identity, viewerGroup);
                    axonInstance.anchoredPosition = new Vector2(posX, posY);
                    axonInstance.eulerAngles = new Vector3(0, 0, angle);

                    axonInstance.sizeDelta = new Vector2(new Vector2(posX, (y - z) * decalY).magnitude, 2);

                    axons[x][y][z] = axonInstance.GetComponent<Image>();
                }

            }
        }
    }

    void Update()
    {
        
    }
}
