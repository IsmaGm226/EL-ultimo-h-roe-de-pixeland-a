using System.Collections;
using UnityEngine;

public class TrampadeFuego : MonoBehaviour
{
    [Header("Temporizador de la Trampa de fuego")]
    [SerializeField] private float danio;
    [SerializeField] private float demora;
    [SerializeField] private float activacionTiempo;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private bool activado;
    private bool activa;

    public TrampadeFuego(MovimientoJugador jugadorDentro)
    {
        this.jugadorDentro = jugadorDentro;
    }

    private MovimientoJugador jugadorDentro; // Referencia al jugador dentro del trigger

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            jugadorDentro = collision.GetComponent<MovimientoJugador>();
            if (!activado)
            {
                StartCoroutine(ActivaTrampaF());
            }
            // Si la trampa ya está activa, aplica daño inmediatamente
            if (activa && jugadorDentro != null)
            {
                Vector2 direccionDanio = new Vector2(transform.position.x, 1);
                jugadorDentro.RecibeDanio(direccionDanio, (int)danio);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            jugadorDentro = null;
        }
    }

    private IEnumerator ActivaTrampaF()
    {
        activado = true;

        yield return new WaitForSeconds(demora);

        activa = true;
        animator.SetBool("activado", true);

        // Si el jugador está dentro cuando se activa el fuego, aplica daño
        if (jugadorDentro != null)
        {
            Vector2 direccionDanio = new Vector2(transform.position.x, 1);
            jugadorDentro.RecibeDanio(direccionDanio, (int)danio);
        }

        yield return new WaitForSeconds(activacionTiempo);
        activa = false;
        activado = false;
        animator.SetBool("activado", false);
    }
}
