using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour {
    public GameObject[] Weapons;
    private bool prevTouch = false;
    private float touchPosX;

	// Use this for initialization
	void Start () {
        Weapons[0].SetActive(true);
        Weapons[1].SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        GetTouchPos();
	}

    private void ToggleWeapon()
    {
        if(Weapons[0].activeSelf == true)
        {
            Weapons[0].SetActive(false);
            Weapons[1].SetActive(true);
        }
        else
        {
            Weapons[0].SetActive(true);
            Weapons[1].SetActive(false);
        }
    }

    private void GetTouchPos()
    {
        if(GvrController.IsTouching == true && prevTouch == false)
        {
            prevTouch = GvrController.IsTouching;
            touchPosX = GvrController.TouchPos.x;
        }
        else if(GvrController.IsTouching == true && prevTouch == true)
        {
            if(GvrController.TouchPos.x <= (touchPosX - 0.5) || GvrController.TouchPos.x >= (touchPosX + 0.5))
            {
                ToggleWeapon();
                prevTouch = false;
                touchPosX = 0;
            }
        }
        else if(GvrController.IsTouching == false)
        {
            prevTouch = false;
            touchPosX = 0;
        }
    }
}
