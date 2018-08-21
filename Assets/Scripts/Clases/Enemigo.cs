using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemigo:MonoBehaviour
{
    private Transform tran;
    private NavMeshAgent agent;
    private Vector3 target;
    private float salud;
    private float velocidad;
    private float danno;
    private int comportamientoFase;


    public virtual void Atacar()
    {

    }

    #region Getters y Setters

    public NavMeshAgent Agent
    {
        get
        {
            return agent;
        }

        set
        {
            agent = value;
        }
    }

    public Vector3 Target
    {
        get
        {
            return target;
        }

        set
        {
            target = value;
        }
    }

    public float Salud
    {
        get
        {
            return salud;
        }

        set
        {
            salud = value;
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

    public Transform Tran
    {
        get
        {
            return tran;
        }

        set
        {
            tran= value;
        }
    }

    public int ComportamientoFase
    {
        get
        {
            return comportamientoFase;
        }

        set
        {
            comportamientoFase = value;
        }
    }

    #endregion
}
