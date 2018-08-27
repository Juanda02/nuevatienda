using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Player : MonoBehaviour {

    public GameObject inventario;
    public GameObject Tienda;
    public MouseLook mouseLook;
    public int Money = 1000;

    void Start()
    {
        //mouseLook = gameObject.GetComponent<FirstPersonController>().m_MouseLook;
    }

    void Update () {

        if (Input.GetKeyDown(KeyCode.I))
        {
            inventario.SetActive(!inventario.activeInHierarchy);
            inventario.transform.parent.transform.position = inventario.GetComponent<InventarioNuevo>().originalPos;
        }

        if (inventario.activeInHierarchy || Tienda.activeInHierarchy)
        {
            mouseLook.SetCursorLock(false);
        }
        else
        {
            mouseLook.SetCursorLock(true);
        }

        if (!inventario.activeInHierarchy && inventario.GetComponent<InventarioNuevo>().objetoSeleccionado != null)
        {
            inventario.GetComponent<InventarioNuevo>().objetoSeleccionado.gameObject.transform.SetParent(inventario.GetComponent<InventarioNuevo>().ExParent);
            inventario.GetComponent<InventarioNuevo>().objetoSeleccionado.gameObject.transform.localPosition = Vector3.zero;
            inventario.GetComponent<InventarioNuevo>().objetoSeleccionado = null;
        }
    }
}
