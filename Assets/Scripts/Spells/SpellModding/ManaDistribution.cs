﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ManaDistribution
{
    public float structure, essence, fire, water, earth, air, nature;

    public ManaDistribution(string dist) {
        string[] distributionSeperator = new string[] {", "};
        string[] splitDist = dist.Split(distributionSeperator, System.StringSplitOptions.None);
        structure = float.Parse(splitDist[0]);
        essence = float.Parse(splitDist[1]);
        fire = float.Parse(splitDist[2]);
        water = float.Parse(splitDist[3]);
        earth = float.Parse(splitDist[4]);
        air = float.Parse(splitDist[5]);
        nature = float.Parse(splitDist[6]);
    }

    public string ToString() {
        return "["+structure+", "+essence+", "+fire+", "+water+", "+earth+", "+air+", "+nature+"]";
    }

    public List<float> ToList() {
        List<float> ls = new List<float>();
        ls.Add(structure);
        ls.Add(essence);
        ls.Add(fire);
        ls.Add(water);
        ls.Add(earth);
        ls.Add(air);
        ls.Add(nature);
        return ls;
    }

    public float CheckDistError(ManaDistribution dist) {
        float error = 0f;
        error += GetAlignedErrorFunc(structure, dist.structure);
        error += GetAlignedErrorFunc(essence, dist.essence);
        error += GetAlignedErrorFunc(nature, dist.nature);
        
        error += Mathf.Abs(fire - dist.fire);
        error += Mathf.Abs(water - dist.water);
        error += Mathf.Abs(earth - dist.earth);
        error += Mathf.Abs(air - dist.air);
    
        return error;
    }

    float GetAlignedErrorFunc(float target, float actual) {
        if (target > 0f && actual > 0f) {
            return target > actual ? target - actual : actual - target;
        } else if (target < 0f && actual < 0f) {
            return target < actual ? Mathf.Abs(target - actual) : Mathf.Abs(actual - target);
        } else if (target > 0f && actual <= 0f) {
            return Mathf.Abs(target + actual);
        } else if (target <= 0f && actual > 0f) {
            return Mathf.Abs(actual + target);
        }
        return 0f;
    }

    public void AddBasicDist(ManaDistribution other) {
        structure += other.structure;
        essence += other.essence;
        fire += other.fire;
        water += other.water;
        earth += other.earth;
        air += other.air;
        nature += other.nature;
    }

    public void AddAuricDist(ManaDistribution auricDist, ManaDistribution aura) {
        structure += auricDist.structure * aura.structure;
        essence += auricDist.essence * aura.essence;
        fire += auricDist.fire * aura.fire;
        water += auricDist.water * aura.water;
        earth += auricDist.earth * aura.earth;
        air += auricDist.air * aura.air;
        nature += auricDist.nature * aura.nature;
    }

    public void ApplyFluxDist(ManaDistribution fluxDist) {
        // Aligned mana moves towards the midpoint of 0
        structure -= structure * fluxDist.structure;
        essence -= essence * fluxDist.essence;
        nature -= nature * fluxDist.nature;

        // Elemental mana moves towards the average of the elements
        float midpoint = GetElementalAverage();
        // Fire
        if (fire > midpoint) {
            fire -= (fire - midpoint) * fluxDist.fire;
        } else if (fire < midpoint) {
            fire += (midpoint - fire) * fluxDist.fire;
        }
        // Water
        if (water > midpoint) {
            water -= (water - midpoint) * fluxDist.water;
        } else if (water < midpoint) {
            water += (midpoint - water) * fluxDist.water;
        }
        // Earth
        if (earth > midpoint) {
            earth -= (earth - midpoint) * fluxDist.earth;
        } else if (earth < midpoint) {
            earth += (midpoint - earth) * fluxDist.earth;
        }
        // Air
        if (air > midpoint) {
            air -= (air - midpoint) * fluxDist.air;
        } else if (air < midpoint) {
            air += (midpoint - air) * fluxDist.air;
        }
    }

    public float GetElementalAverage() {
        return (fire + water + earth + air) / 4f;
    }

    public void PopulateFromList(List<float> ls) {
        structure = ls[0];
        essence = ls[1];
        fire = ls[2];
        water = ls[3];
        earth = ls[4];
        air = ls[5];
        nature = ls[6];
    }
}
