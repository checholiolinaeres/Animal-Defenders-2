using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ControlZorro : MonoBehaviour, ISeleccionable {

    [SerializeField]private GameObject prefabObj; //el prefab que representará el objetivo al que se dirigirá el zorro
    [SerializeField]private Ray rayo;
    [SerializeField] private RaycastHit hit;
    private NavMeshAgent agent;
    private Animator anim;
    private Vector3 puntoDesplegar;
    private float danno;
    private int estado;
    private Proyectil proyectil;
    private Transform disparador;
    private Transform enemigo;

    // Use this for initialization
    void Start () {
        danno = 10;
        estado = 1;
        agent = GetComponent<NavMeshAgent>(); //se inicializa el navmesh agent del personaje
        anim = this.gameObject.GetComponentInChildren<Animator>();//se obtiene el componente animator del hijo de este objeto
        proyectil = transform.Find("Proyectil").GetComponent<Proyectil>();
        proyectil.Danno = danno;
        disparador = transform.Find("BulletShells/Armature/ROOt/Spine/Spine.001/Spine.002/Clavicula.R/Brazo.R/AnteBrazo.R/Mano.R/Arma 1/Arma_end/Disparador");
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (anim.GetInteger("Fase") != estado)
        {
            anim.SetInteger("Fase", estado);
        }
        Comportamiento(estado);
    }

    public void AlSeleccionar()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemigo") && enemigo == null)
        {
            enemigo = other.transform;
            estado = 4;
        }
    }

    public void Disparar()
    {
        if (!proyectil.GetComponent<Proyectil>().Disparado)
        {
            proyectil.transform.position = disparador.transform.position;
            proyectil.GetComponent<Proyectil>().Objetivo = enemigo;
            proyectil.GetComponent<Proyectil>().Danno = danno;
            proyectil.GetComponent<Proyectil>().Disparado = true;
        }
    }

    #region Comportamiento

    public void Comportamiento(int estado = 1)
    {
        switch (estado)
        {
              case 1:
                {                    
                    rayo = Camera.main.ScreenPointToRay(Input.mousePosition); //se dispara un rayo desde la posicion del mouse
                    if (ManagerJuego.instancia.ObjSeleccionado == this.gameObject)
                    {                       
                        if (Input.GetButtonDown("Fire1") ) //
                        {
                            if (Physics.Raycast(rayo, out hit)) //se castea un rayo desde la posicion con un click y se guarda la informacion de colision en el hit
                            {
                                if (hit.collider.gameObject.CompareTag("Caminable")) //si el rayo golpea un objeto en el que se pueda navegaar
                                {
                                    if (GameObject.FindWithTag("Objetivo") == null) // y si no existe un objetivo
                                        Instantiate(prefabObj, hit.point, Quaternion.identity); // se crea un objetivo en el pundo donde se clickeó
                                }
                            }
                        }
                    }

                    if (GameObject.FindWithTag("Objetivo") != null) // si el objetivo existe...
                    {
                        agent.destination = GameObject.FindWithTag("Objetivo").transform.position; //el destino del navmesh agent será el objetivo
                        if (Vector3.Distance(transform.position, agent.destination) <= 2f) //si el zorro se encuentra a 2 unidades o menos del objetivo...
                            Destroy(GameObject.FindWithTag("Objetivo")); //se destruye el objetivo
                    }
                    else
                        agent.destination = transform.position; // en caso de que no exista objetivo el destino es su propia posicion

                    anim.SetFloat("Velocidad", agent.velocity.magnitude); // control de animación
                }
                break;

            case 2:
                {
                    Vector3 destino = ManagerJuego.instancia.BaseDef.transform.position;

                    agent.destination = destino;

                    if (Vector3.Distance(destino, transform.position) <= 2f)
                    {
                        estado = 3;
                    }
                }
                break;

            case 3:
                {
                    
                }
                break;

            case 4:
                {
                    if (enemigo != null)
                    {
                        transform.forward = new Vector3(enemigo.position.x - transform.position.x, transform.position.y, enemigo.position.x - transform.position.x);
                        Disparar();
                    }
                    else { estado = 1; }
                }
                break;
        }
    }
    #endregion

    #region Getters Setters
    public int Estado
    {
        get
        {
            return estado;
        }

        set
        {
            estado = value;
        }
    }

    public Transform Enemigo
    {
        get
        {
            return enemigo;
        }

        set
        {
            enemigo = value;
        }
    }

    public float Danno
    {
        get
        {
            return danno;
        }

        set
        {
            danno = value;
        }
    }
    #endregion
}

