using System.Collections;
using UnityEngine;

public class Hongo : MonoBehaviour
{
    public Transform player;
    public float detectarRango = 5f;
    public float velocidadMovimiento = 2f;
    public float fuerzaRebote = 6f;
    public int vida = 3;

    [Header("Animación")]
    public Animator animator;

    private Rigidbody2D rb;
    private Vector2 direccionMovimiento;
    private Vector2 direccionDano;
    private bool muerto;
    private bool recibiendoDanio;
    private bool jugadorvivo;
    private bool EnMovimiento;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        jugadorvivo = true;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (jugadorvivo && !muerto)
        {
            Movimiento();
        }
        animator.SetBool("muerto", muerto);
        animator.SetBool("RecibeDanio", recibiendoDanio);
    }

    private void Movimiento()
    {
        float distanciaAlJugador = Vector2.Distance(transform.position, player.position);

        if (distanciaAlJugador < detectarRango)
        {
            Vector2 direccion = (player.position - transform.position).normalized;

            if (direccion.x < 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (direccion.x > 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }

            // El movimiento del hongo es solo horizontal.
            direccionMovimiento = new Vector2(direccion.x, 0);

            EnMovimiento = true;
        }
        else
        {
            direccionMovimiento = Vector2.zero;
            EnMovimiento = false;
        }

        if (!recibiendoDanio)
        {
            rb.MovePosition(rb.position + direccionMovimiento * velocidadMovimiento * Time.deltaTime);
        } 
        animator.SetBool("Enmovimiento", EnMovimiento);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Lógica de daño al jugador.
            Vector2 direccionDanio = new Vector2(transform.position.x, 0);
            MovimientoJugador jugador = collision.gameObject.GetComponent<MovimientoJugador>();

            jugador.RecibeDanio(direccionDanio, 1);
            jugadorvivo = !jugador.muerto;
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
        if (!recibiendoDanio)
        {
            vida -= cantDanio;
            recibiendoDanio = true;
            animator.SetBool("RecibeDanio", true);
            if (vida <= 0)
            {
                muerto = true;
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
        animator.SetBool("RecibeDanio", false);
        rb.linearVelocity = Vector2.zero;
    }

    public void EliminarCuerpo()
    {
        Destroy(gameObject);
    }
}