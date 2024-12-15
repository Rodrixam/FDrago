using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] public float speed;
    [SerializeField] protected bool vanishing;
    [SerializeField] protected bool dead;
    protected bool attacking;
    protected Rigidbody rb;

    [Header("Rotation")]
    protected float lerpSpeed = 0.01f;
    protected float lerpTimeCount = 0.0f;
    protected Quaternion toRotation;

    [Header("Life")]
    public float lifePoints = 1;
    public int burnLevel = 0;
    public float burnRiseTime = 1f;
    protected float burnRiseTimeCount = 0;

    [Header("Refereces")]
    public EnemyGenerator generator;
    protected Vector3 direction;
    protected ParticleSystem.TriggerModule[] triggerModules;
    protected int indexInsideTriggerColliders;
    protected Animator animator;
    protected ParticleSystem ps;

    [Header("Debug")]
    public bool showLifePoints = false;
    [SerializeField] protected TextMeshPro lifePointsText;


    private void Awake()
    {
        //Get components
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        ps = GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        //Start movement booleans
        vanishing = false;
        dead = false;
        attacking = false;

        //Get direction to player
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
            BurnInteraction();
            if (!attacking)
            {
                MoveToPlayer();
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
        rb.velocity = speed * direction;
    }

    void BurnInteraction()
    {
        if(burnRiseTimeCount > 0)
        {
            burnRiseTimeCount -= Time.deltaTime;
        }

        if(burnLevel > 0)
        {
            lifePoints -= burnLevel / 3f * Time.deltaTime;
            if(lifePoints <= 0)
            {
                KillEnemy();
            }
        }
    }

    void VisualDebug()
    {
        if (showLifePoints)
        {
            lifePointsText.text = "" + burnLevel + "\n" + lifePoints;
        }
    }

    public void SetShowLifePoints(bool value)
    {
        showLifePoints = value;

        if (showLifePoints)
        {
            lifePointsText.enabled = true;
        }
        else
        {
            lifePointsText.enabled = false;
        }
    }

    virtual public void SetInsideFlames()
    {
        if(burnRiseTimeCount <= 0)
        {
            burnLevel++;
            burnRiseTimeCount = burnRiseTime;
            if (!ps.isPlaying)
            {
                ps.Play();
            }

            ParticleSystem.EmissionModule emission = ps.emission;
            emission.rateOverTime = 10 * burnLevel;
        }
    }

    void KillEnemy()
    {
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

        //Deactivate collider and make kinematic
        GetComponent<Collider>().enabled = false;
        rb.isKinematic = true;

        //Call startvanishing in some time
        Invoke("StartVanish", 1.5f);

        //Change animation
        animator.SetBool("Dead", true);

        ps.Stop();

        dead = true;
    }

    virtual public void MakeAttack()
    { 
        //Change animation
        animator.SetBool("Attack", true);
        rb.velocity = Vector3.zero;
        attacking = true;
    }

    protected void StartVanish()
    {
        vanishing = true;
    }

    protected void Vanish()
    {
        Debug.Log("Vanishing");
        transform.position += Vector3.down * 0.1f * Time.deltaTime;
        if(transform.position.y <= -1f)
        {
            gameObject.SetActive(false);
        }
    }

    public void ReduceColliderIndex()
    {
        indexInsideTriggerColliders--;
    }

    public void DoGameOver()
    {
        generator.gameObject.GetComponent<GameController>().EndGame();
    }
}
