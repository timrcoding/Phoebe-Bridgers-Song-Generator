using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiscResources : MonoBehaviour
{
    public static MiscResources instance;

    public Color[] ChordColors;
    void Start()
    {
        instance = this;    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
