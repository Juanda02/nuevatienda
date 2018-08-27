using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




public class ShopManager : MonoBehaviour {

    public GameObject Player;
    public GameObject Inv;
    private List<ItemTienda> ItemCompra;
    private List<ItemTienda> itDesactivar = new List<ItemTienda>();
    private List<ItemTienda> itActivar = new List<ItemTienda>();
    private List<ItemTienda> ItemsVendidos = new List<ItemTienda>();
    [SerializeField]
    public Database DB;
    public GameObject ContenedorItems;
    [Header("Carteles")]
    public Text cartelMoney;
    public GameObject insuficienteDinero;
    public GameObject confCompra;
    public GameObject compraMas;
  
    void Start () {

        ItemCompra = new List<ItemTienda>();
        for (int i = 0; i < ContenedorItems.transform.childCount; i++)
        {
            ItemCompra.Add(ContenedorItems.transform.GetChild(i).GetComponent<ItemTienda>());
        }

        confCompra.SetActive(false);
        compraMas.SetActive(false);
        insuficienteDinero.SetActive(false);
    }
	
	void Update () {

        cartelMoney.text = "Money: " + Player.GetComponent<Player>().Money.ToString();

    }

    public void ComprarItem(int ItemID, int cantidad)
    {
        if(Player.GetComponent<Player>().Money >= ItemCompra[ItemID].precio * cantidad)
        {

            Debug.Log("ItemTienda comprado");
            Player.GetComponent<Player>().Money -= ItemCompra[ItemID].precio * cantidad;
            //Agregar el item a tu Inventario
            if (ItemCompra[ItemID].acumulable)
            {
                Inv.GetComponent<InventarioNuevo>().AgrregarItem(ItemID, cantidad);
            }
            else
            {
                for (int item = 0; item < cantidad; item++)
                {
                    Inv.GetComponent<InventarioNuevo>().AgrregarItem(ItemID, 1);
                }   
            }
                
            ItemCompra[ItemID].cantidad -= cantidad;
        }
        else
        {
            insuficienteDinero.SetActive(true);
        }
    }

    public void VenderItem(int ItemID, int cantidad)
    {
        for (int i = 0; i < ItemCompra.Count; i++)
        {
            if (ItemCompra[i].ID == ItemID && ItemCompra[i].acumulable && ItemCompra[i].gameObject.activeInHierarchy)
            {
                Player.GetComponent<Player>().Money += DB.baseDatos[ItemID].precioVenta * cantidad;
                ItemsVendidos.Add(ItemCompra[i]);
                ItemCompra[i].cantidad += cantidad;
                Inv.GetComponent<InventarioNuevo>().EliminarItem(ItemID, cantidad, false);
                return;
            }

            if (ItemCompra[i].ID == 5)
            {
                Debug.Log("Agregaste un item que no estaba en la tienda");
                ItemCompra[i].ID = ItemID;
                ItemCompra[i].cantidad = cantidad;
                ItemCompra[i].gameObject.SetActive(true);
                ItemCompra[i].ActualizarItem();
                ItemsVendidos.Add(ItemCompra[i]);
                Player.GetComponent<Player>().Money += DB.baseDatos[ItemID].precioVenta * cantidad;
                Inv.GetComponent<InventarioNuevo>().EliminarItem(ItemID, cantidad, false);
                break;
            }
        }
    }

    public void EsconderItems(int caso)
    {
        for (int i = 0; i < ItemCompra.Count; i++)
        {
            if (caso == 0)       
            {
                itActivar.Clear();
                itActivar = ItemCompra.FindAll(x => x.ID != 5);
                foreach (ItemTienda itemA in itActivar)
                {
                    if (!itemA.gameObject.activeInHierarchy)
                    {
                        itemA.gameObject.SetActive(true);
                    }
                }
                return;
            }
            else
            {
                if(caso == 5)       //Items vendidos
                {
                    DesactivacionDeItems(caso);
                    foreach (ItemTienda itemVendido in ItemsVendidos)
                    {
                        if (!itemVendido.gameObject.activeInHierarchy)
                        {
                            itemVendido.gameObject.SetActive(true);
                        }
                    }
                    return;
                }
            }

            DesactivacionDeItems(caso);
            ActivacionDeItems(caso);

        }
    }

    void ActivacionDeItems(int numero)
    {
        itActivar.Clear();
        itActivar = ItemCompra.FindAll(x => x.clase == numero);
        foreach (ItemTienda itemA in itActivar)
        {
            if (!itemA.gameObject.activeInHierarchy)
            {
                itemA.gameObject.SetActive(true);
            }
        }
    }

    void DesactivacionDeItems(int numero)
    {
        itDesactivar.Clear();
        itDesactivar = ItemCompra.FindAll(x => x.clase != numero);
        foreach (ItemTienda item in itDesactivar)
        {
            if (item.gameObject.activeInHierarchy)
            {
                item.gameObject.SetActive(false);
            }
        }
    }

}
