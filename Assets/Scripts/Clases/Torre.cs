using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torre
{
    private float vida;
    private float dano;
    private float precio;
    private bool posicionada;

    public Torre()
    {
        Vida = 0;
        Dano = 0;
        Precio = 0;
        Posicionada = false;
    }

    public float Vida
    {
        get
        {
            return vida;
        }

        set
        {
            vida = value;
        }
    }

    public float Dano
    {
        get
        {
            return dano;
        }

        set
        {
            dano = value;
        }
    }

    public float Precio
    {
        get
        {
            return precio;
        }

        set
        {
            precio = value;
        }
    }

    public bool Posicionada
    {
        get
        {
            return posicionada;
        }

        set
        {
            posicionada = value;
        }
    }
}
