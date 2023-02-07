using System.Collections;
using UnityEngine.UI;
using UnityEngine;

namespace BlackPearl
{
    public class HUDWeapon : MonoBehaviour
    {
        public static HUDWeapon instance = null;

        public Text ammo_text;

        public bool gunEquiped = false;
        public GameObject gunInputreload;
        public GameObject gunfireInput;
        public int amountAmmoIninventory = 0;
        public int maxAmmo = 9;
        public int currentAmmo = 0;

        private void Awake()
        {
            if(instance == null)
                instance = this;
          
                ammo_text = transform.Find("ammos").GetComponentInChildren<Text>();
  
   
        }

        private void Start()
        {
            gunInputreload.SetActive(false);
            gunfireInput.SetActive(false);

            amountAmmoIninventory = CheckRemainingAmmoGunInInventory();
            currentAmmo = 0;
            InitRendererAmmo();
        }

        public void InitRendererAmmo()
        {

            gameObject.SetActive(false);
        }

        public int CheckRemainingAmmoGunInInventory()
        {
            return Inventory.instance.AmountItemInInventory("ammo_gun");
        }


        public void ShowReloadInput()
        {
            gunInputreload.SetActive(true);
            gunfireInput.SetActive(true);
        }

        public void HideReloadInput()
        {
            gunInputreload.SetActive(false);
            gunfireInput.SetActive(false);
        }

        public void ShowHudAmmo()
        {
            gameObject.SetActive(true);
        }

        public void HideHudAmmo()
        {
            gameObject.SetActive(false);
        }



        private void Update()
        {
            amountAmmoIninventory = CheckRemainingAmmoGunInInventory();
            if (currentAmmo <= 0)
            {
                currentAmmo = 0;

            }

            if (gunEquiped)
            {
                ammo_text.text = currentAmmo + " / " + maxAmmo;
            }
        }


    }
}
