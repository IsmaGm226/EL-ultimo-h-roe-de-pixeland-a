using UnityEngine;

public class Moneda : MonoBehaviour
{

    public int valor = 1;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica si el objeto que colisiona es el jugador
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.SumarPuntos(valor);
            // Destruye la moneda al recogerla
            Destroy(gameObject);
        }
    }
}
