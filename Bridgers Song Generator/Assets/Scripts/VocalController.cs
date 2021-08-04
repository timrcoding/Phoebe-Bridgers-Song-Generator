using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VocalController : MonoBehaviour
{
    
    [SerializeField] private ClipLength clipLength;
    [SerializeField] private List<bool> coolDownList;
    [SerializeField] private bool CanSing = true;
    [SerializeField] private Image GhostButton;
    [SerializeField] private Sprite[] ButtonSprites;

    void Start()
    {
        TimingController.instance.TriggerVocal += SelectClipLength;
    }

    public void setButton()
    {
        CanSing = !CanSing;

        if (CanSing)
        {
            GhostButton.sprite = ButtonSprites[0];
        }
        else
        {
            GhostButton.sprite = ButtonSprites[1];
        }
    }
    

    void SelectClipLength()
    {
        if (CanSing)
        {
            int rand = new int();

            if (coolDownList.Count > 0)
            {
                coolDownList.RemoveAt(0);
            }

            else
            {
                if (TimingController.instance.BarCount == 0)
                {
                    rand = Random.Range(2, 4);
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
                Debug.Log(clipLength);
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
