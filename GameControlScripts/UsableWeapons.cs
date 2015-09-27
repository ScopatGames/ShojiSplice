using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UsableWeapons : MonoBehaviour {

	//The lists are populated in the Unity Inspector.  
	public List<string> primaryUsableWeapons = new List<string>();
	public List<Rigidbody> worldPrimaryUsableWeapons = new List<Rigidbody>();
	public List<Transform> equippedPrimaryUsableWeapons = new List<Transform>();

	//<for enemies only> Percent chance the weapon will spawn... sum should = 100
	public List<float> percentChanceWeaponSpawn = new List<float> ();

	public List<string> secondaryUsableWeapons = new List<string>();
	public List<Rigidbody> worldSecondaryUsableWeapons = new List<Rigidbody>();
	public List<Transform> equippedSecondaryUsableWeapons = new List<Transform>();




}
