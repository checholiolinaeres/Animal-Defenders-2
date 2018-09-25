using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TiendaAsset : MonoBehaviour
{

    //clase diseñada para poder re escalar con productos nuevos
    public List<Producto> productos; //lista de productos (agregados previamente)
    List<GameObject> articulos; //articulos graficos(botones)
    public GameObject plantilla; //prefab del grafico articulo con todo vacio

    private void Start()
    {
        articulos = new List<GameObject>(); //se crea una nueva lista sin asignarse
        for (int i = 0; i < productos.Count; i++)
        {
            articulos.Add(Instantiate(plantilla,transform)); //para el numero de productos asignados previamente en el editor se añaden al campo de la tienda plantillas vacias de articulos
        }
        for (int i = 0; i < articulos.Count; i++)
        {
            articulos[i].GetComponent<Articulo>().producto = productos[i]; //a cada articulo se le asigna uno de los productos previamente asignados
            articulos[i].GetComponent<Articulo>().SetSpritePreview(); //se pone el sprite en correspondiente al producto que le tocó al articulo en cuestion
        }
    }

    public void PosicionarComprado(int compra)
    {
        ManagerJuego.instancia.ArticuloComprado = Instantiate(ManagerJuego.instancia.PlantillasPrfb[compra], new Vector3(1000,1000,1000), ManagerJuego.instancia.PlantillasPrfb[compra].transform.rotation);
    }


    private void Update()
    {
        /*foreach (GameObject art in articulos)
        {
            art.GetComponent<Button>().interactable = (propiedadesMain.ArticuloComprado == null); 
        }*/
    }
}
