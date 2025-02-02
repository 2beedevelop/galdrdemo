using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLoader : MonoBehaviour
{
    public AudioManager theAM;
    private void Awake()
    {
        if(AudioManager.instance == null)
        {
            AudioManager newAm = Instantiate(theAM);
            AudioManager.instance = newAm;
            DontDestroyOnLoad(newAm.gameObject);
        }
    }
}
