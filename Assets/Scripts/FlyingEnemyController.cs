using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class FlyingEnemyController : EnemyController
{
    [Header("Movement")]
    [SerializeField] float height;
    [SerializeField] bool rising;
    [SerializeField] bool falling;

    [Header("Refereces")]
    [SerializeField] BalloonsController balloons;


    private void Awake()
    {
        //Get Rigidbody
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        //Start movement booleans
        rising = true;
        falling = false;
        vanishing = false;
        attacking = false;


        //Get direction to player (0,0,0) in the x and z axis
        direction = new Vector3(-transform.position.x, 0, -transform.position.z);
        direction.Normalize();

        //Look to the player
        transform.rotation = Quaternion.LookRotation(direction, transform.up);
        toRotation = transform.rotation;

        //Add collider to flame trigger system
        GameObject[] collisionParticlesObj = GameObject.FindGameObjectsWithTag("CollisionParticles");
        triggerModules = new ParticleSystem.TriggerModule[collisionParticlesObj.Length];
        for(int i = 0; i < collisionParticlesObj.Length; i++)
        {
            triggerModules[i] = collisionParticlesObj[i].GetComponent<ParticleSystem>().trigger;
            triggerModules[i].AddCollider(GetComponent<Collider>());

            indexInsideTriggerColliders = triggerModules[i].colliderCount - 1;
        }
    }

    void Update()
    {
        if (!dead)
        {
            FaceToPlayer();
            if (!attacking)
            {
                MoveToPlayer();
            }
            else if(transform.position.y <= 1)
            {
                DoGameOver();
                attacking = false;
            }
        }
        else if(vanishing)
        {
            Vanish();
        }

        VisualDebug();
    }

    void FaceToPlayer()
    {
        if (transform.rotation == toRotation) { return; }

        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, lerpTimeCount * lerpSpeed);
        lerpTimeCount = lerpTimeCount + Time.deltaTime;

        if (transform.rotation == toRotation) { lerpTimeCount = 0; }
    }

    void MoveToPlayer()
    {
        if (rising && transform.position.y >= height)
        {
            rising = false;
        }

        if (rising)
        {
            rb.velocity = speed * Vector3.up;
        }
        else if(!falling)
        {
            rb.velocity = speed * direction;
        }
    }

    void VisualDebug()
    {
        if (showLifePoints)
        {
            lifePointsText.text = "" + lifePoints;
        }
    }

    override public void SetInsideFlames()
    {
        if (!falling)
        {
            falling = true;
            lifePoints = 0;
            balloons.Pop();
            KillEnemy();
        }

    }

    void KillEnemy()
    {
        //Set physic values
        rb.velocity = Vector3.zero;
        rb.useGravity = true;

        //Traverse all triggerModules
        for (int i = 0; i < triggerModules.Length; i++)
        {
            //Remove collider from the triggerModule
            triggerModules[i].RemoveCollider(indexInsideTriggerColliders);
        }

        //Increase the enemy kill count
        generator.AddEnemyKillCount();

        //Erase enemy from list in generator
        generator.RemoveEnemyFromList(indexInsideTriggerColliders);

        //Call startvanishing in some time
        Invoke("StartVanish", 2f);

        //Change animation
        animator.SetBool("Dead", true);

        dead = true;
    }

    override public void MakeAttack()
    {
        Debug.Log("ME CAIGO");
        balloons.Pop();

        //Change animation
        animator.SetBool("Attack", true);

        //Set physic values
        rb.velocity = Vector3.zero;
        rb.useGravity = true;

        //Set movement values
        falling = true;
        attacking = true;

        Debug.Log(rb.useGravity + " " + falling + " " + attacking);
    }

    new void StartVanish()
    {
        vanishing = true;

        //Deactivate collider and make kinematic
        GetComponent<Collider>().enabled = false;
        rb.isKinematic = true;
    }

    new void Vanish()
    {
        Debug.Log("Vanishing");
        transform.position += Vector3.down * 0.1f * Time.deltaTime;
        if (transform.position.y <= -1f)
        {
            gameObject.SetActive(false);
        }
    }//*/
}
