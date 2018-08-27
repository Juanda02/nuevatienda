using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CartelEliminacion : MonoBehaviour {

    [SerializeField]
    InventarioNuevo Inv;
    public Slider slider;
    public Text CantidadText;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

        if (this.gameObject.activeInHierarchy)
        {
            slider.maxValue = Inv.OSC;
            CantidadText.text = slider.value.ToString();
        }
	}

    public void Aceptar ()
    {
        Inv.EliminarItem(Inv.OSID, Mathf.RoundToInt(slider.value), false);
        slider.value = 1;
        this.gameObject.SetActive(false);
    }

    public void Cancelar()
    {
        slider.value = 1;
        this.gameObject.SetActive(false);
    }

}
