using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aleatorio_3 : MonoBehaviour
{
    private float velocidad = 2;    //Velocidad de desplazamiento del fantasma

    void Update()
    {
        Debug.Log(velocidad);
        transform.Translate(velocidad * Time.deltaTime, 0, 0);
        if (transform.position.x < 23.30f)   //Rango de desplazamieneto en "+X"
            velocidad = -velocidad;
        if (transform.position.x > 29.7f)  //Rango de desplazamieneto en "-X"
            velocidad = -velocidad;
    }
}
