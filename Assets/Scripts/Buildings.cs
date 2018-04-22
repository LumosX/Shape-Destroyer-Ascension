using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Building {
    public readonly string Name;
    public readonly int CostMats; // Materials needed to build
    public readonly int CostPower; // Power consumed
    public readonly int CostPop; // Population required
    public readonly int Upkeep; // Mats required to maintain per "day"

    public readonly Building UpgradesTo; // to which building this one upgrades
    public readonly Produce Produces; // what the building generates every "day"

    public Building(string name, int costMats, int costPower, int costPop, int upkeep, Building upgradesTo, Produce produces) {
        Name = name;
        CostMats = costMats;
        CostPower = costPower;
        CostPop = costPop;
        Upkeep = upkeep;
        UpgradesTo = upgradesTo;
        Produces = produces;
    }

    public string GetBuildCost() {
        return "Mats: " + CostMats + ", Power: " + CostPower + ", Pop: " + CostPop;
    }

    public string GetUpkeep() {
        return Upkeep + " Mats/day";
    }

}

// What buildings generate
public class Produce {
    public int Happiness;
    public int Materials;
    public int Ammunition; // Magazines of ammunition

    public Produce(int Hap = 0, int Mats = 0, int Ammo = 0) {
        Happiness = Hap;
        Materials = Mats;
        Ammunition = Ammo;
    }

    public override string ToString() {
        var result = "";
        if (Happiness != 0) result += "Hap: " + Happiness + ", ";
        if (Materials != 0) result += "Mats: " + Materials + ", ";
        if (Ammunition != 0) result += "Ammo: " + Ammunition + ", ";
        return result.Substring(0, result.Length - 2);
    }

    public static Produce operator +(Produce c1, Produce c2) {
        return new Produce(c1.Happiness + c2.Happiness, c1.Materials + c2.Materials, c1.Ammunition + c2.Ammunition); 
    }
}


public class Buildings {
    public static Color RoadColour = new Color32(65, 65, 65, 255);

    public static Building Road = new Building("Road", 5, 0, 0, 1, null, null);
    public static Building Generator = new Building("Skydome Generator", 0, 0, 0, 0, null, null); // special
    
    // Remember that power costs are NOT cumulative
    public static Building Residential3 = new Building("Residential III", 220, 2, -10, 15, null,         Produce(Hap: -3));
    public static Building Residential2 = new Building("Residential II",  100, 1,  -5, 10, Residential3, Produce(Hap: -2));
    public static Building Residential1 = new Building("Residential I",    20, 1,  -2,  5, Residential2, Produce(Hap: -1));

    public static Building Commercial3 = new Building("Commercial III", 500, 9, 3, 50, null,        Produce(Hap: 5));
    public static Building Commercial2 = new Building("Commercial II",  200, 7, 2, 40, Commercial3, Produce(Hap: 3));
    public static Building Commercial1 = new Building("Commercial I",    40, 5, 1, 30, Commercial2, Produce(Hap: 1));
    
    public static Building Industrial3 = new Building("Industrial III", 1000, 20, 5, 0, null,        Produce(Hap: -5, Mats: 80));
    public static Building Industrial2 = new Building("Industrial II",   600, 15, 3, 0, Industrial3, Produce(Hap: -2, Mats: 25));
    public static Building Industrial1 = new Building("Industrial I",     40, 10, 1, 0, Industrial2, Produce(Hap: -1, Mats: 10));

    public static Building Military3 = new Building("Military III", 1000, 20, 5, 40, null,      Produce(Hap: -10, Ammo: 6));
    public static Building Military2 = new Building("Military II",   600, 15, 3, 30, Military3, Produce(Hap: -5,  Ammo: 3));
    public static Building Military1 = new Building("Military I",     40, 10, 1, 20, Military2, Produce(Hap: -2,  Ammo: 1));

    // ayyyyyyy
    private static Produce Produce(int Hap = 0, int Mats = 0, int Ammo = 0) {
        return new Produce(Hap, Mats, Ammo);
    }

}
