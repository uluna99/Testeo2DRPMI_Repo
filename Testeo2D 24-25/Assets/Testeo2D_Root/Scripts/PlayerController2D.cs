using System.Collections;
using System.Collections.Generic;
using UnityEditor.XR;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController2D : MonoBehaviour
{
    //Referencia a las antiguas inputs
    float horInput;

    //Referencias generales
    [SerializeField] Rigidbody2D playerRb; //Ref al rigidbody del player
    [SerializeField] PlayerInput playerInput; //Ref al gestor del input del jugador
    [SerializeField] Animator playerAnim; //ref al animator para gestionar las transiciones de animación

    [Header("Movement Parameters")]
    private Vector2 moveInput; //Almacén del input del player
    public float speed;
    [SerializeField] bool isFacingRight; 

    [Header("Jump Parameters")]
    public float jumpForce;
    [SerializeField] bool isGrounded;
    //Variables para el Groundcheck
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius = 0.1f;
    [SerializeField] LayerMask groundLayer;
 
    // Start is called before the first frame update
    void Start()
    {
        //Autoreferenciar componentes: nombre de variable = GetComponent()
        playerRb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        playerAnim = GetComponent<Animator>();
        isFacingRight = true; 
    }

    // Update is called once per frame
    void Update()
    {
        HandleAnimation();
        GroundCheck();

        //Flip
        if (moveInput.x > 0)
        {
            if (!isFacingRight)
            {
                Flip();
            }
        }
        if (moveInput.x < 0)
        {
            if (isFacingRight)
            {
                Flip();
            }
        }
    }

    private void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        playerRb.velocity = new Vector3(moveInput.x * speed, playerRb.velocity.y, 0);
    }

    void Flip()
    {
        Vector3 CurrentScale = transform.localScale;
        CurrentScale.x *= -1;
        transform.localScale = CurrentScale;
        isFacingRight = !isFacingRight; //nombre de bool = !nombre de bool (cambio al estado contrario)

    }

    void GroundCheck()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    void HandleAnimation()
    {
        //Conector de valores generales con parámetros de cambios de animación
        playerAnim.SetBool("isJumping", !isGrounded);
        if (moveInput.x > 0 || moveInput.x < 0) playerAnim.SetBool("isRunning", true);
        else playerAnim.SetBool("isRunning", false);
    }




    #region Input Events 
    //Para crear un evento:
    //Se define PUBLIC sin tipo de dato (VOID) y con una referencia al input (Callback.Context)
    public void HandleMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }


    public void HandleJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (isGrounded)
            {
                playerRb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
            }
            
        }   
        
       
    }




    #endregion
}
