using UnityEngine;

public class Trampapic : MonoBehaviour
{
    [SerializeField] private Transform[] puntosMovimientos;
    [SerializeField] private float velocidadMovimiento;
    private int sigPlataforma = 0; // Empieza en el primer punto
    private int direccion = 1; // 1 para avanzar, -1 para retroceder
    private bool jugadorvivo;

    private void Update()
    {
        // Mueve la plataforma hacia el punto objetivo actual
        transform.position = Vector2.MoveTowards(transform.position, puntosMovimientos[sigPlataforma].position, velocidadMovimiento * Time.deltaTime);

        // Comprueba si la plataforma ha llegado al punto objetivo actual
        if (Vector2.Distance(transform.position, puntosMovimientos[sigPlataforma].position) < 0.1f)
        {
            // Si avanza y llega al último punto, cambia de dirección
            if (direccion == 1 && sigPlataforma >= puntosMovimientos.Length - 1)
            {
                direccion = -1;
            }
            // Si retrocede y llega al primer punto, cambia de dirección
            else if (direccion == -1 && sigPlataforma <= 0)
            {
                direccion = 1;
            }

            // Pasa al siguiente punto en la dirección actual
            sigPlataforma += direccion;
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Vector2 direccionDanio = new Vector2(transform.position.x, 0);
            MovimientoJugador jugador = other.gameObject.GetComponent<MovimientoJugador>();

            if (jugador != null) // Asegúrate de que el componente exista
            {
                jugador.RecibeDanio(direccionDanio, 1);
                jugadorvivo = !jugador.muerto;
            }
        }
    }
}
