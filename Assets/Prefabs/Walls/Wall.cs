using UnityEngine;
using System.Collections;

[CreateAssetMenuAttribute]
public class Wall : ScriptableObject {
    public Vector3 start;
    public Vector3 end;
    public GameObject wallPrefab;

	public void GenWall()
    {
        (GameObject.Instantiate(wallPrefab, Vector3.Lerp(start, end, 0.5f) + Vector3.up / 2, Quaternion.FromToRotation(Vector3.right, (start - end).normalized)) as GameObject).transform.localScale = Vector3.right * Vector3.Distance(start, end) + new Vector3(0, 1, 1);
    }
}
