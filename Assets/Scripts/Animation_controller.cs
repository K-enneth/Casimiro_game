using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimationId                 //Definimos un enumerado
{
    Idle = 0,
    Run = 1,
    PrepararBrinco=2,
    Brincar=3,
}

public class Animation_controller : MonoBehaviour
{

    Animator animator;                              //Variable tipo Animator
    private void Awake()
    {
        animator = GetComponent<Animator>();        //Instanciamiento  variable "animator"
    }

    public void Play(AnimationId animationId)       //Método para ejecutar la animación solicitada enviada dentro de
                                                    //"AnimationId" ("Idle" o "Run")
    {
        animator.Play(animationId.ToString());      //Ejecuta la animacion gurdada en "animationID"


    }
   

   
}
