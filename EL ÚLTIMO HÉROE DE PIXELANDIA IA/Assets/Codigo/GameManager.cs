using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public static GameManager Instance { get; private set; }
    public HUD hud;
    public int PuntosTotales { get; private set; }
 
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("Más de un Game Manager en escena!");
        }
    }

    public void SumarPuntos(int puntosAsumar)
    {
        PuntosTotales += puntosAsumar;
        hud.ActualizarPuntos(PuntosTotales);
    }
}
