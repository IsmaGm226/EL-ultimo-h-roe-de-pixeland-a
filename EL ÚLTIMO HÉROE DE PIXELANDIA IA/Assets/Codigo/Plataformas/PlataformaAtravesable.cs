using UnityEngine;

public class PlataformaAtravesable : MonoBehaviour
{
    private GameObject player;
    private PolygonCollider2D ccPlayer;
    private BoxCollider2D ccPlata;
    private Bounds ccPlataBounds;
    private float topPlata, piePlayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        // Verifica si se encontró el jugador para evitar errores de referencia nula más adelante
        if (player == null)
        {
            Debug.LogError("¡No se encontró el GameObject 'Player' con la etiqueta 'Player'!", this);
            enabled = false; // Desactiva este script si no se encuentra el jugador
            return;
        }

        ccPlayer = player.GetComponent<PolygonCollider2D>();
        // Verifica si se encontró el PolygonCollider2D en el jugador
        if (ccPlayer == null)
        {
            Debug.LogError("¡El GameObject 'Player' no tiene un PolygonCollider2D adjunto!", player);
            enabled = false; // Desactiva este script si no se encuentra el collider
            return;
        }

        ccPlata = GetComponent<BoxCollider2D>();
        // Verifica si se encontró el BoxCollider2D en esta plataforma
        if (ccPlata == null)
        {
            Debug.LogError("¡Este GameObject PlataformaAtravesable no tiene un BoxCollider2D!", this);
            enabled = false; // Desactiva este script si no se encuentra el collider
            return;
        }

        ccPlataBounds = ccPlata.bounds;
        topPlata = ccPlataBounds.center.y + ccPlataBounds.extents.y;
    }

    // Update is called once per frame
    void Update()
    {
        // Calcula la parte inferior del collider del jugador usando sus 'bounds'
        // ccPlayer.bounds.min.y te da la coordenada Y más baja de la caja delimitadora del collider
        piePlayer = ccPlayer.bounds.min.y;

        if (piePlayer >= topPlata)
        {
            ccPlata.isTrigger = false;
            gameObject.tag = "suelo";
            gameObject.layer = LayerMask.NameToLayer("suelo");
        }

        // Esta condición hace que la plataforma sea atravesable de nuevo cuando el jugador está por debajo
        if (!ccPlata.isTrigger && (piePlayer < topPlata - 0.1f))
        {
            ccPlata.isTrigger = true;
            gameObject.tag = "Untagged"; // Generalmente, es buena práctica usar "Untagged" para el valor predeterminado
            gameObject.layer = LayerMask.NameToLayer("Default"); // Asegúrate de que la capa "Default" exista
        }
    }
}