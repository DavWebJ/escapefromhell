using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using BlackPearl;
using UnityEngine;

public class HUDShotgun : MonoBehaviour
{
    public static HUDShotgun instance = null;

    public Text ammo_text;

    public bool shotGunEquiped = false;
    public GameObject shotgunInputreload;
    public GameObject shotgunfireInput;
    public int amountAmmoIninventory = 0;
    public int maxAmmo = 6;
    public int currentAmmo = 0;
    private void Awake()
    {
        if (instance == null)
            instance = this;

        ammo_text = transform.Find("shotgun_ammos").GetComponentInChildren<Text>();


    }

    private void Start()
    {
        shotgunInputreload.SetActive(false);
        shotgunfireInput.SetActive(false);

        amountAmmoIninventory = CheckRemainingAmmoShotGunInInventory();
        currentAmmo = 0;
        InitRendererAmmo();
    }

    public void InitRendererAmmo()
    {

        gameObject.SetActive(false);
    }

    public int CheckRemainingAmmoShotGunInInventory()
    {
        return Inventory.instance.AmountItemInInventory(2);
    }
    public void ShowReloadInput()
    {
        shotgunInputreload.SetActive(true);
        shotgunfireInput.SetActive(true);
    }

    public void HideReloadInput()
    {
        shotgunInputreload.SetActive(false);
        shotgunfireInput.SetActive(false);
    }

    public void ShowHudAmmo()
    {
        gameObject.SetActive(true);
    }

    public void HideHudAmmo()
    {
        gameObject.SetActive(false);
    }



    void Update()
    {
        amountAmmoIninventory = CheckRemainingAmmoShotGunInInventory();
        if (currentAmmo <= 0)
        {
            currentAmmo = 0;

        }

        if (shotGunEquiped)
        {
            ammo_text.text = currentAmmo + " / " + maxAmmo;
        }
    }
}
