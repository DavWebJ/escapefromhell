using System.Collections;
using UnityEngine.UI;
using UnityEngine;
namespace BlackPearl
{
    public class HUDWeapon : MonoBehaviour
    {
        public static HUDWeapon instance = null;
        public Text weapont_text = null;
        private Text weapont_ammo_text = null;
        public Image  icon = null;
        public Text reload_text = null;
        public GameObject reload_go = null;

        [SerializeField] private Image batery_fill = null;
        public GameObject batery = null;
        public Sprite batery_normal = null;
        public Sprite batery_empty = null;
        public bool isFlashlight = false;
        [SerializeField] public GameObject action_flashlight = null;
        public bool canUseFlashLight = false;
        private void Awake()
        {
            if(instance == null)
                instance = this;
                
                weapont_ammo_text = transform.Find("ammos").GetComponent<Text>();
                icon  = transform.Find("icon").GetComponent<Image>();
                // icon.sprite = null;
                GetWeaponInfos(null);
                reload_go.SetActive(false);
                
   
        }

        public void GetWeaponInfos(Item item)
        {
            WeaponItem weapon = item as WeaponItem;
            
            if(weapon == null)
            {
                
                weapont_ammo_text.text = string.Empty;
                icon.sprite = null;
                gameObject.SetActive(false);
                isFlashlight = false;
                return;
            }
            gameObject.SetActive(true);
            if (weapon != null && weapon.itemType == ItemType.Weapon)
            {
                isFlashlight = false;
                batery.SetActive(false);
                
                weapont_ammo_text.text = weapon.ammo + " / " + weapon.max_ammo;
                Item Ammo = GameManager.instance.resources.GetitemByName("AmmoGun");
                icon.sprite = Ammo.ItemIcon;
            }
            if (weapon != null && weapon.itemType == ItemType.FlashLight)
            {
                isFlashlight = true;
                batery.SetActive(true);
                weapont_ammo_text.text = (int)weapon.batery + " / " + weapon.batery_max;
            }


        }



        public void Ui_Batery(float value,float max)
        {

            float percent = Inventory.instance.GetPercentage(value,max);
           
            batery_fill.fillAmount = percent;
            batery_fill.color = HUD.instance.bateryBarColor.Evaluate(percent);
            
        }

        public void ShowReload(bool active,string message)
        {
    
            reload_go.SetActive(active && isFlashlight);
            reload_text.text = message;
        }


        private void Update()
        {
            action_flashlight.SetActive(isFlashlight && canUseFlashLight);
        }


    }
}
