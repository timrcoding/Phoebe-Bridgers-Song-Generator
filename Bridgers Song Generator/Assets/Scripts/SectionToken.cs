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
        if (button != null)
        {
            SongChoreographer.instance.moveSectionRight(SongIndexPosition);
        }
    }

    public void MoveSectionLeft()
    {
        if (button != null)
        {
            SongChoreographer.instance.moveSectionLeft(SongIndexPosition);
        }
    }

    public void updateIndex()
    {
        List<SongSection> lst = SongChoreographer.instance.m_SongSections;
        for (int i = 0; i < lst.Count; i++)
        {
            if(lst[i].SectionToken == gameObject && lst[i].SectionToken != null)
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

    void MakeNonInteractable()
    {
        if (button != null)
        {
            button.interactable = false;
        }
    }

    void MakeInteractable()
    {
        if (button != null)
        {
            button.interactable = true;
        }
    }

    public void SetSongSectionCounter()
    {
        SongChoreographer.instance.SetSongSectionCounter(SongIndexPosition);
    }

    public void SelectButton()
    {
        if (button != null)
        {
            SongState songState = SongChoreographer.instance.m_SongState;
            if (songState == SongState.STOPPED || songState == SongState.NOSECTIONS)
            {
                button.Select();
                button.onClick.Invoke();
            }
        }
        
    }
    public void RemoveFromSongSectionList()
    {
        SongChoreographer.instance.RemoveFromSongSectionList(SongIndexPosition);
        MakeNonInteractable();
        transform.SetParent(null);
    }
}
