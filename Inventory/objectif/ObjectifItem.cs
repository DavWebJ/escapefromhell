using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Inventory/Objectif/Create New Objectif")]
public class ObjectifItem : ScriptableObject
{

    [SerializeField] public string objectif;
    public int Id;

    public bool objectifIsValidate = false;


}
