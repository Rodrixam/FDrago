using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyController>().MakeAttack();
        }
    }
}
