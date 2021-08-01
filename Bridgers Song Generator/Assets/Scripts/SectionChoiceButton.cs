using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SectionChoiceButton : MonoBehaviour
{
    private SongChoreographer songChoreographer;
    private Button button;
    [SerializeField] private SongSectionName m_SongSection;

    private void Start()
    {
        button = GetComponent<Button>();
        songChoreographer = FindObjectOfType<SongChoreographer>();
        //BIND EVENTS
        songChoreographer.PlayerNoSections += MakeInteractable;
        songChoreographer.PlayerStopped += MakeInteractable;
        songChoreographer.PlayerPlaying += MakeNonInteractable;
    }
    public void AddSongSection()
    {
        songChoreographer.AddSongSection(m_SongSection);
    }

    void MakeNonInteractable() => button.interactable = false;

    void MakeInteractable() => button.interactable = true;
}
