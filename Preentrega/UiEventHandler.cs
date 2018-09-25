using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class UiEventHandler : MonoBehaviour
{
    public delegate void AccionClick();
    public static event AccionClick OnClick;
    public static event AccionClick Compra;
    //MainProps props;
    Text dineroText;
    Slider vida;
    Slider retraso;
    Button botonCentrar;   

    private void Start()
    {
        //props = GameObject.Find("Main").GetComponent<MainProps>();
        dineroText = GameObject.Find("Dinero").GetComponent<Text>();
        botonCentrar = GameObject.Find("Button").GetComponent<Button>();
        //vida = GameObject.Find("SliderVida").GetComponent<Slider>();
        retraso = GameObject.Find("SliderVidaRetraso").GetComponent<Slider>();
        //vida.maxValue = MainProps.instancia.BaseVida;
        //retraso.maxValue = MainProps.instancia.BaseVida;
    }

    private void Update()
    {
        dineroText.text = " $ " + ManagerJuego.instancia.Dinero;
        //vida.value = MainProps.instancia.BaseVida;
        //BajarRetraso();
    }


    public void Comprar()
    {
        if(Compra != null)
            Compra();
    }

    public void CentrarUi()
    {
        if(OnClick != null)
            OnClick();
    }

    public void BajarRetraso()
    {
        if(retraso.value < vida.value)
        {
            retraso.value = vida.value;
        }
        else if(retraso.value > vida.value)
        {
            retraso.value -= 0.01f;
        }        
    }
    
}

