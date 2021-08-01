using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum InstrumentType
{
    INVALID,
    ACOUSTIC,
    ELECTRIC,
    BASS,
    DRUMS,
    KEYS,
    STRINGS,
    SFX,
}

public enum InstrumentPlayingStyle
{
    None,
    Electric,
    Acoustic,
    Picked,
    Strummed,
    Bowed,
    Brushes,
    Sticks,
    Felt,
    Upright,
    Shimmer,
    Radio,
    Horns,
}

public class Instrument : MonoBehaviour
{
    private TimingController timingController;
    private SongChoreographer songChoreographer;
    [SerializeField] private InstrumentType m_InstrumentType;
    [SerializeField] private List<InstrumentPlayingStyle> InstrumentStylesForDropdownMenu;

    [SerializeField] private InstrumentPlayingStyle[] InstrumentInEachSection = new InstrumentPlayingStyle[7];
    public InstrumentType g_InstrumentType { get { return m_InstrumentType; } }
    public FMODUnity.StudioEventEmitter AudioSource;
    [SerializeField] private TMP_Dropdown m_Dropdown;

    public Toggle m_Toggle;
    public Image m_ToggleImage;
    public bool m_InstrumentIsActive;
    public bool ChordFollowingInstrument;
    public bool LastsWholeSection;
    void Start()
    {
        timingController = FindObjectOfType<TimingController>();
        songChoreographer = FindObjectOfType<SongChoreographer>();
        //BINDING EVENTS
        TimingController.instance.OnSongBarBegin += PlayInstrumentAtStartOfBar;
        TimingController.instance.OnSongStart += PlayWholeSectionInstrument;
        SongChoreographer.instance.SongNoSections += MakeInteractable;
        SongChoreographer.instance.SongPlaying += MakeNonInteractable;
        SongChoreographer.instance.SongStopped += MakeInteractable;
        SongChoreographer.instance.SongStopped += StopEmitter;
        SongChoreographer.instance.SongCounterChanged += ReadInstrumentFromMemory;
        SongChoreographer.instance.SectionAdded += WritePreviousInstrumentToMemory;
        WriteInstrumentToMemory();



        SetDropDownOptions();
        setToggleColor();
        AudioSource = GetComponent<FMODUnity.StudioEventEmitter>();
    }

    void SetDropDownOptions()
    {
        m_Dropdown.ClearOptions();
        List<string> DropdownOptions = new List<string>();
        foreach (InstrumentPlayingStyle instStyle in InstrumentStylesForDropdownMenu)
        {
            DropdownOptions.Add(instStyle.ToString());
        }
        m_Dropdown.AddOptions(DropdownOptions);
    }

    public void SetToggle()
    {
        m_InstrumentIsActive = m_Toggle.isOn;
        setToggleColor();
    }

    void setToggleColor()
    {
        if (m_InstrumentIsActive) { m_ToggleImage.color = Color.green; }
        else { m_ToggleImage.color = Color.red; }
    }

    public void WriteInstrumentToMemory()
    {
        int count = SongChoreographer.instance.SectionCounter;
        InstrumentInEachSection[count] = InstrumentStylesForDropdownMenu[m_Dropdown.value];
    }

    public void WritePreviousInstrumentToMemory()
    {
        if (SongChoreographer.instance.m_SongState != SongState.NOSECTIONS)
        {
            int count = SongChoreographer.instance.SectionCounter;
            InstrumentInEachSection[count] = InstrumentInEachSection[count - 1];
        }
    }

    public void ReadInstrumentFromMemory()
    {
        int count = SongChoreographer.instance.SectionCounter;
        InstrumentPlayingStyle inst = InstrumentInEachSection[count];
        if(inst == InstrumentPlayingStyle.None)
        {
            return;
        }
        for(int i = 0; i < InstrumentStylesForDropdownMenu.Count; i++)
        {
            if(inst == InstrumentStylesForDropdownMenu[i])
            {
                m_Dropdown.value = i;
            }
        }
    }

    void MakeNonInteractable() => m_Dropdown.interactable = false;
    
    void MakeInteractable() => m_Dropdown.interactable = true;

    public void PlayInstrumentAtStartOfBar()
    {
        if (m_InstrumentIsActive)
        {
            if (!LastsWholeSection)
            {
                if (ChordFollowingInstrument)
                {
                    if (timingController.g_CurrentChord != ChordType.INVALID)
                    {
                        string FMODEvent = $"event:/{m_InstrumentType}/{timingController.g_CurrentChord}{InstrumentStylesForDropdownMenu[m_Dropdown.value]}";
                        Debug.Log(FMODEvent);
                        FMODUnity.RuntimeManager.PlayOneShot(FMODEvent);
                    }
                }
                else
                {
                    string FMODEvent = $"event:/{m_InstrumentType}/{InstrumentStylesForDropdownMenu[m_Dropdown.value]}";
                    Debug.Log(FMODEvent);
                    FMODUnity.RuntimeManager.PlayOneShot(FMODEvent);
                }
            }
        }
    }

    public void PlayWholeSectionInstrument()
    {
        if (m_InstrumentIsActive && LastsWholeSection)
        {
            string FMODEvent = $"event:/{m_InstrumentType}/{InstrumentStylesForDropdownMenu[m_Dropdown.value]}";
            AudioSource.Event = FMODEvent;
            AudioSource.Play();
        }

    }
    public void StopEmitter()
    {
        AudioSource.Stop();
    }
}
