using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SectionToken : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI SectionTokenText;
    [SerializeField] private int SongIndexPosition;
    [SerializeField] private Button DestroyButton;
    [SerializeField] private Button CopyButton;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        SongChoreographer.instance.SongNoSections += MakeInteractable;
        SongChoreographer.instance.SongStopped += MakeInteractable;
        SongChoreographer.instance.SongPlaying += MakeNonInteractable;
        SongChoreographer.instance.SwitchIndexesElement += updateIndex;
    }
    
    public void MoveSectionRight()
    {
        SongChoreographer.instance.moveSectionRight(SongIndexPosition);
    }

    public void MoveSectionLeft()
    {
        SongChoreographer.instance.moveSectionLeft(SongIndexPosition);
    }

    public void updateIndex()
    {
        List<SongSection> lst = SongChoreographer.instance.m_SongSections;
        for (int i = 0; i < lst.Count; i++)
        {
            if(lst[i].SectionToken == gameObject)
            {
                SongIndexPosition = i;
                break;
            }
        }
    }

    public void SetVariables(SongSectionName songSectionName, int songIndexPosition)
    {
        SectionTokenText.text = songSectionName.ToString();
        SongIndexPosition = songIndexPosition;
    }

    void MakeNonInteractable() => button.interactable = false;

    void MakeInteractable() => button.interactable = true;

    public void SetSongSectionCounter()
    {
        SongChoreographer.instance.SetSongSectionCounter(SongIndexPosition);
    }

    public void SelectButton()
    {
        SongState songState = SongChoreographer.instance.m_SongState;
        if (songState == SongState.STOPPED || songState == SongState.NOSECTIONS)
        {
            button.Select();
            button.onClick.Invoke();
        }
        
    }
    public void RemoveFromSongSectionList()
    {
        SongChoreographer.instance.RemoveFromSongSectionList(SongIndexPosition);
    }
}
