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
public enum SongState
{
    INVALID,
    NOSECTIONS,
    STOPPED,
    PLAYING
}

public class SongChoreographer : Controller
{
    public static SongChoreographer instance;
    public int SectionCounter;

    public List<SongSection> m_SongSections;
    public SongState m_SongState;

    //PLAY STATE CONTROLLER
    public event Action SongNoSections;
    public event Action SongStopped;
    public event Action SongPlaying;
    public event Action SongCounterChanged;
    public event Action SectionAdded;
    public event Action SetupCopyElement;

    //SONG SEQUENCE AREA
    [SerializeField] private List<Transform> m_SongSectionPositionalNodes;
    public Transform m_SectionTokenParent;
    public GameObject m_SectionTokenPrefab;

    //SECTION SELECTED
    
    [SerializeField] private TextMeshProUGUI m_SongSectionHeader;
    [SerializeField] private List<TMP_Dropdown> ChordDropdowns;
    private bool CanWriteChords = true;

    //INSTRUMENT CONTROLLER;
    [SerializeField] private List<Instrument> Instruments;

    //COPYING
    [SerializeField] int indexToCopy;
    [SerializeField] int indexCopiedTo;
    [SerializeField] bool readyToCopy;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartCoroutine(Init());
    }

    IEnumerator Init()
    {
        yield return new WaitForEndOfFrame();
        SetSongSectionCounter(0);
        TimingController.instance.OnNewSectionBegin += IncrementSectionCounter;
        AddSongSection(SongSectionName.INTRO);
        SetFirstSectionOnLoad();
    }

    void SetFirstSectionOnLoad()
    {
        SongSection Section = m_SongSections[0];
        Section.m_SongChords[0] = ChordType.ChordOne;
        Section.m_SongChords[1] = ChordType.ChordThree;
        Section.m_SongChords[2] = ChordType.ChordSix;
        Section.m_SongChords[3] = ChordType.ChordTwo;
        Section.m_SongChords[4] = ChordType.ChordOne;
        Section.m_SongChords[5] = ChordType.ChordSix;
        Section.m_SongChords[6] = ChordType.ChordFour;
        Section.m_SongChords[7] = ChordType.ChordFive;
        ReadSongSectionToDropdowns();
        Debug.Log("CHORDS SET");
    }

    public void SetSongState(SongState songState)
    {
        m_SongState = songState;

        switch (songState)
        {
            case SongState.NOSECTIONS:
                SongNoSections();
                break;

            case SongState.STOPPED:
                SongStopped();
                break;

            case SongState.PLAYING:
                SongPlaying();
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
        SectionAdded();
        SetSongState(SongState.STOPPED);
        
    }
    public void IncrementSectionCounter()
    {
        int num = new int();
        if(SectionCounter < m_SongSections.Count-1)
        {
            num = SectionCounter;
            num++;
            SetSongSectionCounter(num);
        }
        else
        {
            //TimingController.instance.StopClock();
            SetSongSectionCounter(0);
        }
    }
    public void SetSongSectionCounter(int i)
    { 
        if (m_SongSections.Count > 0)
        {
            SectionCounter = i;
            m_SongSectionHeader.text = m_SongSections[i].m_SongSectionName.ToString();
            m_SongSections[i].SectionToken.GetComponent<Button>().Select();
            ReadSongSectionToDropdowns();
        }
        else
        {
            m_SongSectionHeader.text = "PICK A SECTION";
            SetSongState(SongState.NOSECTIONS);
        }
        SongCounterChanged();
        Debug.Log($"SongSection: {SectionCounter}");
    }

    public void WriteToSongSection()
    {
        if (m_SongSections.Count > 0 && CanWriteChords)
        {
            for (int i = 0; i < ChordDropdowns.Count; i++)
            {
                m_SongSections[SectionCounter].m_SongChords[i] = (ChordType)ChordDropdowns[i].value;
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
                ChordDropdowns[i].value = (int) m_SongSections[SectionCounter].m_SongChords[i];
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
            SetSongState(SongState.NOSECTIONS);
        }
    }

    void UpdateAllSongSectionIndexes()
    {
        for(int i = 0; i < m_SongSections.Count; i++)
        {
            m_SongSections[i].UpdateIndexes(i);
        }
    }

    public void setupCopy(int index)
    {
        
        if (!readyToCopy)
        {
            readyToCopy = true;
            indexToCopy = index;
        }
        else
        {
            
            readyToCopy = false;
            copyChords();
            Debug.Log("CHORDS COPIED");
            
        }
    }

    public void copyChords()
    {
        SongSection SectionCopy = m_SongSections[indexToCopy];
        SongSection SectionReceived = m_SongSections[indexCopiedTo];

        SectionReceived.m_SongChords = SectionCopy.m_SongChords;
        
            ReadSongSectionToDropdowns();
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
        SectionToken.transform.SetParent(SongChoreographer.instance.m_SectionTokenParent);
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