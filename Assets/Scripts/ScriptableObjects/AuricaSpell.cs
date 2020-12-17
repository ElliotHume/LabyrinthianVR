using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AuricaSpell", menuName = "Labyrinthian/AuricaSpell", order = 1)]
public class AuricaSpell : ScriptableObject {
    public string c_name;
    [TextArea(15,3)]
    public string description;
    public ManaDistribution targetDistribution;
    public List<AuricaSpellComponent> keyComponents;
    public float errorThreshold = 3.0f;
    
}