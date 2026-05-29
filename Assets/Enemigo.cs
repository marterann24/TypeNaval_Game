using UnityEngine;
using TMPro;

public class Enemigo : MonoBehaviour
{
    [Header("Palabra")]
    public string palabra;
    public TMP_Text texto;

    [Header("Movimiento")]
    public float velocidad = 2f;
    public float amplitud = 0.15f;
    public float frecuencia = 2f;

    [Header("Recompensas")]
    public int coinsAlMorir = 5;

    private SpriteRenderer sr;
    private Animator anim;
    private Vector3 posicionInicial;

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
        posicionInicial = transform.position;

        if (anim != null)
            anim.SetBool(isMoving, true);
    }

    void Update()
    {
        if (muriendo) return;

        transform.position += Vector3.left * velocidad * Time.deltaTime;

        float movimientoY = Mathf.Sin(Time.time * frecuencia) * amplitud;

        transform.position = new Vector3(
            transform.position.x,
            posicionInicial.y + movimientoY,
            transform.position.z
        );
    }

    public void SetPalabra(string nuevaPalabra)
    {
        palabra = nuevaPalabra;

        if (texto != null)
            texto.text = palabra;
    }

    public void SetActivo(bool activo)
    {
        if (sr == null) return;

        sr.color = activo ? Color.yellow : Color.white;
    }

    public void Destruir()
    {
        if (muriendo) return;

        muriendo = true;

        if (sr != null)
            sr.color = Color.white;

        if (anim != null)
        {
            anim.SetBool(isMoving, false);
            anim.SetBool(isDead, true);
        }

        if (GameManager.instancia != null)
            GameManager.instancia.AgregarCoins(coinsAlMorir);
    }

    public void OnDestroyComplete()
    {
        Destroy(gameObject);
    }

    public void ActualizarTexto(string input)
    {
        if (texto == null) return;
        if (input.Length > palabra.Length) return;

        string resaltado =
            "<color=green>" +
            input +
            "</color>" +
            palabra.Substring(input.Length);

        texto.text = resaltado;
    }

    public void ResetTexto()
    {
        if (texto != null)
            texto.text = palabra;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (GameManager.instancia != null)
                GameManager.instancia.Perder();
        }
    }
}