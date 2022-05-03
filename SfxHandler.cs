using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxHandler : MonoBehaviour
{
    public AudioSource tileSource, terraformSource, systemSource;
    public AudioClip[] tileSFX, terraformSFX, systemSFX;
    public SfxHandler instance;
    public float defaultSFXVolume;

    private void Awake()
    {
        instance = this;
        PlayerPrefs.SetFloat("Sfx Volume", defaultSFXVolume);
        ChangeVolumes();
    }

    public void PlayTileSFX(int sfxID)
    {
        if (tileSource.clip != tileSFX[sfxID])
        {
            tileSource.clip = tileSFX[sfxID];
            tileSource.Play();
        }
    }

    public void PlayTerraformSFX(int sfxID)
    {
        terraformSource.clip = terraformSFX[sfxID];
        terraformSource.Play();
    }

    public void PlaySystemSFX(int sfxID)
    {
        systemSource.clip = systemSFX[sfxID];
        systemSource.Play();
    }

    public void ChangeVolumes()
    {
        tileSource.volume = PlayerPrefs.GetFloat("Sfx Volume");
        terraformSource.volume = PlayerPrefs.GetFloat("Sfx Volume");
        systemSource.volume = PlayerPrefs.GetFloat("Sfx Volume");
    }
}
