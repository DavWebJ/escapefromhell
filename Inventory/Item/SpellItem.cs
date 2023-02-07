using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BlackPearl
{
    [CreateAssetMenu(menuName = "Inventory/Item/Create Spell")]
    public class SpellItem : Item
    {

        public SpellActions spellActions = new SpellActions();
        public float coolDown = 0.5f;
        public float manaCost = 5;
        public GameObject fx;
        public SpellItem()
        {
       
            
            this.itemType = ItemType.Spell;
            
        }
    }

    [System.Serializable]
    public class SpellActions
    {
        public SpellActionType actionType;
        public int value = 0;
    }

    public enum SpellActionType
    {
        Heal,
        Attack,
        Defense,
        Armor,
        Fire,
        Ice,
        Poison,
        Necro,
        Golem
    }

}
