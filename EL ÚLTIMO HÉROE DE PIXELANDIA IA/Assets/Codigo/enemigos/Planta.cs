using UnityEngine;
using System.Collections;

public class DisparoEnemigo : MonoBehaviour
{
    public Transform controladorDisparo;
    public float distanciaLinea;
    public LayerMask capaJugador;
    public bool jugadorEnRango;
    public GameObject bala;

    public float tiempoEntreDisparos; 
    public float tiempoUlimoDisparo; // Tiempo del �ltimo disparo
    public float tiempoEsperaDisparo;
    public bool jugadorvivo;
    public Animator animator;
    
    public int vida = 1;
    private bool muerto;
    private bool recibiendoDanio;
   
    void Update()
    {    
        jugadorEnRango = Physics2D.Raycast(controladorDisparo.position, -transform.right, distanciaLinea, capaJugador);
        if (jugadorEnRango)
        {
            if(Time.time > tiempoEntreDisparos + tiempoUlimoDisparo) 
            { 
                tiempoUlimoDisparo = Time.time; // Actualiza el tiempo del �ltimo disparo
                animator.SetTrigger("Disparar"); // Activa la animaci�n de disparo
                Invoke(nameof(Disparar), tiempoEsperaDisparo); // Llama al m�todo de disparo despu�s de un peque�o retraso
            }

        }
        animator.SetBool("muerto", muerto);
        animator.SetBool("recibeDanio", recibiendoDanio);
    }

    private void Disparar()
    {
        Instantiate(bala, controladorDisparo.position, controladorDisparo.rotation);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 direccionDanio = new Vector2(transform.position.x, 0);
            MovimientoJugador jugador = collision.gameObject.GetComponent<MovimientoJugador>();

            if (jugador != null) // Aseg�rate de que el componente exista
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
        if (!recibiendoDanio && !muerto) // Solo recibe da�o si no est� ya recibi�ndolo o muerto
        {
            vida -= cantDanio;
            recibiendoDanio = true;

            if (vida <= 0)
            {
                muerto = true;
            }
            else
            {
                StartCoroutine(ReiniciarRecibiendoDanio());
            }
        }
    }

    private IEnumerator ReiniciarRecibiendoDanio()
    {
        yield return new WaitForSeconds(0.3f);
        recibiendoDanio = false;
    }
    public void DestruirPlanta()
    {
        if (muerto)
        {
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        // Dibuja una l�nea de depuraci�n para visualizar el rango de disparo
        Gizmos.color = Color.red;
        Gizmos.DrawLine(controladorDisparo.position, controladorDisparo.position + transform.right * distanciaLinea);
    }
}

