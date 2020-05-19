using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeControl : MonoBehaviour
{
    // Start is called before the first frame update
    [Range(0f,1.0f)]public static float SFX;
    [Range(0f,1.0f)]public static float music;
    [Range(0f,1.0f)]public static float master;

    void Start(){
        if (PlayerPrefs.HasKey("SFX") && PlayerPrefs.HasKey("master")) SFX = PlayerPrefs.GetFloat("SFX"); else SFX = 0.5f;
        if (PlayerPrefs.HasKey("music") && PlayerPrefs.HasKey("master")) music = PlayerPrefs.GetFloat("music"); else music = 0.5f;
        if (PlayerPrefs.HasKey("master")) master = PlayerPrefs.GetFloat("master"); else master = 0.5f;
        if (BlockBehavior.blockAudio != null) BlockBehavior.blockAudio.volume = master * SFX;
        if (GameLoop.musicAudio != null) GameLoop.musicAudio.volume = master * music;
    }

    void OnDestroy(){
        PlayerPrefs.SetFloat("music", music);
        PlayerPrefs.SetFloat("master", master);
        PlayerPrefs.SetFloat("SFX", SFX);
    }

    public static void UpdateValue(VolumeSlider.volSlider sliderID,float newValue){
        switch (sliderID){
            case VolumeSlider.volSlider.master:
            master = newValue;
            if (BlockBehavior.blockAudio != null) BlockBehavior.blockAudio.volume = master * SFX;
            if (GameLoop.musicAudio != null) GameLoop.musicAudio.volume = master * music;
            break;
            case VolumeSlider.volSlider.SFX:
            SFX = newValue;
            if (BlockBehavior.blockAudio != null) BlockBehavior.blockAudio.volume = master * SFX;
            break;
            case VolumeSlider.volSlider.music:
            music = newValue;
            if (GameLoop.musicAudio != null) GameLoop.musicAudio.volume = master * music;
            break;
        }

    }

    // Update is called once per frame
    
}
