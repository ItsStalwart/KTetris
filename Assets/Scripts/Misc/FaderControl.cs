using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaderControl : MonoBehaviour
{
    private static UnityEngine.UI.RawImage faderImage;
    void Awake(){
        faderImage = gameObject.GetComponent<UnityEngine.UI.RawImage>();
    }

    void Start(){
        faderImage.raycastTarget = false;
        FadeIn();
    }

    public static void FadeIn(){
        faderImage.CrossFadeAlpha(0,1.0f,false);
    }
    public static void FadeOut(){
        faderImage.CrossFadeAlpha(1,1.0f,false);
    }
}
