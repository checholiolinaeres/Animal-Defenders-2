using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Aber : MonoBehaviour {

    public GameObject prefabObj; //el prefab que representará el objetivo al que se dirigirá el zorro
    public Ray rayo; 
    public RaycastHit hit;
    public MainProps propiedadesMain;
    Vector3 puntoDesplegar;
    int Estado;

    Zorro zorro;

    // Use this for initialization
    void Start () {
        zorro = new Zorro();
        Estado = 1;
        propiedadesMain = GameObject.Find("Main").GetComponent<MainProps>(); //se consiguen las propiedades comunes del nivel
        zorro.Navs = GameObject.FindGameObjectsWithTag("Caminable"); //se agregan todos los objetos navegables en escena
        zorro.Agent = GetComponent<NavMeshAgent>(); //se inicializa el navmesh agent del personaje
        zorro.Anim = this.gameObject.GetComponentInChildren<Animator>();//se obtiene el componente animator del hijo de este objeto
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        Comportamiento();
    }

    #region Comportamiento

    public void Comportamiento(int estado = 1)
    {
        switch (estado)
        {
              case 1:
                {                    
                    rayo = Camera.main.ScreenPointToRay(Input.mousePosition); //se dispara un rayo desde la posicion del mouse
                    if (propiedadesMain.ObjSeleccionado == this.gameObject)
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
                        zorro.Agent.destination = GameObject.FindWithTag("Objetivo").transform.position; //el destino del navmesh agent será el objetivo
                        if (Vector3.Distance(transform.position, zorro.Agent.destination) <= 2f) //si el zorro se encuentra a 2 unidades o menos del objetivo...
                            Destroy(GameObject.FindWithTag("Objetivo")); //se destruye el objetivo
                    }
                    else
                        zorro.Agent.destination = transform.position; // en caso de que no exista objetivo el destino es su propia posicion

                    zorro.Anim.SetFloat("Velocidad", zorro.Agent.velocity.magnitude); // control de animación
                }
                break;

            case 2:
                {
                    Vector3 destino = propiedadesMain.BaseDef.transform.position;

                    zorro.Agent.destination = destino;

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
                    //se implementa el ataque
                }
                break;
        }
    }
    #endregion
}

