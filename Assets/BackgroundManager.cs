using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    [SerializeField] private Sprite[] nivel1Frames;
    [SerializeField] private Sprite[] nivel2Frames;
    [SerializeField] private Sprite[] nivel3Frames;
    [SerializeField] private Sprite[] nivel4Frames;
    [SerializeField] private float framesPorSegundo = 6f;

    private SpriteRenderer spriteRenderer;
    private Sprite[] framesActuales;
    private float tiempoAnimacion;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        Animator animator = GetComponent<Animator>();
        if (animator != null)
            animator.enabled = false;

        Sprite[][] fondosPorNivel =
        {
            nivel1Frames,
            nivel2Frames,
            nivel3Frames,
            nivel4Frames
        };

        int nivel = Mathf.Clamp(TypingManager.nivelActual, 0, fondosPorNivel.Length - 1);
        framesActuales = fondosPorNivel[nivel];

        if (spriteRenderer != null && framesActuales != null && framesActuales.Length > 0)
            spriteRenderer.sprite = framesActuales[0];
    }

    void Update()
    {
        if (spriteRenderer == null || framesActuales == null || framesActuales.Length == 0)
            return;

        tiempoAnimacion += Time.deltaTime;
        int frame = Mathf.FloorToInt(tiempoAnimacion * framesPorSegundo) % framesActuales.Length;
        spriteRenderer.sprite = framesActuales[frame];
    }
}
