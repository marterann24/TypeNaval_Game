using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectNavigation : MonoBehaviour
{
    private const string SceneName = "LevelSelect";
    private const string GameSceneName = "SampleScene";
    private const string MenuSceneName = "MainMenu";

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Inicializar()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
        ConstruirSelector();
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == SceneName)
            ConstruirSelector();
    }

    private static void ConstruirSelector()
    {
        if (SceneManager.GetActiveScene().name != SceneName)
            return;

        Canvas canvas = FindAnyObjectByType<Canvas>();
        if (canvas == null || canvas.transform.Find("SelectorNiveles") != null)
            return;

        Transform fondo = canvas.transform.Find("Fondo");
        foreach (Transform child in canvas.transform)
        {
            if (child != fondo)
                Destroy(child.gameObject);
        }

        GameObject raiz = CrearPanel(canvas.transform, "SelectorNiveles", new Color(0.015f, 0.08f, 0.14f, 0.64f));
        CrearTexto(raiz.transform, "Titulo", "CARTA DE NAVEGACION", new Vector2(0f, 390f), new Vector2(900f, 90f), 45f, new Color(1f, 0.82f, 0.35f));
        CrearTexto(raiz.transform, "Subtitulo", "ELIGE TU RUTA POR LOS CUATRO MARES", new Vector2(0f, 320f), new Vector2(900f, 50f), 20f, new Color(0.82f, 0.94f, 1f));

        GameObject tablero = CrearTarjeta(raiz.transform, "Tablero", new Vector2(0f, 5f), new Vector2(1060f, 540f), new Color(0.02f, 0.16f, 0.25f, 0.94f));
        CrearTexto(tablero.transform, "Encabezado", "RUTAS DISPONIBLES", new Vector2(0f, 205f), new Vector2(700f, 55f), 24f, new Color(1f, 0.82f, 0.35f));

        CrearBotonNivel(tablero.transform, 0, "NIVEL 1", "MAR TRANQUILO", new Vector2(-255f, 75f), new Color(0.08f, 0.46f, 0.58f));
        CrearBotonNivel(tablero.transform, 1, "NIVEL 2", "COSTA VENTOSA", new Vector2(255f, 75f), new Color(0.08f, 0.38f, 0.52f));
        CrearBotonNivel(tablero.transform, 2, "NIVEL 3", "OCEANO PROFUNDO", new Vector2(-255f, -100f), new Color(0.05f, 0.28f, 0.45f));
        CrearBotonNivel(tablero.transform, 3, "NIVEL 4", "HORIZONTE PIRATA", new Vector2(255f, -100f), new Color(0.04f, 0.20f, 0.36f));

        CrearBoton(raiz.transform, "BtnVolver", "VOLVER AL PUERTO", new Vector2(0f, -390f), new Vector2(360f, 66f), new Color(0.38f, 0.18f, 0.08f), IrMenu);
    }

    private static void CrearBotonNivel(Transform parent, int nivel, string titulo, string subtitulo, Vector2 posicion, Color color)
    {
        Button boton = CrearBoton(parent, "BtnNivel" + (nivel + 1), titulo + "\n" + subtitulo, posicion, new Vector2(430f, 130f), color, () => CargarNivel(nivel));
        TextMeshProUGUI texto = boton.GetComponentInChildren<TextMeshProUGUI>();
        texto.fontSize = 23f;
        texto.lineSpacing = 15f;
    }

    private static void CargarNivel(int nivel)
    {
        TypingManager.nivelActual = Mathf.Clamp(nivel, 0, 3);
        Time.timeScale = 1f;
        SceneManager.LoadScene(GameSceneName);
    }

    private static void IrMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(MenuSceneName);
    }

    private static GameObject CrearPanel(Transform parent, string nombre, Color color)
    {
        GameObject panel = new GameObject(nombre, typeof(RectTransform), typeof(Image));
        panel.transform.SetParent(parent, false);
        RectTransform rect = panel.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        panel.GetComponent<Image>().color = color;
        return panel;
    }

    private static GameObject CrearTarjeta(Transform parent, string nombre, Vector2 posicion, Vector2 tamano, Color color)
    {
        GameObject tarjeta = new GameObject(nombre, typeof(RectTransform), typeof(Image), typeof(Outline), typeof(Shadow));
        tarjeta.transform.SetParent(parent, false);
        RectTransform rect = tarjeta.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = posicion;
        rect.sizeDelta = tamano;
        tarjeta.GetComponent<Image>().color = color;
        Outline borde = tarjeta.GetComponent<Outline>();
        borde.effectColor = new Color(0.73f, 0.55f, 0.24f, 1f);
        borde.effectDistance = new Vector2(4f, -4f);
        tarjeta.GetComponent<Shadow>().effectColor = new Color(0f, 0f, 0f, 0.55f);
        return tarjeta;
    }

    private static Button CrearBoton(Transform parent, string nombre, string etiqueta, Vector2 posicion, Vector2 tamano, Color color, UnityEngine.Events.UnityAction accion)
    {
        GameObject botonObj = CrearTarjeta(parent, nombre, posicion, tamano, color);
        Button boton = botonObj.AddComponent<Button>();
        ColorBlock colores = boton.colors;
        colores.normalColor = Color.white;
        colores.highlightedColor = new Color(1.18f, 1.18f, 1.18f);
        colores.pressedColor = new Color(0.72f, 0.82f, 0.9f);
        boton.colors = colores;
        boton.onClick.AddListener(accion);

        CrearTexto(botonObj.transform, "Texto", etiqueta, Vector2.zero, tamano, 22f, Color.white);
        return boton;
    }

    private static TextMeshProUGUI CrearTexto(Transform parent, string nombre, string contenido, Vector2 posicion, Vector2 tamano, float fuente, Color color)
    {
        GameObject textoObj = new GameObject(nombre, typeof(RectTransform), typeof(TextMeshProUGUI));
        textoObj.transform.SetParent(parent, false);
        RectTransform rect = textoObj.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = posicion;
        rect.sizeDelta = tamano;
        TextMeshProUGUI texto = textoObj.GetComponent<TextMeshProUGUI>();
        texto.text = contenido;
        texto.fontSize = fuente;
        texto.alignment = TextAlignmentOptions.Center;
        texto.color = color;
        texto.fontStyle = FontStyles.Bold;
        return texto;
    }
}
