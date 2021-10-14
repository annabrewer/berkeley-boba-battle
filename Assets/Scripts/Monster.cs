using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour {

    public enum State {
        ALIVE, DYING, SINKING, ATTACKING, VICTORY
    }

    public State monsterState = State.ALIVE;
    private State previousMonsterState = State.ALIVE;

    public GameObject player;
    public GameObject scriptManager;
    public float attackRange;
    public int attackDamage;
    public float attackTime;

    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private AudioSource audioSource;

    public AudioClip spawnClip;
    public AudioClip hitClip;
    public AudioClip dieClip;

    public int maxHealth;
    private int currHealth;

    public float sinkSpeed;

    public GameObject walking;
    public GameObject attack1;
    public GameObject attack2;
    public GameObject dancing;

    private bool startedAttacking;

    private float timer;

	// Use this for initialization
	void Start () {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(spawnClip);
        currHealth = maxHealth;

        SetActiveModel();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 distanceVector = transform.position - player.transform.position;
        distanceVector.y = 0;
        float distance = distanceVector.magnitude;

        if (monsterState == State.ALIVE)
        {
            navMeshAgent.SetDestination(player.transform.position);
            navMeshAgent.isStopped = false;

            if (distance <= attackRange)
            {
                monsterState = State.ATTACKING;
            }
        }
        else if (monsterState == State.ATTACKING)
        {
            navMeshAgent.isStopped = true;
            if (distance >= attackRange + 1)
            {
                monsterState = State.ALIVE;
            }

            timer += Time.deltaTime;
            if (timer > attackTime) {
                timer = 0.0f;
                Attack();
            }
        }
        else if (monsterState == State.SINKING)
        {
            float sinkDistance = sinkSpeed * Time.deltaTime;
            transform.Translate(new Vector3(0, -sinkDistance, 0));
        }

        if (monsterState != previousMonsterState)
        {
            SetActiveModel();
            previousMonsterState = monsterState;
        }
	}

    public void SetActiveModel() {
        Debug.Log("Debug: Monster changed active model");
        Debug.Log(monsterState);
        walking.SetActive(monsterState == State.ALIVE || monsterState == State.DYING || monsterState == State.SINKING);
        attack1.SetActive(monsterState == State.ATTACKING);
        attack2.SetActive(false);
        dancing.SetActive(monsterState == State.VICTORY);
    }

    public void Attack() {
        //audioSource.PlayOneShot(hitClip);
        scriptManager.GetComponent<Player>().Hurt(attackDamage);
    }

    public void Hurt(int damage) {
        Debug.Log("Debug: Monster hurt");
        if (monsterState == State.ALIVE || monsterState == State.ATTACKING) {
            currHealth -= damage;
            if (currHealth <= 0)
                Die();
        }
    }

    void Die() {
        Debug.Log("Debug: Monster died");
        monsterState = State.DYING;
        audioSource.PlayOneShot(dieClip);
        navMeshAgent.isStopped = true;
        StartSinking();
    }

    public void StartSinking() {
        monsterState = State.SINKING;
        navMeshAgent.enabled = false;
        Destroy(gameObject, 5);
    }
}