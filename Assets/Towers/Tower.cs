using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VolumetricLines;

public class Tower : MonoBehaviour 
{
    public Path path;
    public float constructionTime = 10;
    public float range;
    public GameObject shotPrefab;

    public enum TOWERSTATE
    {
        PLACING,
        CONSTRUCTING,
        READY
    }

    public TOWERSTATE state;

	void Start ()
    {
	   state = TOWERSTATE.PLACING;
	}
	
    private bool _validLocation()
    {
        return path.Contains(transform.position);
    }
	// Update is called once per frame
	void Update ()
    {
	   switch(state)
       {
            case TOWERSTATE.PLACING:
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1<<10))
                {
                    transform.position = hit.point;
                    transform.position = new Vector3(transform.position.x, 0, transform.position.z);
                }
                if (_validLocation())
                {
                    GetComponent<Renderer>().material.color = Color.green;
                }
                else
                {
                    GetComponent<Renderer>().material.color = Color.red;
                }
                if (Input.GetMouseButtonUp(0))
                {
                    state = TOWERSTATE.CONSTRUCTING;
                    GetComponent<Renderer>().material.color = Color.white;
                }
                break;
            case TOWERSTATE.CONSTRUCTING:
                if (constructionTime > 0f)
                {
                    constructionTime -= Time.deltaTime;
                }
                else
                {
                    state = TOWERSTATE.READY;
                }
                break;
            case TOWERSTATE.READY:
                List<GameObject> enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
                for (int i = 0; i < enemies.Count; ++i)
                {
                        Debug.Log("Firing");
                    GameObject enemy = enemies[i];
                    Vector3 pos = enemy.transform.position;
                    if ((pos - transform.position).magnitude < range)
                    {
                        Debug.Log("Firing");
                        GameObject.Destroy(enemy);
                        GameObject shot = GameObject.Instantiate(shotPrefab);
                        VolumetricLineBehavior line = shot.GetComponent<VolumetricLineBehavior>();
                        line.StartPos = transform.position;
                        line.EndPos = pos;
                        GameObject.Destroy(shot, 1f);
                    }
                }

                break;
       }
	}
}
