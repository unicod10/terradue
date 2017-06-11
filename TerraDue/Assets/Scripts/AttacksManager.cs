using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class AttacksManager : NetworkBehaviour {

    public GameObject humanAttackPrefab;
    public GameObject alienAttackPrefab;
    public GameObject monsterAttackPrefab;
    public GameObject primeAttackPrefab;
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
            instance = Instantiate(primeAttackPrefab, target.transform);
        }
        NetworkServer.Spawn(instance);
        float exp = target.GetComponent<LifeBehaviour>().TakeDamage(damage);
        if (attacker.GetComponent<PlayerBehaviour>() != null && exp > 0)
        {
            attacker.GetComponent<PlayerBehaviour>().AddExperience(exp);
            attacker.GetComponent<PlayerBehaviour>().RpcPlayEnemyDeath();
        }
        StartCoroutine(AsyncDestroyAttack(instance));
    }

    private IEnumerator AsyncDestroyAttack(GameObject particles)
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
        float exp = target.GetComponent<LifeBehaviour>().TakeDamage(damage);
        if(attacker.GetComponent<PlayerBehaviour>() != null && exp > 0)
        {
            attacker.GetComponent<PlayerBehaviour>().AddExperience(exp);
            attacker.GetComponent<PlayerBehaviour>().RpcPlayEnemyDeath();
        }
        StartCoroutine(AsyncDestroyAbility(instance));
    }

    private IEnumerator AsyncDestroyAbility(GameObject particles)
    {
        yield return new WaitForSeconds(Constants.ABILITY_PARTICLES_DURATION);
        NetworkServer.Destroy(particles);
    }
}
