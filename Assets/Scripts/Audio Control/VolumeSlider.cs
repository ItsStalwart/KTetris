using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeSlider : MonoBehaviour
{
    // Start is called before the first frame update
    [System.Serializable]
    public enum volSlider{
        SFX,
        music,
        master
    }

    public volSlider identifier;
    private UnityEngine.UI.Slider slider;
    void Awake(){
        //firstEnable = true;
        slider = gameObject.GetComponent<UnityEngine.UI.Slider>();
    }
    void Start()
    {
        slider.onValueChanged.AddListener(delegate {UpdateVolumeController();});
    }

    void UpdateVolumeController(){
        VolumeControl.UpdateValue(identifier,gameObject.GetComponent<UnityEngine.UI.Slider>().value);
    }

    void OnEnable(){
        switch(identifier){
            case volSlider.master:
            slider.value = VolumeControl.master;
            break;
            case volSlider.music:
            slider.value = VolumeControl.music;
            break;
            case volSlider.SFX:
            slider.value = VolumeControl.SFX;
            break;
        }
         
    }

    // Update is called once per frame
    
}
