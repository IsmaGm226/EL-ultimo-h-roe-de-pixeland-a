using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para recargar la escena

public class MuertePorVacio : MonoBehaviour
{
    // Esta función se llama cuando otro Collider 2D entra en este trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Comprueba si el objeto que entró en el trigger tiene la etiqueta "Player"
        if (other.CompareTag("Player"))
        {
            Morir();
        }
    }

    void Morir()
    {
        Debug.Log("¡El personaje ha caído al vacío y ha muerto!");

        // Aquí es donde en el futuro agregarás la lógica del menú de "perdiste".
        // Por ahora, simplemente recargaremos la escena actual.

        // Puedes guardar el índice de la escena actual para recargarla
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);

        // O también puedes recargarla por nombre:
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}