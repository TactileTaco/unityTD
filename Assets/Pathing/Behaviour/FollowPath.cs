using UnityEngine;
using System.Collections;

public class FollowPath : MonoBehaviour {
    public PathNodeData path;
    
    private CharacterController controller;
    private int current;
	// Use this for initialization
	void Start ()
    {
        current = 0;
        controller = GetComponent<CharacterController>();
        path = ScriptableObject.CreateInstance<PathNodeData>();
        path.path.Add(GameObject.Find("navNode"));
        for (int i = 0; path.path[i].GetComponent<AdjacentNodes>().nodes.Count > 0; ++i)
        {
            path.path.Add(path.path[i].GetComponent<AdjacentNodes>().nodes[0]);
        }
	}

	// Update is called once per frame
	void Update () 
    {
        Vector3 desired = (path.path[current].transform.position - transform.position);
        if (Vector3.Magnitude(Vector3.ProjectOnPlane(desired, Vector3.up)) < 0.5)
        {
            ++current;
        }
        else
        {
            controller.SimpleMove(desired.normalized * 5);
        }
	}
}
