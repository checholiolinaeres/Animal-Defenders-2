using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour, ISeleccionable {

    [SerializeField]private float vida;




    void Destruir()
    {
        //Animacion destruirse
    }

    public void AlSeleccionar()
    {

    }

    #region Getters y Setters
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
    #endregion
}
