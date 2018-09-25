using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorreBase : MonoBehaviour,ISeleccionable {

    [SerializeField]float danno; //vida de la torre y daño
    bool posicionada;
    Transform enemigo;
    Transform disparador;
    Transform huesoCabina;
    RaycastHit hit;
    Ray r;
    private GameObject proyectil;   
    Animator anim;
    int fase; 
    // Use this for initialization
    void Start()
    {
        proyectil = transform.Find("Proyectil").gameObject;
        proyectil.GetComponent<Proyectil>().Danno = 1000f;
        danno = 1;
        enemigo = null;
        posicionada = false;
        disparador = transform.Find("ROOT/Bone/Bone.001/LaserDisparador");
        huesoCabina = transform.Find("ROOT/Bone/Bone.001");
        anim = GetComponent<Animator>();
	}

    // Update is called once per frame
    void FixedUpdate ()
    {
        if (enemigo != null)
        {
            Debug.Log(enemigo.name);
        }
        r = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (posicionada == true)
        {
            fase = 0;
        }
        //Bloque de codigo para Posicionamiento de torres
        else
        {
            fase = 1;
        }

        Comportamiento(fase);
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemigo") && enemigo == null)
        {
            enemigo = other.transform;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag != "Enemigo")
        {

        }
    }

    private void OnTriggerExit(Collider other)
    {
            enemigo = null;
    }

    public void Comportamiento(int fase) //"State machine" para definir el comportamiento
    {
        switch (fase)
        {
            case 0:
                if (!anim.enabled)
                    anim.enabled = true;
                if (enemigo != null)
                {
                    
                    Vector3 direccionEnemigo = new Vector3(enemigo.transform.position.x - huesoCabina.position.x, 0, enemigo.transform.position.z - huesoCabina.position.z);
                    huesoCabina.forward = direccionEnemigo.normalized;
                    if (!proyectil.GetComponent<Proyectil>().Disparado)
                    {
                        proyectil.transform.position = disparador.transform.position;
                        proyectil.GetComponent<Proyectil>().Objetivo = enemigo;
                        proyectil.GetComponent<Proyectil>().Danno = danno;
                        proyectil.GetComponent<Proyectil>().Disparado = true;                       
                    }
                }
                break;

            case 1:
                {

                    anim.enabled = false;
                    ManagerJuego.instancia.ObjSeleccionado = this.gameObject;
                    if (Physics.Raycast(r, out hit))
                    {
                        if (hit.collider.gameObject.tag == "Posicionable" && Input.GetButtonDown("Fire1"))
                        {
                            transform.position = hit.collider.gameObject.transform.position;
                            posicionada = true;
                        }
                    }
                    
                }
                break;
        }
    }

    public void AlSeleccionar() {

    }
}
