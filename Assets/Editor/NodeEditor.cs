using UnityEngine;
using UnityEditor;
using System.Collections;


[CustomEditor(typeof(NavNode))]
[CanEditMultipleObjects]
public class NodeEditor : Editor 
{
	void OnSceneGUI ()
    {
        NavNode node = (NavNode) target;
        EditorGUI.BeginChangeCheck();
        Handles.color = Color.yellow;
        Vector3 pos = Handles.PositionHandle (node.Position, Quaternion.identity);
        Handles.color = Color.blue;
        Handles.DrawSolidDisc(pos, Vector3.up, 1f);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Moved Navigation Node");
            node.Position = pos;
        }
    }
}
