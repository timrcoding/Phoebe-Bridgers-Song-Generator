using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DropDown : MonoBehaviour
{
    private SongChoreographer songChoreographer;
    private TMP_Dropdown m_Dropdown;
    public void OnValueChanged() => SongChoreographer.instance.WriteToSongSection();

    private void Awake()
    {
        m_Dropdown = GetComponent<TMP_Dropdown>(); 
    }
    private void Start()
    {
        SongChoreographer.instance.SongNoSections += MakeNonInteractable;
        SongChoreographer.instance.SongPlaying += MakeNonInteractable;
        SongChoreographer.instance.SongStopped += MakeInteractable;
    }

    void MakeNonInteractable() => m_Dropdown.interactable = false;
    void MakeInteractable() => m_Dropdown.interactable = true;
}
