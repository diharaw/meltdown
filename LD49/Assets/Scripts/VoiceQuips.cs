using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceQuips : MonoBehaviour
{
    public static VoiceQuips sharedInstance;

    public AudioSource m_voiceAudioSource;
    public AudioClip[] m_reactorHealthClips;
    public AudioClip[] m_reactorHealthCriticalClips;
    public AudioClip[] m_playerHealthClips;
    public AudioClip[] m_playerLevelUpClips;
    public AudioClip[] m_waveCompleteClips;

    void Awake()
    {
        sharedInstance = this;
    }

    public void PlayReactorHealthClip(float hitPoints)
    {
        if (!m_voiceAudioSource.isPlaying)
        {
            AudioClip clip = null;

            if (hitPoints > 0.7f && hitPoints <= 0.75f)
                clip = m_reactorHealthClips[0];
            else if (hitPoints > 0.45f && hitPoints <= 0.5f)
                clip = m_reactorHealthClips[1];
            else if (hitPoints > 0.2f && hitPoints <= 0.25f)
                clip = m_reactorHealthClips[2];
            else if (hitPoints < 0.2f)
            {
                int index = Random.Range(0, m_reactorHealthCriticalClips.Length - 1);
                clip = m_reactorHealthCriticalClips[index];
            }

            if (clip != null)
            {
                m_voiceAudioSource.clip = clip;
                m_voiceAudioSource.Play();
            }
        }
    }

    public void PlayPlayerHealthClip()
    {
        float probability = Random.Range(0.0f, 1.0f);

        if (!m_voiceAudioSource.isPlaying && probability > 0.5f)
        {
            int index = Random.Range(0, m_playerHealthClips.Length - 1);
            m_voiceAudioSource.clip = m_playerHealthClips[index];
            m_voiceAudioSource.Play();
        }
    }

    public void PlayLevelUpClip()
    {
        if (!m_voiceAudioSource.isPlaying)
        {
            int index = Random.Range(0, m_playerLevelUpClips.Length - 1);
            m_voiceAudioSource.clip = m_playerLevelUpClips[index];
            m_voiceAudioSource.Play();
        }
    }

    public void PlayWaveCompleteClip()
    {
        if (!m_voiceAudioSource.isPlaying)
        {
            int index = Random.Range(0, m_waveCompleteClips.Length - 1);
            m_voiceAudioSource.clip = m_waveCompleteClips[index];
            m_voiceAudioSource.Play();
        }
    }
}
