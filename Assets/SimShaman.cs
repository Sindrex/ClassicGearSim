using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimShaman : MonoBehaviour {

    //Shaman, Flurry based, 2h

    public GameController GC;

    public Text dpsText;
    public Text attackText;
    public float waitTime = 0.05f;

    public Item[] gear;

    public int maxEpochs;
    public int currentEpoch;
    public int maxTimer; //sec
    public float timer; //seconds and millis since start (2.8)
    public float weaponTimer;
    public float damage;
    public float totdps;
    public int totAttacks;
    public int attacks;

    //Calculated
    float dps;
    float crit;
    int hit;
    float weaponSwingTime;
    bool flurry;
    int flurryCount;
    int flurryMax = 3;

    //EzSim
    private readonly float wf = 466 / 14; //466
    private readonly float attackFactor = 1.06f;
    //private readonly float flurryDpsFactor = 0.85f;
    private readonly float flurryAttackFactor = 0.84f;

    private bool checkSet(ItemSet set, int minDps, int minCrit, int minHit)
    {
        if (set.hit < minHit || (set.dps < minDps && set.crit < minCrit))
        {
            //print(">>> Not enough hit/dps/crit!");
            dpsText.text = "hit < " + minHit + "%!";
            return false;
        }
        return true;
    }

    public Simulation easySim(int time, int minHit, int minDps, int minCrit, ItemSet set)
    {
        float dps = set.dps;
        float crit = set.crit;
        int hit = set.hit;
        float swing = set.swing;

        if(!checkSet(set, minDps, minCrit, minHit))
        {
            return new Simulation(0,0);
        }
        set = null;

        crit = crit / 100;

        //  120       0.4*120 = 48             10/100 * 0.85 * 120 = 10  
        float att = time / swing + 0.2f * 2 * time / swing + crit * flurryAttackFactor * (time / swing) + 0.2f * 2 * crit * flurryAttackFactor * (time / swing);
        //print("Attack: " + att);
        int attacks = (int)Mathf.Ceil(
            att
        );
        attackText.text = "" + attacks;
        //print("Attacks: " + (time / swing));

        float flurryDpsFactor = 0.85f;
        if(crit <= 0.09)
        {
            //print("Flurry 1");
            flurryDpsFactor = 0.08f * Mathf.Log(crit) + 1.05f;
        }
        else if (crit <= 0.18)
        {
            //print("Flurry 2");
            flurryDpsFactor = 0.853f;
        }
        else
        {
            //print("Flurry 3");
            flurryDpsFactor = -0.1151f * crit * crit - 0.4023f * crit + 0.9309f;
        }
        //print("FlurryDpsFactor: " + flurryDpsFactor);
        float realDPS = dps + 0.2f*2*(dps+wf+crit*dps) + crit*dps + crit*flurryDpsFactor*dps + 0.2f*2*crit*flurryDpsFactor*dps + crit*crit*flurryDpsFactor*dps;
        dpsText.text = "" + realDPS;

        Simulation sim = new Simulation(realDPS, attacks);

        return sim;
    }

    public Simulation simulate(int epochs, int maxTimer, Item[] gear, int minHit, int minDps, int minCrit)
    {
        //print(">>> Starting simulation!");
        timer = 0;
        weaponTimer = 0;
        damage = 0;
        totdps = 0;
        flurry = false;
        flurryCount = flurryMax;
        dps = 0;
        crit = 0;
        hit = 0;
        attacks = 0;

        maxEpochs = epochs;
        this.maxTimer = maxTimer;

        this.gear = gear;
        ItemSet set = new ItemSet(gear, GC.isOrc.isOn);
        dps = set.dps;
        crit = set.crit;
        hit = set.hit;
        weaponSwingTime = set.swing;

        if (!checkSet(set, minDps, minCrit, minHit))
        {
            return new Simulation(0, 0);
        }

        weaponTimer = weaponSwingTime;

        //print(">>> Sim: Base dps/crit/swing: " + dps + "/" + crit + "/" + weaponSwingTime);

        currentEpoch = 0;
        return epoch();
    }

    Simulation epoch()
    {
        millisec();

        if(currentEpoch >= maxEpochs)
        {
            //print(">>> All epochs finished!");

            float avg = totdps / maxEpochs;

            dpsText.text = "" + avg;
            attacks = attacks / maxEpochs;
            attackText.text = "" + attacks;

            Simulation sim = new Simulation(avg, attacks);

            return sim;
        }
        float dps = damage / maxTimer;
        if (currentEpoch % 10 == 0)
        {
            //print(">>> Epoch " + currentEpoch + " finished! Dps: " + dps);
        }
        dpsText.text = "" + dps;
        totdps += dps;
        damage = 0;

        currentEpoch++;
        return epoch();
    }

    void millisec()
    {
        //Weapon swing
        //print("Timer: " + timer + ", weaponTimer: " + weaponTimer);
        if (weaponTimer >= weaponSwingTime || (flurry && weaponTimer >= weaponSwingTime * 0.7f))
        {
            weaponSwing(0);
            int random = Random.Range(1, 100);
            if (random <= 20 && GC.wfToggle.isOn)
            {
                weaponSwing(wf);
                weaponSwing(wf);
            }
        }
        else
        {
            weaponTimer += 0.1f;
        }

        if (timer >= maxTimer)
        {
            timer = 0;
            weaponTimer = 0;
            return;
        }
        timer += 0.1f;
        millisec();
    }

    void weaponSwing(float wfExtra)
    {
        if (maxEpochs < 50)
        {
            print("Weaponswing at Timer: " + timer);
        }
        if (flurryCount > 0)
        {
            flurryCount--;
            if (flurryCount == 0)
            {
                flurry = false;
            }
        }

        int factor = 1;
        int random = Random.Range(1, 1000);
        if (random <= crit * 10)
        {
            if(GC.critToggle.isOn) factor = 2;
            if(GC.flurryToggle.isOn) flurry = true;
            flurryCount = flurryMax;
            //print("Flurry!");
        }
        damage += dps * weaponSwingTime * factor + wfExtra;
        attacks++;

        weaponTimer = 0.1f;
    }
}

public class Simulation
{
    public float dps;
    public float attacks;

    public Simulation(float dps, float attacks)
    {
        this.dps = dps;
        this.attacks = attacks;
    }
}