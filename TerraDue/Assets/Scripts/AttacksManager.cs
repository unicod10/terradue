﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AttacksManager : NetworkBehaviour {

    public GameObject humanAttackPrefab;
    public GameObject alienAttackPrefab;
    public GameObject monsterAttackPrefab;
    public GameObject humanAbilityPrefab;
    public GameObject alienAbilityPrefab;

    public void Attack(GameObject attacker, GameObject target, float damage)
    {
        GameObject instance;
        if (attacker.tag == "Human")
        {
            instance = Instantiate(humanAttackPrefab, target.transform);
        }
        else if(attacker.tag == "Alien")
        {
            instance = Instantiate(alienAttackPrefab, target.transform);
        }
        else if(attacker.tag == "Monster")
        {
            instance = Instantiate(monsterAttackPrefab, target.transform);
        }
        else
        {
            // TODO prefab
            instance = Instantiate(monsterAttackPrefab, target.transform);
        }
        NetworkServer.Spawn(instance);
        float exp = target.GetComponent<LifeBehaviour>().TakeDamage(damage);
        if (attacker.GetComponent<PlayerBehaviour>() != null && exp > 0)
        {
            attacker.GetComponent<PlayerBehaviour>().AddExperience(exp);
        }
        StartCoroutine(AsyncDestroy(instance));
    }

    private IEnumerator AsyncDestroy(GameObject particles)
    {
        yield return new WaitForSeconds(Constants.ATTACK_PARTICLES_DURATION);
        NetworkServer.Destroy(particles);
    }

    public void CastAbility(GameObject attacker, GameObject target, float damage)
    {
        GameObject instance;
        if (attacker.tag == "Human")
        {
            instance = Instantiate(humanAbilityPrefab, target.transform);
        }
        else
        {
            instance = Instantiate(alienAbilityPrefab, target.transform);
        }
        NetworkServer.Spawn(instance);
        StartCoroutine(AsyncTakeDamage(attacker, target, instance, damage));
    }

    private IEnumerator AsyncTakeDamage(GameObject attacker, GameObject target, GameObject particles, float damage)
    {
        // Withdraw life after particle system
        yield return new WaitForSeconds(Constants.ABILITY_PARTICLES_DURATION);
        float exp = target.GetComponent<LifeBehaviour>().TakeDamage(Constants.HERO_ABILITY_BASE_DAMAGE);
        if(attacker.GetComponent<PlayerBehaviour>() != null && exp > 0)
        {
            attacker.GetComponent<PlayerBehaviour>().AddExperience(exp);
        }
        NetworkServer.Destroy(particles);
    }
}
