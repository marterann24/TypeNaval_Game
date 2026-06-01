using UnityEngine;
using TMPro;

public class Enemigo : MonoBehaviour
{
    [Header("Palabra")]
    public string palabra;
    public TMP_Text texto;

    [Header("Movimiento")]
    public float velocidad = 2f;
    public Vector2 direccionAvance = Vector2.left;
    public float amplitud = 0.15f;
    public float frecuencia = 2f;
    public float amplitudVaivenVertical = 0.06f;
    public float rotacionMaxima = 3f;

    [Header("Recompensas")]
    public int coinsAlMorir = 5;

    private SpriteRenderer sr;
    private Animator anim;
    private Rigidbody2D rb;
    private Collider2D col;
    private Vector3 posicionBase;
    private float faseMovimiento;
    private float rotacionInicialZ;

    public bool muriendo = false;

    static readonly int isMoving = Animator.StringToHash("isMoving");
    static readonly int isDead = Animator.StringToHash("isDead");

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    void Start()
    {
        posicionBase = transform.position;
        faseMovimiento = Random.Range(0f, Mathf.PI * 2f);
        rotacionInicialZ = transform.eulerAngles.z;

        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.gravityScale = 0f;
        }

        if (col != null)
            col.isTrigger = true;

        if (anim != null)
            anim.SetBool(isMoving, true);
    }

    void Update()
    {
        if (muriendo) return;
        if (GameManager.instancia != null && GameManager.instancia.JuegoTerminado) return;

        Vector3 direccion = new Vector3(direccionAvance.x, direccionAvance.y, 0f);

        if (direccion.sqrMagnitude <= 0.0001f)
            direccion = Vector3.left;

        direccion.Normalize();
        posicionBase += direccion * velocidad * Time.deltaTime;

        Vector3 lateral = new Vector3(-direccion.y, direccion.x, 0f);
        float tiempo = Time.time * frecuencia + faseMovimiento;
        float movimientoLateral = Mathf.Sin(tiempo) * amplitud;
        float vaivenVertical = Mathf.Sin(tiempo * 1.35f) * amplitudVaivenVertical;
        float rotacion = Mathf.Sin(tiempo * 0.9f) * rotacionMaxima;

        transform.position = posicionBase + lateral * movimientoLateral + Vector3.up * vaivenVertical;
        transform.rotation = Quaternion.Euler(0f, 0f, rotacionInicialZ + rotacion);
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

        if (col != null)
            col.enabled = false;

        if (anim != null)
        {
            anim.SetBool(isMoving, false);
            anim.SetBool(isDead, true);
        }
        else
        {
            Destroy(gameObject);
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
