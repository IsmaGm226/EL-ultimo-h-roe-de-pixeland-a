using UnityEngine;
using TMPro;
using System.Collections;

public class BMO : MonoBehaviour
{
    [SerializeField] private GameObject signo;
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text texto;
    [SerializeField, TextArea(4, 6)] private string[] lineasdeDialogo;

    private float tiempodeMecanografia = 0.05f;
    
    private bool jugadorEnRango;
    private bool comenzarDialogo;
    private int lineaEnPantalla;

    // Update is called once per frame
    void Update()
    {
        if (jugadorEnRango && Input.GetKeyDown(KeyCode.X))
        {
            if (!comenzarDialogo)
            {
                ComenzarDialogo();
            }
            else if (lineaEnPantalla < lineasdeDialogo.Length && texto.text == lineasdeDialogo[lineaEnPantalla])
            {
                SiguienteDialogo();
            }
            else
            {
                StopAllCoroutines();
                texto.text = lineasdeDialogo[lineaEnPantalla];
            
            }

            
        }
        
    }

    private void SiguienteDialogo() 
    {
        lineaEnPantalla++;
        if(lineaEnPantalla < lineasdeDialogo.Length)
        {
            StartCoroutine(MostrarLinea());
        }
        else
        {
            comenzarDialogo = true;
            panel.SetActive(false);
            signo.SetActive(true);
            Time.timeScale = 1;
            Destroy(gameObject); 
        }
    }

    private void ComenzarDialogo() 
    {
        comenzarDialogo = true;
        panel.SetActive(true);
        signo.SetActive(false);
        lineaEnPantalla = 0;
        Time.timeScale =  0f;
        StartCoroutine(MostrarLinea());

    }

    private IEnumerator MostrarLinea() 
    {
        texto.text = string.Empty;

        foreach (char ch in lineasdeDialogo[lineaEnPantalla])
        {
            texto.text += ch;
            yield return new WaitForSecondsRealtime(tiempodeMecanografia);
        }
    } 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            jugadorEnRango = true;
            signo.SetActive(true);
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            jugadorEnRango = false;
            signo.SetActive(false);
        }
    }
}
