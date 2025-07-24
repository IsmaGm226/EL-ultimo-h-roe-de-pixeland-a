using UnityEngine;

public class ElLich : MonoBehaviour
{
    public Sonidosdeenemigos sonidosEnemigos;
    public Transform player;
    public Transform controladorDisparo;
    public float detectarRango = 5f;
    public float velocidadMovimiento = 2f;
    public float fuerzaRebote = 6f;
    public int vida = 3;


    [Header("Animación")]
    public Animator animator;

    private Rigidbody2D rb;
    private Vector2 direccionMovimiento;

    private bool muerto;
    private bool recibiendoDanio;
    private bool jugadorvivo;
    public int valor = 1;
    public GameManager gameManager;

    public float distanciaLinea = 5f;
    public LayerMask capaJugador;
    public GameObject bala;
    public float tiempoEntreDisparos = 2f;
    public float tiempoUltimoDisparo;
    public float tiempoEsperaDisparo = 0.2f;
    public bool jugadorEnRango;
    private bool enMovimiento;


    void Start()
    {
        jugadorvivo = true;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (jugadorvivo && !muerto)
        {
            Movimiento();

            // Detección de jugador en la dirección que mira
            Vector2 direccionRaycast = transform.localScale.x > 0 ? Vector2.left : Vector2.right;
            jugadorEnRango = Physics2D.Raycast(controladorDisparo.position, direccionRaycast, distanciaLinea, capaJugador);
            if (jugadorEnRango)
            {
                if (Time.time > tiempoEntreDisparos + tiempoUltimoDisparo)
                {
                    tiempoUltimoDisparo = Time.time;
                    animator.SetTrigger("Disparar");
                    Invoke(nameof(Disparar), tiempoEsperaDisparo);
                }
            }
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

            if (jugador != null) 
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
        if (!recibiendoDanio && !muerto)
        {
            vida -= cantDanio;

            // Sonido de recibir daño
            if (sonidosEnemigos != null)
            {
                sonidosEnemigos.playRecibirDaño();
            }

            recibiendoDanio = true;

            if (vida <= 0)
            {
                muerto = true;

                // Sonido de muerte
                if (sonidosEnemigos != null)
                {
                    sonidosEnemigos.playMuerte();
                }
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

    public void MorirAnimacion()
    {
        if (gameManager != null)
        {
            gameManager.SumarPuntos(valor);
        }
        ;
    }

    public void DestruirElLich ()
    {
        if (muerto)
        {
            Destroy(gameObject);
        }
    }

    private void Disparar()
    {
        GameObject nuevaBala = Instantiate(bala, controladorDisparo.position, Quaternion.identity);
        Vector2 direccionDisparo = transform.localScale.x > 0 ? Vector2.left : Vector2.right;
        bala balaScript = nuevaBala.GetComponent<bala>();
        if (balaScript != null)
        {
            balaScript.SetDireccion(direccionDisparo);
        }
    }
}
