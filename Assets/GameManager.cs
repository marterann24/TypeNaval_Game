using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instancia;

    public int coins = 0;
    public TMP_Text textoCoins;

    public GameObject panelGameOver;

    private bool juegoTerminado = false;

    void Awake()
    {
        instancia = this;

        if (panelGameOver != null)
            panelGameOver.SetActive(false);
    }

    public void AgregarCoins(int cantidad)
    {
        if (juegoTerminado) return;

        coins += cantidad;
        textoCoins.text = "Coins: " + coins;
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
}