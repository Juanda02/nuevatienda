using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class confirmarCompra : MonoBehaviour {

    [SerializeField]
   public Database db;
   public ShopManager SM;
   public Text texto;
    
    public int ID;
    
    public int cantidad;
  
    public bool compra = true;


	void Start () {

        texto = gameObject.GetComponentInChildren<Text>();
        SM = transform.parent.GetComponent<ShopManager>();

	}
	
	void Update () {

        if (compra)
        {
            texto.text = "¿Comprar " + cantidad + " " + db.baseDatos[ID].nombre + " por un valor de " + db.baseDatos[ID].precio * cantidad + "?";
        }
        else
        {
            texto.text = "¿Vender " + cantidad + " " + db.baseDatos[ID].nombre + " por un valor de " + db.baseDatos[ID].precioVenta * cantidad + "?";
        }


    }

    public void Aceptar()
    {
        if (compra)
        {
            SM.ComprarItem(ID, cantidad);
        }
        else
        {
            SM.VenderItem(ID, cantidad);
        }
        this.gameObject.SetActive(false);
    }

    public void Cancelar()
    {
        this.gameObject.SetActive(false);
        print("Compra cancelada");
    }
}
