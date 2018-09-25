using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proyectil : MonoBehaviour {
    private bool disparado;
    private Transform objetivo;
    private float danno;
    private float velocidad;
    private Rigidbody rb;
    private float tiempo;
    private Vector3 posicionDescanso;
    //private GameObject particulasExplosion;

    // Use this for initialization
    void Start () {
        tiempo = 30;
        posicionDescanso = transform.position;
        disparado = false;
        objetivo = null;
        velocidad = 30f;
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if (!disparado)
        {
            tiempo = 30;
            transform.position = posicionDescanso;
        }

        if (disparado && objetivo != null)
        {            
            foreach (Transform t in transform)
            {
                t.gameObject.SetActive(true);
            }           
            transform.LookAt(objetivo.transform, Vector3.up);
            transform.Translate(transform.forward * 0.5f);
            tiempo -= Time.deltaTime;
            /*if (tiempo >= 0)
            {
                disparado = false;
            }*/
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemigo"))
        {
            collision.gameObject.GetComponent<Enemigo>().Vida -= danno;
            Explotar();
        }
        //else {Explotar(); }
    }
    void Explotar()
    {
        foreach (Transform t in transform)
        {
            t.gameObject.SetActive(false);
        }
        objetivo = null;
        disparado = false;
        //Instanciar Explosion
    }

    #region Getters y Setters
    public bool Disparado
    {
        get
        {
            return disparado;
        }

        set
        {
            disparado = value;
        }
    }

    public Transform Objetivo
    {
        get
        {
            return objetivo;
        }

        set
        {
            objetivo = value;
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

    public Rigidbody Rb
    {
        get
        {
            return rb;
        }

        set
        {
            rb = value;
        }
    }

    public float Velocidad
    {
        get
        {
            return velocidad;
        }

        set
        {
            velocidad = value;
        }
    }

    #endregion
}
