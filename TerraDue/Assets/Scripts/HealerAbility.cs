using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerAbility : MonoBehaviour {
    //Selecting a target, it gains the 10% of its health points. It can be casted to himself.
    //Cooldown: 10seconds, unlock @lvl3
    // Use this for initialization

    //Parameters to check if the ability is available or not
    private float cooldown;
    private bool active;

    float interval = 0.1f;
    float nextTime = 0;


    void Start()
    {
        this.cooldown = 10.0f;
        this.active = false;
    }
    public bool cast()
    {
        if (this.cooldown == 0.0f)
        {
            this.cooldown = 10.0f;
            this.active = true;
            return this.active;
        }
        return this.active;
    }
    public void endEffect()
    {
        this.active = false;

    }
    // Update is called once per frame
    void Update()
    {

        if (Time.time >= nextTime)
        {
            //If the ability is active...
            if (this.active)
            {
                this.endEffect();

            }
            //If not...
            else
            {
                if (this.cooldown >= 0.0f)
                {
                    this.cooldown -= 0.1f;
                }

            }
            nextTime += interval;
        }
    }
}
