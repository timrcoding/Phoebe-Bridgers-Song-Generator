using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimingController : MonoBehaviour
{ 
    [SerializeField] private AudioClip BPM;
    [SerializeField] private AudioClip FourBarBPM;
    public int BeatCounter, BarCount, SectionCount;
    private bool clockStarted;
    public int GetSectionCount { get { return SectionCount; } }
    public int GetBarCount { get { return BarCount; } }

    public event Action OnSongBarBegin;
    public event Action OnSectionBegin;
    public event Action IncrementSection;

    [SerializeField] private ChordType CurrentChord;
    [SerializeField] private AudioSource UnityAudioSource;
    [SerializeField] private Slider m_SliderOne;
    public ChordType g_CurrentChord { get { return CurrentChord; } }

    private void Awake()
    {
        m_SliderOne.maxValue = BPM.length * 4;
        UnityAudioSource.Play();
    }
    public void StartClock()
    {
        if (SongChoreographer.instance.m_PlayerState == PlayerState.STOPPED)
        {
            StartCoroutine(ClockCoroutine());
            OnSectionBegin();
            SongChoreographer.instance.SetPlayerState(PlayerState.PLAYING);
        }
    }

    private void Update()
    {
        m_SliderOne.value = UnityAudioSource.time;
        Debug.Log(UnityAudioSource.time);
    }

    public void StopClock()
    {
        if (SongChoreographer.instance.m_PlayerState == PlayerState.PLAYING)
        {
            StopAllCoroutines();
            BarCount = 0;
            BeatCounter = 0;
            SongChoreographer.instance.SetPlayerState(PlayerState.STOPPED);
            clockStarted = false;
        }
    }
    IEnumerator ClockCoroutine()
    {
        if (!clockStarted)
        {
            clockStarted = !clockStarted;
            //TODO Better Encapsulation
            CurrentChord = SongChoreographer.instance.m_SongSections[SectionCount].m_SongChords[BarCount];
            OnSongBarBegin();
            if (!UnityAudioSource.isPlaying)
            {
                UnityAudioSource.Play();
            }
            yield return new WaitForSeconds(BPM.length);
            BarCount++;
            if(BarCount >= 4)
            {
                UnityAudioSource.Stop();
            }

            if(BarCount >= 8)
            {
                BarCount = 0;
                SectionCount++;
                OnSectionBegin();
                IncrementSection();
            }
            
            clockStarted = !clockStarted;
            StartCoroutine(ClockCoroutine());
        }
    }
}
