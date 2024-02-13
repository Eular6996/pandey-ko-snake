using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public string SNAKE_DEATH_SFX = "Death";
    public string EAT_FOOD_SFX = "Eat Food";

    public string LEVEL1_MUSIC = "Level 1";

    [SerializeField] private Sound[] _music, _sfx;
    [SerializeField] private AudioSource _musicSource, _sfxSource;

    /// <summary>
    /// Play Music of Give Name
    /// </summary>
    /// <param name="name"></param>
    public void PlayMusic(string name)
    {
        Sound s = Array.Find(_music, x => x._name == name);

        if (s == null)
        {
            Debug.Log(name + " Music Not is Found!!!");
        }
        else
        {
            _musicSource.clip = s._clip;
            _musicSource.Play();
        }
    }

    /// <summary>
    /// Play SFX of Give Name
    /// </summary>
    /// <param name="name"></param>
    public void PlaySFX(string name)
    {
        Sound s = Array.Find(_sfx, x => x._name == name);

        if (s == null)
        {
            Debug.Log(name + " SFX is Not Found !!!");
        }
        else
        {
            _sfxSource.clip = s._clip;
            _sfxSource.Play();
        }
    }
}
