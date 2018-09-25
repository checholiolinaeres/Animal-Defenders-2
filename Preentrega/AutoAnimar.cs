using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AutoAnimar : MonoBehaviour {
    NavMeshAgent agent;
    Transform cilindro;
    Transform cabina;
    private void Start()
    {
        cilindro = GetComponent<Transform>().Find("Rig/ROOT/Steer/Cilindro");
        cabina = transform.Find("Rig/ROOT/Cabina");
        agent = GetComponent<NavMeshAgent>();
    }
    // Update is called once per frame
    void FixedUpdate ()
    {
        cilindro.Rotate(new Vector3(agent.velocity.magnitude, 0, 0));
        Mathf.PerlinNoise(cabina.position.x, cabina.position.y);
	}
}
