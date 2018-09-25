using UnityEngine;
using UnityEngine.UI;

public class Articulo: MonoBehaviour
{
    public Producto producto; //Asset de producto correspondiente a este articulo
    Text precio; //texto que mostrará el precio
    Text Descripcion; //texto que mostrará la descrpcion
    Image Preview; //preview del objeto que se va a comprar
    public Sprite[] posiblesPreviews; //0 = "TorreGenerica", 1 = "TorreV", 2 = "TrampaGenerica", 3 = "TrampaV" 
    TiendaAsset tienda;
    //MainProps props;

    private void OnEnable() // aqui se suscriben metodos de este escript para ser manejados en la UI
    {
        UiEventHandler.Compra += Comprar;
    }

    private void OnDisable()
    {
        UiEventHandler.Compra -= Comprar;
    }

    private void Start()
    {
        tienda = transform.parent.gameObject.GetComponent<TiendaAsset>();
    }

    //se obtienen los componentes necesarios 
    private void Update()
    {
        Preview = Buscar("Preview").GetComponent<Image>();
        precio = Buscar("Precio").GetComponent<Text>();
        precio.text = " $ " + producto.precio.ToString();
        Descripcion = Buscar("Descripcion").GetComponent<Text>();
        Descripcion.text = producto.Descripcion;       
    }

    //funcion para buscar un GameObject especifico dentro de los hijos
    GameObject Buscar(string nombre)
    {
        GameObject retorno = null;

        foreach (Transform t in transform)
        {
            if (t.name == nombre)
                retorno = t.gameObject;
        }
        return retorno;
    }


    //Funcion para comprar, esta se suscribe al evento comprar de la UI
    public void Comprar()
    {      
        if (ManagerJuego.instancia.Dinero >= producto.precio)
        {
            ManagerJuego.instancia.Dinero -= producto.precio;// buscar que no se queme codigo
            if (producto.name == "Torre1")
            {
                tienda.PosicionarComprado(0);
            }
            if (producto.name == "Torre2")
            {
                tienda.PosicionarComprado(1);
            }
            if (producto.name == "Trampa1")
            {
                tienda.PosicionarComprado(2);
            }
            if (producto.name == "Trampa2")
            {
                tienda.PosicionarComprado(3);
            }
        }       
    }

    public void SetSpritePreview()
    {
        switch (producto.name)
        {
            case "Torre1 (ProductoTorre)":
                Preview.sprite = posiblesPreviews[0];
                break;
            case "Torre2 (ProductoTorre)":
                Preview.sprite = posiblesPreviews[1];
                break;
            case "Trampa1 (ProductoTrampa)":
                Preview.sprite = posiblesPreviews[2];
                break;
            case "Trampa2 (ProductoTrampa)":
                Preview.sprite = posiblesPreviews[3];
                break;
        }
    }
}
