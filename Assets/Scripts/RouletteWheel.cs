using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RouletteWheel
{
    public float SelectFittesIndex(List<float> fitness)
    {
        float sumOfFitnes = fitness.Sum();
        float previousProbability = 0.0f;
        float[] probabilitys = new float[fitness.Count];

        for (int i = 0; i < fitness.Count; i++)
        {
            probabilitys[i] = previousProbability = previousProbability + (fitness[i] / sumOfFitnes);
        }
        
        float rnd = Random.value;
        float probability = 0;
        int a = 0;

        for (int i = 0; i < probabilitys.Length; i++)
        {
            if (rnd < probabilitys[i])
            {
                a = i;
                break;
            }
        }
        return fitness[a];
    }
}