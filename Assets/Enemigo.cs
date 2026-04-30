using UnityEngine;
using TMPro;

public class Enemigo : MonoBehaviour
{
    public string palabra;
    public TMP_Text texto;

    public float velocidad = 1f;

    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        transform.Translate(Vector2.left * velocidad * Time.deltaTime);
    }

    public void SetPalabra(string nuevaPalabra)
    {
        palabra = nuevaPalabra;
        texto.text = palabra;
    }

    public void SetActivo(bool activo)
    {
        if (activo)
            sr.color = Color.yellow;
        else
            sr.color = Color.white;  
    }

    public void Destruir()
    {
        Destroy(gameObject);
    }
}