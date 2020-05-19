using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockDisplay : MonoBehaviour
{
    public Sprite[] blockImageArray;
    private Image displayImage;

    void Awake(){
        displayImage = gameObject.GetComponentsInChildren<Image>()[1];
        UpdateDisplay(7);
    }
    public void UpdateDisplay(int block){
        if(block != 7){
            displayImage.gameObject.SetActive(true);
            displayImage.sprite = blockImageArray[block];
        }else{
            displayImage.gameObject.SetActive(false);
        }
        
    }
    
}
