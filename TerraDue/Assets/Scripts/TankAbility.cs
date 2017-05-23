using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAbility : MonoBehaviour {
    //Attack power increased by 50% for 5 seconds, cooldown 10 seconds, unlock @lvl3
    //Parameters to check if the ability is available or not
    private float cooldown;
    private bool active;
    private float activeTime;
   
    float interval = 0.1f;
    float nextTime = 0;
    // Use this for initialization
    void Start()
    {
        this.cooldown = 10.0f;
        this.active = false;
        this.activeTime = 0.0f;
    }
    public bool cast()
    {
        if (this.cooldown == 0.0f)
        {
            this.active = true;
            this.cooldown = 10.0f;
            return this.active;
        }
        return this.active;
    }
    public bool endEffect()
    {
        if (this.activeTime == 5.0f)
        {
            this.active = false;
            this.activeTime = 0.0f;
            
            return this.active;
        }
        return this.active;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextTime)
        {
            //If the ability is active...
            if (this.active)
            {
                if (this.activeTime >= 5.0f)
                {
                    this.endEffect();
                    
                }
                else
                {
                    if (this.cooldown > 0.0f)
                    {
                        this.cooldown = 0.1f;
                    }
                    
                    this.activeTime += 0.1f;
                }
            }
            //If not...
            else
            {
                if (this.cooldown >= 0.0f)
                {
                    this.cooldown-= 0.1f;
                }
                
            }
            nextTime += interval;
        }
    }

}
