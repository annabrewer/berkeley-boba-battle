using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum State
    {
        ALIVE, DEAD
    }

    public State playerState = State.ALIVE;

    public int maxHealth;

    private GameObject monster;

    private int health;
    private Monster monsterScript;

    void Start()
    {
        health = maxHealth;
        monsterScript = monster.GetComponent<Monster>();
    }

    public void Hurt(int damage)
    {
        Debug.Log("Debug: Player hurt");
        if (playerState == State.ALIVE)
        {
            health -= damage;
            if (health <= 0)
            {
                Die();
            }
        }
    }

    void Die()
    {
        Debug.Log("Debug: Player died");
        monsterScript.monsterState = Monster.State.VICTORY;
        playerState = State.DEAD;
    }
}
