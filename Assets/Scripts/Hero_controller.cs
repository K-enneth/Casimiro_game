using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_controller : MonoBehaviour
{
    [Header("Animation Variable")]
    [SerializeField] Animation_controller animatorController;
    
   [SerializeField] private float speed_;                                  //"SerializeField" significa que desde el inspector podemos manipular o ver su valor.
   [SerializeField] private Vector2 movementDirection;     //"SerializeField" significa que desde el inspector podemos manipular o ver su valor.
   private Rigidbody2D rigidbody2D_;                                   //Variable de instanciamiento\


    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D_ = GetComponent<Rigidbody2D>();
        animatorController.Play(AnimationId.Idle);
    }

    // Update is called once per frame
    void Update()
    {
        HandleControls();              //invocando el método "HandleControls" (abre el puerto de entrada del teclado)
        HandleMovement();         //invocando el método "HandleMovement" (multiplica el valor de "x" por "speed".
        HandleFlip();                      //invocando el método "HandleFlip" (rota el personaje a la izquierda o a la derecha)
    }

    void HandleControls()
     {
         movementDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
       
    }
  void HandleMovement()
     {
         rigidbody2D_.velocity = new Vector2(movementDirection.x * speed_, rigidbody2D_.velocity.y);


            if (Mathf.Abs(rigidbody2D_.velocity.x) > 0)                         //comprobamos si se esta moviendo en el eje "X"
            {
                animatorController.Play(AnimationId.Run);
            }
            else
            {
                animatorController.Play(AnimationId.Idle);
            }
        
    }
  void HandleFlip()
     {
         if (rigidbody2D_.velocity.magnitude > 0)                //Sólo si el personaje se está moviendo ejecuta estas lineas...
         {
             if (rigidbody2D_.velocity.x >= 0)                           //si la velocidad en "x" es mayor que cero ejecuta la siguiente linea....
             {
                 this.transform.rotation = Quaternion.Euler(0, 0, 0);            //No rotes
             }
             else                                                                            //de otro modo ejecuta las siguientes lineas.....
             {
                 this.transform.rotation = Quaternion.Euler(0, 180, 0);              //rota en "y" 180º
             }
         }
     }
}
