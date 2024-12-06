using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicHandler : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private float titleVolume;
    [SerializeField] private float shopVolume;
    [SerializeField] private float fightVolume;

    [Header("References")]
    [SerializeField] private Slider volumeSlider;

    [Header("Sound References")]
    [SerializeField] private AudioClip titleMusic;
    [SerializeField] private AudioClip shopMusic;
    [SerializeField] private AudioClip fightMusic;

    private AudioSource musicPlayer;
    private float currentTrackVolume;
    private string currentTrackName;
    private bool isLowerVolume;

    // Start is called before the first frame update
    void Start()
    {
        musicPlayer = this.GetComponent<AudioSource>();
        isLowerVolume = false;

        currentTrackName = "";
        SwitchAudioTrack("title");
    }

    public void SwitchAudioTrack(string name)
    {
        if (name == currentTrackName)
            return;

        musicPlayer.Stop();

        switch (name)
        {
            case "title":
                musicPlayer.clip = titleMusic;
                musicPlayer.volume = titleVolume;
                break;

            case "shop":
                musicPlayer.clip = shopMusic;
                musicPlayer.volume = shopVolume;
                break;

            case "fight":
                musicPlayer.clip = fightMusic;
                musicPlayer.volume = fightVolume;
                break;

            default:
                break;
        }

        currentTrackVolume = musicPlayer.volume;
        currentTrackName = name;

        if (volumeSlider.value != 0)
            musicPlayer.volume = musicPlayer.volume / volumeSlider.value;
        else
            musicPlayer.volume = 0;

        musicPlayer.Play();
    }

    public void LowerVolume()
    {
        isLowerVolume = true;
    }

    public void RestoreVolume()
    {
        isLowerVolume = false;
    }

    void Update()
    {
        if (volumeSlider.value != 0)
        {
            if (!isLowerVolume)
                musicPlayer.volume = currentTrackVolume * volumeSlider.value;
            else
                musicPlayer.volume = (currentTrackVolume / 3) * volumeSlider.value;
        }
        else
        {
            musicPlayer.volume = 0;
        }
    }
}
