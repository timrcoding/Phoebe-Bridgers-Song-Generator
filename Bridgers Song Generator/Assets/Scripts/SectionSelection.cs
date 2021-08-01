using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionSelection : MonoBehaviour
{
    public  event Action Hover;
    public void HoverOver()
    {
        Hover();
    }
}
