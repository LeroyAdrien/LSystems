using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeGenerator : MonoBehaviour
{
    public string m_axiom;
    public Dictionary<char, string> m_rules;
    //[Range(0, 6)]
    public int m_nbOfIterations;

    [Range(0, 5)]
    public int m_nbOfRules;
    public float m_referenceAngleMean = 22.5f;
    public float m_referenceAngleVariance = 0.01f;
    public float m_SegmentLength = 100f;


    public char[] m_keys;
    public string[] m_values;

    public bool m_autoUpdate;

    public Material m_woodMaterial;
    public int m_woodCount = 0;

    public Material m_leafMaterial;
    public int m_leafCount = 0;

    public int m_FCount;

    private Stack<float> m_alphaAngleStack = new Stack<float>();
    private Stack<float> m_betaAngleStack = new Stack<float>();
    private Stack<Vector3> m_positionStack = new Stack<Vector3>();

    private float m_currentAlphaAngle = 0f;
    private float m_currentbetaAngle = 0f;
    private Vector3 m_currentPosition = Vector3.zero;


    public void GenerateTree()
    {
        //Clean Previous Iteration
        CleanPreviousIteration();

        //Convert to Dictionnary
        m_rules = new Dictionary<char, string>();
        for (int i = 0; i < m_keys.Length; i++)
        {
            m_rules.Add(m_keys[i], m_values[i]);
        }

        // Generate the string
        string lsystemString = StringGenerator.StringGeneration(m_axiom, m_rules, m_nbOfIterations);

        //Collect Information on the string

        // Number of "F"
        for (int i = 0; i < lsystemString.Length; i++)
        {
            if (lsystemString[i] == 'F')
            {
                m_FCount += 1;
            }
        }


        // Generate the tree
        for (int i = 0; i < lsystemString.Length; i++)
        {
            ReadLetter(lsystemString[i]);
        }
        //CreateWoodSegment(Vector3.zero, 0f, 0f, 0f);


    }

    private void CleanPreviousIteration()
    {
        m_woodCount = 0;
        m_leafCount = 0;
        GameObject[] linesToClean = GameObject.FindGameObjectsWithTag("Line");
        m_currentAlphaAngle = 0f;
        m_currentbetaAngle = 0f;
        m_currentPosition = Vector3.zero;
        m_FCount = 0;

        foreach (GameObject line in linesToClean)
        {
            GameObject.DestroyImmediate(line);
        }
    }

    public void Reset()
    {
        m_keys = new char[0];
        m_values = new string[0];
        m_rules = new Dictionary<char, string>();

    }

    void ReadLetter(char letter)
    {

        //Debug 

        float test=Statistics.NormalGeneration(m_referenceAngleMean, m_referenceAngleVariance);
        Debug.Log(test);
        string angles = "+-*/";
        string brackets = "[]";
        string symbols = "FX";

        if (angles.Contains(letter.ToString()))
        {

            switch (letter)
            {
                case '+':
                    m_currentAlphaAngle += m_referenceAngleMean;
                    break;
                case '-':
                    m_currentAlphaAngle -= m_referenceAngleMean;
                    break;
                case '*':
                    m_currentbetaAngle += m_referenceAngleMean;
                    break;
                case '/':
                    m_currentbetaAngle -= m_referenceAngleMean;
                    break;
            }
        }
        else if (brackets.Contains(letter.ToString()))
        {
            switch (letter)
            {
                case '[':
                    m_alphaAngleStack.Push(m_currentAlphaAngle);
                    m_betaAngleStack.Push(m_currentbetaAngle);
                    m_positionStack.Push(m_currentPosition);
                    break;
                case ']':
                    m_currentAlphaAngle = m_alphaAngleStack.Pop();
                    m_currentbetaAngle = m_betaAngleStack.Pop();
                    m_currentPosition = m_positionStack.Pop();
                    break;

            }

        }
        else if (symbols.Contains(letter.ToString()))
        {
            switch (letter)
            {
                case 'F':
                    CreateWoodSegment(m_currentPosition, m_SegmentLength, m_currentAlphaAngle, m_currentbetaAngle);
                    float x0 = Mathf.Cos(m_currentAlphaAngle) * m_SegmentLength;
                    float y0 = Mathf.Sin(m_currentbetaAngle) * m_SegmentLength;
                    float z0 = Mathf.Sin(m_currentAlphaAngle) * m_SegmentLength;
                    m_currentPosition += new Vector3(x0, y0, z0);
                    break;
                case 'X':
                    CreateLeafSegment(m_currentPosition, m_SegmentLength, m_currentAlphaAngle, m_currentbetaAngle);
                    float x1 = Mathf.Cos(m_currentAlphaAngle) * m_SegmentLength;
                    float y1 = Mathf.Sin(m_currentbetaAngle) * m_SegmentLength;
                    float z1 = Mathf.Sin(m_currentAlphaAngle) * m_SegmentLength;
                    m_currentPosition += new Vector3(x1, y1, z1);
                    break;
                default:
                    break;

            }
        }

    }



    void CreateWoodSegment(Vector3 position, float length, float alphaAngles, float betaAngles)
{

    GameObject lineWood = new GameObject($"LineWood_{m_woodCount}");
    lineWood.transform.position = Vector3.zero;
    lineWood.transform.parent = transform;

    LineRenderer newLine = SetupLine(lineWood, m_woodMaterial);

    newLine.SetPosition(0, position);

    float x = Mathf.Cos(alphaAngles) * length;
    float y = Mathf.Sin(betaAngles) * length;
    float z = Mathf.Sin(alphaAngles) * length;
    newLine.SetPosition(1, position + new Vector3(x, y, z));
    m_woodCount++;



}

void CreateLeafSegment(Vector3 position, float length, float alphaAngles, float betaAngles)
{
    GameObject lineLeaf = new GameObject($"LineLeaf_{m_leafCount}");
    lineLeaf.transform.position = Vector3.zero;
    lineLeaf.transform.parent = transform;

    LineRenderer newLine = SetupLine(lineLeaf, m_woodMaterial);

    newLine.SetPosition(0, position);

    float x = Mathf.Cos(alphaAngles) * length;
    float y = Mathf.Sin(betaAngles) * length;
    float z = Mathf.Sin(alphaAngles) * length;
    newLine.SetPosition(1, position + new Vector3(x, y, z));

    m_leafCount++;
}

private LineRenderer SetupLine(GameObject line, Material material)
{
    LineRenderer newLine = line.AddComponent<LineRenderer>();
    newLine.useWorldSpace = true;
    newLine.positionCount = 2;
    newLine.tag = "Line";
    newLine.material = material;
    newLine.startWidth = 0.3f;
    newLine.endWidth = 0.15f;

    return newLine;


}

private float Rad(float degree)
{
    return degree * Mathf.PI / 180;
}
}
