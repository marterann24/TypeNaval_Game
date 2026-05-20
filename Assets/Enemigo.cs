using UnityEngine;
using TMPro;

public class Enemigo : MonoBehaviour
{
    public string palabra;
    public TMP_Text texto;

    public float velocidad = 1f;

    private SpriteRenderer sr;
    private Animator anim;
    public bool muriendo = false;

    static readonly int isMoving = Animator.StringToHash("isMoving");
    static readonly int isDead = Animator.StringToHash("isDead");

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        anim.SetBool(isMoving, true);
    }

    void Update()
    {
        if(muriendo) return;
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
        muriendo = true;
        sr.color = Color.white;
        anim.SetBool(isMoving, false);
        anim.SetBool(isDead, true);
    }

    public void OnDestroyComplete()
    {
        Destroy(gameObject);
    }

    public void ActualizarTexto(string input)
    {
        string resaltado =
            "<color=green>" +
            input +
            "</color>" +
            palabra.Substring(input.Length);

        texto.text = resaltado;
    }

    public void ResetTexto()
    {
        texto.text = palabra;
    }
}