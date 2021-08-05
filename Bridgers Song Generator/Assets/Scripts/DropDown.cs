using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DropDown : MonoBehaviour
{
    private TMP_Dropdown m_Dropdown;
    [SerializeField] private Image DropDownImage;

    public void OnValueChanged()
    {
        SongChoreographer.instance.WriteToSongSection();
        SetColor();
    }

    private void Awake()
    {
        m_Dropdown = GetComponent<TMP_Dropdown>();
    }
    private void Start()
    {
        SongChoreographer.instance.SongNoSections += MakeNonInteractable;
        SongChoreographer.instance.SongPlaying += MakeNonInteractable;
        SongChoreographer.instance.SongStopped += MakeInteractable;;
    }

    public void SetColor()
    {
        Debug.Log("COLOR SET");
        DropDownImage.color = MiscResources.instance.ChordColors[m_Dropdown.value];
    }

    void MakeNonInteractable() => m_Dropdown.interactable = false;
    void MakeInteractable() => m_Dropdown.interactable = true;
}
