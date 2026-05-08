using UnityEngine;

public class TypingManager : MonoBehaviour
{
    private string input = "";
    public GameObject enemyPrefab;
    

    public string[] palabras = {
        "barco", "mar", "ola", "viento", "nave"
    };

    public float spawnX = 8f;
    public float minY = -3f;
    public float maxY = 3f;

    public float tiempoSpawn = 2f;
    private float timer;

    private Enemigo enemigoActual;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= tiempoSpawn)
        {
            SpawnEnemy();
            timer = 0f;
        }

        foreach (char c in Input.inputString)
        {
            string nuevoInput = input + c;

            Debug.Log("Intento: " + nuevoInput);

            if (enemigoActual == null)
            {
                Enemigo[] enemigos = FindObjectsByType<Enemigo>(FindObjectsSortMode.None);

                foreach (Enemigo enemigo in enemigos)
                {
                    if (enemigo.palabra.StartsWith(nuevoInput))
                    {
                        enemigoActual = enemigo;
                        enemigoActual.SetActivo(true);
                        break;
                    }
                }
            }
            
            if (enemigoActual != null)
            {
                if (enemigoActual.palabra.StartsWith(nuevoInput))
                {
                    input = nuevoInput;
                    enemigoActual.ActualizarTexto(input);
                    Debug.Log("Input: " + input);
                    if (enemigoActual.palabra == input)
                    {
                        enemigoActual.Destruir();
                        enemigoActual = null;
                        input = "";
                    }
                }
                else
                {
                    Debug.Log("Error");

                    enemigoActual.SetActivo(false);
                    enemigoActual.ResetTexto();
                    enemigoActual = null;
                    input = "";
                }
            }
            else
            {
                Debug.Log("Error");
                input="";
            }
        }
    }
    void SpawnEnemy()
    {
        float y = Random.Range(minY, maxY);

        GameObject enemigoObj = Instantiate(enemyPrefab, new Vector2(spawnX, y), Quaternion.identity);

        Enemigo enemigo = enemigoObj.GetComponent<Enemigo>();

        string palabraRandom = palabras[Random.Range(0, palabras.Length)];

        enemigo.SetPalabra(palabraRandom);
    }
}