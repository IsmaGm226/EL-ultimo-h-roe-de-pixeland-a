using UnityEngine;

public class Balaenemigo : MonoBehaviour
{
    public float velocidad;
    public int daño;

    private void Update()
    {
        transform.Translate(Time.deltaTime * velocidad * Vector2.right);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<MovimientoJugador>(out var jugador))
        {
            // Calcula la dirección del daño desde la bala hacia el jugador
            Vector2 direccionDanio = (other.transform.position - transform.position).normalized;
            jugador.RecibeDanio(direccionDanio, daño);
            Destroy(gameObject);
        }
        // Si no es el jugador, la bala no hace nada y sigue su camino
    }
}
