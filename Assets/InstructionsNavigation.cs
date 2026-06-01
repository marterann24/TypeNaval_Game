using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InstructionsNavigation : MonoBehaviour
{
    private const string SceneName = "Instructions";
    private const string MenuSceneName = "MainMenu";

    private const string TextoInstrucciones =
        "Escribe correctamente las palabras que aparecen sobre los barcos enemigos antes de que lleguen a tu barco.\n\n" +
        "Cada palabra completada destruye un enemigo y suma coins. Si un enemigo alcanza tu barco, pierdes el nivel.\n\n" +
        "Completa todas las palabras del nivel para avanzar al siguiente. Practica tu velocidad y precision como en un juego de mecanografia.";

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Inicializar()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
        ConstruirPantalla();
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == SceneName)
            ConstruirPantalla();
    }

    private static void ConstruirPantalla()
    {
        if (SceneManager.GetActiveScene().name != SceneName)
            return;

        Canvas canvas = FindAnyObjectByType<Canvas>();
        if (canvas == null || canvas.transform.Find("PantallaInstrucciones") != null)
            return;

        Transform fondo = canvas.transform.Find("Fondo");
        foreach (Transform child in canvas.transform)
        {
            if (child != fondo)
                Destroy(child.gameObject);
        }

        GameObject raiz = CrearPanel(canvas.transform, "PantallaInstrucciones", new Color(0.015f, 0.08f, 0.14f, 0.68f));
        CrearTexto(raiz.transform, "Titulo", "INSTRUCCIONES", new Vector2(0f, 390f), new Vector2(900f, 90f), 46f, new Color(1f, 0.82f, 0.35f), TextAlignmentOptions.Center);
        CrearTexto(raiz.transform, "Subtitulo", "MANUAL DEL CAPITAN", new Vector2(0f, 325f), new Vector2(800f, 45f), 21f, new Color(0.82f, 0.94f, 1f), TextAlignmentOptions.Center);

        GameObject tablero = CrearTarjeta(raiz.transform, "Bitacora", new Vector2(0f, 20f), new Vector2(1120f, 540f), new Color(0.02f, 0.16f, 0.25f, 0.96f));
        CrearTexto(tablero.transform, "Encabezado", "COMO NAVEGAR", new Vector2(0f, 205f), new Vector2(850f, 55f), 25f, new Color(1f, 0.82f, 0.35f), TextAlignmentOptions.Center);
        TextMeshProUGUI cuerpo = CrearTexto(tablero.transform, "TextoInstrucciones", TextoInstrucciones, new Vector2(0f, -20f), new Vector2(950f, 350f), 24f, new Color(0.92f, 0.98f, 1f), TextAlignmentOptions.TopLeft);
        cuerpo.lineSpacing = 12f;

        CrearBoton(raiz.transform, "BtnVolver", "VOLVER AL PUERTO", new Vector2(0f, -390f), new Vector2(360f, 66f), new Color(0.38f, 0.18f, 0.08f), IrMenu);
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
        CrearTexto(botonObj.transform, "Texto", etiqueta, Vector2.zero, tamano, 22f, Color.white, TextAlignmentOptions.Center);
        return boton;
    }

    private static TextMeshProUGUI CrearTexto(Transform parent, string nombre, string contenido, Vector2 posicion, Vector2 tamano, float fuente, Color color, TextAlignmentOptions alineacion)
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
        texto.alignment = alineacion;
        texto.color = color;
        texto.fontStyle = FontStyles.Bold;
        return texto;
    }
}
