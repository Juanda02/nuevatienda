using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataBase", menuName = "Inventoario/Lista", order = 1)]
public class Database : ScriptableObject
{
	[System.Serializable]
	public struct ObjetoInventario
	{
		public string nombre;
		public int ID;
        public GameObject Prefab;
		public Sprite icono;
        public int precio;
        public int precioVenta;
        public Clase clase;
		public Tipo tipo;
		public bool acumulable;
		public string descripcion;
        public string Void;

	}
    public enum Clase
    {
       Arma,
       Armadura,
       Pocion,
       Alimento

        
    }
    public enum Tipo
	{
		consumible,
		equipable
	}

	public ObjetoInventario[] baseDatos;

}


