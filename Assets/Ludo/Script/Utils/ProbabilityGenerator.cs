using System.Collections.Generic;

public class ProbabilityGenerator
{
    public static List<int> GenerateMovesBasedOnProbability(int moveCount, List<float> probabilityArr)
    {
        List<int> arr = new List<int>();
        for (int i = 0; i < moveCount; i++)
        {
            arr.Add(GenerateRandomDiceValue(probabilityArr));
        }
        return arr;
    }

    private static int GenerateRandomDiceValue(List<float> probabilityArr)
    {
        float sum = 0;
        foreach (float a in probabilityArr)
        {
            sum += a;
        }
        float pick = UnityEngine.Random.value * sum;
        for (int i = 0; i < probabilityArr.Count; i++)
        {
            pick -= probabilityArr[i];
            if (pick <= 0)
            {
                return i + 1;
            }
        }
        return 1;
    }
}