using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Objeto : MonoBehaviour {
    public int cantidad = 1;
    public Text textoCantidad;
    public int ID; 
    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        textoCantidad.text = cantidad.ToString(); 

    }
}
