using UnityEngine;
using System.Collections;

public class Gizmo : MonoBehaviour {
    void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 1);
    }
}
