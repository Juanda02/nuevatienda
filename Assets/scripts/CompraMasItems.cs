using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompraMasItems : MonoBehaviour {

    [SerializeField]
   public ShopManager tienda;
    public Slider slider;
    public Text Texto;
    public Text CantidadText;
   
    public bool compra = true;
 
    public int id;
   public GameObject cartelConf;

    // Use this for initialization
    void Start () {

        tienda = transform.parent.GetComponent<ShopManager>();
        slider = gameObject.GetComponentInChildren<Slider>();
        Texto = transform.GetChild(0).gameObject.GetComponent<Text>();
        CantidadText = transform.GetChild(1).gameObject.GetComponent<Text>();
        cartelConf = tienda.confCompra;

    }
	
	// Update is called once per frame
	void Update () {

        if (this.gameObject.activeInHierarchy)
        {
            CantidadText.text = slider.value.ToString();
        }

        if (compra)
        {
            Texto.text = "¿Cuánto deseas comprar?";
        }
        else
        {
            Texto.text = "¿Cuánto deseas vender?";
        }

	}

    public void Aceptar ()
    {
        cartelConf.SetActive(true);
        cartelConf.GetComponent<confirmarCompra>().ID = id;
        cartelConf.GetComponent<confirmarCompra>().cantidad = Mathf.RoundToInt(slider.value);
        slider.value = 1;
        if (compra)
        {
            cartelConf.GetComponent<confirmarCompra>().compra = true;
        }
        else
        {
            cartelConf.GetComponent<confirmarCompra>().compra = false;
        }
        this.gameObject.SetActive(false);
    }

    public void Cancelar()
    {
        slider.value = 1;
        this.gameObject.SetActive(false);
    }

}
