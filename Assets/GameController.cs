using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public float waitPerSim;
    public Dropdown[] dropdowns; //in order listed below
    public InputField inputEpoch;
    public InputField inputHit;
    public InputField inputTime;
    public InputField inputMinDps;
    public InputField inputMinCrit;
    public Toggle flurryToggle;
    public Toggle wfToggle;
    public Toggle critToggle;
    public Toggle easySimAllToggle;
    public Toggle isOrc;
    public SimShaman sim;
    public Text iNumber;
    public Dropdown phase;
    public Text comboNr;
    public Text dpsText;
    public Text critText;
    public Text swingText;
    public Text hitText;

    Item[][] currentGearSet;

    Item[][] none;
    Item[][] preRaidGear;
    Item[][] MCGear;
    Item[][] DMGear;
    Item[][] WBGear;
    Item[][] BGGear;
    Item[][] BWLGear;
    Item[][] ZGGear;
    Item[][] DragonGear;
    Item[][] AQGear;
    Item[][] NaxxGear;

    List<List<Item>> bestCombos;
    List<ItemSet> bestCombosItemSet;
    public Dropdown bestSetDropdown;
    long comboCount = 0;
    int printIndex = 10000;

    // Use this for initialization
    void Start () {
        noneInit();
        preRaidInit();
        MCInit();
        DMInit();
        WBInit();
        BWLInit();
        ZGInit();

        //currentGearSet = joinItemArray(preRaidGear, DMGear);
        List<String> phaseOptions = new List<string>();
        phaseOptions.Add("None");
        phaseOptions.Add("P1: PreRaid");
        phaseOptions.Add("P1: MC");
        phaseOptions.Add("P1.5: PreRaid + DM");
        phaseOptions.Add("P1.5: MC + DM");
        phaseOptions.Add("P2: MC + World Bosses/Honor");
        phaseOptions.Add("P3: BWL");
        phaseOptions.Add("P4: ZG");
        phase.ClearOptions();
        phase.AddOptions(phaseOptions);

        currentGearSet = none;

        foreach (Item[] arr in currentGearSet)
        {
            foreach(Item it in arr)
            {
                //print(it.name);
            }
        }

        for (int i = 0; i < dropdowns.Length; i++)
        {
            dropdowns[i].ClearOptions();
            List<string> itemNames = new List<string>();
            foreach (Item it in currentGearSet[i])
            {
                itemNames.Add(it.name);
            }
            dropdowns[i].AddOptions(itemNames);
        }

        onChangePhase();
        onChangeItem();
    }
	
    void noneInit()
    {
        none = new Item[][]{
            new Item[] { new Item("N/A Head", 0, 0, 3) },
            new Item[] { new Item("N/A Neck", 0, 0) },
            new Item[] { new Item("N/A Shoulders", 0, 0) },
            new Item[] { new Item("N/A Cloak", 0, 0) },
            new Item[] { new Item("N/A Chest", 0, 0) },
            new Item[] { new Item("N/A Bracers", 0, 0) },
            new Item[] { new Item("N/A Hands", 0, 0) },
            new Item[] { new Item("N/A Waist", 0, 0) },
            new Item[] { new Item("N/A Legs", 0, 0) },
            new Item[] { new Item("N/A Feet", 0, 0) },
            new Item[] { new Item("N/A Ring1", 0, 0) },
            new Item[] { new Item("N/A Ring2", 0, 0) },
            new Item[] { new Item("N/A Trink1", 0, 0) },
            new Item[] {
                new Item("Crit 0%", 0, 0),
                new Item("Crit 1%", 0, 1),
                new Item("Crit 2%", 0, 2),
                new Item("Crit 3%", 0, 3),
                new Item("Crit 4%", 0, 4),
                new Item("Crit 5%", 0, 5),
                new Item("Crit 6%", 0, 6),
                new Item("Crit 7%", 0, 7),
                new Item("Crit 8%", 0, 8),
                new Item("Crit 9%", 0, 9),
                new Item("Crit 10%", 0, 10),
                new Item("Crit 11%", 0, 11),
                new Item("Crit 12%", 0, 12),
                new Item("Crit 13%", 0, 13),
                new Item("Crit 14%", 0, 14),
                new Item("Crit 15%", 0, 15),
                new Item("Crit 16%", 0, 16),
                new Item("Crit 17%", 0, 17),
                new Item("Crit 18%", 0, 18),
                new Item("Crit 19%", 0, 19),
                new Item("Crit 20%", 0, 20),
                new Item("Crit 30%", 0, 30),
                new Item("Crit 40%", 0, 40),
                new Item("Crit 50%", 0, 50),
                new Item("Crit 60%", 0, 60),
                new Item("Crit 70%", 0, 70),
                new Item("Crit 80%", 0, 80),
                new Item("Crit 90%", 0, 90),
                new Item("Crit 100%", 0, 100)
            },
            new Item[] {
                new Item("Wep1-1-3-1", 1, 1, 3, 1),
                new Item("Wep1-0-3-1", 1, 0, 3, 1),
                new Item("Wep2-0-3-1", 2, 0, 3, 1),
                new Item("Wep1-2-3-1", 1, 2, 3, 1)
            },
            new Item[] { new Item("N/A Wep2", 0, 0) }
        };
    }
    void preRaidInit()
    {
        preRaidGear = new Item[][]{
            new Item[] { //0
                new Item("Eye of Rend", 1.9f, 2),
                new Item("Crown of Tyranny", 2.9f, 1),
                new Item("Mask of the Unforgiven", 0, 1, 2),
            },
            new Item[] { //1
                new Item("Mark of Fordring", 1.9f, 1),
                new Item("Imperial Jewel", 2.3f, 0)
            },
            new Item[] { //2
                new Item("Black Dragonsc. Sh.", 2.9f, 0),
                new Item("Truestrike Shoulders", 1.7f, 0, 2),
                new Item("Wyrmtongue Shoulders", 0, 1.2f),
                new Item("Death's Clutch", 1.4f, 0.5f)
            },
            new Item[] { //3
                new Item("Cape of the Black Baron", 1.4f, 0.8f),
                new Item("Phantasmal Cloak", 1.7f, 0)
            },
            new Item[] { //4
                new Item("Savage Gladiator's Chain", 1.9f, 2.7f),
                new Item("Cadaverous Armor", 5.4f, 0.4f),
                new Item("Black Dragonsc. Chest", 3.6f, 0),
            },
            new Item[] { //5
                new Item("Bracers of the Stone Princess", 2.1f, 0),
                new Item("Slashclaw Bracers", 0, 0.4f, 1),
                new Item("Blackmist Armguards", 0.7f, 0, 1),
                new Item("Shadowcraft Bracers", 0, 0.8f)
            },
            new Item[] { //6
                new Item("Devilsaur Gauntlets", 2, 1),
                new Item("Cadaverous Gloves", 3.1f, 0),
                new Item("Gargoyle Slashers", 1.4f, 1.3f),
                new Item("Voone's Vice Grips", 0, 0.4f, 2)
            },
            new Item[] { //7
                new Item("Girdle of Bestial Fury", 3.3f, 0),
                new Item("Cloudrunner Girdle", 2, 0.8f),
                new Item("Vosh'gajin's Strand", 1.3f, 1f)
            },
            new Item[] { //8
                new Item("Devilsaur Leggings", 3.3f, 1),
                new Item("Black Dragonsc. Leg.", 3.9f, 0),
                new Item("Legguards of Chromatic Defiler", 1.3f, 1.7f),
                new Item("Bladermaster Leggings", 0, 1.3f, 1),
            },
            new Item[] { //9
                new Item("Pads of the Dread Wolf", 2.9f, 0),
                new Item("Bloodmail Boots", 1.3f, 0.5f, 1),
                new Item("Beaststalker's Boots", 0, 1.1f),
                new Item("Savage Glad. Greaves", 1.4f, 0.8f)
            },
            new Item[] { //10
                new Item("Blackstone Ring", 1.4f, 0, 1, true),
                new Item("Painweaver Band", 1.1f, 1, true),
            },
            new Item[] { //11
                new Item("Blackstone Ring", 1.4f, 0, 1, true),
                new Item("Painweaver Band", 1.1f, 1, true),
            },
            new Item[] {
                new Item("Hand of Justice", 1.4f, 0, true),
            },
            new Item[] {
                new Item("Blackhand's Breadth", 0, 2, true),
                new Item("Rune of the Guard Captain", 1.4f, 0, 1, true),
            },
            new Item[] {
                new Item("Arcanite Reaper", 58.2f, 0, 0, 3.8f, true)
            },
            new Item[] {
                new Item("N/A", 0, 0, 0)
            }
        };

        //1: head, 2

        preRaidGear[2][0].hasSet = true;
        preRaidGear[4][2].hasSet = true;
        preRaidGear[6][0].hasSet = true;
        preRaidGear[8][0].hasSet = true;
        preRaidGear[8][1].hasSet = true;
    }
    void MCInit()
    {
        MCGear = new Item[][]{
            new Item[] {
                new Item("Crown of Destruction", 3.1f, 2)
            },
            new Item[] {
                new Item("Onyxia Tooth Pendant", 0, 1.6f, 1),
            },
            new Item[] {
            },
            new Item[] {
            },
            new Item[] {
            },
            new Item[] {
                new Item("Wristguards of Stability", 3.4f, 0),
                new Item("Wristguards of the True Flight", 0, 1, 1)
            },
            new Item[] {
                new Item("Aged Core Leather Gloves", 2.1f, 1)
            },
            new Item[] {
            },
            new Item[] {
            },
            new Item[] {
            },
            new Item[] {
                new Item("Band of Accuria", 0, 0.8f, 2, true),
                new Item("Quick Strike Ring", 2.9f, 1, true),
            },
            new Item[] {
                new Item("Band of Accuria", 0, 0.8f, 2, true),
                new Item("Quick Strike Ring", 2.9f, 1, true),
            },
            new Item[] {
            },
            new Item[] {
            },
            new Item[] {
                new Item("Spinal Reaper", 74.7f, 0, 0, 3.8f, true)
            },
            new Item[] {
            }
        };
    }
    void DMInit()
    {
        DMGear = new Item[][]{
            new Item[] { //head
            },
            new Item[] { //neck
                new Item("Beads of Ogre Might", 1.7f, 0, 1)
            },
            new Item[] { //shoulder
                new Item("Flamescarred Shoulders", 1.7f, 0.6f)
            },
            new Item[] { //back
                new Item("Shadewood Cloak", 1.9f, 0f)
            },
            new Item[] { //chest
            },
            new Item[] { //bracers
                new Item("Bracers of the Eclipse", 1.7f, 0.5f)
            },
            new Item[] { //hand
            },
            new Item[] { //waist
                new Item("Warpwood Binding", 0, 1.7f)
            },
            new Item[] { //legs
            },
            new Item[] { //feet
            },
            new Item[] {//ring1
                new Item("Tarnished Elven Ring", 0, 0.8f, 1),
                new Item("Band of the Ogre King", 2, 0)
            },
            new Item[] { //ring2
                new Item("Tarnished Elven Ring", 0, 0.8f, 1),
                new Item("Band of the Ogre King", 2, 0)
            },
            new Item[] { //trinket 1
            },
            new Item[] { //trinket 2
                new Item("Counterattack Lodestone", 1.6f, 0)
            },
            new Item[] { //wep1
                new Item("Treant's Bane", 63, 2, 0, 2.7f, true)
            },
            new Item[] { //wep2
            }
        };
    }
    void WBInit()
    {
        WBGear = new Item[][]{
            new Item[] { //head
            },
            new Item[] { //neck
            },
            new Item[] { //shoulder
            },
            new Item[] { //back
                new Item("Eskhandar's Pelt", 0, 1f),
                new Item("Puissant Cape", 2.9f, 0 ,1)
            },
            new Item[] { //chest
            },
            new Item[] { //bracers
            },
            new Item[] { //hand
                new Item("Doomhide Gauntlets", 3, 0.7f)
            },
            new Item[] { //waist
            },
            new Item[] { //legs
            },
            new Item[] { //feet
            },
            new Item[] {
                new Item("Don Julio's Band", 1.1f, 1f, 1, true),
            },
            new Item[] {
                new Item("Don Julio's Band", 1.1f, 1f, 1, true),
            },
            new Item[] { //hoj
            },
            new Item[] {
            },
            new Item[] {
            },
            new Item[] {
            }
        };
    }
    void BWLInit()
    {
        BWLGear = new Item[][]{
            new Item[] { //head
            },
            new Item[] { //neck
                new Item("Prestor's Talisman of Connivery", 0, 1.5f, 1)
            },
            new Item[] { //shoulder
                new Item("Tout Dragonhide Shoulderpads", 3.3f, 0)
            },
            new Item[] { //back
                new Item("Cloak of Draconic Might", 2.3f, 0.8f),
                new Item("Cloak of Firemaw", 3.6f, 0)
            },
            new Item[] { //chest
                new Item("Malfurion's Blessed Bulwark", 5.7f, 0)
            },
            new Item[] { //bracers
            },
            new Item[] { //hand
            },
            new Item[] { //waist
                new Item("Therazane's Link", 3.1f, 1),
                new Item("Taut Dragonhide Belt", 4.3f, 0)
            },
            new Item[] { //legs
            },
            new Item[] { //feet
                new Item("Boots of the Shadow Flame", 3.1f, 0, 2)
            },
            new Item[] {
                new Item("Circle of Applied Force", 1.7f, 1.1f, true),
                new Item("Master Dragonslayer's Ring", 3.4f, 0, 1, true)
            },
            new Item[] {
                new Item("Circle of Applied Force", 1.7f, 1.1f, true),
                new Item("Master Dragonslayer's Ring", 3.4f, 0, 1, true)
            },
            new Item[] { //hoj
            },
            new Item[] {
                new Item("Drake Fang Talisman", 4, 0, 2, true)
            },
            new Item[] {
                new Item("Drake Talon Cleaver", 76.5f, 0, 0, 3.4f, true),
                new Item("Herald of Woe", 77.8f, 0, 0, 3.4f)
            },
            new Item[] {
            }
        };
    }
    void BGInit()
    {
        BGGear = new Item[][]{
            new Item[] { //head
            },
            new Item[] { //neck
            },
            new Item[] { //shoulder
            },
            new Item[] { //back
            },
            new Item[] { //chest
            },
            new Item[] { //bracers
            },
            new Item[] { //hand
            },
            new Item[] { //waist
            },
            new Item[] { //legs
            },
            new Item[] { //feet
            },
            new Item[] { //ring
                new Item("Don Julio's Band", 1.1f, 1, 1, true),
            },
            new Item[] {
            },
            new Item[] { //hoj
            },
            new Item[] {
            },
            new Item[] { //Wep
            },
            new Item[] {
            }
        };
    }
    void ZGInit()
    {
        ZGGear = new Item[][]{
            new Item[] { //head
                new Item("Foror's Eyepatch", 3.1f, 2f),
                new Item("Bloodstained Coif", 2, 2)
            },
            new Item[] { //neck
                new Item("Eye of Hakkar", 2.9f, 1)
            },
            new Item[] { //shoulder
            },
            new Item[] { //back
                new Item("Might of the Tribe", 2, 0),
            },
            new Item[] { //chest
                new Item("Runed Bloodstained Hauberk", 4.1f, 1)
            },
            new Item[] { //bracers
            },
            new Item[] { //hand
                new Item("Seafury Gauntlets", 1.4f, 1),
                new Item("Blooddrenched Grips", 2.4f, 1f)
            },
            new Item[] { //waist
            },
            new Item[] { //legs
            },
            new Item[] { //feet
            },
            new Item[] { //ring
                new Item("Seal of Jin", 1.4f, 1, true),
                new Item("Seal of the Gurubashi Berserker", 2.9f, 0, true)
            },
            new Item[] {
                new Item("Seal of Jin", 1.4f, 1, true),
                new Item("Seal of the Gurubashi Berserker", 2.9f, 0, true)
            },
            new Item[] { //hoj
            },
            new Item[] {
            },
            new Item[] {
                new Item("Zulian Stone Axe", 61.7f, 1, 0, 2.8f, true),
            },
            new Item[] {
            }
        };
    }

    Item[][] joinItemArray(Item[][] arr1, Item[][] arr2)
    {
        Item[][] arr3 = new Item[arr1.Length][];
        arr3[1] = new Item[arr1[1].Length];

        for (int i = 0; i < arr1.Length; i++)
        {
            arr3[i] = new Item[arr1[i].Length + arr2[i].Length];
            Array.Copy(arr1[i], arr3[i], arr1[i].Length);
        }
        for (int i = 0; i < arr2.Length; i++)
        {
            Array.Copy(arr2[i], 0, arr3[i], arr1[i].Length, arr2[i].Length);
        }

        for(int i = 0; i < arr3.Length; i++)
        {
            if(i == 10 || i == 11 || i == 12 || i == 13)
            {
                //nothing
            }
            else
            {
                List<Item> removeList = new List<Item>();
                Item[] itArr = arr3[i];
                for (int j = 0; j < itArr.Length; j++)
                {
                    for (int k = 0; k < itArr.Length; k++)
                    {
                        if (
                            (itArr[j].dps < itArr[k].dps && itArr[j].crit <= itArr[k].crit && itArr[j].hit <= itArr[k].hit)
                            || (itArr[j].dps <= itArr[k].dps && itArr[j].crit < itArr[k].crit && itArr[j].hit <= itArr[k].hit)
                            || (itArr[j].dps <= itArr[k].dps && itArr[j].crit <= itArr[k].crit && itArr[j].hit < itArr[k].hit)
                            )
                        {
                            //both same wep type?
                            if (
                                (itArr[j].axe && itArr[k].axe)
                                ||
                                (!itArr[j].axe && !itArr[k].axe)
                              )
                            {
                                //it1 is just worse than it2
                                if (!itArr[j].hasSet)
                                {
                                    print(itArr[j].name + " is worse than " + itArr[k].name);
                                    removeList.Add(itArr[j]);
                                }
                            }
                        }
                    }
                }
                foreach (Item it in removeList)
                {
                    //print("removing: " + it.name);
                    itArr = itArr.Where(val => val != it).ToArray();
                }
                arr3[i] = itArr;
                //print("itArr length:" + itArr.Length);
            }
        }

        return arr3;
    }

    public void easySim()
    {
        int[] chosen = new int[16];
        for (int i = 0; i < dropdowns.Length; i++)
        {
            chosen[i] = dropdowns[i].value;
        }
        Item[] simGear = new Item[]
        {
            currentGearSet[0][chosen[0]], currentGearSet[1][chosen[1]], currentGearSet[2][chosen[2]], currentGearSet[3][chosen[3]],
            currentGearSet[4][chosen[4]], currentGearSet[5][chosen[5]], currentGearSet[6][chosen[6]], currentGearSet[7][chosen[7]],
            currentGearSet[8][chosen[8]], currentGearSet[9][chosen[9]], currentGearSet[10][chosen[10]], currentGearSet[11][chosen[11]],
            currentGearSet[12][chosen[12]], currentGearSet[13][chosen[13]], currentGearSet[14][chosen[14]], currentGearSet[15][chosen[15]]
        };
        Simulation newSim = new Simulation(0, 0);
        ItemSet set = new ItemSet(simGear, isOrc.isOn);
        newSim = sim.easySim(Int32.Parse(inputTime.text), Int32.Parse(inputHit.text), Int32.Parse(inputMinDps.text), Int32.Parse(inputMinCrit.text), set);
    }

    public void simu()
    {
        int[] chosen = new int[16];
        for (int i = 0; i < dropdowns.Length; i++)
        {
            chosen[i] = dropdowns[i].value;
        }

        Item[] simGear = new Item[]
        {
            currentGearSet[0][chosen[0]], currentGearSet[1][chosen[1]], currentGearSet[2][chosen[2]], currentGearSet[3][chosen[3]],
            currentGearSet[4][chosen[4]], currentGearSet[5][chosen[5]], currentGearSet[6][chosen[6]], currentGearSet[7][chosen[7]],
            currentGearSet[8][chosen[8]], currentGearSet[9][chosen[9]], currentGearSet[10][chosen[10]], currentGearSet[11][chosen[11]],
            currentGearSet[12][chosen[12]], currentGearSet[13][chosen[13]], currentGearSet[14][chosen[14]], currentGearSet[15][chosen[15]]
        };
        sim.simulate(Int32.Parse(inputEpoch.text), Int32.Parse(inputTime.text), simGear, Int32.Parse(inputHit.text), Int32.Parse(inputMinDps.text), Int32.Parse(inputMinCrit.text));
    }

    private void printSets(List<ItemSet> sets, List<Simulation> sims)
    {
        //Create sets
        print(">>> Top 10 <<<");
        bestSetDropdown.ClearOptions();
        List<string> itemNames = new List<string>();
        IDictionary<string, int> dict = new Dictionary<string, int>();
        int index = 0;
        foreach (ItemSet itSet in sets)
        {
            itemNames.Add("Set " + index);

            //Add items to dictionary & print dps + dps/crit/swing
            foreach (Item item in itSet.gear)
            {
                if (!dict.ContainsKey(item.name))
                {
                    dict.Add(new KeyValuePair<string, int>(item.name, 1));
                }
                else
                {
                    int count = dict[item.name];
                    dict.Remove(item.name);
                    dict.Add(new KeyValuePair<string, int>(item.name, count + 1));
                }
            }
            print("Best " + index + " dps/crit: " + itSet.dps + "/" + itSet.crit + ", simDPS: " + sims[index].dps + ", attacks: " + sims[index].attacks);
            index++;
        }
        bestSetDropdown.AddOptions(itemNames);

        //Print item occurances
        print(">>> Item occurances <<<");
        foreach (KeyValuePair<string, int> item in dict)
        {
            print(item.Key + ": " + item.Value);
        }
    }

    public void simuAll()
    {
        StartCoroutine(simAndWait());
    }

    IEnumerator simAndWait()
    {
        System.Diagnostics.Stopwatch timer = System.Diagnostics.Stopwatch.StartNew();
        List<Simulation> bestSims = new List<Simulation>();
        bestCombosItemSet = new List<ItemSet>();
        int bestMax = 50;
        int index = 0;
        print("Starting new SimAll!");

        foreach (Item a in currentGearSet[0])
        {
            foreach (Item b in currentGearSet[1])
            {
                foreach (Item c in currentGearSet[2])
                {
                    foreach (Item d in currentGearSet[3])
                    {
                        foreach (Item e in currentGearSet[4])
                        {
                            foreach (Item f in currentGearSet[5])
                            {
                                foreach (Item g in currentGearSet[6])
                                {
                                    foreach (Item h in currentGearSet[7])
                                    {
                                        foreach (Item i in currentGearSet[8])
                                        {
                                            foreach (Item j in currentGearSet[9])
                                            {
                                                foreach (Item k in currentGearSet[10])//ring
                                                {
                                                    foreach (Item l in currentGearSet[11])//ring
                                                    {
                                                        if (k.name.Equals(l.name) && k.unique)
                                                        {
                                                            //print("Unique: " + k.name + "/" + l.name);
                                                            break;
                                                        }
                                                        foreach (Item m in currentGearSet[12])//trink
                                                        {
                                                            foreach (Item n in currentGearSet[13])//trink
                                                            {
                                                                if (m.name.Equals(n.name) && m.unique)
                                                                {
                                                                    //print("Unique: " + k.name + "/" + n.name);
                                                                    break;
                                                                }
                                                                foreach (Item o in currentGearSet[14])//wep1
                                                                {
                                                                    foreach (Item p in currentGearSet[15])//wep2
                                                                    {
                                                                        ItemSet set = new ItemSet(new Item[]
                                                                        {
                                                                            a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p
                                                                        }, isOrc.isOn);
                                                                        Simulation newSim = new Simulation(0, 0);
                                                                        if (easySimAllToggle.isOn)
                                                                        {
                                                                            newSim = sim.easySim(Int32.Parse(inputTime.text), Int32.Parse(inputHit.text), Int32.Parse(inputMinDps.text), Int32.Parse(inputMinCrit.text), set);
                                                                        }
                                                                        else if(comboCount < 10000000)
                                                                        {
                                                                            newSim = sim.simulate(Int32.Parse(inputEpoch.text), Int32.Parse(inputTime.text), set.gear, Int32.Parse(inputHit.text), Int32.Parse(inputMinDps.text), Int32.Parse(inputMinCrit.text));

                                                                        }
                                                                        float dps = newSim.dps;
                                                                        if (bestSims.Count < bestMax)
                                                                        {
                                                                            bestSims.Add(newSim);
                                                                            bestCombosItemSet.Add(set);
                                                                            //print("Added: " + index);
                                                                        }
                                                                        else
                                                                        {
                                                                            for (int z = 0; z < bestSims.Count; z++)
                                                                            {
                                                                                if (dps > bestSims[z].dps)
                                                                                {
                                                                                    //print("Overwriting: " + j + "/" + bestDps[j] + ", for: " + i + "/" + dps);
                                                                                    bestSims.Insert(z, newSim);
                                                                                    bestSims.RemoveAt(bestSims.Count - 1);
                                                                                    bestCombosItemSet.Insert(z, set);
                                                                                    bestCombosItemSet.RemoveAt(bestCombosItemSet.Count - 1);

                                                                                    z = bestSims.Count;
                                                                                }
                                                                            }
                                                                        }

                                                                        index++;
                                                                        if (index % printIndex == 0)
                                                                        {
                                                                            yield return new WaitForEndOfFrame();
                                                                            iNumber.text = index + "/" + comboCount;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        long elapsed = timer.ElapsedMilliseconds / 100;
        print("Time elapsed: " + elapsed + "s");
        if(bestSims.Count > 10)
        {
            bestSims.RemoveRange(10, bestSims.Count - 10);
            bestCombosItemSet.RemoveRange(10, bestCombosItemSet.Count - 10);
        }
        //Add to sets list
        bestCombos = new List<List<Item>>();
        foreach (ItemSet set in bestCombosItemSet)
        {
            List<Item> setList = set.gear.ToList();
            bestCombos.Add(setList);
            //print("Index of best: " + i);
        }

        //Open best one
        for (int i = 0; i < dropdowns.Length; i++)
        {
            for (int j = 0; j < currentGearSet[i].Length; j++)
            {
                if (currentGearSet[i][j].Equals(bestCombosItemSet[0].gear[i]))
                {
                    dropdowns[i].value = j;
                }
            }
        }
        sim.dpsText.text = "" + bestSims[0].dps;

        printSets(bestCombosItemSet, bestSims);
    }

    public void onChangePhase()
    {
        switch (phase.value)
        {
            case 0:
                currentGearSet = none;
                break;
            case 1:
                currentGearSet = preRaidGear;
                break;
            case 2:
                currentGearSet = joinItemArray(preRaidGear, MCGear);
                break;
            case 3:
                currentGearSet = joinItemArray(preRaidGear, DMGear);
                break;
            case 4:
                currentGearSet = joinItemArray(joinItemArray(preRaidGear, DMGear), MCGear);
                break;
            case 5:
                currentGearSet = joinItemArray(joinItemArray(joinItemArray(preRaidGear, DMGear), MCGear), WBGear);
                break;
            case 6:
                currentGearSet = joinItemArray(joinItemArray(joinItemArray(joinItemArray(preRaidGear, DMGear), MCGear), WBGear), BWLGear);
                break;
            case 7:
                currentGearSet = joinItemArray(joinItemArray(joinItemArray(joinItemArray(joinItemArray(preRaidGear, DMGear), MCGear), WBGear), BWLGear), ZGGear);
                break;
            default:
                break;
        }

        comboCount = 1;
        for (int i = 0; i < dropdowns.Length; i++)
        {
            dropdowns[i].ClearOptions();
            List<string> itemNames = new List<string>();
            foreach (Item it in currentGearSet[i])
            {
                itemNames.Add(it.name);
            }
            comboCount = comboCount * currentGearSet[i].Length;
            dropdowns[i].AddOptions(itemNames);
            dropdowns[i].value = 0;
        }
        comboNr.text = "" + comboCount;
        bestSetDropdown.ClearOptions();
        onChangeItem();
    }

    public void onChangeSet()
    {
        //print("Change!");
        for (int i = 0; i < dropdowns.Length; i++)
        {
            dropdowns[i].value = 0;
        }
        int index = bestSetDropdown.value;
        for (int i = 0; i < dropdowns.Length; i++)
        {
            for (int j = 0; j < currentGearSet[i].Length; j++)
            {
                if (currentGearSet[i][j].Equals(bestCombos[index][i]))
                {
                    dropdowns[i].value = j;
                }
            }
        }
        onChangeItem();
    }

    public void onChangeItem()
    {
        Item[] newGear = new Item[16];
        for (int i = 0; i < dropdowns.Length; i++)
        {
            //print("i: " + i + ", value: " + dropdowns[i].value);
            newGear[i] = currentGearSet[i][dropdowns[i].value];
        }

        ItemSet set = new ItemSet(newGear, isOrc.isOn);

        //print("Current: dps/crit/swing: " + dps + "/" + crit + "/" + swing);
        dpsText.text = "DPS: " + set.dps;
        critText.text = "CRIT: " + set.crit;
        swingText.text = "SWING: " + set.swing;
        hitText.text = "HIT: " + set.hit;
    }
}

public class ItemSet
{
    public Item[] gear;
    public float dps;
    public float swing; //weapons
    public float crit; //percent
    public int hit; //percent

    public ItemSet(Item[] Gear, bool orc)
    {
        gear = Gear;
        for (int i = 0; i < gear.Length; i++)
        {
            Item it = gear[i];
            dps += it.dps;
            crit += it.crit;
            hit += it.hit;
            if (i == 14)
            {
                swing = it.swing;
            }
        }
        if (gear[6].name.Equals("Devilsaur Gauntlets") && gear[8].name.Equals("Devilsaur Leggings"))
        {
            hit += 2;
        }
        else if (gear[2].name.Equals("Black Dragonsc. Sh.") && gear[8].name.Equals("Black Dragonsc. Leg.") && gear[4].name.Equals("Black Dragonsc. Chest"))
        {
            hit += 1;
            crit += 2;
        }
        else if (gear[2].name.Equals("Black Dragonsc. Sh.") && gear[8].name.Equals("Black Dragonsc. Leg."))
        {
            hit += 1;
        }

        if(orc && gear[14].axe)
        {
            hit += 3;
        }
    }
}

[System.Serializable]
public class Item
{
    [SerializeField]
    public string name;
    [SerializeField]
    public float dps;
    [SerializeField]
    public float swing; //weapons
    [SerializeField]
    public float crit; //percent
    [SerializeField]
    public int hit; //percent

    public bool unique;
    public bool hasSet;
    public bool axe;

    //gear
    public Item(string Name, float Dps, float Crit)
    {
        name = Name;
        dps = Dps;
        crit = Crit;
    }

    //has hit
    public Item(string Name, float Dps, float Crit, int Hit)
    {
        name = Name;
        dps = Dps;
        crit = Crit;
        hit = Hit;
    }

    //has unique
    public Item(string Name, float Dps, float Crit, int Hit, bool Unique)
    {
        name = Name;
        dps = Dps;
        crit = Crit;
        hit = Hit;
        unique = Unique;
    }

    //has unique no hit
    public Item(string Name, float Dps, float Crit, bool Unique)
    {
        name = Name;
        dps = Dps;
        crit = Crit;
        unique = Unique;
    }

    //weapon
    public Item(string Name, float Dps, float Crit, int Hit, float Swing)
    {
        name = Name;
        dps = Dps;
        crit = Crit;
        swing = Swing;
        hit = Hit;
    }

    //axe
    public Item(string Name, float Dps, float Crit, int Hit, float Swing, bool Axe)
    {
        name = Name;
        dps = Dps;
        crit = Crit;
        swing = Swing;
        hit = Hit;
        axe = Axe;
    }
}