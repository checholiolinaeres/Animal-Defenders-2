using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlCamara : MonoBehaviour
{
    //propiedades "estaticas":
    //baseDef
    //zorro
    //objSeleccionado
    //cam
    //

    #region Propiedades

    MainProps propiedadesMain;
    Vector3 posicionCentro; //posicion central del mouse en los pixeles de la pantalla
    Transform zorro, baseDef; //transforms del zorro y la base principal 

    private Transform objSeleccionado;

    Ray ObjSeleccionadoRayo; 
    Camera cam; //una camará que será en la que se realizarán las acciones escritas aqui

    float minX; //minima coordenada en X de la pantalla para que la camara se desplace
    float maxX; //maxima coordenada en X de la pantalla para que la camara se desplace
    float minY;
    float maxY;
    float t; //parametro t que se usará en las interpolaciones de zoom y velocidad de movimiento
    public float sensibilidadZoom = 0.5f;
    public float velocidad = 1; //velocidad cambiable desde el editor

    #endregion

    private void OnEnable()
    {
        UiEventHandler.OnClick += PosicionarCamara;
    }

    private void OnDisable()
    {
        UiEventHandler.OnClick -= PosicionarCamara;
    }

    // Use this for initialization
    void Start ()
    {
        

        propiedadesMain = GameObject.Find("Main").GetComponent<MainProps>();
        cam = propiedadesMain.Cam; // se define la camara
        zorro = propiedadesMain.Zorro.transform; // se define el zorro
        baseDef = propiedadesMain.BaseDef.transform; //se define la base
        objSeleccionado = propiedadesMain.BaseDef.transform; // se define por defecto que el objeto seleccionado es la base
        PosicionarCamara(); // se posiciona la camara mirando a la base
        posicionCentro = new Vector3(Screen.width / 2, Screen.height / 2, 0); //se define el centro de la pantalla
        minX = Screen.width / 20;
        maxX = Screen.width - (Screen.width / 20);
        minY = Screen.height / 11;
        maxY = Screen.height - (Screen.height / 11);
        t = 0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UiEventHandler.OnClick += PosicionarCamara;

        Debug.Log(objSeleccionado.gameObject.name);

        t += Input.GetAxis("Mouse ScrollWheel") * sensibilidadZoom; //defino el factor t para interpolar entre el FOV maximo y el minimo multiplicado por la sensibilidad

        if (Input.GetMouseButtonDown(2)) //se reestablece el zoom si se presiona el boton central del mouse
            t = 0;

        Zoom(t); // se llama una funcion para realizar un zoom en la camara

        velocidad = Mathf.Lerp(1, 0.2f, t); //se calcula la velocidad siendo 1 el maximo valor y 0.2 el minimo dependiendo del nivel de zoom

        Vector3 posicionMouse = Input.mousePosition; //se define un Vector3 que se va a ir actalizando conforme el mouse se mueva por la pantalla
        Vector3 direccion = new Vector3((posicionMouse - posicionCentro).x, 0, (posicionMouse - posicionCentro).y).normalized; //se calcula la direccion en la que se está moviendo el mouse desde el centro hasta su posicion

        //Inicia las validaciones para decidir si la camara s emueve o no
        if (posicionMouse.x >= maxX)
            transform.position += direccion * velocidad; // se le suma la direccion multiplicado con la velocidad a un vector que transforme la coordenada y del mouse en la z de la camara
        if (posicionMouse.x <= minX)
            transform.position += direccion * velocidad;
        if (posicionMouse.y >= maxY)
            transform.position += direccion * velocidad;
        if (posicionMouse.y <= minY)
            transform.position += direccion * velocidad;

        //validacion de si se dio doble click a un objeto que sea seleccionable
        if (DobleClick())
        {
            Shake(4, 2); // I M P O R T A N T E DEBUG BORRAR
            ObjSeleccionadoRayo = cam.ScreenPointToRay(Input.mousePosition); //se lanza un rayo desde el punto donde esta el mouse en la pantalla hacia adelante de la camara
            RaycastHit hit; // se crea un hit de rayo para poder analizar su informacion
            if (Physics.Raycast(ObjSeleccionadoRayo, out hit))
                if (hit.collider.gameObject.GetComponent<InteraccionCamara>() != null) // si el objeto con el que golpeó tiene un componente especifico, este pasará a ser el objeto actualmente seleccionado
                {
                    propiedadesMain.ObjSeleccionado = hit.collider.gameObject;
                    PosicionarCamara(); // y se posiciona la camara con ese objeto en el centro
                }
        }
	}


    #region Metodos Custom

    /// <summary>
    /// posiciona la camara en donde el objeto seleccionado se encuentre con un determinado desfase para que esté centrado
    /// </summary>
    /// <param name="seleccionado">el objeto seleccionado actual</param>
    /// <param name="inicio">Define si es el primer posicionamiento de la camara o ya es a traves del usuario</param>
    /// <param name="desfaseZ">El desfase en la coordenada Z, por defecto está definido en -20</param>
    /// <param name="desfaseY">El desfase en la coordenada Y, por defecto está definido en 20</param>
    void PosicionarCamara()
    {
        float desfaseZ = -20f;
        float desfaseY = 20f;
        Transform seleccionado = propiedadesMain.ObjSeleccionado.transform;
        transform.position = new Vector3(seleccionado.position.x, seleccionado.position.y + desfaseY, seleccionado.position.z + desfaseZ);       
    }



    /// <summary>
    /// Camera Shake
    /// </summary>
    /// <param name="NumeroSacudidas">El numero de sacudidas que se van a realizar</param>
    /// <param name="Fuerza">La fuerza de las sacudidas</param>
    /// <param name="Intervalo">El tiempo que hay entre sacudida y sacudida</param>
    /// <returns></returns>
    public IEnumerator Shake(int NumeroSacudidas,float Fuerza, float Intervalo = 0.2f)
    {
        for (int i = 0; i < NumeroSacudidas; i++)
        {
            transform.Translate(new Vector3(Fuerza, Fuerza, Fuerza) * ((i % 2 == 0) ? -1 : 1));
            yield return new WaitForSeconds(Intervalo);
        }
    }



    /// <summary>
    /// Define el objeto seleccionado
    /// </summary>
    /// <param name="seleccionable">el objeto a seleccionar</param>
    public Transform ObjSeleccionado
    {
        get { return objSeleccionado; }
        set { ObjSeleccionado = value; }
    }



    /// <summary>
    /// funcion que simula un doble click para pasarlo como un input mas
    /// </summary>
    /// <returns></returns>
    public bool DobleClick()
    {
        int clicks = 0;
        bool retorno = false;
        if (Input.GetButtonDown("Fire1"))
        {
            clicks++;
            if (Input.GetButtonDown("Fire1") && clicks == 1)
            {
                clicks++;
                if (clicks >= 2)
                {
                    clicks = 2;
                    retorno = true;
                }
            }
        }
        return retorno;
    }



    /// <summary>
    /// funcion que simula zoom de camara interpolando linealmente un valor maximo y minimo de FOV
    /// </summary>
    /// <param name="t"></param>
    void Zoom(float t)
    {
        cam.fieldOfView = Mathf.Lerp(60, 20, t);
    }

    #endregion


}
