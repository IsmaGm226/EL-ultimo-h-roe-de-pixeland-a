using UnityEngine;

public class Sonidosdeenemigos : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip sonidoEnemigosRecibeDa�o;
    public AudioClip sonidoEnemigosMuerte;

    public void playRecibirDa�o()
    {
        audioSource.PlayOneShot(sonidoEnemigosRecibeDa�o);
    }

    public void playMuerte()
    {
        audioSource.PlayOneShot(sonidoEnemigosMuerte);
    }
}
