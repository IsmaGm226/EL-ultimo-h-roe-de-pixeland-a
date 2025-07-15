using System.Collections;
using UnityEngine;

public class controlesdeenemigo : MonoBehaviour
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
    private bool muertoe;
    private bool recibiendoDanio;
    private bool jugadorvivo;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        jugadorvivo = true;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(jugadorvivo && !muertoe)
        {
            Movimiento();
        }

        animator.SetBool("muertoe", muertoe);
    }

    private void Movimiento()
    {
        float distanciaAlJugador = Vector2.Distance(transform.position, player.position);

        if (distanciaAlJugador < detectarRango)
        {
            Vector2 direccion = (player.position - transform.position).normalized;

            direccionMovimiento = new Vector2(direccion.x, 0);
        }
        else
        {
            direccionMovimiento = Vector2.zero;
        }
        if (!recibiendoDanio)
        {
            rb.MovePosition(rb.position + direccionMovimiento * velocidadMovimiento * Time.deltaTime);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Lógica de daño al jugador
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
            if (vida <= 0)
            {
                muertoe = true;
            }
            else
            {
                Vector2 rebote = new Vector2(transform.position.x - direccion.x, 0.2f).normalized;
                rb.AddForce(rebote * fuerzaRebote, ForceMode2D.Impulse);
                StartCoroutine(DesactivaDanio());
            }
           
        }
    }
    IEnumerator DesactivaDanio()
    {
        yield return new WaitForSeconds(0.4f);
        recibiendoDanio = false;
        rb.linearVelocity = Vector2.zero;
    }

}
