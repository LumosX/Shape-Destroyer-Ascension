using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building {
    public readonly string Name;
    public readonly int CostMats; // Materials needed to build
    public readonly int CostPower; // Power consumed
    public readonly int Upkeep; // Mats required to maintain per "day"

    public readonly Building UpgradesTo; // to which building this one upgrades
    public readonly Produce Produces; // what the building generates every "day"

    public Building(string name, int costMats, int costPower, int upkeep, Building upgradesTo, Produce produces) {
        Name = name;
        CostMats = costMats;
        CostPower = costPower;
        Upkeep = upkeep;
        UpgradesTo = upgradesTo;
        Produces = produces;
    }
}

// What buildings generate
public class Produce {
    public int Happiness;
    public int Materials;
    public int Ammunition; // Magazines of ammunition
    public int Population;

    public Produce(int Hap = 0, int Mats = 0, int Ammo = 0, int Pop = 0) {
        Happiness = Hap;
        Materials = Mats;
        Ammunition = Ammo;
        Population = Pop;
    }
}


public class Buildings {
    public static Building Road = new Building("Road", 5, 0, 1, null, null);
    
    // Remember that power costs are NOT cumulative
    public static Building Residential3 = new Building("Residential Tier III", 220, 3, 15, null, Produce(Hap: -3, Pop: 10));
    public static Building Residential2 = new Building("Residential Tier II", 100, 2, 10, Residential3, Produce(Hap: -2, Pop: 5));
    public static Building Residential1 = new Building("Residential Tier I", 20, 1, 5, Residential2, Produce(Hap: -1, Pop: 2));

    public static Building Commercial3 = new Building("Commercial Tier III", 500, 9, 50, null, Produce(Hap: 5, Pop: -5));
    public static Building Commercial2 = new Building("Commercial Tier II", 200, 7, 50, Commercial3, Produce(Hap: 3, Pop: -3));
    public static Building Commercial1 = new Building("Commercial Tier I", 40, 5, 50, Commercial2, Produce(Hap: 1, Pop: -1));
    
    public static Building Industrial3 = new Building("Industrial Tier III", 1000, 20, 0, null, Produce(Hap: -5, Pop: -3, Mats: 50));
    public static Building Industrial2 = new Building("Industrial Tier II", 600, 15, 0, Industrial3, Produce(Hap: -2, Pop: -2, Mats: 20));
    public static Building Industrial1 = new Building("Industrial Tier I", 40, 10, 0, Industrial2, Produce(Hap: -1, Pop: -1, Mats: 10));

    public static Building Military3 = new Building("Industrial Tier III", 1000, 20, 0, null, Produce(Hap: -10, Pop: -3, Ammo: 15));
    public static Building Military2 = new Building("Industrial Tier II", 600, 15, 0, Military3, Produce(Hap: -5, Pop: -2, Ammo: 7));
    public static Building Military1 = new Building("Industrial Tier I", 40, 10, 0, Military2, Produce(Hap: -2, Pop: -1, Ammo: 4));

    // ayyyyyyy
    private static Produce Produce(int Hap = 0, int Mats = 0, int Ammo = 0, int Pop = 0) {
        return new Produce(Hap, Mats, Ammo, Pop);
    }

}
