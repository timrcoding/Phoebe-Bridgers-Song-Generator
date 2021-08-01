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
}

public class Instrument : MonoBehaviour
{
    private TimingController timingController;
    private SongChoreographer songChoreographer;
    [SerializeField] private InstrumentType m_InstrumentType;
    [SerializeField] private List<InstrumentPlayingStyle> InstrumentStylesForDropdownMenu;
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
        timingController.OnSongBarBegin += PlayInstrumentAtStartOfBar;
        timingController.OnSectionBegin += PlayWholeSectionInstrument;
        songChoreographer.PlayerNoSections += MakeInteractable;
        songChoreographer.PlayerPlaying += MakeNonInteractable;
        songChoreographer.PlayerStopped += MakeInteractable;

        SetDropDownOptions();
        setToggleColor();
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
                    string FMODEvent = $"event:/{m_InstrumentType}/{timingController.g_CurrentChord}{InstrumentStylesForDropdownMenu[m_Dropdown.value]}";
                    Debug.Log(FMODEvent);
                    //TODO This requires a null check
                    FMODUnity.RuntimeManager.PlayOneShot(FMODEvent);
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
            Debug.Log(FMODEvent);
            FMODUnity.RuntimeManager.PlayOneShot(FMODEvent);
        }

    }
}
