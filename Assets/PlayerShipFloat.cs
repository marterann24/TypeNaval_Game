using UnityEngine;

public class PlayerShipFloat : MonoBehaviour
{
    public float amplitudVertical = 0.12f;
    public float frecuenciaVertical = 1.35f;
    public float inclinacionMaxima = 2.5f;

    private Vector3 posicionInicial;
    private Quaternion rotacionInicial;

    void Awake()
    {
        posicionInicial = transform.localPosition;
        rotacionInicial = transform.localRotation;
    }

    void LateUpdate()
    {
        float fase = Time.time * frecuenciaVertical;
        transform.localPosition = posicionInicial + Vector3.up * (Mathf.Sin(fase) * amplitudVertical);
        transform.localRotation = rotacionInicial * Quaternion.Euler(0f, 0f, Mathf.Sin(fase * 0.85f) * inclinacionMaxima);
    }
}
