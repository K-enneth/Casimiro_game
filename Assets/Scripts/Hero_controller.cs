using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO.Ports;

public class Hero_controller : MonoBehaviour, ITargetCombat_1
{

    [Header("Attack Variables")]
    [SerializeField] SwordController_1 swordController;

    [Header("Health Variables")]//
    [SerializeField] int health = 10;//

    [Header("Animation Variable")]
    [SerializeField] Animation_controller animatorController;

    [Header("Checker Variables")]                                //Cabecera del ComboBox "Variables"  //"SerializeField" significa que desde el inspector podemos  manipular o ver su valor.
    [SerializeField] LayerChecker_1 footA;                  //Instanciamento a la Clase "LayerChecker_1" = footA
    [SerializeField] LayerChecker_1 footB;                  //Instanciamento a la Clase "LayerChecker_1" = footB

    [Header("Boolean Variables")]                           //"Pesta?a" con t?tulo en el Inspector 
    public bool canDoubleJump;                              //variable boleana(verdadero/falso) para ejecutar el salto doble
    public bool playerIsAttacking;                          //Esta atacando el h?roe??

    [Header("Interruption Variables")]                      //"Pesta?a" con t?tulo en el Inspector              
    public bool canCheckGround;                             //Variable booleana, usada para detectar si tocas el piso
    public bool canMove; //Usamos la variable para anular el movimiento "Horizontal" "Run" y "Idle"

    public Image[] hearts;
    public Sprite fullheart;
    public Sprite emptyheart;


    [Header("Rigid Variables")]
    [SerializeField] private float doubleJumpForce;         //Agregamos una variable flotante para agrear furza al DobleSalto
    [SerializeField] private float jumpForce;               //Agregamos una variable flotante para agrear furza al salto
    [SerializeField] private float speed_;                  //"SerializeField" significa que desde el inspector podemos  manipular o ver su valor.
    [SerializeField] private Vector2 movementDirection;     //"SerializeField" significa que desde el inspector podemos  manipular o ver su valor.

    private bool attackPressed = false;

    private Rigidbody2D rigidbody2D_;                       //Variable de instanciamiento
    private bool jumpPressed = false;                       //variable usadas para saber si se apret? la barra espaciadora
                                                            //y es personaje salt?.
    private bool playerIsOnGround;                          //Variable privada tipo Bool, el Heroe esta tocando el piso?

    
    public GameObject heroe;

    public TMP_Text Contador;                               //variable tipo "TMP_Text" = Contador (salud del H?roe)
                                                            //El valor de "Contador" esta linkeado al texto del Canvas
                                                            //y va variando de acuerdo el m?todo "TakeDamage"

    public static Hero_controller instance;
     private void Awake()
     {
         if (instance == null)
         {
             instance = this;
             DontDestroyOnLoad(this.gameObject);
         }
         else
         {
             Destroy(this.gameObject);
         }
     }
    void Start()
    {
      

        canMove = true; //Al iniciar el juego el personaje se mueve "Run" y "Idle"
        canCheckGround = true;                              //inicializamos la variable "canCheckGround" como verdadera
        rigidbody2D_ = GetComponent<Rigidbody2D>();         //Instanciando la variable.
        animatorController.Play(AnimationId.Idle);
        //jumpPressed = Input.GetButtonDown("Jump");

       
    }

    // Update is called once per frame
    void Update()
    {


        HandleIsGrounding();                                 //Invoca al m?todo "HandleIsGrounding" (El h?roe est? tocando el piso?). 
        HandleControls();                                    //invocando el m?todo "HandleControls" (abre el puerto de entrada del teclado)
        HandleMovement();                                    //invocando el m?todo "HandleMovement" (multiplica el valor de "x" por "speed".
        HandleFlip();
        HandleJump();                                         //invocando el m?todo "HandleFlip" (rota el personaje a la izquierda o a la derecha)
        HandleAttack(); //invocando el m?todo "HandleAttack" (agregamos clip de animaci?n Attack)

        foreach(Image img in hearts)
        {
            img.sprite = emptyheart;
        }
        for(int i = 0; i < health; i++)
        {
            hearts[i].sprite = fullheart;
        }
    }








    void HandleIsGrounding()
    {
        if (!canCheckGround) return;   //Si NO esta tocando el piso NO se ejecuta nada de este m?todo
                                       //(esta variable se apaga en la corrutina)
        //Debug.Log(!canCheckGround);


        playerIsOnGround = footA.isTouching || footB.isTouching;  //Falta comentar..........
        //Debug.Log("Tocando el pioo");
    }

    void HandleControls()
    {
        attackPressed = Input.GetButtonDown("Attack"); //linkeamos el RMB a variable "attackPressed"
        movementDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        jumpPressed = Input.GetButtonDown("Jump");

    }

    void HandleMovement()
    {
        if (!canMove) return; //Si est? volando no hagas nada.....

        rigidbody2D_.velocity = new Vector2(movementDirection.x * speed_, rigidbody2D_.velocity.y);

        if (playerIsOnGround)
        {

            if (Mathf.Abs(rigidbody2D_.velocity.x) > 0)                         //comprobamos si se esta moviendo en el eje "X"
            {
                animatorController.Play(AnimationId.Run);
            }
            else
            {
                animatorController.Play(AnimationId.Idle);
            }
        }

    }
    void HandleFlip()
    {
        if (rigidbody2D_.velocity.magnitude > 0)                //S?lo si el personaje se est? moviendo ejecuta estas lineas...
        {
            if (rigidbody2D_.velocity.x >= 0)                           //si la velocidad en "x" es mayor que cero ejecuta la siguiente linea....
            {
                this.transform.rotation = Quaternion.Euler(0, 0, 0);            //No rotes
            }
            else                                                                            //de otro modo ejecuta las siguientes lineas.....
            {
                this.transform.rotation = Quaternion.Euler(0, 180, 0);              //rota en "y" 180?
            }
        }
    }

    void HandleJump()           //M?todo para agregarle fuerza la RigidBody del Hero
    {
        if (canDoubleJump && jumpPressed && !playerIsOnGround)  //"!playerIsOnGround" esta variable nos indica que NO esta tocando el piso
        {

            this.rigidbody2D_.velocity = Vector2.zero;
            this.rigidbody2D_.AddForce(Vector2.up * doubleJumpForce, ForceMode2D.Impulse);//agrega impulso de fuerza instant?nea hacia arriba al doble salto           
            canDoubleJump = false;                                                        //apagamos la variable "canDoubleJump? para que no brinque infinitamente
        }

        if (jumpPressed && playerIsOnGround)

        {
            this.rigidbody2D_.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            animatorController.Play(AnimationId.Idle);

            StartCoroutine(HandleJumpAnimation());
            canDoubleJump = true;                                                   //prendemos la variable "canDoubleJump" para que brinque
                                                                                    //de nuevo si apretamos la barra espaciadora 

        }
    }

    IEnumerator HandleJumpAnimation()                       //Corrutina que ejecuta dos "clips" desfasados en tiempo 0.4f unidades de tiempo
    {
        canCheckGround = false;
        playerIsOnGround = false;
        yield return new WaitForSeconds(0.1f);                  //Ejecutar el Clip "Brinco" durante 0.1f uniades de tiempo
        animatorController.Play(AnimationId.PrepararBrinco);   //Ejecuta el clip "PrepararBrinco"
        yield return new WaitForSeconds(0.2f);                  //Ejecutar el Clip "PrepararBrinco" durante 0.2f uniades de tiempo
        animatorController.Play(AnimationId.Brincar);           //Ejecuta el clip "Brinco"
        //yield return new WaitForSeconds(1);                     //Ejecutar el Clip "Brinco" durante 1f uniades de tiempo
        canCheckGround = true;
    }

    void HandleAttack()                         //M?todo de animaci?n Attack (puede atacar en el piso y en el aire)
    {
        //Debug.Log("ok");
        if (attackPressed && !playerIsAttacking)          //Si apretamos RMB y NO est?  atacando..
        {
            animatorController.Play(AnimationId.Attack);  //ejecutamos Clip "Atack"
            playerIsAttacking = true;                     //Prendemos la variable como verdadera (el h?roe est? atacando)
            swordController.Attack(0.4f);
            StartCoroutine(RestoreAttack());              //Inicia corrutina "RestoreAttack" (reinicia
        }
    }

    public void TakeDamage(int damagePoints)
     {
       
        health = Mathf.Clamp(health - damagePoints, 0, 10);
        


         //Debug.Log(health);
         if(health == 0)
         {
             Destroy(gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

     }




    IEnumerator RestoreAttack()                         //Corrutina "RestoreAttack"
    {
        if (playerIsOnGround)                          //Si el h?roe esta en el suelo?
            canMove = false;                            //apaga la variable "canMove"
        yield return new WaitForSeconds(0.4f);          //espera 0.4f 
        playerIsAttacking = false;                      //Apaga variable "heroe esta tacando"
        if (!playerIsOnGround)                          //Si el h?roe NO est? en el piso.....
            animatorController.Play(AnimationId.PrepararBrinco);  //Inicia clip "preparaBrinco"
        canMove = true;                                 //prende la variable "canMove"
    }

    public void UpdatePosition(Vector2 position)
     {
         this.transform.position = position;
         rigidbody2D_.velocity = Vector2.zero;
     }
}


