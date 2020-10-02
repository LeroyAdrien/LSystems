using System.Collections;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(TreeGenerator))]
public class TreeGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {

        TreeGenerator treeGen = (TreeGenerator)target;

        

        if (DrawDefaultInspector())
        {

            //treeGen.m_keys = new char[treeGen.m_nbOfRules];
            //treeGen.m_values = new string[treeGen.m_nbOfRules];

            if (treeGen.m_autoUpdate)
            {
                treeGen.GenerateTree();
            }
        }

        if (GUILayout.Button("Generate"))
        {
            treeGen.GenerateTree();
        }

        if (GUILayout.Button("Reset"))
        {
            treeGen.Reset();
        }

        
    }
}
