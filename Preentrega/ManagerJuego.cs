using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerJuego : MonoBehaviour {
    //Clase que Define los objetos que en otro caso serian estaticos, se usa para acceder a ellos desde varios scripts

    public static ManagerJuego instancia;

    [SerializeField]private GameObject[] plantillasPrfb; // torre1, torre2, trampa1, trampa2
    [SerializeField]private GameObject prfbIndicador;
    [SerializeField]private GameObject indicador;
    [SerializeField]private int dinero;
    private GameObject[] spawners;
    private GameObject baseDef;
    private GameObject zorro;
    private GameObject objSeleccionado;
    private Camera cam;
    private GameObject articuloComprado;
    private Base baseScript;
    private float baseVida;
    private int necesarios;
    private int destruidos;

    void Awake () {
        if (instancia == null)
        {
            instancia = this;
        }
        else if (instancia != null)
        {
            Destroy(gameObject);
        }
        Referencias();
    }

    public void Indicar()
    {
        Collider col;
        col = objSeleccionado.GetComponent<Collider>();
        indicador.transform.parent = objSeleccionado.transform;
        indicador.transform.position = objSeleccionado.transform.position + new Vector3(0,col.bounds.extents.y + 3f,0);
    }

    private void Update()
    {
        EstadoJuego();
    }

    void EstadoJuego()
    {
        if (this.baseScript.Vida > 0)
        {
            //estado normal
        }

        if (this.baseScript.Vida <= 0)
        {
            Time.timeScale = 0;
            //estado de perder
            //Se llama metodo de destruir de la base
            //pantalla de morir
            //Se vuelve a cargar el nivel
        }

        if (destruidos >= necesarios)
        {
            //estado de ganar
            //en la ui se muestra que perdiste
            //se reinicia el nivel
        }


    }

    void Referencias()
    {
        indicador = Instantiate(prfbIndicador, new Vector3(1000, -1000, 1000), Quaternion.identity);
        baseDef = GameObject.Find("base");
        zorro = GameObject.FindWithTag("Player");
        ObjSeleccionado = baseDef;
        cam = Camera.main;
        Dinero = 1000;
        baseScript = baseDef.GetComponent<Base>();
        baseVida = baseScript.Vida;
        ArticuloComprado = null;
        spawners = GameObject.FindGameObjectsWithTag("Spawner");
    }

    #region getters y setters

    public GameObject BaseDef
    {
        get { return baseDef; }
        set { baseDef = value; }
    }

    public GameObject Zorro
    {
        get { return zorro; }
    }

    public GameObject ObjSeleccionado
    {
        get { return objSeleccionado; }
        set { objSeleccionado = value; }
    }

    public Camera Cam
    {
        get { return cam; }
    }

    public int Dinero
    {
        get
        {
            return dinero;
        }

        set
        {
            dinero = value;
        }
    }

    public float BaseVida
    {
        get
        {
            return baseVida;
        }

        set
        {
            baseVida = value;
        }
    }

    public GameObject ArticuloComprado
    {
        get
        {
            return articuloComprado;
        }

        set
        {
            articuloComprado = value;
        }
    }

    public GameObject[] PlantillasPrfb
    {
        get
        {
            return plantillasPrfb;
        }

        set
        {
            plantillasPrfb = value;
        }
    }

    public Base BaseScript
    {
        get { return baseScript; }
        set { baseScript = value; }
    }

    public ControlZorro Bullet
    {
        get { return zorro.GetComponent<ControlZorro>(); }
    }

    public GameObject[] Spawners
    {
        get
        {
            return spawners;
        }

        set
        {
            spawners = value;
        }
    }

    public int Necesarios
    {
        get
        {
            return necesarios;
        }

        set
        {
            necesarios = value;
        }
    }

    public int Destruidos
    {
        get
        {
            return destruidos;
        }

        set
        {
            destruidos = value;
        }
    }
    #endregion
}
