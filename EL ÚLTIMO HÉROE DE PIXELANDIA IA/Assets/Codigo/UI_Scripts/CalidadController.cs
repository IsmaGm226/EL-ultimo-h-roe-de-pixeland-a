using UnityEngine;
using TMPro;

public class CalidadController : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public int calidad;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        calidad = PlayerPrefs.GetInt("numeroDeCalidad", 3);
        dropdown.value = calidad;
        AjustarCalidad();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AjustarCalidad()
    {
        QualitySettings.SetQualityLevel(dropdown.value);
        PlayerPrefs.SetInt("numeroDeCalidad", dropdown.value);
        calidad = dropdown.value;   
    }
}
