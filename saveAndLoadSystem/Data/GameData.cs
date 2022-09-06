using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class GameData
{
    public Vector3 playerPos;


    public float speed;
    public float health;
    public float stamina;
    public float hunger;
    public float thirsty;
    public float fatigue;
  

    public float jumppower;
    public float reflectionIntensity;
    public float lightIntensity;
    public float tonemapExposurePre;
    public float tonemapBrightnessPost;
    public bool isNewGame;



    // this variables is loaded by default if no data game is found or if a new game begin
    public GameData()
    {
        //playerPos = Vector3.zero;

        this.speed = 2.5f;
        this.jumppower = 0.3f;
        this.health = 100;
        this.stamina = 100;

        this.hunger = 100;
        this.thirsty = 100;

        this.fatigue = 100;



        this.isNewGame = true;

    }
}
