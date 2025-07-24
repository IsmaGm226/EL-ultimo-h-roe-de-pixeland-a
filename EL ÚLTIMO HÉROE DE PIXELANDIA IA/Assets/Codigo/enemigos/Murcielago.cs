using UnityEngine;

public class Murcielago : MonoBehaviour
{
    public Sonidosdeenemigos sonidosEnemigos;
    public float radioBusqueda;
    public float detectarRango = 5f;
    public LayerMask capaJugador;
    public Transform transformJugador;
    public float velocidadMovimiento;
    public float distanciaMaxima;
    public Vector3 puntoIncial;
    public float fuerzaRebote = 6f;
    public int vida = 3;

    private Vector2 direccionMovimiento;
    private bool muerto;
    private bool recibiendoDanio;
    private bool jugadorvivo;
    private Rigidbody2D rb;
    public Animator animator;
    public bool mirandoDerecha;
    public int valor = 1;

    public EstadoMovimiento estadoActual;
    public GameManager gameManager;
    public enum EstadoMovimiento
    {
        Esperando,
        Siguiendo,
        Volviendo,

    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        puntoIncial = transform.position;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (jugadorvivo && !muerto)
        {
            Movimiento();
        }

        animator.SetBool("muerto", muerto);
        animator.SetBool("recibeDanio", recibiendoDanio);

        switch (estadoActual)
        {
            case EstadoMovimiento.Esperando:
                EstadoEsperando();
                break;

            case EstadoMovimiento.Siguiendo:
                EstadoSiguiendo();
                break;

            case EstadoMovimiento.Volviendo:
                EstadoVolviendo();
                break;
        }
    }
    private void Movimiento()
    {
        if (transformJugador == null)
            return;

        float distanciaAlJugador = Vector2.Distance(transform.position, transformJugador.position);

        if (distanciaAlJugador < detectarRango)
        {
            Vector2 direccion = (transformJugador.position - transform.position).normalized;
            direccionMovimiento = new Vector2(direccion.x, 0);
        }
        else
        {
            direccionMovimiento = Vector2.zero;
        }

        if (!recibiendoDanio && !muerto)
        {
            rb.MovePosition(rb.position + direccionMovimiento * velocidadMovimiento * Time.deltaTime);
        }
    }



    private void EstadoEsperando()
    {
        Collider2D jugadorCollider = Physics2D.OverlapCircle(transform.position, radioBusqueda, capaJugador);
        if (jugadorCollider)
        {
            transformJugador = jugadorCollider.transform;

            estadoActual = EstadoMovimiento.Siguiendo;
        }
    }

    private void EstadoSiguiendo()
    {
        if (transformJugador == null)
        {
            estadoActual = EstadoMovimiento.Volviendo;
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, transformJugador.position, velocidadMovimiento * Time.deltaTime);

        GirarObjetivo(transformJugador.position);

        if (Vector3.Distance(transform.position, puntoIncial) > distanciaMaxima || Vector2.Distance(transform.position, transformJugador.position) > distanciaMaxima)
        {
            estadoActual = EstadoMovimiento.Volviendo;
            transformJugador = null;
        }
    }

    private void EstadoVolviendo()
    {
        transform.position = Vector3.MoveTowards(transform.position, puntoIncial, velocidadMovimiento * Time.deltaTime);

        GirarObjetivo(puntoIncial);

        if (Vector2.Distance(transform.position, puntoIncial) < 0.1f)
        {
            estadoActual = EstadoMovimiento.Esperando;
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
            // La dirección de daño de la espada debe ser desde la espada hacia el enemigo.
            // Vector2 direccionDanio = (transform.position - collision.transform.position).normalized; // Mejor cálculo
            Vector2 direccionDanio = new Vector2(collision.gameObject.transform.position.x, 0); // Tu lógica original
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

    public void DestruirMurcielago()
    {
        if (muerto)
        {
            Destroy(gameObject);
        }
    }

    public void GirarObjetivo(Vector3 objetivo)
    {
        if (objetivo.x > transform.position.x && !mirandoDerecha)
        {
            Girar();
        }
        else if (objetivo.x < transform.position.x && mirandoDerecha)
        {
            Girar();
        }
    }

    private void Girar()
    {
        mirandoDerecha = !mirandoDerecha;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
    }

    public void MorirAnimacion()
    {
        if (gameManager != null)
        {
            gameManager.SumarPuntos(valor);
        };
    }
}
