using UnityEngine;
using UnityEngine.Audio;

public class Moneda : MonoBehaviour
{
    public int valor = 1;
    public AudioClip sonidoMoneda;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica si el objeto que colisiona es el jugador
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.SumarPuntos(valor);
            // Destruye la moneda al recogerla
            Destroy(gameObject);
            AudioManager.Instance.ReproducirSonido(sonidoMoneda);
        }


    }

}
