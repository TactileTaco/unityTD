using UnityEngine;
using System.Collections;

public class NodeGizmo : MonoBehaviour {
    void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 1);
    }
}
