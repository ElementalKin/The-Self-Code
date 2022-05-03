using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicHandler : MonoBehaviour
{
    public AudioClip[] music;
    public AudioSource musicSource;
    public int mySongID;
    public bool musicIsPlaying;
    public float timeToNextPlay, minTime, maxTime;

    private void Awake()
    {
        PlaySong();
    }

    //Plays music and starts checks for when it's not playing
    public void PlaySong()
    {
        mySongID = Random.Range(0, music.Length);
        musicSource.clip = music[mySongID];
        musicSource.Play();
        musicIsPlaying = true;
        InvokeRepeating("CheckMusic", 3f, 3f);
    }

    //Repeating check to see if the music stopped. If it did, set a random timer until the next song plays.
    public void CheckMusic()
    {
        if ((musicIsPlaying == true) && musicSource.isPlaying == false)
        {
            CancelInvoke();
            musicIsPlaying = false;
            timeToNextPlay = Random.Range(minTime, maxTime);
            StartCoroutine(WaitAndPlayNextSong());
        }
    }

    public IEnumerator WaitAndPlayNextSong()
    {
        yield return new WaitForSeconds(timeToNextPlay);
        PlaySong();
    }
}
