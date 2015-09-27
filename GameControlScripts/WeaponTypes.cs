using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponTypes : MonoBehaviour {

	//The lists are populated in the Unity Inspector.  Dictionaries are built from these lists in Start()
	public List<string> primaryWeaponKey = new List<string>();
	public List<Rigidbody> worldPrimaryWeaponValue = new List<Rigidbody>();
	public List<Transform> equippedPrimaryWeaponValue = new List<Transform>();
	public List<Transform> enemyPrimaryWeaponValue = new List<Transform>();
	public List<string> secondaryWeaponKey = new List<string>();
	public List<Rigidbody> worldSecondaryWeaponValue = new List<Rigidbody>();
	public List<Transform> equippedSecondaryWeaponValue = new List<Transform>();


	public Dictionary<string, Rigidbody> worldPrimaryWeaponDictionary = new Dictionary<string, Rigidbody>();
	public Dictionary<string, Rigidbody> worldSecondaryWeaponDictionary = new Dictionary<string, Rigidbody> ();
	public Dictionary<string, Transform> equippedPrimaryWeaponDictionary = new Dictionary<string, Transform> ();
	public Dictionary<string, Transform> enemyPrimaryWeaponDictionary = new Dictionary<string, Transform> ();
	public Dictionary<string, Transform> equippedSecondaryWeaponDictionary = new Dictionary<string, Transform> ();


	void Awake()
	{
		//Build dictionary for world based primary weapons
		for(int i = 0; i < primaryWeaponKey.Count; i++)
		{
			worldPrimaryWeaponDictionary.Add(primaryWeaponKey[i],worldPrimaryWeaponValue[i]);
		}
		//Debug.Log ("Built worldPrimaryWeaponDictionary.");

		//Build dictionary for world based secondary weapons
		for(int i = 0; i < secondaryWeaponKey.Count; i++)
		{
			worldSecondaryWeaponDictionary.Add(secondaryWeaponKey[i],worldSecondaryWeaponValue[i]);
		}
		//Debug.Log ("Built worldSecondaryWeaponDictionary.");

		//Build dictionary for equipped primary weapons
		for(int i = 0; i < primaryWeaponKey.Count; i++)
		{
			equippedPrimaryWeaponDictionary.Add(primaryWeaponKey[i],equippedPrimaryWeaponValue[i]);
		}
		//Debug.Log ("Built equippedPrimaryWeaponDictionary.");

		//Build dictionary for enemy primary weapons
		for(int i = 0; i < primaryWeaponKey.Count; i++)
		{
			enemyPrimaryWeaponDictionary.Add(primaryWeaponKey[i],enemyPrimaryWeaponValue[i]);
		}
		//Debug.Log ("Built enemyPrimaryWeaponDictionary.");
		
		//Build dictionary for equipped secondary weapons
		for(int i = 0; i < secondaryWeaponKey.Count; i++)
		{
			equippedSecondaryWeaponDictionary.Add(secondaryWeaponKey[i],equippedSecondaryWeaponValue[i]);
		}
		//Debug.Log ("Built equippedSecondaryWeaponDictionary.");


	}
}
