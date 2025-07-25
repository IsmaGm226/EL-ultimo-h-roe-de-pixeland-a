using UnityEngine;
using UnityEngine.SceneManagement;

public class Cambiar_Escena : MonoBehaviour
{
    public void CambiarEscena(string nombre)
    {
        SceneManager.LoadScene(nombre);
    }

    public void SalirJuego()
    {
        Debug.Log("Saliendo...");
        Application.Quit();
    }
}
