using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODParams : MonoBehaviour
{
    [SerializeField] private FMODUnity.StudioEventEmitter AudioSource;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TurnOffDiscreteValue(string param)
    {
        AudioSource.SetParameter(param, 0);
    }

    public void StopAudio()
    {
        AudioSource.Stop();
    }
}
