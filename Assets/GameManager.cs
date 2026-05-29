using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instancia;

    public int coins = 0;
    public TMP_Text textoCoins;

    public GameObject panelGameOver;

    private bool juegoTerminado = false;
    public bool JuegoTerminado => juegoTerminado;

    void Awake()
    {
        instancia = this;
        Time.timeScale = 1f;
        juegoTerminado = false;

        if (textoCoins == null)
            textoCoins = BuscarTextoCoins();

        if (panelGameOver == null)
            panelGameOver = CrearPanelGameOver();

        if (panelGameOver != null)
            panelGameOver.SetActive(false);

        ActualizarTextoCoins();
    }

    public void AgregarCoins(int cantidad)
    {
        if (juegoTerminado) return;

        coins += cantidad;
        ActualizarTextoCoins();
    }

    public void Perder()
    {
        if (juegoTerminado) return;

        juegoTerminado = true;

        Debug.Log("GAME OVER");

        if (panelGameOver != null)
            panelGameOver.SetActive(true);

        Time.timeScale = 0f;
    }

    public void Reintentar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void IrMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    private void ActualizarTextoCoins()
    {
        if (textoCoins != null)
            textoCoins.text = "Coins: +" + coins;
    }

    private TMP_Text BuscarTextoCoins()
    {
        TMP_Text[] textos = FindObjectsByType<TMP_Text>(FindObjectsSortMode.None);

        foreach (TMP_Text texto in textos)
        {
            if (texto.text.StartsWith("Coins"))
                return texto;
        }

        return null;
    }

    private GameObject CrearPanelGameOver()
    {
        Canvas canvas = FindAnyObjectByType<Canvas>();

        if (canvas == null)
            return null;

        GameObject panel = new GameObject("PanelGameOver", typeof(RectTransform), typeof(Image));
        panel.transform.SetParent(canvas.transform, false);

        RectTransform rect = panel.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        Image imagen = panel.GetComponent<Image>();
        imagen.color = new Color(0f, 0f, 0f, 0.65f);

        GameObject textoObj = new GameObject("TextoGameOver", typeof(RectTransform), typeof(TextMeshProUGUI));
        textoObj.transform.SetParent(panel.transform, false);

        RectTransform textoRect = textoObj.GetComponent<RectTransform>();
        textoRect.anchorMin = new Vector2(0.5f, 0.5f);
        textoRect.anchorMax = new Vector2(0.5f, 0.5f);
        textoRect.sizeDelta = new Vector2(500f, 120f);
        textoRect.anchoredPosition = Vector2.zero;

        TextMeshProUGUI texto = textoObj.GetComponent<TextMeshProUGUI>();
        texto.text = "GAME OVER";
        texto.fontSize = 54f;
        texto.alignment = TextAlignmentOptions.Center;
        texto.color = Color.white;

        return panel;
    }
}
