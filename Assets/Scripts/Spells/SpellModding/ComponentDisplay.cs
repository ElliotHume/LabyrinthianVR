using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[ExecuteInEditMode]
public class ComponentDisplay : MonoBehaviour
{
    public AuricaSpellComponent spellComponent;
    public TextMeshProUGUI title, description;
    public Image structure, essence, fire, water, earth, air, nature;
    public Color order, chaos, life, death, divine, demonic;

    void OnEnable(){
        Rebuild();
    }

    #if UNITY_EDITOR
    void OnValidate()
    {
        if (gameObject.activeInHierarchy) Rebuild();
    }
    #endif

    public void Rebuild () {
        if (spellComponent == null) return;

        title.text = spellComponent.c_name;
        description.text = spellComponent.description;

        structure.rectTransform.sizeDelta = new Vector2( 50, Mathf.Abs(spellComponent.basicDistribution.structure) * 200);
        essence.rectTransform.sizeDelta = new Vector2( 50, Mathf.Abs(spellComponent.basicDistribution.essence) * 200);
        fire.rectTransform.sizeDelta = new Vector2( 50, spellComponent.basicDistribution.fire * 200);
        water.rectTransform.sizeDelta = new Vector2( 50, spellComponent.basicDistribution.water * 200);
        earth.rectTransform.sizeDelta = new Vector2( 50, spellComponent.basicDistribution.earth * 200);
        air.rectTransform.sizeDelta = new Vector2( 50, spellComponent.basicDistribution.air * 200);
        nature.rectTransform.sizeDelta = new Vector2( 50, Mathf.Abs(spellComponent.basicDistribution.nature) * 200);

        structure.color = Color.Lerp(chaos, order, (spellComponent.basicDistribution.structure+1f)/2f);
        essence.color = Color.Lerp(death, life, (spellComponent.basicDistribution.essence+1f)/2f);
        nature.color = Color.Lerp(demonic, divine, (spellComponent.basicDistribution.nature+1f)/2f);
    }
}
