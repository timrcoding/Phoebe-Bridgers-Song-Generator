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
    }
    public void SetVariables(SongSectionName songSectionName, int songIndexPosition)
    {
        SectionTokenText.text = songSectionName.ToString();
        SongIndexPosition = songIndexPosition;
    }

    public void SetSongSectionCounter()
    {
        SongChoreographer.instance.SetSongSectionCounter(SongIndexPosition);
    }

    public void SelectButton()
    {
        button.Select();
        button.onClick.Invoke();
    }

    public void RemoveFromSongSectionList()
    {
        SongChoreographer.instance.RemoveFromSongSectionList(SongIndexPosition);
    }
}
