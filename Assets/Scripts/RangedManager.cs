using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedManager : MonoBehaviour {
    public GameObject Projectile;
    private bool isFiring;
    private GameObject currProjectile;
    public float DrawSpeed = 2;
    public int ArrowSpeed = 1;
    private float timeDrawn = 0;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        UpdateProjectile();
    }

    private void UpdateProjectile()
    {
        bool prevIsFiring = isFiring;
        GetIsFiring();
        if(isFiring)                                        //Drawing arrow
        {
            if(prevIsFiring == false)                       //Create arrow
            {
                GrabArrow();
            }
            else                                            //Increase draw
            {
                DrawArrow();
            }
        }
        else if(isFiring == false && prevIsFiring == true)  //Arrow fired
        {
            FireArrow();
        }
    }

    private void GetIsFiring()
    {
        if(GvrController.ClickButton && GameObject.FindGameObjectWithTag("Bow").activeSelf == true)
        {
            isFiring = true;
        }
        else
        {
            isFiring = false;
            /*if(currProjectile != null && GameObject.FindGameObjectWithTag("Bow").activeSelf == false)
            {
                Destroy(currProjectile);
            }*/
        }
    }

    private void GrabArrow()
    {
        currProjectile = Instantiate(Projectile, new Vector3(0, 0, 0),
            Quaternion.Euler(0,
            Camera.main.transform.rotation.eulerAngles.y, 0));
        currProjectile.transform.localPosition = new Vector3(Camera.main.transform.position.x + 0.275f, 0, Camera.main.transform.position.z + 3f);
        currProjectile.transform.SetParent(this.transform, false);
        timeDrawn = Time.deltaTime;
    }

    private void DrawArrow()
    {
        float newPos = currProjectile.transform.localPosition.y;
        if (currProjectile.transform.localPosition.y < 8)
        {
            newPos = newPos + (DrawSpeed * Time.deltaTime);
            if(newPos > 8)
            {
                newPos = 8;
            }
            Vector3 temp = currProjectile.transform.localPosition;
            temp.y = newPos;
            currProjectile.transform.localPosition = temp;
            timeDrawn = timeDrawn + Time.deltaTime;
        }
    }

    private void FireArrow()
    {
        currProjectile.GetComponent<Rigidbody>().useGravity = true;
        currProjectile.transform.SetParent(null, true);
        currProjectile.GetComponent<Rigidbody>().velocity = ArrowSpeed * Camera.main.transform.forward * timeDrawn;
        timeDrawn = 0;
    }
}
