using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public static SpawnHandler singleton;
    [SerializeField]
    private GameObject[] BlockArray;

    private BlockDisplay blockPreview, blockStorage;


    public static int savedBlockIndex = 7;
    public static int lastSpawnedBlock = 7;
    public static int nextBlockToSpawn = 7;

    public static GameObject currentBlock;

    void Awake(){
        if(singleton == null){
            singleton = this;
        }else{
            Destroy(this.gameObject);
        }
    }
    void Start()
    {
        FindBlockDisplayers();
        NewBlock();
    }

    public void NewBlock(){
        if(nextBlockToSpawn ==7){
            lastSpawnedBlock = Random.Range(0,BlockArray.Length);
            if (singleton != null) currentBlock = Instantiate(BlockArray[lastSpawnedBlock], transform.position, Quaternion.identity);
            nextBlockToSpawn = Random.Range(0,BlockArray.Length);
            blockPreview.UpdateDisplay(nextBlockToSpawn);
        }else{
            SpecificBlock(nextBlockToSpawn);
        }
        
    }

    void FindBlockDisplayers(){
        blockPreview = GameObject.FindGameObjectWithTag("BlockPreviewer").GetComponent<BlockDisplay>();
        blockStorage = GameObject.FindGameObjectWithTag("BlockStorage").GetComponent<BlockDisplay>();
    }

    public void SpecificBlock(int blockIndex){
        if (singleton != null) currentBlock = Instantiate(BlockArray[blockIndex], transform.position, Quaternion.identity);
        lastSpawnedBlock = blockIndex;
        nextBlockToSpawn = Random.Range(0,BlockArray.Length);
        blockPreview.UpdateDisplay(nextBlockToSpawn);
    }

    public void StoreBlock(){
        if(savedBlockIndex ==7){
            savedBlockIndex = lastSpawnedBlock;
            Destroy(currentBlock);
            SpecificBlock(nextBlockToSpawn);
        }else{ //has sabed Block
            int auxBlockIndex = lastSpawnedBlock; //remember current block
            Destroy(currentBlock); //delete current block
            LoadBlock(savedBlockIndex);//spawn copy of saved block
            savedBlockIndex = auxBlockIndex;//save deleted block
        }
        blockStorage.UpdateDisplay(savedBlockIndex);
    }

    void LoadBlock(int blockIndex){
        if (singleton != null) currentBlock = Instantiate(BlockArray[blockIndex], transform.position, Quaternion.identity);
        lastSpawnedBlock = blockIndex;
    }
}
