using UnityEngine;
using UnityEditor;
using System.Collections;

public class PathNodeDataAsset 
{
    [MenuItem("Assets/Create/PathNodeData")]
    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<PathNodeData> ();
    }
}
