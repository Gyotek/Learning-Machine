using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralNetwork
{
    public int[] layers;
    public float[][] neurons;
    public float[][][] axons;

    int x;
    int y;
    int z;

    public NeuralNetwork()
    {

    }

    public NeuralNetwork(int[] _layers)
    {
        layers = new int[_layers.Length];

        for (x = 0; x < _layers.Length; x++)
        {
            layers[x] = _layers[x];
        }

        InitNeurons();
        InitAxons();
    }

    void InitNeurons()
    {
        List<float[]> neuronsList = new List<float[]>();

        for (x= 0; x < layers.Length; x++)
        {
            neuronsList.Add(new float[layers[x]]);
        }

        neurons = neuronsList.ToArray();
    }

    void InitAxons()
    {
        List<float[][]> axonsList = new List<float[][]>();
        for (x = 1; x < layers.Length; x++)
        {
            List<float[]> layerAxonsList = new List<float[]>();

            int neuronsInPreviousLayer = layers[x - 1];

            for (y = 0; y < layers[x]; y++)
            {
                float[] neuronAxons = new float[neuronsInPreviousLayer];

                for (z = 0; z < neuronsInPreviousLayer; z++)
                {
                    neuronAxons[z] = Random.Range(-1f, 1f);
                }

                layerAxonsList.Add(neuronAxons);
            }

            axonsList.Add(layerAxonsList.ToArray());
        }

        axons = axonsList.ToArray();
    }

    float value;
    public void FeedForward(float[] inputs)
    {
        neurons[0] = inputs;

        for (x = 1; x < layers.Length; x++)
        {
            for (y = 0; y < layers[x]; y++)
            {
                value = 0;

                for (z = 0; z < layers[x-1]; z++)
                {
                    value = neurons[x - 1][z]*axons[x-1][y][z];
                }

                neurons[x][y] = (float)System.Math.Tanh(value);
            }
        }
    }

    float randomNumber;
    public void Mutate(float probability)
    {
        for (x = 0; x < axons.Length; x++)
        {
            for (y = 0; y < axons[x].Length; y++)
            {
                for (z = 0; z < axons[x][y].Length; z++)
                {
                    value = axons[x][y][z];

                    randomNumber = Random.Range(0f, 100f);

                    if (randomNumber < 0.06f * probability)
                        value = Random.Range(-1f, 1f);
                    else if (randomNumber < 0.07f * probability)
                        value *= -1f;
                    else if (randomNumber < 0.5f * probability)
                        value += 0.1f * Random.Range(-1f, 1f);
                    else if (randomNumber < 0.75f * probability)
                        value *= 1 + Random.Range(0f, 1f);
                    else if (randomNumber < 1f * probability)
                        value *= Random.Range(0f, 1f);

                    axons[x][y][z] = value;
                }
            }
        }
    }
}
