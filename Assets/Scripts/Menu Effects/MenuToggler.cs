using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuToggler : MonoBehaviour
{
    public static UnityEngine.UI.Toggle effectToggle;
    void Awake(){
        effectToggle = gameObject.GetComponent<UnityEngine.UI.Toggle>();
    }

    void Start(){
        if(PlayerPrefs.HasKey("PieceFX")) MenuSpawn.active = (PlayerPrefs.GetInt("PieceFX") == 1);
        else MenuSpawn.active = true;
        effectToggle.isOn = MenuSpawn.active;
        effectToggle.onValueChanged.AddListener(delegate {Toggle();});
    }
    
    void OnEnable(){
        effectToggle.isOn = MenuSpawn.active;
    }

    void Toggle(){
        if(effectToggle.isOn) MenuSpawn.singleton.Enable();
        else MenuSpawn.singleton.Disable();
        PlayerPrefs.SetInt("PieceFX", effectToggle.isOn ? 1 : 0);
    }
}
