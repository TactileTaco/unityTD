using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class NavNode : ScriptableObject
{
    public Vector3 Position;
    public List<NavNode> adjacent;
}
