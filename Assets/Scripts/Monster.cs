using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour {

    public enum State {
        ALIVE, DYING, SINKING, ATTACKING, VICTORY
    }

    public State monsterState = State.ALIVE;
    public State previousMonsterState = State.ALIVE;

    public GameObject player;
    public float attackRange;

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
        if (monsterState == State.ALIVE || monsterState == State.ATTACKING) {
            navMeshAgent.SetDestination(player.transform.position);

            Vector3 distanceVector = transform.position - player.transform.position;
            distanceVector.y = 0;
            float distance = distanceVector.magnitude;

            if (distance <= attackRange) {
                monsterState = State.ATTACKING;
                navMeshAgent.isStopped = true;
            } else {
                navMeshAgent.isStopped = false;
            }
        } else if (monsterState == State.SINKING) {
            float sinkDistance = sinkSpeed * Time.deltaTime;
            transform.Translate(new Vector3(0, -sinkDistance, 0));
        }

        if (monsterState != previousMonsterState)
        {
            SetActiveModel();
            monsterState = previousMonsterState;
        }
	}

    public void SetActiveModel() {
        walking.SetActive(monsterState == State.ALIVE || monsterState == State.DYING);
        attack1.SetActive(monsterState == State.ATTACKING);
        attack2.SetActive(false);
        dancing.SetActive(monsterState == State.VICTORY);
    }

    public void Attack() {
        audioSource.PlayOneShot(hitClip);
    }

    public void Hurt(int damage) {
        if (monsterState == State.ALIVE || monsterState == State.ATTACKING) {
            animator.SetTrigger("Hurt");
            currHealth -= damage;
            if (currHealth <= 0)
                Die();
        }
    }

    void Die() {
        monsterState = State.DYING;
        audioSource.PlayOneShot(dieClip);
        navMeshAgent.isStopped = true;
    }

    public void StartSinking() {
        monsterState = State.SINKING;
        navMeshAgent.enabled = false;
        Destroy(gameObject, 5);
    }
}