using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Robot : Enemigo
{
    
    GameObject prefabExplosion;
    MainProps props;
    float energia;
    Animator anim;
    LineRenderer linea;

	// Use this for initialization
	void Start () {
        energia = 30;
        Tran = transform;
        Agent = GetComponent<NavMeshAgent>();
        Target = Tran.position;
        Salud = Random.Range(200, 205);
        Velocidad = Random.Range(20, 25);
        Danno = Random.Range(10, 15);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (Vector3.Distance(Tran.position, Target) <= 1)
        {
            ComportamientoFase = 1;
        }
        else
        {
            ComportamientoFase = 0;
        }
        Comportamiento(ComportamientoFase);

        anim.SetFloat("velocidad", Agent.velocity.magnitude);
	}

    void Comportamiento(int fase)
    {
        switch (fase)
        {
            case 0:
                {
                    Agent.destination = props.BaseDef.transform.position;
                }
                break;
            case 1:
                {
                    anim.SetBool("atacando", true);
                    DannoPorTic(2);
                }
                break;
            default:
                Agent.destination = Tran.position;
                break;
        }
    }

    public IEnumerator DannoPorTic(int segundosTic)
    {        
        // hacer daño
        yield return new WaitForSeconds(segundosTic);  
    }

    public override void Atacar()
    {
        Vector3[] posiciones = { Tran.position, Target };
        linea.SetPositions(posiciones);
    }

    void Morir()
    {

    }
}
