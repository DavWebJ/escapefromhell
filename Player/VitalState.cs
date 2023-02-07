using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using SUPERCharacter;
using System.Linq;
//using UnityEngine.Rendering.PostProcessing;
using UnityEngine;
using System;

namespace BlackPearl{
public class VitalState : MonoBehaviour {

        [Header("Reference")]
        private SUPERCharacterAIO player  = null;
        public float timer = 0;
        public GameObject hitprefab;

        [Header("vital state parameter")]
        public bool isdead = false;
        [Header("Stamina")]
        public float stamina = 100;
        public float StaminaReduceValue = 5f;
        public float StaminaRegainValue = 2.5f;
        public float staminaDecrease = 1;
        public float staminaIncrease = 1;
        public float staminaMax = 100;
        [Header("Health")]
        public float healthMax = 200;
        public float health = 100;
        [Header("Hunger")]
        public float hungerMax = 100;
        public float hunger;
        public float hungerDecrease;
        public bool useHungerStats = false;
        [Header("Thirsty")]
        public float thirstMax = 100;
        public float thirst;
        public float thirstDecrease;
        public bool useThirstyStats = false;
        [Header("Mental")]
        public float mental;
        public float mentalMax = 100;

        public float StaminaTimer =0;
        public bool isTired = false;


        public GameObject seringueArm;
        


        void Start () {

            health = 100;
            mental = mentalMax;
            hunger = hungerMax;
            thirst = thirstMax;
            player = GetComponent<SUPERCharacterAIO>();
            useHungerStats = true;
            useThirstyStats = true;

        }
	
	
	    void Update () {


           SetHealth();
            SetHunger();
            SetThirsty();
            SetMental();
  


        }

        private void SetMental()
        {
            if (mental >= mentalMax)
            {
                mental = mentalMax;
            }

            if(mental <= 0)
            {
                mental = 0;
            }
            HUDVitals.instance.Ui_Mental(mental, mentalMax);
        }

        private void SetThirsty()
        {
            if (useThirstyStats)
            {
                thirst -= Time.deltaTime / thirstDecrease;
            }

            if (thirst >= thirstMax)
            {
                thirst = thirstMax;
            }
            if (thirst <= 0)
            {
                useThirstyStats = false;
                thirst = 0;
            }
            HUDVitals.instance.Ui_Thirsty(thirst, thirstMax);
        }

        private void SetHunger()
        {
            if (useHungerStats)
            {
                hunger -= Time.deltaTime / hungerDecrease;
            }
            if (hunger >= hungerMax)
            {
                hunger = hungerMax;
            }
            if (hunger <= 0)
            {
                useHungerStats = false;
                hunger = 0;
            }
            HUDVitals.instance.Ui_Hunger(hunger, hungerMax);
        }

        public void SetHealth()
        {

            if(health >= healthMax)
            {
                health = healthMax;

            }
                HUDVitals.instance.Ui_Health(health, healthMax);
        }



        public bool CheckIfStatsIsFull(float stats,float max)
        {
            if(stats >= max)
            {
                return true;
            }
            return false;
        }

        public void ShowStatsFullMessage(bool healthFull,bool hungerFull,bool thirstyFull)
        {
            if (healthFull)
            {
                ScreenEventsManager.instance.SetVisualMessage("Votre vie est au maximum", ScreenEventsManager.instance.prf_inventory_message, ScreenEventsManager.instance.gridInventoryMessage);
            }
            else if (hungerFull)
            {
                ScreenEventsManager.instance.SetVisualMessage("Votre faim est au maximum", ScreenEventsManager.instance.prf_inventory_message, ScreenEventsManager.instance.gridInventoryMessage);
            }
            else if(thirstyFull)
            {
                ScreenEventsManager.instance.SetVisualMessage("Votre soif est au maximum", ScreenEventsManager.instance.prf_inventory_message, ScreenEventsManager.instance.gridInventoryMessage);
            }
            return;
        }

        public void AddHealth(float value)
        {

            health += value;
  
        }
        public void AddHunger(float value)
        {

            hunger += value;

        }

        public void AddThirsty(float value)
        {

            thirst += value;

        }


        public void TakeDamage(float damage)
        {

            if(isdead)
                return;

            
    
            AudioM.instance.PlayOneShotClip(AudioM.instance.VoicesAudiosource, AudioM.instance.Pain);

            // modelsAnimator.PlayTargetAnimation("hit",true);

                health -= damage;
        
            HUDVitals.instance.Ui_Health(health, healthMax);

            if (health <= 0)
            {
                health = 0;
        
                isdead = true;
                player.enableMovementControl = false;
                InputManager.instance.inventoryInputs.InventoryAction.Disable();
           

            }
        
        }

        public void PlayDieSound()
        {
            AudioM.instance.SetBackGroundAudioType(AudioBackGroundType.GameOver);
            HUD.instance.ScreenEffect("GameOver");
        }
        public void SetStamina()
        {
       

        }
    }
}
