using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour {

    public GameObject TiendaUI;
    private bool showShop = false;

	void Start () {
		
	}
	
	void Update () {

        if (showShop)
        {
            TiendaUI.SetActive(true);
            
        }
        else
        {
            TiendaUI.SetActive(false);
            TiendaUI.GetComponent<ShopManager>().EsconderItems(0);
        }

	}

    void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Collider>().CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                showShop = !showShop;
            }
        }
    }
    
    void OnTriggerExit (Collider other)
    {
        showShop = false;
    } 
}
