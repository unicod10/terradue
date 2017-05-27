using System.Collections;
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
        target.GetComponent<LifeBehaviour>().TakeDamage(damage);
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
        target.GetComponent<LifeBehaviour>().TakeDamage(Constants.ABILITY_DAMAGE);
        NetworkServer.Destroy(particles);
    }
}
