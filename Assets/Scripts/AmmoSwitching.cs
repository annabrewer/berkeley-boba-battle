
using UnityEngine;

public class AmmoSwitching : MonoBehaviour
{
    public int selectedAmmo = 0;
    public GameObject gun;

 
    private bool switch_button = false;
    // Start is called before the first frame update
    void Start()
    {

        SelectAmmo();
    }

    // Update is called once per frame
    void Update()
    {
        int previousSelectedAmmo = selectedAmmo;
        switch_button = OVRInput.GetDown(OVRInput.Button.Two);
        if (switch_button==true)
        {
            if (selectedAmmo >= transform.childCount - 1)
                selectedAmmo = 0;
            else
                selectedAmmo++;
        }
        //could do button X for back
        if (previousSelectedAmmo != selectedAmmo)
        {
            SelectAmmo();
        }
    }

    //enable selected ammo only
    void SelectAmmo()
    {
        int i = 0;
        foreach (Transform ammo in transform)
        {
            if (i == selectedAmmo)
            {
                ammo.gameObject.SetActive(true);
                //put it into bullet field of gun
                GunProjectile gunScript = gun.GetComponent<GunProjectile>();
                gunScript.bullet = ammo.gameObject;
            }
            else
                ammo.gameObject.SetActive(false);
            i++;
        }
    }
}
