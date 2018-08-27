using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class ItemSuelto : MonoBehaviour {

    public int ID;
    public int cantidad;
    private InventarioNuevo Inv;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Inv = other.GetComponent<Player>().inventario.GetComponent<InventarioNuevo>();
            Inv.AgrregarItem(ID, cantidad);
            Destroy(this.gameObject);
        }
    }
}
