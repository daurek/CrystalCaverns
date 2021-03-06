﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : Character
{
    [SerializeField] protected string enemyName;

    /// <summary>
    /// Spawner that spawned this enemy
    /// </summary>
    protected GameObject spawner;
    /// <summary>
    /// Reference to the agent
    /// </summary>    
    protected NavMeshAgent agent;
    protected GameObject player;
    protected GameObject currentTarget;
    protected GameObject shop;
    protected bool defaultTarget;
    protected float visionRange = 10;

    protected bool slowed;
    protected float slowDuration;
    protected float currentSlowValue = 1;

    [SerializeField] protected int value= 1;

    [SerializeField] protected int goldDrop;
    [SerializeField] protected int goldDropPercent;
    [SerializeField] protected GameObject[] lootPrefabs;
    [SerializeField] private GameObject damageTextPrefab;

    void Awake()
    {
        defaultTarget = true;
    }


    public virtual void Update()
    {
        if (slowed)
        {
            slowDuration -= Time.deltaTime;
            if (slowDuration < 0)
            {   

                slowed = false;
                currentSlowValue = 1;
                SetSpeed(baseSpeed);

            }
        }
        
    }

    public string EnemyName
    {
        get { return enemyName; }
        set { enemyName = value; }
    }

    protected override void SetSpeed(float speedValue)
    {
        currentSpeed = speedValue;
        agent.speed = currentSpeed;
    }

    protected override void SetHealth(float healthValue)
    {
        SpawnDamageNumber(healthValue);
        base.SetHealth(healthValue);
    }

    public void SetTarget(GameObject _target)
    {
        currentTarget = _target;
        agent.SetDestination(currentTarget.transform.position);
    }

    public void SetSpawner(GameObject _spawner)
    {
        spawner = _spawner;
    }

    public void SetShop(GameObject _shop)
    {
        shop = _shop;
    }

    protected override void GameOver()
    {
       
        Loot();
        Kill();
       
    }

    public void Kill()
    {
        spawner.GetComponent<Spawn>().KillEnemy();
        Destroy(gameObject);
    }

    public void Slow(float value, float duration)
    {
        if (value <= currentSlowValue )
        {
            
            slowDuration = duration;
            slowed = true;
            currentSlowValue = value;
            SetSpeed(currentSlowValue);
        }
    }


    public void SpawnDamageNumber(float damage)
    {
        GameObject damageText = Instantiate(damageTextPrefab, transform.position + new Vector3(Random.Range(-1f, 1f), transform.lossyScale.y, Random.Range(-1f, 1f)), Quaternion.identity);
        damageText.GetComponent<TextMesh>().text = System.Math.Abs(damage) + "";
        if (damage < 0)
        {
            damageText.GetComponent<TextMesh>().color = Color.red;
            if (damage > -10) damageText.GetComponent<TextMesh>().color = new Color32(239, 127, 26, 255);
        }
        else
        {
            damageText.GetComponent<TextMesh>().color = new Color32(0, 255, 23, 255);
        }

    }

    protected void Loot()
    {
        if (Random.Range(1, 100) <= goldDropPercent)
        {
            GameObject cristal = Instantiate(lootPrefabs[0], transform.position, transform.rotation);
            cristal.GetComponent<Looteable>().Crystal = goldDrop;
        }
    }
}
