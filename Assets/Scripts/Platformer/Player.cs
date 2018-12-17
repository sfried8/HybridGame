using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
        public GameObject inventory;


    public void CollectCoin (Shape shape) {

    }
    public void AddToInventory(GameObject shape) {
        inventory.GetComponent<Inventory>().AddShape(shape);
    }
}
