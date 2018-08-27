using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class ItemTienda : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{

    public Database DB;
    
    public ShopManager SM;
    
    public int clase;
    public int ID;
   
    public int precio;
    public int cantidad;

    public bool acumulable;
    private Image icono;
    private Text precioText;
    private Text CantidadText;
    public GameObject _descripcion;
    private Text Nombre_;
    private Text Dato_;
  
    public Vector3 offset;
    GameObject cartelConf;
    GameObject compraMas;
    
	void Start () {

        SM = transform.parent.transform.parent.GetComponent<ShopManager>();
        precioText = transform.GetChild(0).gameObject.GetComponent<Text>();
        CantidadText = transform.GetChild(1).gameObject.GetComponent<Text>();

        Nombre_ = _descripcion.transform.GetChild(0).GetComponent<Text>();
        Dato_ = _descripcion.transform.GetChild(1).GetComponent<Text>();
        _descripcion.SetActive(false);

        acumulable = DB.baseDatos[ID].acumulable;

        switch (DB.baseDatos[ID].clase)
        {
            case Database.Clase.Arma:
                clase = 1;
                break;
            case Database.Clase.Armadura:
                clase = 2;
                break;
            case Database.Clase.Pocion:
                clase = 3;
                break;
            case Database.Clase.Alimento:
                clase = 4;
                break;
            default:
                clase = 0;
                break;
        }

        cartelConf = SM.confCompra;
        compraMas = SM.compraMas;
        precio = DB.baseDatos[ID].precio;
        icono = GetComponent<Image>();
        icono.sprite = DB.baseDatos[ID].icono;
        precioText.text = precio.ToString();

    }
	
	public void OnPointerClick (PointerEventData eventData) {

        if(cantidad <= 1)
        {
            cartelConf.SetActive(true);
            cartelConf.GetComponent<confirmarCompra>().ID = ID;
            cartelConf.GetComponent<confirmarCompra>().cantidad = cantidad;
            cartelConf.GetComponent<confirmarCompra>().compra = true;
        }
        else
        {
            compraMas.SetActive(true);
            compraMas.GetComponent<CompraMasItems>().id = ID;
            compraMas.GetComponent<CompraMasItems>().slider.maxValue = cantidad;
            compraMas.GetComponent<CompraMasItems>().compra = true;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(ID != 5)
        {
            _descripcion.SetActive(true);
            Nombre_.text = DB.baseDatos[ID].nombre;
            Dato_.text = DB.baseDatos[ID].descripcion;
            _descripcion.transform.position = transform.position + offset;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _descripcion.SetActive(false);
    }

    void Update()
    {

        if (transform.parent == InventarioNuevo.canvas)
        {
            _descripcion.SetActive(false);
        }
            
        if(ID == 5)
        {
            icono.enabled = false;
            precioText.enabled = false;
            CantidadText.enabled = false;
        }
        else
        {
            icono.enabled = true;
            precioText.enabled = true;
            CantidadText.enabled = true;
        }

        if(cantidad <= 0)
        {
            cantidad = 0;
            this.gameObject.SetActive(false);
        }
        else
        {
            this.gameObject.SetActive(true);
            CantidadText.text = cantidad.ToString();
        }
    }

    public void ActualizarItem()
    {
        print("Item actualizado");
        switch (DB.baseDatos[ID].clase)
        {
            case Database.Clase.Arma:
                clase = 1;
                break;
            case Database.Clase.Pocion:
                clase = 3;
                break;
            case Database.Clase.Armadura:
                clase = 2;
                break;
            case Database.Clase.Alimento:
                clase = 4;
                break;
            default:
                clase = 0;
                break;
        }

        acumulable = DB.baseDatos[ID].acumulable;
        precio = DB.baseDatos[ID].precio;
        icono = GetComponent<Image>();
        icono.sprite = DB.baseDatos[ID].icono;
        precioText = GetComponentInChildren<Text>();
        precioText.text = precio.ToString();
    }
}
