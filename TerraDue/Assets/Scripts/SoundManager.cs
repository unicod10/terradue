using UnityEngine;

public class SoundManager : MonoBehaviour {

    public AudioClip humanAttack;
    public AudioClip alienAttack;
    public AudioClip takeDamage;
    public AudioClip levelUp;
    public AudioClip spawn;
    public AudioClip castAbility;
    public AudioClip buildTower;
    public AudioClip enemyDeath;

	public void PlayHumanAttack()
    {
        Play(humanAttack);
    }

    public void PlayAlienAttack()
    {
        Play(alienAttack);
    }

    public void PlayTakeDamage()
    {
        Play(takeDamage);
    }

    public void PlayLevelUp()
    {
        Play(levelUp);
    }

    public void PlaySpawn()
    {
        Play(spawn);
    }

    public void PlayCastAbility()
    {
        Play(castAbility);
    }

    public void PlayBuildTower()
    {
        Play(buildTower);
    }

    public void PlayEnemyDeath()
    {
        Play(enemyDeath);
    }

    private void Play(AudioClip clip)
    {
        GetComponent<AudioSource>().PlayOneShot(clip);
    }
}
