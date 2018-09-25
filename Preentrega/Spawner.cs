using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    [SerializeField] private GameObject prfbEnmigo;
    [SerializeField] private int numeroOleadas;
    [SerializeField] private int enemigosPorOleada;
    [SerializeField]GameObject[] enemigosDisponibles;


	void Start ()
    {
        enemigosDisponibles = new GameObject[enemigosPorOleada];
        Spawnear();
        ManagerJuego.instancia.Necesarios = numeroOleadas * enemigosPorOleada;
	}

    private void FixedUpdate()
    {
        Invoke("PoolObjetos", 10f);
        
    }

    void PoolObjetos()
    {
        if (!OleadaActiva())
        {
            for (int i = 0; i < enemigosPorOleada; i++)
            {
                enemigosDisponibles[i].GetComponent<Enemigo>().Activar();
                enemigosDisponibles[i].GetComponent<Enemigo>().ReiniciarPosicion();
                
            }
        }
    }

    public bool OleadaActiva()
    {
        bool retorno = false;

        for (int i = 0; i < enemigosDisponibles.Length; i++)
        {
            if (!enemigosDisponibles[i].GetComponent<Enemigo>().Guardado)
            {
                retorno = true;
                break;
            }
        }

        return retorno;
    }

    void Spawnear()
    {
        for (int i = 0; i < enemigosPorOleada; i++)
        {
            enemigosDisponibles[i] = Instantiate(prfbEnmigo, transform.position, Quaternion.identity);
            enemigosDisponibles[i].GetComponent<Enemigo>().Desactivar();
        }
    }
}
