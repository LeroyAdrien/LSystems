using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Statistics
{
    public static List<float> NormalGeneration(float mean, float variance, int nOfPicks)
    {
        float u1 = Random.Range(0f, 1f);
        float u2 = Random.Range(0f, 1f);
        float n = 0f;


        return values;
    }

    public static float NormalGeneration(float mean, float variance)
    {
        float sum = 0f;
        for (int i = 0; i < 6;i++){
            sum += (Random.Range(0f, 1f));
        }

        float normalPick = sum - 6;

        float res = normalPick * variance + mean;

        return res;
    }
}
