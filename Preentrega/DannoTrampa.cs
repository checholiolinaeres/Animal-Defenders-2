using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DannoTrampa : MonoBehaviour {

    private float danno;
    private float activacion;
    private GameObject explosion;
    private string tipo; //pasar propiedad de producto
    bool explotar = false;

	// Use this for initialization
	void Start () {
        explosion = GameObject.Find("Explosion prefab");
        switch (tipo)
        {
            case "vida":
                {
                    danno = 100f;
                }
                break;
            case "velocidad":
                {
                    danno = 60f; //porcentaje
                }
                break;
        }
        activacion = 10f;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (explotar == true)
        {
            explosion.transform.localScale += Vector3.one * Time.deltaTime * 5;
            if (explosion.transform.localScale.x >= 50f)
            {
                Destroy(explosion.gameObject);
                Destroy(gameObject);
                explotar = false;
            }
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemigo"))
        {
            Invoke("Explotar", 2f);
        }
    }

    private void Explotar()
    {
        transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().enabled = false;
        //cosas raras con listas
        explosion = Instantiate(explosion, transform.position, Quaternion.identity);
        explotar = true;
        
        //cuando acabe de explotar se destruye
    }
}
