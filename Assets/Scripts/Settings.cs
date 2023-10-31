using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class Settings : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider generalVolSlider;
    [SerializeField] private Slider musicVolSlider;

    private const string generalVolumeKeyAndParameterName = "GeneralVolume";
    private const string musicVolumeKeyAndParameterName = "MusicVolume";

    private void Start()
    {
        if (PlayerPrefs.HasKey(generalVolumeKeyAndParameterName))
        {
            int generalVolume = PlayerPrefs.GetInt(generalVolumeKeyAndParameterName);
            generalVolSlider.value = 0.01f * generalVolume + 0.8f;

            int musicVolume = PlayerPrefs.GetInt(musicVolumeKeyAndParameterName);
            musicVolSlider.value = 0.01f * musicVolume + 0.8f;
        }
        else 
        { 
            mixer.GetFloat(generalVolumeKeyAndParameterName, out float generalVolume);
            mixer.GetFloat(musicVolumeKeyAndParameterName, out float musicVolume);

            float LerpT = ReverseLerp(generalVolume);
            generalVolSlider.value = LerpT;

            LerpT = ReverseLerp(musicVolume);
            musicVolSlider.value = LerpT;
        }
    }

    private float ReverseLerp(float result) 
    {
        float t = (result + 80f) / 100f;
        return t;
    }

    public void OnMusicVolumeUpdated(float value) 
    {
        int decibelValue = Mathf.RoundToInt(Mathf.Lerp(-80f, 20f, value));
        mixer.SetFloat(musicVolumeKeyAndParameterName, decibelValue);
        PlayerPrefs.SetInt(musicVolumeKeyAndParameterName, decibelValue);
        PlayerPrefs.Save();
    }

    public void OnGeneralVolumeUpdated(float value) 
    {
        int decibelValue = Mathf.RoundToInt(Mathf.Lerp(-80f, 20f, value));
        mixer.SetFloat(generalVolumeKeyAndParameterName, decibelValue);
        PlayerPrefs.SetInt(generalVolumeKeyAndParameterName, decibelValue);
        PlayerPrefs.Save();
    }
}