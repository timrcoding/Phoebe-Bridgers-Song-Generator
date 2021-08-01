using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingController : MonoBehaviour
{ 
    [SerializeField] private AudioClip BPM;
    public int BeatCounter, BarCount, SectionCount;
    private bool clockStarted;
    public int GetSectionCount { get { return SectionCount; } }
    public int GetBarCount { get { return BarCount; } }

    public event Action OnSongBarBegin;
    public event Action OnSectionBegin;

    [SerializeField] private ChordType CurrentChord;
    public ChordType g_CurrentChord { get { return CurrentChord; } }

    private void Awake()
    {
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
            yield return new WaitForSeconds(BPM.length);
            BarCount++;
            if(BarCount >= 8)
            {
                BarCount = 0;
                SectionCount++;
                OnSectionBegin();
            }
            clockStarted = !clockStarted;
            StartCoroutine(ClockCoroutine());
        }
    }
}
