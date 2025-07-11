using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.Rendering;

public class MovimientoJugador : MonoBehaviour
{
    // --- Parámetros públicos ---
    [Header("Movimiento")]
    public float velocidad = 5f;
    public float fuerzasalto = 6f;
    public float fuerzaRebote = 6f;

    public int vida = 3;

    [Header("Detección de suelo")]
    public float longitudRaycast = 0.1f;
    public LayerMask capaSuelo;

    [Header("Animación")]
    public Animator animator;

    // --- Campos privados ---
    private Rigidbody2D rb;
    private bool enSuelo;
    public bool recibiendoDanio;
    private bool atacando;
    public bool muerto;

    // --- Métodos Unity ---
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
        Animaciones();

        if (!muerto)
        {
            if (!atacando)
            {
                Movimiento();

                // Comprobar si está en el suelo
                RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, longitudRaycast, capaSuelo);
                enSuelo = hit.collider != null;

                // Saltar
                if (enSuelo && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && !recibiendoDanio)
                {
                    rb.AddForce(new Vector2(0f, fuerzasalto), ForceMode2D.Impulse);
                }
            }


            if (Input.GetKeyDown(KeyCode.Z) && !atacando && enSuelo)
            {
                Atacando();
            }
        }
        
           
    }

    public void Movimiento()
    {
        // Movimiento horizontal
        float velocidadX = Input.GetAxis("Horizontal") * velocidad * Time.deltaTime;
        animator.SetFloat("movement", velocidadX * velocidad);

        // Girar sprite según dirección
        if (velocidadX < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (velocidadX > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        // Mover jugador manualmente
        Vector3 posicion = transform.position;

        if (!recibiendoDanio)
        {
            transform.position = new Vector3(posicion.x + velocidadX, posicion.y, posicion.z);
        }
    }

    public void Animaciones()
    {
        // Actualizar animación de suelo
        animator.SetBool("ensuelo", enSuelo);
        animator.SetBool("recibeDanio", recibiendoDanio);
        animator.SetBool("Atacando", atacando);
        animator.SetBool("muerto", muerto);
    }

    public void RecibeDanio(Vector2 direccion, int cantDanio)
    {
        if(!recibiendoDanio)
        {
            recibiendoDanio = true;
            vida -= cantDanio;
            if(vida <= 0 )
            {
                muerto = true;
            }
            if(!muerto)
            {
                Vector2 rebote = new Vector2(transform.position.x - direccion.x, 1).normalized;
                rb.AddForce(rebote * fuerzaRebote, ForceMode2D.Impulse);
            }
            
        }
        
    }

    public void DesactivaDanio()
    {
        recibiendoDanio = false;
        rb.linearVelocity = Vector2.zero;  
    }

    public void Atacando()
    {
        atacando = true;
    }
    public void DesactivaAtaque()
    {
        atacando = false;
    }


}
