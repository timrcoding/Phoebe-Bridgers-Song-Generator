using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum ChordType
{
    INVALID,
    ChordOne,
    ChordTwo,
    ChordThree,
    ChordFour,
    ChordFive,
    ChordSix
}

public enum SongSectionName
{
    INVALID,
    INTRO,
    VERSE,
    BRIDGE,
    CHORUS,
    OUTRO
}

public enum PlayerState
{
    INVALID,
    NOSECTIONS,
    STOPPED,
    PLAYING
}

public class SongChoreographer : MonoBehaviour
{
    public static SongChoreographer instance;
    public TimingController m_TimingController;

    public List<SongSection> m_SongSections;
    public PlayerState m_PlayerState;

    //PLAY STATE CONTROLLER
    public event Action PlayerNoSections;
    public event Action PlayerStopped;
    public event Action PlayerPlaying;


    //SONG SEQUENCE AREA
    [SerializeField] private List<Transform> m_SongSectionPositionalNodes;
    public Transform m_SectionTokenParent;
    public GameObject m_SectionTokenPrefab;

    //SECTION SELECTED
    [SerializeField] private int m_SongSectionCounter;
    [SerializeField] private TextMeshProUGUI m_SongSectionCounterTMP;
    [SerializeField] private List<TMP_Dropdown> ChordDropdowns;
    private bool CanWriteChords = true;

    //INSTRUMENT CONTROLLER;
    [SerializeField] private List<Instrument> Instruments;
    [SerializeField] private InstrumentDictionary so_InstrumentDictionary;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SetPlayerState(PlayerState.NOSECTIONS);
    }

    public void SetPlayerState(PlayerState playerState)
    {
        m_PlayerState = playerState;

        switch (playerState)
        {
            case PlayerState.NOSECTIONS:
                PlayerNoSections();
                break;

            case PlayerState.STOPPED:
                PlayerStopped();
                break;

            case PlayerState.PLAYING:
                PlayerPlaying();
                break;
        }
    }

    private void Update()
    {
        UpdateAllTokenPositions();
    }

    void UpdateAllTokenPositions()
    {
        for (int i = 0; i < m_SongSections.Count; i++)
        {
            m_SongSections[i].UpdateTokenPosition(m_SongSectionPositionalNodes[i]);
        }
    }

    public void AddSongSection(SongSectionName songSectionName)
    {
        if (m_SongSections.Count < m_SongSectionPositionalNodes.Count)
        {
            m_SongSections.Add(new SongSection(songSectionName, m_SongSections.Count));
            m_SongSections[m_SongSections.Count - 1].SectionToken.GetComponent<SectionToken>().SelectButton();
        }
        
    }

    public void SetSongSectionCounter(int i)
    {
        if (m_SongSections.Count > 0)
        {
            m_SongSectionCounter = i;
            m_SongSectionCounterTMP.text = m_SongSections[i].m_SongSectionName.ToString();
            m_SongSections[i].SectionToken.GetComponent<Button>().Select();
            ReadSongSectionToDropdowns();
            SetPlayerState(PlayerState.STOPPED);
        }
        else
        {
            m_SongSectionCounterTMP.text = "PICK A SECTION";
            SetPlayerState(PlayerState.NOSECTIONS);
        }
    }

    public void WriteToSongSection()
    {
        if (m_SongSections.Count > 0 && CanWriteChords)
        {
            for (int i = 0; i < ChordDropdowns.Count; i++)
            {
                m_SongSections[m_SongSectionCounter].m_SongChords[i] = (ChordType)ChordDropdowns[i].value;
            }
        }
    }

    public void ReadSongSectionToDropdowns()
    {
        CanWriteChords = false;
        if (m_SongSections.Count > 0)
        {
            for (int i = 0; i < ChordDropdowns.Count; i++)
            {
                ChordDropdowns[i].value = (int) m_SongSections[m_SongSectionCounter].m_SongChords[i];
            }
        }
        CanWriteChords = true;
    }

    public void RemoveFromSongSectionList(int i)
    {

        Debug.Log(i);
        if (m_SongSections.Count > 1)
        {
            Destroy(m_SongSections[i].SectionToken);
            m_SongSections.RemoveAt(i);
            UpdateAllSongSectionIndexes();
            SetSongSectionCounter(0);
        }
        else
        {
            Destroy(m_SongSections[i].SectionToken);
            m_SongSections.Clear();
            SetSongSectionCounter(0);
            SetPlayerState(PlayerState.NOSECTIONS);
        }
    }

    void UpdateAllSongSectionIndexes()
    {
        for(int i = 0; i < m_SongSections.Count; i++)
        {
            m_SongSections[i].UpdateIndexes(i);
        }
    }
}

[System.Serializable]
 public class SongSection
 {
    public SongSection(SongSectionName songSectionName, int SongIndexPosition)
    {
        m_SongSectionName = songSectionName;
        m_SongIndexPosition = SongIndexPosition;
        CreateSectionToken();
    }


    public SongSectionName m_SongSectionName;
    public int m_SongIndexPosition;
    public ChordType[] m_SongChords = new ChordType[8];
    public GameObject SectionToken;

    void CreateSectionToken()
    {
        SectionToken = GameObject.Instantiate(SongChoreographer.instance.m_SectionTokenPrefab);
        SectionToken.GetComponent<SectionToken>().SetVariables(m_SongSectionName, m_SongIndexPosition);
        SectionToken.transform.parent = SongChoreographer.instance.m_SectionTokenParent;
        SectionToken.transform.localPosition = new Vector3(500, 0, 0);
        SectionToken.transform.localScale = new Vector3(1, 1, 1);
    }

    public void UpdateIndexes(int i)
    {
        m_SongIndexPosition = i;
        SectionToken.GetComponent<SectionToken>().SetVariables(m_SongSectionName, m_SongIndexPosition);
    }

    public void UpdateTokenPosition(Transform TargetTransform)
    {
        Vector3 TokenPosition = SectionToken.transform.localPosition;
        SectionToken.transform.localPosition = Vector2.Lerp(TokenPosition, TargetTransform.localPosition, .25f);
    }
}