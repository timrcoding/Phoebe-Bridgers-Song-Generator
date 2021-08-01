using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SectionChoiceButton : MonoBehaviour
{
   

    private Vector2 OriginalPosition;
    private Vector2 TargetPosition;
    private SongChoreographer songChoreographer;
    private Button button;
    [SerializeField] SectionSelection m_SectionSelection;
    [SerializeField] private SongSectionName m_SongSection;

    private void Start()
    {
        OriginalPosition = transform.localPosition;
        TargetPosition = m_SectionSelection.gameObject.transform.localPosition;
        button = GetComponent<Button>();
        songChoreographer = FindObjectOfType<SongChoreographer>();
        //BIND EVENTS
        songChoreographer.SongNoSections += MakeInteractable;
        songChoreographer.SongStopped += MakeInteractable;
        songChoreographer.SongPlaying += MakeNonInteractable;
        m_SectionSelection.Hover += RevealButton;
    }

    private void Update()
    {
        transform.localPosition = Vector2.Lerp(transform.localPosition, TargetPosition, Time.deltaTime * 10);
    }

    void RevealButton()
    {
        TargetPosition = OriginalPosition;
        StartCoroutine(ConcealButton());
    }

    IEnumerator ConcealButton()
    {
        yield return new WaitForSeconds(5);
        TargetPosition = m_SectionSelection.gameObject.transform.localPosition;
    }
    public void AddSongSection()
    {
        songChoreographer.AddSongSection(m_SongSection);
    }

    void MakeNonInteractable() => button.interactable = false;

    void MakeInteractable() => button.interactable = true;
}
