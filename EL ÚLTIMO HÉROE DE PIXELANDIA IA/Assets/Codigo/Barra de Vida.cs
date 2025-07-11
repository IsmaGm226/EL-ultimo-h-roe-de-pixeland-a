using UnityEngine.UI;
using UnityEngine;


public class BarradeVida : MonoBehaviour
{
    public Image rellenoBarraVida;
    private MovimientoJugador movimientoJugador;
    private float vidaMaxima;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movimientoJugador = GameObject.Find("Player").GetComponent<MovimientoJugador>();
        vidaMaxima = movimientoJugador.vida;
    }

    // Update is called once per frame
    void Update()
    {
        ActualizarBarraVida();
    }

    private void ActualizarBarraVida()
    {
        if (rellenoBarraVida != null && movimientoJugador != null && vidaMaxima > 0)
        {
            rellenoBarraVida.fillAmount = Mathf.Clamp01(movimientoJugador.vida / vidaMaxima);
        }
    }
}
