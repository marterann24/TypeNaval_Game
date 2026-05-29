using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuNavigation : MonoBehaviour
{
    private const string MenuSceneName = "MainMenu";
    private const string GameSceneName = "SampleScene";

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Inicializar()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
        ConectarMenuActual();
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == MenuSceneName)
            ConectarMenuActual();
    }

    private static void ConectarMenuActual()
    {
        Scene escenaActiva = SceneManager.GetActiveScene();

        if (escenaActiva.name != MenuSceneName)
            return;

        Button[] botones = Resources.FindObjectsOfTypeAll<Button>();

        foreach (Button boton in botones)
        {
            if (!boton.gameObject.scene.IsValid() || boton.gameObject.scene != escenaActiva)
                continue;

            if (boton.name == "BtnJugar")
            {
                boton.onClick.RemoveListener(Jugar);
                boton.onClick.AddListener(Jugar);
            }

            if (boton.name == "BtnSalir")
            {
                boton.onClick.RemoveListener(Salir);
                boton.onClick.AddListener(Salir);
            }
        }
    }

    private static void Jugar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(GameSceneName);
    }

    private static void Salir()
    {
        Application.Quit();
    }
}
