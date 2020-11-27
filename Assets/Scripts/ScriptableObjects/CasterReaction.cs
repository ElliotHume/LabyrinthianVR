using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CasterReaction", menuName = "Labyrinthian/CasterReaction", order = 0)]
public class CasterReaction : ScriptableObject {
    public string spell;
    public float reactionChance, blockTime, animationTime = 0.1f;
    public string castType;
    public GameObject reactionSpell;
}
