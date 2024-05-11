using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource playerJumpSource;
    public List<AudioClip> playerJumpSoundClips;
    public AudioSource playerDashSource;
    public List<AudioClip> playerDashSoundClips;
    public AudioSource playerDeathSource;
    public List<AudioClip> playerDeathSoundClips;
    public AudioSource playerDamageSource;
    public List<AudioClip> playerDamageSoundClips;
    public AudioSource playerAttackSource;
    public List<AudioClip> playerAttackSoundClips;
    public AudioSource enemyDeathSource;
    public List<AudioClip> enemyDeathSoundClips;
    public AudioSource enemyAttackSource;
    public List<AudioClip> enemyAttackSoundClips;
    public void PlayPlayerJumpSoundClip()
    {
        if (playerJumpSoundClips.Count > 0)
        {
            int index = Random.Range(0, playerJumpSoundClips.Count);

            playerJumpSource.clip = playerJumpSoundClips[index];

            playerJumpSource.Play();
        }
    }
    public void PlayPlayerDashSoundClip()
    {
        if (playerDashSoundClips.Count > 0)
        {
            int index = Random.Range(0, playerDashSoundClips.Count);

            playerDashSource.clip = playerDashSoundClips[index];

            playerDashSource.Play();
        }
    }
    public void PlayPlayerDeathSoundClip()
    {
        if (playerDeathSoundClips.Count > 0)
        {
            int index = Random.Range(0, playerDeathSoundClips.Count);

            playerDeathSource.clip = playerDeathSoundClips[index];

            playerDeathSource.Play();
        }
    }
    public void PlayPlayerDamageSoundClip()
    {
        if (playerDamageSoundClips.Count > 0)
        {
            int index = Random.Range(0, playerDamageSoundClips.Count);

            playerDamageSource.clip = playerDamageSoundClips[index];

            playerDamageSource.Play();
        }
    }
    public void PlayPlayerAttackSoundClip()
    {
        if (playerAttackSoundClips.Count > 0)
        {
            int index = Random.Range(0, playerAttackSoundClips.Count);

            playerAttackSource.clip = playerAttackSoundClips[index];

            playerAttackSource.Play();
        }
    }
    public void PlayEnemyDeathSoundClip()
    {
        if (enemyDeathSoundClips.Count > 0)
        {
            int index = Random.Range(0, enemyDeathSoundClips.Count);

            enemyDeathSource.clip = enemyDeathSoundClips[index];

            enemyDeathSource.Play();
        }
    }
    public void PlayEnemyAttackSoundClip()
    {
        if (enemyAttackSoundClips.Count > 0)
        {
            int index = Random.Range(0, enemyAttackSoundClips.Count);

            enemyAttackSource.clip = enemyAttackSoundClips[index];

            enemyAttackSource.Play();
        }
    }
}
