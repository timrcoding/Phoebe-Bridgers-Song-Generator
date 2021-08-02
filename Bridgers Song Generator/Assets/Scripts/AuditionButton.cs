using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class AuditionButton : MonoBehaviour
{
    private Vector3 OriginalPosition;
    [SerializeField] private Button button;
    [SerializeField] private InstrumentType instrumentType;
    [SerializeField] private InstrumentPlayingStyle playingStyle;
    [SerializeField] private ChordType ChordType;
    [FMODUnity.EventRef]
    [SerializeField] string evRef;
    private bool CanMove;
    private float DragCooldown;
    [SerializeField] TMP_Dropdown dropdownOver;
    [SerializeField] List<GameObject> ListOfDropdowns;
    private Vector3 dragOffset;
    void Start()
    {
        OriginalPosition = transform.localPosition;
        gameObject.name = ChordType.ToString();
        setEvent();
        FindDropdowns();
    }

    public void PlayChord()
    {
        FMODUnity.RuntimeManager.PlayOneShot(evRef);
    }

    private void Update()
    {
        DragCooldown += 0.1f;

        if (CanMove && DragCooldown > 0)
        {
            var screenPoint = (Vector3)Input.mousePosition;
            screenPoint.z = 10.0f; //distance of the plane from the camera
            transform.position = returnCameraPoint(Input.mousePosition) + dragOffset;
            sortDropDowns(transform.position);            
        }
        else
        {
            transform.localPosition = OriginalPosition;
        }
    }

    Vector3 returnCameraPoint(Vector3 InputVector)
    {
        var screenPoint = InputVector;
        screenPoint.z = 10.0f;
        return Camera.main.ScreenToWorldPoint(screenPoint);
    }

    public void StartDrag()
    {
        CanMove = true;
        DragCooldown = -.5f;
        dragOffset = transform.position - returnCameraPoint(Input.mousePosition);
    }

    void setEvent()
    {
        evRef = $"event:/{instrumentType}/{ChordType}{playingStyle}";
    }

    public void StopDrag()
    {
        CanMove = false;
        setDropdownValue();
    }

    public void setDropdownValue()
    {
        if(dropdownOver != null && dropdownOver.interactable)
        {
            dropdownOver.value = (int) ChordType;
        }
    }

    void setDropdownColor(GameObject obj)
    {
        foreach(GameObject g in ListOfDropdowns)
        {
            if (g.GetComponent<TMP_Dropdown>().interactable)
            {
                if (g == obj && SongChoreographer.instance.m_SongState != SongState.PLAYING)
                {
                    
                    dropdownOver = ListOfDropdowns[0].GetComponent<TMP_Dropdown>();
                    g.GetComponent<Image>().color = Color.yellow;
                    dropdownOver = g.GetComponent<TMP_Dropdown>();
                }
                else
                {
                    g.GetComponent<Image>().color = Color.white;
                }
            }
        }
    }

    void FindDropdowns()
    {
        GameObject[] GO_Dropdowns = GameObject.FindGameObjectsWithTag("Dropdown");
        ListOfDropdowns.Clear();
        foreach (GameObject g in GO_Dropdowns)
        {
            ListOfDropdowns.Add(g);
        }
        
    }

    void sortDropDowns(Vector3 pos)
    {
        ListOfDropdowns = ListOfDropdowns.OrderBy(ctx => Vector2.Distance(pos, ctx.transform.position)).ToList();
        if (Vector2.Distance(pos, ListOfDropdowns[0].transform.position) < 1){
            setDropdownColor(ListOfDropdowns[0]);
        }
        else
        {
            setDropdownColor(null);
            dropdownOver = null;
        }
    }

}
