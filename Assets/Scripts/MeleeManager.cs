using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeManager : MonoBehaviour {
    public GameObject Weapon;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        UpdateWeaponOrientation();
    }

    private void UpdateWeaponOrientation()
    {
        Weapon.transform.rotation = Quaternion.Euler(GvrController.Orientation.x * 180 + 90, GvrController.Orientation.y * 180, 0);
    }
}
