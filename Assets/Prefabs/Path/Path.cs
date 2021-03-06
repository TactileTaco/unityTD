﻿using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Path : MonoBehaviour {

    public Vector3[] nodes;
    public int pathRadius = 5;
    public GameObject wallPrefab;
    public GameObject enemyPrefab;
    public float spawnRate = 10;
    private float timeSinceSpawn = 0;
    private Vector3[] nodesInScene;

	// Use this for initialization
	void Start () 
    {
	   _reset();
	}

    private void _wallFactory(Vector3 start, Vector3 end, Vector3 visibleNode)
    {
        RaycastHit unimportant;
        GameObject wall = GameObject.Instantiate(wallPrefab, Vector3.Lerp(start, end, 0.5f) + Vector3.up / 2, Quaternion.FromToRotation(Vector3.right, (start - end).normalized)) as GameObject;
        wall.transform.localScale = Vector3.right * Vector3.Distance(start, end) + new Vector3(0, 1, 1);
        if (wall.GetComponent<Collider>().Raycast(new Ray(visibleNode, (wall.transform.position - visibleNode).normalized), out unimportant, Mathf.Infinity))
        {
            GameObject.DestroyImmediate(wall);
            _wallFactory(end, start, visibleNode);
        }
    }

    [ContextMenu("GenWalls")]
    void GenWalls()
    {
        nodesInScene = new Vector3[nodes.Length];
        Vector3 dir = (nodes[1] - nodes[0]).normalized;
        Vector3 diff = -dir * pathRadius;
        Vector3 prevEnd = Quaternion.Euler(0, 90, 0) * dir * pathRadius + nodes[0];
        Vector3 prevEndP =  (nodes[0] * 2 - prevEnd) + diff;
        prevEnd += diff;
        nodesInScene[0] = new Vector3(nodes[0].x, nodes[0].y, nodes[0].z);
        _wallFactory(prevEnd, prevEndP, nodes[0]);

        for (int i = 1; i < nodes.Length - 1; ++i)
        {
            Vector3 start = nodes[i - 1];
            Vector3 end = nodes[i];
            Vector3 next = nodes[i + 1];
            Vector3 wallEnd = end - Vector3.Slerp(start - end, next - end, 0.5f).normalized * pathRadius;
            Vector3 wallEndP = end * 2 - wallEnd;
            if (Vector3.Dot(wallEnd - wallEndP, prevEnd - prevEndP) < 0)
            {
                Vector3 temp = new Vector3(wallEnd.x, wallEnd.y, wallEnd.z);
                wallEnd = new Vector3(wallEndP.x, wallEndP.y, wallEndP.z);
                wallEndP = new Vector3(temp.x, temp.y, temp.z);
            }

            _wallFactory(prevEnd, wallEnd, end);
            _wallFactory(prevEndP, wallEndP, end);

            prevEnd = wallEnd;
            prevEndP = wallEndP;
            nodesInScene[i] = new Vector3(end.x, end.y, end.z);
        }

        Vector3 finalNode = nodes[nodes.Length - 1];
        Vector3 prevNode = nodes[nodes.Length - 2];
        dir = (prevNode - finalNode).normalized;
        diff = -dir * pathRadius;
        Vector3 final = Quaternion.Euler(0, 90, 0) * dir * pathRadius + finalNode + diff;
        nodesInScene[nodesInScene.Length - 1] = new Vector3(finalNode.x, finalNode.y, finalNode.z);

        _wallFactory(prevEndP, final, finalNode);
        _wallFactory(prevEnd, (finalNode + diff) * 2 - final, finalNode);
        _wallFactory(final, (finalNode + diff) * 2 - final, finalNode);
    }
	
    private void _reset()
    {
        Debug.Log("Resetting");
        foreach(GameObject wall in GameObject.FindGameObjectsWithTag("Wall"))
        {
            GameObject.DestroyImmediate(wall);
        }
        GenWalls();
    }

	void Update ()
    {
	   if (nodesInScene.Length != nodes.Length)
       {
            _reset();
       }
       else
       {
            for(int i = 0; i < nodes.Length; ++i)
            {
                if(nodes[i] != nodesInScene[i])
                {
                    _reset();
                    break;
                }
            }
       }
       if (Application.isPlaying)
       {
            timeSinceSpawn += Time.deltaTime;
            if (timeSinceSpawn > spawnRate)
            {
                timeSinceSpawn = 0;
                GameObject enemy = GameObject.Instantiate(enemyPrefab);
                enemy.transform.position = nodes[0];
            }
       }
	}
    public bool Contains(Vector3 location)
    {
        Vector3 closestNode;
        float dist = Mathf.Infinity;
        foreach (Vector3 node in nodes)
        {
            float d = (node - location).magnitude;
            if (d < dist)
            {
                closestNode = node;
                dist = d;
            }
        }
        return Physics.Raycast(location, (closestNode - location).normalized, dist, 1 << 9);
    }

    void OnDrawGizmos()
    {
        foreach (Vector3 node in nodes)
        {
            Gizmos.DrawSphere(node, 0.5f);
        }
    }
}
