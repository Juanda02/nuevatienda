using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;




public class InventarioNuevo : MonoBehaviour
{

    [System.Serializable]
    public struct ObjetoInvId
    {
        public int id;
        public int cantidad;

        public ObjetoInvId(int id, int cantidad)
        {
            this.id = id;
            this.cantidad = cantidad;
        }
    }

    [SerializeField]
    Database data;
    [Header("Variables del Drag and Drop")]

    [Tooltip("Aquí arrastra el Canvas")]
    public GraphicRaycaster graphRay;
    private PointerEventData pointerData;
    private List<RaycastResult> raycastResults;
    public static Transform canvas;                     //La volvemos static para utilizarlo en Item

    [Tooltip("Esto te mostrará el item el cual estas arrastrando")]
    public GameObject objetoSeleccionado;

    [Tooltip("Aquí verás el antiguo parent (padre) del objeto que estas arrastrando")]
    public Transform ExParent;
    [Space]

    [Header("Prefabs e Items")]
    
    public CartelEliminacion CE;
    public static GameObject Descripcion;

    [HideInInspector]
    public int OSC;

    [HideInInspector]
    public int OSID;
    public Transform Contenido;
    
    public Item item;
    private List<ObjetoInvId> inventarioo = new List<ObjetoInvId>();
    [HideInInspector]
    public List<ItemSuelto> itemsSueltos = new List<ItemSuelto>();
    [HideInInspector]
    public List<ItemSuelto> copiasItemsSueltos = new List<ItemSuelto>();
    [Space]
    [Header("Items Soltados")]
    [Tooltip("Aquí se arrastra un GameObject vacío en donde reapareceran los items eliminados del inventario")]
    public Transform ItemSueltoRespawn;
    [HideInInspector]
    public Vector3 originalPos;

    public ShopManager shopManager;

    void Start()
    {

        originalPos = transform.parent.position;

        InventoryUpdate();

        pointerData = new PointerEventData(null);
        raycastResults = new List<RaycastResult>();

        Descripcion = GameObject.Find("Descripcion");

        CE.gameObject.SetActive(false);

        canvas = graphRay.transform;
    }

    void Update()
    {
        Arrastrar();
    }

    void Arrastrar()
    {

        if (Input.GetMouseButtonDown(1))
        {
            pointerData.position = Input.mousePosition;
            graphRay.Raycast(pointerData, raycastResults);
            if (raycastResults.Count > 0)
            {
                if (raycastResults[0].gameObject.GetComponent<Item>())
                {
                    objetoSeleccionado = raycastResults[0].gameObject;
                    OSC = objetoSeleccionado.GetComponent<Item>().cantidad;
                    OSID = objetoSeleccionado.GetComponent<Item>().ID;
                    ExParent = objetoSeleccionado.transform.parent;
                    ExParent.GetComponent<Image>().fillCenter = false;
                    objetoSeleccionado.transform.SetParent(canvas);
                }
            }
        }


        if (objetoSeleccionado != null)
        {
            objetoSeleccionado.GetComponent<RectTransform>().localPosition = CanvasScreen(Input.mousePosition);
        }

        if (objetoSeleccionado != null)
        {
            if (Input.GetMouseButtonUp(1))
            {
                pointerData.position = Input.mousePosition;
                raycastResults.Clear();
                graphRay.Raycast(pointerData, raycastResults);
                objetoSeleccionado.transform.SetParent(ExParent);
                if (raycastResults.Count > 0)
                {
                    foreach (var resultado in raycastResults)
                    {
                        if (resultado.gameObject == objetoSeleccionado) continue;
                        if (resultado.gameObject.CompareTag("Slot"))
                        {
                            if (resultado.gameObject.GetComponentInChildren<Item>() == null)
                            {
                                objetoSeleccionado.transform.SetParent(resultado.gameObject.transform);
                                Debug.Log("Slot libre");
                            }
                        }
                        if (resultado.gameObject.CompareTag("Item"))
                        {
                            if (resultado.gameObject.GetComponentInChildren<Item>().ID == objetoSeleccionado.GetComponent<Item>().ID && objetoSeleccionado.gameObject.GetComponent<Item>().acumulable)
                            {
                                Debug.Log("Tienen el mismo ID");
                                resultado.gameObject.GetComponentInChildren<Item>().cantidad += objetoSeleccionado.GetComponent<Item>().cantidad;
                            }
                            else
                            {
                                Debug.Log("Distinto ID");
                                objetoSeleccionado.transform.SetParent(resultado.gameObject.transform.parent);
                                resultado.gameObject.transform.SetParent(ExParent);
                                resultado.gameObject.transform.localPosition = Vector3.zero;
                            }
                        }
                        if (resultado.gameObject.CompareTag("Eliminar"))
                        {
                            if (objetoSeleccionado.gameObject.GetComponent<Item>().cantidad >= 2)
                            {
                                CE.gameObject.SetActive(true);
                            }
                            else
                            {
                                CE.gameObject.SetActive(false);
                                EliminarItem(objetoSeleccionado.gameObject.GetComponent<Item>().ID, 1, false);
                            }
                        }
                        if (resultado.gameObject.CompareTag("Tienda"))
                        {
                            if (objetoSeleccionado.GetComponent<Item>().cantidad >= 2)
                            {
                                shopManager.compraMas.SetActive(true);
                                shopManager.compraMas.GetComponent<CompraMasItems>().id = objetoSeleccionado.gameObject.GetComponent<Item>().ID;
                                shopManager.compraMas.GetComponent<CompraMasItems>().slider.maxValue = objetoSeleccionado.gameObject.GetComponent<Item>().cantidad;
                                shopManager.compraMas.GetComponent<CompraMasItems>().compra = false;
                            }
                            else
                            {
                                shopManager.confCompra.SetActive(true);
                                shopManager.confCompra.GetComponent<confirmarCompra>().ID = objetoSeleccionado.gameObject.GetComponent<Item>().ID;
                                shopManager.confCompra.GetComponent<confirmarCompra>().cantidad = objetoSeleccionado.gameObject.GetComponent<Item>().cantidad;
                                shopManager.confCompra.GetComponent<confirmarCompra>().compra = false;
                            }

                        }
                    }
                }
                objetoSeleccionado.transform.localPosition = Vector3.zero;
                objetoSeleccionado = null;
            }
        }
        raycastResults.Clear();
    }

    public Vector2 CanvasScreen(Vector2 screenPos)
    {
        Vector2 viewportPoint = Camera.main.ScreenToViewportPoint(screenPos);
        Vector2 canvasSize = canvas.GetComponent<RectTransform>().sizeDelta;

        return (new Vector2(viewportPoint.x * canvasSize.x, viewportPoint.y * canvasSize.y) - (canvasSize / 2));
    }

    public void AgrregarItem(int id, int cantidad)
    {
        for (int i = 0; i < inventarioo.Count; i++)
        {
            if (inventarioo[i].id == id && data.baseDatos[id].acumulable)
            {
                inventarioo[i] = new ObjetoInvId(inventarioo[i].id, inventarioo[i].cantidad + cantidad);
                InventoryUpdate();
                return;
            }
        }

        if (!data.baseDatos[id].acumulable)
        {
            inventarioo.Add(new ObjetoInvId(id, 1));
        }
        else
        {
            inventarioo.Add(new ObjetoInvId(id, cantidad));
        }

        InventoryUpdate();
    }
    public void EliminarItem(int id, int cantidad, bool porUso)
    {
        for (int i = 0; i < inventarioo.Count; i++)
        {
            if (inventarioo[i].id == id)
            {
                if (!porUso)
                {
                    inventarioo[i] = new ObjetoInvId(inventarioo[i].id, inventarioo[i].cantidad - cantidad);

                    GameObject GO = Instantiate(data.baseDatos[id].Prefab, ItemSueltoRespawn.position, ItemSueltoRespawn.rotation);
                    GO.GetComponent<ItemSuelto>().ID = id;
                    GO.GetComponent<ItemSuelto>().cantidad = cantidad;
                }
                else
                {
                    inventarioo[i] = new ObjetoInvId(inventarioo[i].id, inventarioo[i].cantidad - cantidad);
                }

                if (inventarioo[i].cantidad <= 0)
                {
                    inventarioo.Remove(inventarioo[i]);
                }
            }
            InventoryUpdate();
            //break;
        }
    }

    List<Item> pool = new List<Item>();

    public void InventoryUpdate()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (i < inventarioo.Count)
            {
                ObjetoInvId o = inventarioo[i];
                pool[i].ID = o.id;                                                      //Aqui le asigno la id del objeto a mi script llamada Item
                pool[i].GetComponent<Image>().sprite = data.baseDatos[o.id].icono;
                pool[i].GetComponent<RectTransform>().localPosition = Vector3.zero;    
                pool[i].cantidad = o.cantidad;
                pool[i].Boton.onClick.RemoveAllListeners();
                pool[i].Boton.onClick.AddListener(() => gameObject.SendMessage(data.baseDatos[o.id].Void, SendMessageOptions.DontRequireReceiver));
                pool[i].gameObject.SetActive(true);
            }
            else
            {
                pool[i].gameObject.SetActive(false);
                pool[i]._descripcion.SetActive(false);
                pool[i].gameObject.transform.parent.GetComponent<Image>().fillCenter = false;
            }
        }
        if (inventarioo.Count > pool.Count)
        {
            for (int i = pool.Count; i < inventarioo.Count; i++)
            {
                Item it = Instantiate(item, Contenido.GetChild(0));         //Aqui el getchild(i) lo utilizo para crear el item dentro del slot
                pool.Add(it);

                if (Contenido.GetChild(0).childCount >= 2)
                {
                    for (int s = 0; s < Contenido.childCount; s++)
                    {
                        if (Contenido.GetChild(s).childCount == 0)
                        {
                            it.transform.SetParent(Contenido.GetChild(s));
                            break;
                        }
                    }
                }
                it.transform.position = Vector3.zero;
                it.transform.localScale = Vector3.one;

                ObjetoInvId o = inventarioo[i];
                pool[i].ID = o.id;                                                              
                pool[i].GetComponent<RectTransform>().localPosition = Vector3.zero;             
                pool[i].GetComponent<Image>().sprite = data.baseDatos[o.id].icono;
                pool[i].cantidad = o.cantidad;
                pool[i].Boton.onClick.RemoveAllListeners();
                pool[i].Boton.onClick.AddListener(() => gameObject.SendMessage(data.baseDatos[o.id].Void, SendMessageOptions.DontRequireReceiver));
                pool[i].gameObject.SetActive(true);
            }
        }
    }

    void Pocion()
    {
        Debug.Log("Pocion consumida");
        EliminarItem(0, 1, true);

    }

    void Manzana()
    {
        EliminarItem(2, 1, true);
        Debug.Log("Manzana consumida");
    }

}