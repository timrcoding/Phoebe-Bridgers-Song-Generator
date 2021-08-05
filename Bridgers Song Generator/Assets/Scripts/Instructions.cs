using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instructions : MonoBehaviour
{
    Vector3 OriginalPosition;
    Vector3 TargetPosition;
    void Start()
    {
        OriginalPosition = transform.localPosition;
        SetToCentre();
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, TargetPosition, 0.1f);
    }

    public void SetToCentre()
    {
        TargetPosition = new Vector3(0, 0, 0);
    }

    public void SetToOriginal()
    {
        TargetPosition = OriginalPosition;
    }
}
