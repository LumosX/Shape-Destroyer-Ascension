using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public int Materials; // currency;
    public int Power; // second currency / generator HP
    public int Happiness; // population happiness; percentage modifier

    // Each point of happiness changes materials income by 5%
    public int IncomeModifierPercent => Happiness * 5;

    public void InitialiseGame() {
        Materials = 100;
        Power = 20;

    }




}
