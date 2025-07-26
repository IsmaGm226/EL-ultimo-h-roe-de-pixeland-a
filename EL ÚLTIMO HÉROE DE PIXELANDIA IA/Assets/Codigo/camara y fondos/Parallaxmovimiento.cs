using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxmovimiento : MonoBehaviour
{
    Transform camara;
    Vector3 posicionCamaraAnterior;
    float distancia; 
    GameObject[] fondos;
    Material[] materiales;
    float[] velocidadFondos;

    float fondoMasLejano;

    [Range(0.01f, 1f)]
    public float velocidadParallax;

    void Start()
    {
        camara = Camera.main.transform;
        posicionCamaraAnterior = camara.position;

        int cantidadFondos = transform.childCount;
        materiales = new Material[cantidadFondos];
        velocidadFondos = new float[cantidadFondos];
        fondos = new GameObject[cantidadFondos];

        for (int i = 0; i < cantidadFondos; i++)
        {
            fondos[i] = transform.GetChild(i).gameObject;
            materiales[i] = fondos[i].GetComponent<Renderer>().material;
        }

        CalcularVelocidadFondos(cantidadFondos);
    }

    void CalcularVelocidadFondos(int cantidadFondos)
    {
        for (int i = 0; i < cantidadFondos; i++) 
        {
            if ((fondos[i].transform.position.z - camara.position.z) > fondoMasLejano)
            {
                fondoMasLejano = fondos[i].transform.position.z - camara.position.z;
            }
        }

        for (int i = 0; i < cantidadFondos; i++) 
        {
            velocidadFondos[i] = 1 - (fondos[i].transform.position.z - camara.position.z) / fondoMasLejano;
        }
    }

    private void LateUpdate()
    {
        distancia = camara.position.x - posicionCamaraAnterior.x;
        transform.position = new Vector3(camara.position.x - 1, transform.position.y, 9.92f);

        for (int i = 0; i < fondos.Length; i++)
        {
            float velocidad = velocidadFondos[i] * velocidadParallax;
            materiales[i].SetTextureOffset("_MainTex", new Vector2(distancia, 0) * velocidad);
        }
    }
}