using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StringGenerator
{
    public static string StringGeneration(string axiom, Dictionary<char, string> rules, int nbOfIterations)
    {
        string chaine = axiom;

        for (int i = 0; i < nbOfIterations; i++)
        {
            string temporaryChain = string.Empty;

            for (int j = 0; j < chaine.Length; j++)
            {
                if (rules.ContainsKey(chaine[j]))
                {
                    temporaryChain += rules[chaine[j]];

                }
                else
                {
                    temporaryChain += chaine[j];
                }
            }

            chaine = temporaryChain;
        }

        return chaine;
    }
}
// Start is called before the first frame update


