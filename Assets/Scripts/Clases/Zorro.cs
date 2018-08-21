using UnityEngine.AI;
using UnityEngine;

public class Zorro
{
    private GameObject[] navs; //array de objetos sobre los que se pueda navegar
    private NavMeshAgent agent; //el agente navmesh del zorro    
    private Animator anim; // componente animator del zorro
    private string estado; // estado del zorro

    public Zorro()
    {
        navs = null;
        agent = null;
        anim = null;
    }

    public GameObject[] Navs
    {
        get
        {
            return navs;
        }

        set
        {
            navs = value;
        }
    }

    public NavMeshAgent Agent
    {
        get
        {
            return agent;
        }

        set
        {
            agent = value;
        }
    }

    public Animator Anim
    {
        get
        {
            return anim;
        }

        set
        {
            anim = value;
        }
    }

    public string Estado
    {
        get
        {
            return estado;
        }

        set
        {
            estado = value;
        }
    }

    public void DefinirEstado(int fase)
    {
        switch (fase)
        {
            case 1:
                estado = "Desplegado";
                break;
            case 2:
                estado = "Retirada";
                break;
            case 3:
                estado = "Replegado";
                break;
            case 4:
                estado = "En Combate";
                break;
        }
    }
}
