using UnityEngine;

public class menuPausa : MonoBehaviour
{
    [SerializeField] GameObject menuPausaUI;

    public void PausarJuego()
    {
        menuPausaUI.SetActive(true);
        Time.timeScale = 0;
    }

    public void Reanudar()
    {
        menuPausaUI.SetActive(false);
        Time.timeScale = 1;
    }

}