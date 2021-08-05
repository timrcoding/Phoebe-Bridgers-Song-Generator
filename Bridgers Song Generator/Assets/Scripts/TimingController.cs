using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimingController : Controller
{
    public static TimingController instance;

    //BAR REFERENCE
    [SerializeField] private AudioClip BPM;
    [SerializeField] private AudioClip FourBarBPM;
    //TIMING REFERENCES
    public int BeatCounter, BarCount;
    private bool clockStarted;
    //ACTIONS TRIGGERING ON STATE CHANGES
    public event Action OnSongBarBegin;
    public event Action OnSongStart;
    public event Action OnNewSectionBegin;
    public event Action IncrementSection;
    public event Action TriggerVocal;

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
        instance = this;
        m_SliderOne.maxValue = BPM.length * 4;
    }
    public void StartClock()
    {
        if (SongChoreographer.instance.m_SongState == SongState.STOPPED)
        { 
            StartCoroutine(ClockCoroutine());
            OnSongStart();
            SongChoreographer.instance.SetSongState(SongState.PLAYING);
        }
    }
    private void Update()
    {
        m_SliderOne.value = UnityAudioSource.time;
    }

    public void StopClock()
    {
        if (SongChoreographer.instance.m_SongState == SongState.PLAYING)
        {
            StopAllCoroutines();
            BarCount = 0;
            BeatCounter = 0;
            SongChoreographer.instance.SetSongState(SongState.STOPPED);
            UnityAudioSource.Stop();
            clockStarted = false;
        }
        else
        {
            SongChoreographer.instance.SetSongSectionCounter(0);
        }
    }
    IEnumerator ClockCoroutine()
    {
        if (!clockStarted)
        {
            clockStarted = !clockStarted;
            CurrentChord = SongChoreographer.instance.m_SongSections[SongChoreographer.instance.SectionCounter].m_SongChords[BarCount];
            OnSongBarBegin();
            TriggerVocal();
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
                OnNewSectionBegin();
            }
            
            clockStarted = !clockStarted;
            StartCoroutine(ClockCoroutine());
        }
    }
}
