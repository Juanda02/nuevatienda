using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

[RequireComponent(typeof(Button))]
[DisallowMultipleComponent]
public class Item : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public Database DB;
    public Text CantidadText;
    
    public int cantidad = 1;
    [HideInInspector]
    public int ID;
    [HideInInspector]
    public bool acumulable;
    public Button Boton;
    [HideInInspector]
    public GameObject _descripcion;
    [HideInInspector]
    public Text Nombre_;
    [HideInInspector]
    public Text Dato_;
   
    public Vector3 offset;

    // Use this for initialization
    void Start()
    {
        acumulable = DB.baseDatos[ID].acumulable;
        Boton = GetComponent<Button>();
        _descripcion = InventarioNuevo.Descripcion;
        Nombre_ = _descripcion.transform.GetChild(0).GetComponent<Text>();
        Dato_ = _descripcion.transform.GetChild(1).GetComponent<Text>();
        _descripcion.SetActive(false);
        if (!_descripcion.GetComponent<Image>().enabled)
        {
            _descripcion.GetComponent<Image>().enabled = true;
            Nombre_.enabled = true;
            Dato_.enabled = true;
        }

    }

    // Update is called once per frame
    void Update()
    {

        if (transform.parent.GetComponent<Image>() != null)
        {
            transform.parent.GetComponent<Image>().fillCenter = true;
        }

        CantidadText.text = cantidad.ToString();

        if(transform.parent == InventarioNuevo.canvas)
        {
            _descripcion.SetActive(false);
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _descripcion.SetActive(true);
        Nombre_.text = DB.baseDatos[ID].nombre;
        Dato_.text = DB.baseDatos[ID].descripcion;
        _descripcion.transform.position = transform.position + offset;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _descripcion.SetActive(false);
    }
}
