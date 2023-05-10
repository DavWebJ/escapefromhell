using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackPearl
{
    [CreateAssetMenu(menuName = "Inventory/Item/CreateItem")]
    public class Item : ScriptableObject
    {
        public int ID;
        public string ItemName = "";
        [TextArea(10,15)]
        public string ItemDescription = "";
        public Sprite ItemIcon = null;
        public CategoryObjectType categoryObjectType;
        public Category itemCategory = Category.None;
        public ItemType itemType = ItemType.None;
        public float damage = 0;
        public int maxStack = 1;
        public bool stackable = false;
        public bool isPerimable = false;
        public bool isInspectable;
        public bool canUse;
        public bool canEquiped;
        public int backpackSize;
        public float timeToDestroy;
        public int maxBatery = 100;
        public GameObject ItemGroundPrefabs = null;
        public GameObject ArmPrefab = null;
        public float hungerAdd = 0;
        public float thirstyAdd = 0;
        public float healthAdd = 0;
        public bool canEat = false;
        public bool canDrink = false;
        public bool canHeal = false;
        [Header("Attributes")]
        public Attributes attributes = new Attributes();

        [Header("Crafting")]
        public Crafting crafting = new Crafting();


    }

    [System.Serializable]
    public class Crafting
    {
        public bool  isCraftable = false;
        public int resultQuantity = 1;
        public float timeToCraft = 1;
        public float timer = 0;
   
        public RecipeCraftItem[] recipeCraftItems = null;

    }

    [System.Serializable]
    public class RecipeCraftItem
    {
        public string CraftItemName = string.Empty;
        public int amountRequired = 1;

    }



    [System.Serializable]
    public class Attributes
    {
        public string name = string.Empty;
        public string type = string.Empty;
        public float value = 0;

        public string consumableType = string.Empty;
        public bool destroyItemAfterAction = false;
  
       

    }






}
