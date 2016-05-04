using UnityEngine;
using System.Collections;

public class Tower : MonoBehaviour 
{
    public GameObject tower;
    public GameObject enemySource;

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
        return !NavMesh.CalculatePath(enemySource.transform.position, transform.position, NavMesh.AllAreas, new NavMeshPath());
    }
	// Update is called once per frame
	void Update ()
    {
	   switch(state)
       {
            case TOWERSTATE.PLACING:
                if (_validLocation())
                {
                    tower.renderer.color = Color.Green;
                }
                else
                {
                    tower.renderer.color = Color.Red;
                }
                if (Input.GetMouseButtonUp())
                {
                    state = TOWERSTATE.CONSTRUCTING;
                }
                break;
            case TOWERSTATE.CONSTRUCTING:
                break;
            case TOWERSTATE.READY:
                break;
       }
	}
}
