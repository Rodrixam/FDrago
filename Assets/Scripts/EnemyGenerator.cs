using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    /// <summary>
    /// The prefab of the enemy to be created.
    /// </summary>
    public GameObject basicEnemyPrefab;
    public GameObject flyingEnemyPrefab;

    /// <summary>
    /// Possible positions where an enemy could spawn.
    /// </summary>
    [SerializeField] List<Vector3> spawnPositions = new List<Vector3>();

    /// <summary>
    /// Wether it is generating enemies or not.
    /// </summary>
    [SerializeField] bool isGenerationActive = false;

    /// <summary>
    /// Factor for elements that raise difficulty.
    /// </summary>
    [SerializeField] int difficultyLevel = 0;

    /// <summary>
    /// List of the active enemyControllers on the scene, this list gets updated through the GenerateEnemy()
    /// and RemoveEnemyFromList() functions.
    /// </summary>
    [SerializeField] List<EnemyController> activeEnemyList = new List<EnemyController>();

    public bool LPTextActivated = false;

    float timeCounter = 0;
    float timeLimit = 6;

    int enemyKillCount = 0;
    int boostKillCount = 0;

    FlameController fc;

    private void Awake()
    {
        fc = GetComponent<FlameController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        ResetGeneration();
    }

    // Update is called once per frame
    void Update()
    {
        if (isGenerationActive)
        {
            //Update timeCounter
            timeCounter += Time.deltaTime;

            //If timeCounter is greater or equal than the time limit formula, which decreases with the difficultyLevel
            if (timeCounter >= timeLimit)
            {
                GenerateEnemy();
            }
        }
    }

    public void GenerateEnemy()
    {
        //Pick either normal or flying eemy
        bool flying = Random.value > 0.9f;

        //Pick random position to spawn
        int randChoice = Random.Range(0, spawnPositions.Count);

        //Instantiate enemy object
        GameObject newEnemy;
        if (flying)
        {
            newEnemy = Instantiate(flyingEnemyPrefab, spawnPositions[randChoice], Quaternion.identity);
            newEnemy.GetComponent<UniversalEnemyDesigner>().GenerateRandomFace();
            newEnemy.GetComponent<UniversalEnemyDesigner>().SetStyle(difficultyLevel);
        }
        else
        {
            newEnemy = Instantiate(basicEnemyPrefab, spawnPositions[randChoice], Quaternion.identity);
            newEnemy.GetComponent<UniversalEnemyDesigner>().GenerateRandomFace();
            newEnemy.GetComponent<UniversalEnemyDesigner>().SetStyle(difficultyLevel);
        }

        //Get and save EnemyController component
        EnemyController enemyCont;
        if (flying)
        {
            enemyCont = newEnemy.GetComponent<FlyingEnemyController>();
        }
        else
        {
            enemyCont = newEnemy.GetComponent<EnemyController>();
        }

        activeEnemyList.Add(enemyCont);

        //Enable visual debug
        if (LPTextActivated)
        {
            enemyCont.SetShowLifePoints(true);
        }

        //Set speed
        enemyCont.speed = 0.5f + difficultyLevel / 10f;

        //Set lifePoints
        enemyCont.lifePoints = 1 + difficultyLevel / 5f;

        //Set generator reference in EnemyController
        enemyCont.generator = this;

        //Reset timeCounter
        timeCounter = 0;

        Debug.Log("Enemigo generado");
    }

    public void RemoveEnemyFromList(int index)
    {
        //Remove enemyController from the list
        activeEnemyList.RemoveAt(index);

        //Update indexInsideTriggerColliders to all the next enemyControllers in the list
        for (int i = index; i < activeEnemyList.Count; i++)
        {
            activeEnemyList[i].GetComponent<EnemyController>().ReduceColliderIndex();
        }
    }

    public void GenerateNextEnemy()
    {
        timeCounter = timeLimit;
    }

    public void ActivateGeneration()
    {
        isGenerationActive = true;
    }

    public void DeactivateGeneration()
    {
        isGenerationActive = false;
    }

    public void ResetGeneration()
    {
        timeCounter = 0;
        enemyKillCount = 0;
        boostKillCount = 0;
        difficultyLevel = 0;

        RecalculatetimeLimit();
    }

    void RecalculatetimeLimit()
    {
        timeLimit = 18 / (float)(difficultyLevel + 3);
        if(timeLimit < 1) { timeLimit = 1; }
    }

    public void AddEnemyKillCount()
    {
        enemyKillCount++;
        if (enemyKillCount >= 2 * difficultyLevel + 1)
        {
            difficultyLevel++;
            enemyKillCount = 0;
            RecalculatetimeLimit();
        }

        if (!fc.IsFlameBoosted())
        {
            boostKillCount++;
            if (boostKillCount >= fc.GetKillsForBoost())
            {
                fc.ActivateBoost();
                boostKillCount = 0;
            }
        }
    }

    public int GetDifficultyLevel()
    {
        return difficultyLevel;
    }

    public int GetEnemiesKilled()
    {
        return enemyKillCount;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        foreach (Vector3 pos in spawnPositions)
        {
            Gizmos.DrawSphere(pos, 0.1f);
        }
    }

    public List<EnemyController> GetEnemyList()
    {
        return activeEnemyList;
    }

    public bool IsGenerationActive()
    {
        return isGenerationActive;
    }
}
