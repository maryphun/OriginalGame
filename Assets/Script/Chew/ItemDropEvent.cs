using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropEvent : MonoBehaviour
{
    public GameObject[] itemDropList;
    public int itemDropNum = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DropItem()
    {
        for (int i = 0; i < itemDropNum; i++)
        {
            int itemIdx = Random.Range(0, itemDropList.Length);
            GameObject droppedItem = Instantiate(itemDropList[itemIdx], transform.position, transform.rotation);
            //Spawn effect
            float forceX = Random.Range(-0.5f, 0.5f);
            float forceZ = Random.Range(-0.2f, 0.2f);

            droppedItem.GetComponent<Rigidbody>().AddForce(new Vector3(forceX, 0.5f, forceZ), ForceMode.Force);
        }
    }


}
