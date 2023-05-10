using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
namespace BlackPearl
{
    
   public class HUDVitals : MonoBehaviour
   {
        public  static HUDVitals instance = null;
        [Header("Stamina")]
        [SerializeField] private GameObject stamina = null;
        [SerializeField] private Image stamina_fill = null;

        [Header("Health")]
        [SerializeField] private GameObject health = null;
        [SerializeField] private Image health_fill = null;

        [Header("Health hidden")]
        [SerializeField] private Image health_fill_hidden = null;

        [Header("Hunger")]
        [SerializeField] private GameObject hunger = null;
        [SerializeField] private Image hunger_fill = null;

        [Header("Thristy")]
        [SerializeField] private GameObject thirsty = null;
        [SerializeField] private Image thirsty_fill = null;

        [Header("Mental")]
        [SerializeField] private GameObject mental = null;
        [SerializeField] private Image mental_fill = null;


        private void Awake() {
            if(instance == null)
            {
                instance = this;
            }

        }

        private void Start()
        {
           
            
        }
        public void Ui_Stamina(float value,float max)
        {
            
            float percent = Inventory.instance.GetPercentage(value,max);
           if(stamina.activeInHierarchy)
                stamina_fill.fillAmount = percent;
           
            
        }

        public void Ui_Health(float value,float max)
        {
            
            float percent = Inventory.instance.GetPercentage(value,max);
            float currVal = health_fill.fillAmount;
            float health_hidden_currval = health_fill_hidden.fillAmount;

            health_fill.fillAmount = Mathf.Lerp(currVal, percent, Time.deltaTime * 10f);
            health_fill_hidden.fillAmount = Mathf.Lerp(health_hidden_currval, percent, Time.deltaTime * 10f);


        }

        public void Ui_Mental(float value, float max)
        {

            float percent = Inventory.instance.GetPercentage(value, max);
            float currVal = mental_fill.fillAmount;
            mental_fill.fillAmount = Mathf.Lerp(currVal, percent, Time.deltaTime * 10f);
            //mental_fill.fillAmount = percent;


        }

        public void Ui_Hunger(float value, float max)
        {

            float percent = Inventory.instance.GetPercentage(value, max);
            float currVal = hunger_fill.fillAmount;
            hunger_fill.fillAmount = Mathf.Lerp(currVal, percent, Time.deltaTime * 10f);
            //hunger_fill.fillAmount = percent;


        }

        public void Ui_Thirsty(float value, float max)
        {

            float percent = Inventory.instance.GetPercentage(value, max);
            float currVal = thirsty_fill.fillAmount;
            thirsty_fill.fillAmount = Mathf.Lerp(currVal, percent, Time.deltaTime * 10f);
            //thirsty_fill.fillAmount = percent;


        }




    }


    
}
