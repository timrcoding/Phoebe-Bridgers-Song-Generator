using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FMODLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FMODUnity.RuntimeManager.LoadBank("Instruments");

        StartCoroutine(ChangeScene(1));
    }


    IEnumerator ChangeScene(int buildID)
    {
        yield return new WaitForSeconds(.1f);

        if (FMODUnity.RuntimeManager.HasBankLoaded("Instruments"))
        {
            SceneManager.LoadScene(buildID);

        }
        else
        {
            Debug.Log("Instrument bank not loaded looping");
            StartCoroutine(ChangeScene(buildID));
        }
    }

}
