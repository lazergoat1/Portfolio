using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public bool pickup;
    public float interactionCheckRadius;
    [Space]

    [Header("Pickup attributes")]
    public float setPickupTime;
    public Item item;
    [Space]

    [Header("Non pickup attributes")]
    public float numberOfDrops;
    public float durability;
    public GameObject itemDrop;
    public EquipmentType requiredTool;

    [HideInInspector]
    public float pickupTime;
    [HideInInspector]
    public bool playing;

    private void Start()
    {
        pickupTime = setPickupTime;
    }

    public void PickupFunction(GameObject player)
    {
        ThirdPersonPlayer thirdPersonPlayer = player.GetComponent<ThirdPersonPlayer>();

        if (playing)
        {
            if (pickup)
            {
                pickupTime -= 1 * Time.deltaTime;
                print(pickupTime);

                if (pickupTime <= 0f)
                {
                    if(thirdPersonPlayer.GetComponent<Inventory>().AddItem(item) == true)
                    {
                        thirdPersonPlayer.moveTowards = null;
                        Destroy(gameObject);
                    }
                }
            }

            else
            {
                if(thirdPersonPlayer.GetComponent<Inventory>().equipmentSlots[0].equipmentType == requiredTool && thirdPersonPlayer.GetComponent<Inventory>().equipmentSlots[0].item != null)
                {
                    //Take away the tools durability
                    //I should Create an event that fires when you press the spacebar this event will do all of the walking and checking for items
                    durability -= 1 * Time.deltaTime;
                    print(durability);

                    if (durability <= 0f)
                    {
                        for (int i = 0; i < numberOfDrops; i++)
                        {
                            GameObject.Instantiate(itemDrop, new Vector3(this.transform.position.x + Random.Range(-5,5), this.transform.position.y, this.transform.position.z + Random.Range(-5, 5)), Quaternion.identity);
                        }

                        thirdPersonPlayer.moveTowards = null;
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(gameObject.transform.position, interactionCheckRadius);
    }

}
