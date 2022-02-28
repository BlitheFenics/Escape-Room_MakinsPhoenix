using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXScript : MonoBehaviour
{
    public AudioSource audio;
    public AudioClip clip;
    public static SFXScript instance;

    private void Awake()
    {
        instance = this;
    }
}