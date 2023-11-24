using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public NavMeshAgent badGuy;
    public GameObject player;
    public float squareOfMovement = 20F;
    public float locationRadius = 2F;
    public float agroRadius = 10F;
    public Material defaultMaterial;
    public Material attackMaterial;

    private MeshRenderer renderer;
    private Vector3 location;
    void Start()
    {
        location = new Vector3(Random.Range(-squareOfMovement, squareOfMovement), transform.position.y, Random.Range(-squareOfMovement, squareOfMovement));
        renderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= agroRadius) { 
            badGuy.SetDestination(player.transform.position);
            renderer.material = attackMaterial;
        }
        else if (Vector3.Distance(transform.position, location) <= locationRadius)
        {
            location = new Vector3(Random.Range(-squareOfMovement, squareOfMovement), transform.position.y, Random.Range(-squareOfMovement, squareOfMovement));
            badGuy.SetDestination(location);
        }
        else
        {
            badGuy.SetDestination(location);
            renderer.material = defaultMaterial;
        }
    }
}
