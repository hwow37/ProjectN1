using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    // for Setting Audio
    public AudioMixer audioMixer;

    // for Setting Resolution
    public Dropdown resolutionDropdown;
    private Resolution[] resolutions;

    // Setting Resolutions
    private void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        
        int currentResolutionIndex = 0;
        for(int i = 0; i < resolutions.Length; i++)
        {
            // Modification Required (To much Resolutions)
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    // Setting volume
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("masterVolume", volume);
    }

    // Setting graphics quality
    public void SetQuality (int quilitIndex)
    {
        QualitySettings.SetQualityLevel(quilitIndex);
        Debug.Log(quilitIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
