using UnityEngine;

public class SonidosPersonajes : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip sonidoSalto;
    public AudioClip sonidoRecibirDaño;
    public AudioClip sonidoAtacar;
    public AudioClip sonidoCaida;
    public AudioClip sonidoMuerte;
    public AudioClip sonidoMov1;
    public AudioClip sonidoMov2;

    public void playSaltar()
    {
        audioSource.PlayOneShot(sonidoSalto);
    }

    public void playRecibirDaño()
    {
        audioSource.PlayOneShot(sonidoRecibirDaño);
    }

    public void playAtacar()
    {
        audioSource.PlayOneShot(sonidoAtacar);
    }

    public void playCaida()
    {
        audioSource.PlayOneShot(sonidoCaida);
    }

    public void playMuerte()
    {
        audioSource.PlayOneShot(sonidoMuerte);
    }

    public void playMov1()
    {
        audioSource.PlayOneShot(sonidoMov1);
    }

    public void playMov2()
    {
        audioSource.PlayOneShot(sonidoMov2);
    }

}

