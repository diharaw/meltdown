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
            if (hitPoints < 0.2f)
            {
                int index = Random.Range(0, m_reactorHealthCriticalClips.Length);
                AudioClip clip = m_reactorHealthCriticalClips[index];

                m_voiceAudioSource.clip = clip;
                m_voiceAudioSource.Play();
            }
        }
    }

    public void PlayPlayerHealthClip()
    {
        float probability = Random.Range(0.0f, 1.0f);

        if (!m_voiceAudioSource.isPlaying && probability > 0.7f)
        {
            int index = Random.Range(0, m_playerHealthClips.Length);
            m_voiceAudioSource.clip = m_playerHealthClips[index];
            m_voiceAudioSource.Play();
        }
    }

    public void PlayLevelUpClip()
    {
        if (!m_voiceAudioSource.isPlaying)
            m_voiceAudioSource.Stop();

        if (!m_voiceAudioSource.isPlaying)
        {
            int index = Random.Range(0, m_playerLevelUpClips.Length);
            m_voiceAudioSource.clip = m_playerLevelUpClips[index];
            m_voiceAudioSource.Play();
        }
    }

    public void PlayWaveCompleteClip()
    {
        if (!m_voiceAudioSource.isPlaying)
            m_voiceAudioSource.Stop();

        if (!m_voiceAudioSource.isPlaying)
        {
            int index = Random.Range(0, m_waveCompleteClips.Length);
            m_voiceAudioSource.clip = m_waveCompleteClips[index];
            m_voiceAudioSource.Play();
        }
    }
}
