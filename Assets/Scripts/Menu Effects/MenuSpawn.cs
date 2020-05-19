using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSpawn : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] pieces;
    public static bool active = false;
    public static MenuSpawn singleton;

    // Update is called once per frame
    void Awake(){
        singleton = this;
        if(PlayerPrefs.HasKey("PieceFX")) active = (PlayerPrefs.GetInt("PieceFX") == 1);
        Debug.Log(PlayerPrefs.GetInt("PieceFX"));
    }

    void Start(){
        if(active) InvokeRepeating("SpawnRandomBlock",0f,0.2f);
    }
    void SpawnRandomBlock()
    {
        Instantiate(pieces[Random.Range(0,pieces.Length)], transform.position, Quaternion.identity);
    }

    public void Disable(){
        CancelInvoke();
        active = false;
    }

    public void Enable(){
        InvokeRepeating("SpawnRandomBlock",0f,0.2f);
        active = true;
    }
}
