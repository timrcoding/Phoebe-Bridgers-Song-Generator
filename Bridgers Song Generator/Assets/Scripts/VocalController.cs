using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VocalController : MonoBehaviour
{
    
    [SerializeField] private ClipLength clipLength;
    [SerializeField] private List<bool> coolDownList;
    [SerializeField] private bool CanSing = true;
    [SerializeField] private bool[] CanSingEachSection = new bool[7];
    [SerializeField] private Animator GhostAnimator;
    [SerializeField] private Sprite[] ButtonSprites;

    void Start()
    {
        TimingController.instance.TriggerVocal += SelectClipLength;
        SongChoreographer.instance.SongCounterChanged += ReadEachSection;
    }

    public void setButton()
    {
        CanSing = !CanSing;
        SetEachSection();
    }

    void ReadEachSection()
    {
        CanSing = CanSingEachSection[SongChoreographer.instance.SectionCounter];
        GhostAnimator.SetBool("SwitchBlack", !CanSing);
    }

    public void SetEachSection()
    {
        CanSingEachSection[SongChoreographer.instance.SectionCounter] = CanSing;
        ReadEachSection();
    }
    

    void SelectClipLength()
    {
        if (CanSingEachSection[SongChoreographer.instance.SectionCounter])
        {
            int rand;

            if (coolDownList.Count > 0)
            {
                coolDownList.RemoveAt(0);
            }

            else
            {
                if (TimingController.instance.BarCount == 0)
                {
                    rand = Random.Range(1, 4);
                    clipLength = (ClipLength)rand;

                }
                else if (TimingController.instance.BarCount == 2)
                {
                    rand = Random.Range(1, 3);
                    clipLength = (ClipLength)rand;
                }

                else if (TimingController.instance.BarCount % 2 != 0)
                {
                    clipLength = ClipLength.OneBar;
                }
                FMODUnity.RuntimeManager.PlayOneShot($"event:/VOCALS/Vocals{clipLength}");
                setCoolDown();
            }
        }
    }

    void setCoolDown()
    {
        if(clipLength == ClipLength.TwoBar)
        {
            addToList(1);
        }
        else if(clipLength == ClipLength.FourBar)
        {
            addToList(3);
        }
    }

    void addToList(int num)
    {
        for(int i = 0; i < num; i++)
        {
            coolDownList.Add(new bool()); 
        }
    }

    public enum ClipLength
    {
        INVALID,
        OneBar,
        TwoBar,
        FourBar,
    }
}
