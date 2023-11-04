using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class Settings : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider generalVolSlider;
    [SerializeField] private Slider musicVolSlider;
    [SerializeField] private Slider sfxVolSlider;

    private const string generalVolumeKeyAndParameterName = "GeneralVolume";
    private const string musicVolumeKeyAndParameterName = "MusicVolume";
    private const string sfxVolumeKeyAndParameterName = "SfxVolume";

    private void Start()
    {
        if (PlayerPrefs.HasKey(generalVolumeKeyAndParameterName))
        {
            int generalVolume = PlayerPrefs.GetInt(generalVolumeKeyAndParameterName);
            generalVolSlider.value = 0.01f * generalVolume + 0.8f;

            int musicVolume = PlayerPrefs.GetInt(musicVolumeKeyAndParameterName);
            musicVolSlider.value = 0.01f * musicVolume + 0.8f;

            int sfxVolume = PlayerPrefs.GetInt(sfxVolumeKeyAndParameterName);
            musicVolSlider.value = 0.01f * musicVolume + 0.8f;
        }
        else 
        { 
            mixer.GetFloat(generalVolumeKeyAndParameterName, out float generalVolume);
            mixer.GetFloat(musicVolumeKeyAndParameterName, out float musicVolume);
            mixer.GetFloat(sfxVolumeKeyAndParameterName, out float sfxVolume);

            float LerpT = ReverseLerp(generalVolume);
            generalVolSlider.value = LerpT;

            LerpT = ReverseLerp(musicVolume);
            musicVolSlider.value = LerpT;

            LerpT = ReverseLerp(sfxVolume);
            sfxVolSlider.value = LerpT;
        }
    }

    private float ReverseLerp(float result) 
    {
        float t = (result + 80f) / 100f;
        return t;
    }

    public void OnMusicVolumeUpdated(float value) 
    {
        OnSpecificVolumeUpdated(value, musicVolumeKeyAndParameterName);
    }

    public void OnGeneralVolumeUpdated(float value) 
    {
        OnSpecificVolumeUpdated(value, generalVolumeKeyAndParameterName);
    }

    public void OnSfxVolumeUpdated(float value) 
    {
        OnSpecificVolumeUpdated(value, sfxVolumeKeyAndParameterName);
    }

    public void OnSpecificVolumeUpdated(float value, string volumeKeyAndParameterName) 
    {
        int decibelValue = Mathf.RoundToInt(Mathf.Lerp(-80f, 20f, value));
        mixer.SetFloat(volumeKeyAndParameterName, decibelValue);
        PlayerPrefs.SetInt(volumeKeyAndParameterName, decibelValue);
        PlayerPrefs.Save();
    }
}