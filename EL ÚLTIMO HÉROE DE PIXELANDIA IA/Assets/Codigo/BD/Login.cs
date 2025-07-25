using UnityEngine;
using TMPro;
using System.Collections;

public class Login : MonoBehaviour
{
    public Servidor servidor;
    public TMP_InputField inEmail;
    public TMP_InputField inPassword;
    public GameObject imLoading;
    public void IniciarSesion()
    {
        StartCoroutine(Iniciar());
    }

    IEnumerator Iniciar()
    {
        imLoading.SetActive(true);
        string[] datos = new string[2];
        datos[0] = inEmail.text;
        datos[1] = inPassword.text;

        StartCoroutine(servidor.ConsumirServicio("login", datos));
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => !servidor.ocupado);
        imLoading.SetActive(false);
    }
}
