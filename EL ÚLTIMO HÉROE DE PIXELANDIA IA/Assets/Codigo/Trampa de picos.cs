using UnityEngine;

public class Trampas : MonoBehaviour
{
    [SerializeField] private float danio = 1f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            {
            MovimientoJugador jugador = collision.gameObject.GetComponent<MovimientoJugador>();
            if (jugador != null)
            {
                // Aplica daño y rebote hacia arriba
                Vector2 direccionDanio = new Vector2(transform.position.x, 1);
                jugador.RecibeDanio(direccionDanio, (int)danio);
            }
        }
    }
}
