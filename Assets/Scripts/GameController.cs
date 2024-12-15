using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] GameObject gameOverCanvas;

    [SerializeField] List<GameObject> dragonHeads;

    [SerializeField] EnemyGenerator generator;

    private void Awake()
    {
        gameOverCanvas.SetActive(false);
    }

    public void EndGame()
    {
        GetComponent<Configuration>().StartFadeMusic();

        gameOverCanvas.SetActive(true);

        generator.DeactivateGeneration();

        foreach(GameObject dh in dragonHeads)
        {
            dh.SetActive(false);
        }
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
