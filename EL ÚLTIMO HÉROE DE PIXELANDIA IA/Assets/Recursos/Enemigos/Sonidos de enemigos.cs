using UnityEngine;

public class Sonidosdeenemigos : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip sonidoEnemigosRecibeDaño;
    public AudioClip sonidoEnemigosMuerte;

    public void playRecibirDaño()
    {
        audioSource.PlayOneShot(sonidoEnemigosRecibeDaño);
    }

    public void playMuerte()
    {
        audioSource.PlayOneShot(sonidoEnemigosMuerte);
    }
}
