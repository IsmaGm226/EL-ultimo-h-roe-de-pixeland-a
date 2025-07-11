using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Threading;


public class HUD : MonoBehaviour
{
    public TextMeshProUGUI puntos;
    void Update()
    {
        puntos.text = GameManager.Instance.PuntosTotales.ToString();
    }

    public void ActualizarPuntos(int puntosTotales)
    {
        puntos.text = puntosTotales.ToString();

    }
   
}

