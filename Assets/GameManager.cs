using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instancia;

    public static int coins = 0;
    public TMP_Text textoCoins;

    public GameObject panelGameOver;
    public GameObject panelVictoria;

    private bool juegoTerminado = false;
    private bool nivelCompleto = false;
    public bool JuegoTerminado => juegoTerminado || nivelCompleto;

    void Awake()
    {
        instancia = this;
        Time.timeScale = 1f;
        juegoTerminado = false;
        nivelCompleto = false;

        if (textoCoins == null)
            textoCoins = BuscarTextoCoins();

        if (panelGameOver == null)
            panelGameOver = CrearPanelGameOver();

        if (panelVictoria == null)
            panelVictoria = CrearPanelVictoria();

        if (panelGameOver != null)
        {
            ConfigurarBotonesGameOver(panelGameOver);
            panelGameOver.SetActive(false);
        }

        if (panelVictoria != null)
            panelVictoria.SetActive(false);

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

    public void NivelCompletado()
    {
        if (nivelCompleto || juegoTerminado) return;
        nivelCompleto = true;

        Debug.Log("Nivel " + (TypingManager.nivelActual + 1) + " completado!");

        if (panelVictoria != null)
            panelVictoria.SetActive(true);
    }

    public void SiguienteNivel()
    {
        TypingManager.nivelActual++;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Reintentar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void IrMenu()
    {
        Time.timeScale = 1f;
        TypingManager.nivelActual = 0;
        coins=0;
        SceneManager.LoadScene("MainMenu");
    }

    public void IrAlMenu()
    {
        IrMenu();
    }

    private void ActualizarTextoCoins()
    {
        if (textoCoins != null)
            textoCoins.text = "Coins: " + coins;
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

        CrearBoton(panel.transform, "BtnReintentar", "Reintentar", new Vector2(-115f, -95f), Reintentar);
        CrearBoton(panel.transform, "BtnMenu", "Menu", new Vector2(115f, -95f), IrMenu);

        return panel;
    }

    private GameObject CrearPanelVictoria()
    {
        Canvas canvas = FindAnyObjectByType<Canvas>();
        if (canvas == null) return null;

        bool esUltimoNivel = TypingManager.nivelActual >= 3;
        string titulo = esUltimoNivel ? "¡VICTORIA!" : "¡NIVEL COMPLETADO!";

        GameObject panel = CrearPanelBase(canvas, "PanelVictoria", titulo);

        if (esUltimoNivel)
        {
            GameObject coinsObj = new GameObject("TextoCoins", typeof(RectTransform), typeof(TextMeshProUGUI));
            coinsObj.transform.SetParent(panel.transform, false);
            RectTransform coinsRect = coinsObj.GetComponent<RectTransform>();
            coinsRect.anchorMin = new Vector2(0.5f, 0.5f);
            coinsRect.anchorMax = new Vector2(0.5f, 0.5f);
            coinsRect.sizeDelta = new Vector2(400f, 60f);
            coinsRect.anchoredPosition = new Vector2(0f, -40f);

            TextMeshProUGUI coinsTexto = coinsObj.GetComponent<TextMeshProUGUI>();
            coinsTexto.text = "Total Coins: " + coins;
            coinsTexto.fontSize = 32f;
            coinsTexto.alignment = TextAlignmentOptions.Center;
            coinsTexto.color = new Color(1f, 0.85f, 0.2f);
            CrearBoton(panel.transform, "BtnMenu", "Menu", new Vector2(0f, -95f), IrMenu);
        }
        else
        {
            CrearBoton(panel.transform, "BtnSiguiente", "Siguiente", new Vector2(-115f, -95f), SiguienteNivel);
            CrearBoton(panel.transform, "BtnMenu", "Menu", new Vector2(115f, -95f), IrMenu);
        }

        return panel;
    }

    private GameObject CrearPanelBase(Canvas canvas, string nombre, string titulo)
    {
        GameObject panel = new GameObject(nombre, typeof(RectTransform), typeof(Image));
        panel.transform.SetParent(canvas.transform, false);

        RectTransform rect = panel.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        Image imagen = panel.GetComponent<Image>();
        imagen.color = new Color(0f, 0f, 0f, 0.65f);

        GameObject textoObj = new GameObject("Titulo", typeof(RectTransform), typeof(TextMeshProUGUI));
        textoObj.transform.SetParent(panel.transform, false);

        RectTransform textoRect = textoObj.GetComponent<RectTransform>();
        textoRect.anchorMin = new Vector2(0.5f, 0.5f);
        textoRect.anchorMax = new Vector2(0.5f, 0.5f);
        textoRect.sizeDelta = new Vector2(500f, 120f);
        textoRect.anchoredPosition = Vector2.zero;

        TextMeshProUGUI texto = textoObj.GetComponent<TextMeshProUGUI>();
        texto.text = titulo;
        texto.fontSize = 54f;
        texto.alignment = TextAlignmentOptions.Center;
        texto.color = Color.white;

        return panel;
    }

    private void ConfigurarBotonesGameOver(GameObject panel)
    {
        Button[] botones = panel.GetComponentsInChildren<Button>(true);
        bool tieneReintentar = false;
        bool tieneMenu = false;

        foreach (Button boton in botones)
        {
            if (boton.name.Contains("Reintentar"))
            {
                boton.onClick.RemoveListener(Reintentar);
                boton.onClick.AddListener(Reintentar);
                tieneReintentar = true;
            }

            if (boton.name.Contains("Menu") || boton.name.Contains("Menú"))
            {
                boton.onClick.RemoveListener(IrMenu);
                boton.onClick.AddListener(IrMenu);
                tieneMenu = true;
            }
        }

        if (!tieneReintentar)
            CrearBoton(panel.transform, "BtnReintentar", "Reintentar", new Vector2(-115f, -95f), Reintentar);

        if (!tieneMenu)
            CrearBoton(panel.transform, "BtnMenu", "Menu", new Vector2(115f, -95f), IrMenu);
    }

    private Button CrearBoton(Transform parent, string nombre, string etiqueta, Vector2 posicion, UnityEngine.Events.UnityAction accion)
    {
        GameObject botonObj = new GameObject(nombre, typeof(RectTransform), typeof(Image), typeof(Button));
        botonObj.transform.SetParent(parent, false);

        RectTransform rect = botonObj.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = new Vector2(190f, 52f);
        rect.anchoredPosition = posicion;

        Image imagen = botonObj.GetComponent<Image>();
        imagen.color = new Color(1f, 1f, 1f, 0.92f);

        Button boton = botonObj.GetComponent<Button>();
        boton.onClick.AddListener(accion);

        GameObject textoObj = new GameObject("Texto", typeof(RectTransform), typeof(TextMeshProUGUI));
        textoObj.transform.SetParent(botonObj.transform, false);

        RectTransform textoRect = textoObj.GetComponent<RectTransform>();
        textoRect.anchorMin = Vector2.zero;
        textoRect.anchorMax = Vector2.one;
        textoRect.offsetMin = Vector2.zero;
        textoRect.offsetMax = Vector2.zero;

        TextMeshProUGUI texto = textoObj.GetComponent<TextMeshProUGUI>();
        texto.text = etiqueta;
        texto.fontSize = 24f;
        texto.alignment = TextAlignmentOptions.Center;
        texto.color = Color.black;

        return boton;
    }

    public void RestarCoins(int cantidad)
    {
        if (juegoTerminado) return;
        coins = Mathf.Max(0, coins - cantidad);
        ActualizarTextoCoins();
    }
}
