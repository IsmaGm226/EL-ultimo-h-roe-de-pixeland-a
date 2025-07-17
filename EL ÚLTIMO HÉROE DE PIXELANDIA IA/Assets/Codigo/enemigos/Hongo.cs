using System.Collections;
using UnityEngine;

public class Hongo : MonoBehaviour
{
    public Transform player;
    public float detectarRango = 5f;
    public float velocidadMovimiento = 2f;
    public float fuerzaRebote = 3f;
    public int vida = 3;

    [Header("Animación")]
    public Animator animator;

    private Rigidbody2D rb;
    private Vector2 direccionMovimiento;
    private bool enMovimiento;

    private bool muerto;
    private bool recibiendoDanio;
    private bool jugadorvivo;

    void Start()
    {
        jugadorvivo = true;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (jugadorvivo && !muerto)
        {
            Movimiento();
        }
        animator.SetBool("Enmovimiento", enMovimiento);
        animator.SetBool("muerto", muerto);
        animator.SetBool("recibeDanio", recibiendoDanio);
    }

    private void Movimiento()
    {
        float distanciaAlJugador = Vector2.Distance(transform.position, player.position);

        if (distanciaAlJugador < detectarRango)
        {
            Vector2 direccion = (player.position - transform.position).normalized;
            direccionMovimiento = new Vector2(direccion.x, 0);

            enMovimiento = true;

            if (direccion.x < 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
        else
        {
            direccionMovimiento = Vector2.zero;
            enMovimiento = false;
        }

        if (!recibiendoDanio && !muerto)
        {
            rb.MovePosition(rb.position + direccionMovimiento * velocidadMovimiento * Time.deltaTime);
            
            animator.SetBool("Enmovimiento", enMovimiento);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 direccionDanio = new Vector2(transform.position.x, 0);
            MovimientoJugador jugador = collision.gameObject.GetComponent<MovimientoJugador>();

            if (jugador != null) // Asegúrate de que el componente exista
            {
                jugador.RecibeDanio(direccionDanio, 1);
                jugadorvivo = !jugador.muerto;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Espada"))
        {
           
            Vector2 direccionDanio = new Vector2(collision.gameObject.transform.position.x, 0); 
            RecibeDanio(direccionDanio, 1);
        }
    }

    public void RecibeDanio(Vector2 direccion, int cantDanio)
    {
        if (!recibiendoDanio && !muerto) // Solo recibe daño si no está ya recibiéndolo o muerto
        {
            vida -= cantDanio;
            recibiendoDanio = true;

            if (vida <= 0)
            {
                muerto = true;
                enMovimiento = false;
            }
            else
            {
                Vector2 rebote = new Vector2(transform.position.x - direccion.x, 0.2f).normalized;
                rb.AddForce(rebote * fuerzaRebote, ForceMode2D.Impulse);
            }
        }
    }

    public void DesactivaDanio()
    {
        recibiendoDanio = false;
        rb.linearVelocity = Vector2.zero;
    }

    public void DestruirHongo()
    {
        if (muerto)
        {
            Destroy(gameObject);
        }
    }
}