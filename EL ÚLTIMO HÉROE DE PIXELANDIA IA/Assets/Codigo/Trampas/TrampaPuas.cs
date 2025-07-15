using UnityEngine;
using System.Collections;

public class TrampaPuas : MonoBehaviour
{
    // --- Variables Configurables ---
    public Rigidbody2D rb2D;                 // Rigidbody 2D de la trampa.
    public LayerMask capaJugador;            // Capa del jugador para daño.
    public LayerMask capaSuelo;              // Capa del suelo para detección de colisiones.
    public int danio = 1;                    // Cantidad de daño.

    public float velocidadBajada;            // Velocidad al bajar.
    public float velocidadSubida;            // Velocidad al subir.
    public float tiempoEsperaAbajo;          // Tiempo de pausa en la parte inferior.
    public float tiempoEsperaArriba;         // Tiempo de pausa en la parte superior.
    public float distanciaBajada;            // Distancia máxima de bajada.
    public float distanciaRaycastSuelo = 0.6f; // Distancia para que el raycast detecte el suelo.

    // --- Variables Privadas ---
    private Vector2 posicionInicial;                 // Posición de inicio de la trampa.
    private Vector2 posicionObjetivoBajaCalculada;   // Límite máximo de bajada.

    private void Start()
    {
        posicionInicial = transform.position;
        posicionObjetivoBajaCalculada = new Vector2(posicionInicial.x, posicionInicial.y - distanciaBajada);

        // Asegura que el Rigidbody2D existe y lo configura como Kinematic.
        if (rb2D == null) rb2D = GetComponent<Rigidbody2D>();
        if (rb2D == null) { Debug.LogError("Rigidbody2D no encontrado.", this); enabled = false; return; }
        rb2D.bodyType = RigidbodyType2D.Kinematic;

        StartCoroutine(CicloTrampa()); // Inicia el ciclo de movimiento.
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Aplica daño si colisiona con el jugador.
        if (((1 << collision.gameObject.layer) & capaJugador) != 0)
        {
            MovimientoJugador jugador = collision.gameObject.GetComponent<MovimientoJugador>();
            if (jugador != null)
            {
                Vector2 direccionDanio = new Vector2(transform.position.x, 1);
                jugador.RecibeDanio(direccionDanio, danio);
            }
        }
    }

    private IEnumerator CicloTrampa()
    {
        while (true)
        {
            yield return new WaitForSeconds(tiempoEsperaArriba); // Pausa en la parte superior.

            // --- Bajar la Trampa ---
            float tiempoInicioBajada = Time.time;
            Vector2 puntoInicioBajada = rb2D.position;
            Vector2 objetivoActualBajada = posicionObjetivoBajaCalculada; // Establece el objetivo inicial.

            while (Vector2.Distance(rb2D.position, objetivoActualBajada) > 0.05f)
            {
                // Usa un raycast para detectar el suelo y detener la trampa con precisión.
                RaycastHit2D hit = Physics2D.Raycast(rb2D.position, Vector2.down, distanciaRaycastSuelo, capaSuelo);

                if (hit.collider != null) // Si el raycast detecta el suelo.
                {
                    // Posiciona la trampa justo sobre la superficie del suelo.
                    rb2D.position = hit.point + Vector2.up * GetComponent<Collider2D>().bounds.extents.y;
                    break; // Sale del bucle de bajada.
                }

                // Mueve la trampa si no ha detectado el suelo.
                float fraccionRecorrida = (Time.time - tiempoInicioBajada) * velocidadBajada / Vector2.Distance(puntoInicioBajada, posicionObjetivoBajaCalculada);
                rb2D.MovePosition(Vector2.Lerp(puntoInicioBajada, objetivoActualBajada, fraccionRecorrida));
                yield return null;
            }
            rb2D.position = objetivoActualBajada; // Asegura la posición final.

            yield return new WaitForSeconds(tiempoEsperaAbajo); // Pausa en la parte inferior.

            // --- Subir la Trampa ---
            float tiempoInicioSubida = Time.time;
            Vector2 puntoInicioSubida = rb2D.position;
            while (Vector2.Distance(rb2D.position, posicionInicial) > 0.05f)
            {
                float fraccionRecorrida = (Time.time - tiempoInicioSubida) * velocidadSubida / Vector2.Distance(puntoInicioSubida, posicionInicial);
                rb2D.MovePosition(Vector2.Lerp(puntoInicioSubida, posicionInicial, fraccionRecorrida));
                yield return null;
            }
            rb2D.position = posicionInicial; // Asegura la posición inicial.
        }
    }
}