using UnityEngine;

public class bala : MonoBehaviour
{
    public float velocidad;
    public int daño;
    public bool jugadorvivo = true;
    public float distanciaMaxima = 15f; // Distancia máxima que puede recorrer la bala

    private Vector3 posicionInicial;

    private void Start()
    {
        posicionInicial = transform.position;
    }

    private void Update()
    {
        transform.Translate(-Vector2.right * velocidad * Time.deltaTime);

        // Destruye la bala si supera la distancia máxima
        if (Vector3.Distance(posicionInicial, transform.position) >= distanciaMaxima)
        {
            Destroy(gameObject);
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
                Destroy(gameObject);
                jugadorvivo = !jugador.muerto;
            }
        }
    }
}
