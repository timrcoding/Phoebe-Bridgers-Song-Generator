using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimingController : Controller
{ 
    [SerializeField] private AudioClip BPM;
    [SerializeField] private AudioClip FourBarBPM;
    public int BeatCounter, BarCount;
    private bool clockStarted;
    public int GetBarCount { get { return BarCount; } }

    public event Action OnSongBarBegin;
    public event Action OnSectionBegin;
    public event Action IncrementSection;

    [SerializeField] private ChordType CurrentChord;
    [SerializeField] private AudioSource UnityAudioSource;
    [SerializeField] private Slider m_SliderOne;
    public ChordType g_CurrentChord { get { return CurrentChord; } }

    //PLAYHEAD
    [SerializeField] private GameObject Playhead;
    [SerializeField] private GameObject PlayheadParentOne;
    [SerializeField] private GameObject PlayheadParentTwo;
    

    private void Awake()
    {
        m_SliderOne.maxValue = BPM.length * 4;
    }
    public void StartClock()
    {
        if (SongChoreographer.instance.m_PlayerState == PlayerState.STOPPED)
        {
            StartCoroutine(ClockCoroutine());
            OnSectionBegin();
            SongChoreographer.instance.IncrementSectionCounter();
            SongChoreographer.instance.SetPlayerState(PlayerState.PLAYING);
        }
    }

    private void Update()
    {
        m_SliderOne.value = UnityAudioSource.time;
    }

    public void StopClock()
    {
        if (SongChoreographer.instance.m_PlayerState == PlayerState.PLAYING)
        {
            StopAllCoroutines();
            BarCount = 0;
            BeatCounter = 0;
            SongChoreographer.instance.SetPlayerState(PlayerState.STOPPED);
            UnityAudioSource.Stop();
            clockStarted = false;
        }
    }
    IEnumerator ClockCoroutine()
    {
        if (!clockStarted)
        {
            clockStarted = !clockStarted;
            //TODO Better Encapsulation
            CurrentChord = SongChoreographer.instance.m_SongSections[SectionCounter].m_SongChords[BarCount];
            OnSongBarBegin();
            if (!UnityAudioSource.isPlaying)
            {
                UnityAudioSource.Play();
                if (BarCount < 4)
                {
                    Playhead.transform.SetParent(PlayheadParentOne.transform);
                }
                else
                {
                    Playhead.transform.SetParent(PlayheadParentTwo.transform);
                }
            }
            yield return new WaitForSeconds(BPM.length);
            BarCount++;
            if(BarCount == 4)
            {
                UnityAudioSource.Stop();
            }

            if(BarCount >= 8)
            {
                BarCount = 0;
                SectionCounter++;
                OnSectionBegin();
               // IncrementSection();
            }
            
            clockStarted = !clockStarted;
            StartCoroutine(ClockCoroutine());
        }
    }
}
