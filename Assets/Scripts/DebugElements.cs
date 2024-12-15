using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugElements : MonoBehaviour
{
    [Header("Flame Breath")]
    [SerializeField] List<ParticleSystem> particleCollision;
    [SerializeField] FlameController flameController;

    [Header("Game Stats")]
    [SerializeField] TextMeshProUGUI statsText;
    [SerializeField] EnemyGenerator enemyGenerator;

    private void Update()
    {
        statsText.text = "Enemies killed: " + enemyGenerator.GetEnemiesKilled() + "\nDifficulty level: " + enemyGenerator.GetDifficultyLevel() + "\nStamina: " + flameController.GetCurrentStamina();
    }

    public void SetParticleCollisionVisibility(bool value)
    {
        foreach (ParticleSystem ps in particleCollision)
        {
            ps.GetComponent<ParticleSystemRenderer>().enabled = value;
        }
    }

    public void SetShowLifePoints(bool value)
    {
        enemyGenerator.LPTextActivated = value;
        foreach (EnemyController ec in enemyGenerator.GetEnemyList())
        {
            ec.SetShowLifePoints(value);
        }
    }
}
