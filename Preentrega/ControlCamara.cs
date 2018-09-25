using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlCamara : MonoBehaviour
{
    #region Propiedades

    private Vector3 posicionCentro; //posicion central del mouse en los pixeles de la pantalla

    private Transform objSeleccionado;

    private Ray ObjSeleccionadoRayo; 
    private Camera cam; //una camará que será en la que se realizarán las acciones escritas aqui
    private float minX; //minima coordenada en X de la pantalla para que la camara se desplace
    private float maxX; //maxima coordenada en X de la pantalla para que la camara se desplace
    private float minY;
    private float maxY;
    private float t; //parametro t que se usará en las interpolaciones de zoom y velocidad de movimiento
    private float tRotY; //parametro t que se usará en las interpolaciones de rotación
    private float tRotX; //parametro t que se usará en las interpolaciones de rotación
    [SerializeField]private float sensibilidadZoom = 0.5f;
    [SerializeField] private float sensibilidadRotacion;
    [SerializeField]private float velocidad = 1; //velocidad cambiable desde el editor

    GameObject controlMovimiento;

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
        cam = ManagerJuego.instancia.Cam; // se define la camara
        objSeleccionado = ManagerJuego.instancia.BaseDef.transform; // se define por defecto que el objeto seleccionado es la base
        posicionCentro = new Vector3(Screen.width / 2, Screen.height / 2, 0); //se define el centro de la pantalla
        minX = Screen.width / 20;
        maxX = Screen.width - (Screen.width / 20);
        minY = Screen.height / 11;
        maxY = Screen.height - (Screen.height / 11);
        t = 0f;
        controlMovimiento = transform.parent.gameObject;
        controlMovimiento.transform.position = ManagerJuego.instancia.ObjSeleccionado.transform.position;
        PosicionarCamara(); // se posiciona la camara mirando a la base
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(objSeleccionado.gameObject.name);

        if(!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftControl))
            t += Input.GetAxis("Mouse ScrollWheel") * sensibilidadZoom; //defino el factor t para interpolar entre el FOV maximo y el minimo multiplicado por la sensibilidad

        if (Input.GetMouseButtonDown(2)) //se reestablece el zoom si se presiona el boton central del mouse
            t = 0;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            tRotY += Input.GetAxis("Mouse ScrollWheel")*sensibilidadRotacion;
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            tRotX += Input.GetAxis("Mouse ScrollWheel")*sensibilidadRotacion;
        }

        controlMovimiento.transform.localEulerAngles = new Vector3(Mathf.Lerp(0,70,tRotX), Mathf.LerpUnclamped(0, 360, tRotY),0);
           

        Zoom(t); // se llama una funcion para realizar un zoom en la camara

        velocidad = Mathf.Lerp(1, 0.2f, t); //se calcula la velocidad siendo 1 el maximo valor y 0.2 el minimo dependiendo del nivel de zoom

        Vector3 posicionMouse = Input.mousePosition; //se define un Vector3 que se va a ir actalizando conforme el mouse se mueva por la pantalla
        Vector3 direccion = new Vector3((posicionMouse - posicionCentro).x, 0, (posicionMouse - posicionCentro).y).normalized; //se calcula la direccion en la que se está moviendo el mouse desde el centro hasta su posicion

        //Inicia las validaciones para decidir si la camara s emueve o no
        if (posicionMouse.x >= maxX)
            controlMovimiento.transform.position += direccion * velocidad; // se le suma la direccion multiplicado con la velocidad a un vector que transforme la coordenada y del mouse en la z de la camara
        if (posicionMouse.x <= minX)
            controlMovimiento.transform.position += direccion * velocidad;
        if (posicionMouse.y >= maxY)
            controlMovimiento.transform.position += direccion * velocidad;
        if (posicionMouse.y <= minY)
            controlMovimiento.transform.position += direccion * velocidad;

        //validacion de si se dio doble click a un objeto que sea seleccionable
        if (DobleClick())
        {
            ObjSeleccionadoRayo = cam.ScreenPointToRay(Input.mousePosition); //se lanza un rayo desde el punto donde esta el mouse en la pantalla hacia adelante de la camara
            RaycastHit hit; // se crea un hit de rayo para poder analizar su informacion
            if (Physics.Raycast(ObjSeleccionadoRayo, out hit))
                if (!hit.collider.isTrigger && TestInterfaz(hit.collider.gameObject)) // si el objeto con el que golpeó tiene un componente especifico, este pasará a ser el objeto actualmente seleccionado
                {
                    ManagerJuego.instancia.ObjSeleccionado = hit.collider.gameObject;
                    ManagerJuego.instancia.Indicar();
                    PosicionarCamara(); // y se posiciona la camara con ese objeto en el centro
                }
        }
	}
    #region Metodos Custom

    /// <summary>
    /// posiciona la camara en donde el objeto seleccionado se encuentre con un determinado desfase para que esté centrado
    /// </summary>
    void PosicionarCamara()
    {
        controlMovimiento.transform.position = ManagerJuego.instancia.ObjSeleccionado.transform.position;       
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

    bool TestInterfaz(GameObject colision)
    {
        bool retorno = false;
        MonoBehaviour[] componentes = colision.GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour m in componentes)
        {
            if (m is ISeleccionable)
            {
                retorno = true;
                break;
            }
        }
        return retorno;
    }

    #endregion


}
