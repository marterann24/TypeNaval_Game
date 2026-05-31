using UnityEngine;

public class TypingManager : MonoBehaviour
{
    public static int nivelActual = 0;
    private string input = "";
    public GameObject enemyPrefab;
    

    private string[][] niveles = new string[][]
    {
        new string[] { "mar", "sol", "ola", "red", "pez" },
        new string[] { "barco", "playa", "viento", "ancla", "costa", "marea", "norte" },
        new string[] { "oceano", "brujula", "capitan", "velero", "pirata", "tesoro", "sirena", "bandera", "tormenta" },
        new string[] { "navegante", "horizonte", "tripulante", "carabela", "explorar",
               "artillero", "brujula", "marineros", "pirateria", "vigilante"}
    };

    private float[] velocidades = { 0.8f, 1.2f, 1.6f, 2.0f };
    private float[] tiemposSpawn = { 2.5f, 2.0f, 1.5f, 1.2f };

    public float spawnX = 8f;
    public float minY = -3f;
    public float maxY = 3f;

    private float tiempoSpawn;
    private float velocidadNivel;
    private float timer;

    private string[] palabrasNivel;
    private int siguientePalabra = 0;
    private int palabrasDestruidas = 0;
    private int totalPalabras;
    private Enemigo enemigoActual;

    void Start()
    {
        if (nivelActual >= niveles.Length)
            nivelActual = niveles.Length - 1;

        palabrasNivel = ShuffleArray(niveles[nivelActual]);
        totalPalabras = palabrasNivel.Length;
        velocidadNivel = velocidades[nivelActual];
        tiempoSpawn = tiemposSpawn[nivelActual];

        Debug.Log("Nivel " + (nivelActual + 1) + " - " + totalPalabras + " palabras");
    }

    void Update()
    {
        if (GameManager.instancia != null && GameManager.instancia.JuegoTerminado)
            return;

        timer += Time.deltaTime;

        if (timer >= tiempoSpawn && siguientePalabra < totalPalabras)
        {
            SpawnEnemy();
            timer = 0f;
        }

        foreach (char c in Input.inputString)
        {
            string nuevoInput = input + c;

            if (enemigoActual == null)
            {
                Enemigo[] enemigos = FindObjectsByType<Enemigo>(FindObjectsSortMode.None);
                foreach (Enemigo enemigo in enemigos)
                {
                    if (enemigo.palabra.StartsWith(nuevoInput) && !enemigo.muriendo)
                    {
                        enemigoActual = enemigo;
                        enemigoActual.SetActivo(true);
                        break;
                    }
                }
            }

            if (enemigoActual != null)
            {
                if (enemigoActual.muriendo)
                {
                    enemigoActual = null;
                    input = "";
                    continue;
                }

                if (enemigoActual.palabra.StartsWith(nuevoInput))
                {
                    input = nuevoInput;
                    enemigoActual.ActualizarTexto(input);

                    if (enemigoActual.palabra == input)
                    {
                        enemigoActual.Destruir();
                        enemigoActual = null;
                        input = "";
                        palabrasDestruidas++;

                        if (GameManager.instancia != null)
                            GameManager.instancia.AgregarCoins(10);

                        if (palabrasDestruidas >= totalPalabras)
                        {
                            if (GameManager.instancia != null)
                                GameManager.instancia.NivelCompletado();
                        }
                    }
                }
                else
                {
                    enemigoActual.SetActivo(false);
                    enemigoActual.ResetTexto();
                    enemigoActual = null;
                    input = "";
                    if (GameManager.instancia != null)
        GameManager.instancia.RestarCoins(5);
                }
            }
            else
            {
                input = "";
                if (GameManager.instancia != null)
                    GameManager.instancia.RestarCoins(5); 
            }
        }
    }
    void SpawnEnemy()
    {
        if (enemyPrefab == null || siguientePalabra >= totalPalabras) return;

        float y = Random.Range(minY, maxY);
        GameObject enemigoObj = Instantiate(enemyPrefab, new Vector2(spawnX, y), Quaternion.identity);
        Enemigo enemigo = enemigoObj.GetComponent<Enemigo>();

        if (enemigo == null) return;

        enemigo.SetPalabra(palabrasNivel[siguientePalabra]);
        enemigo.velocidad = velocidadNivel;
        siguientePalabra++;
    }

    string[] ShuffleArray(string[] original)
    {
        string[] arr = (string[])original.Clone();
        for (int i = arr.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            string temp = arr[i];
            arr[i] = arr[j];
            arr[j] = temp;
        }
        return arr;
    }

}
