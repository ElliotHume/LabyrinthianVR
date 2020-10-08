using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{

    public List<string> discoveredSpells;
    public List<string> discoveredScenes;
    public float health = 100;
    float maxHealth;
    public string previousScene;

    public Color baseColour, damageColour;
    public Material damageMaterial;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        StartCoroutine(RegenHealth());
        maxHealth = health;
    }

    public void DiscoverScene(string scene) {
        if (!discoveredScenes.Contains(scene)) discoveredScenes.Add(scene);
    }

    public void DiscoverSpell(string spell) {
        if (!discoveredSpells.Contains(spell)) discoveredSpells.Add(spell);
    }

    public void Damage(float damage) {
        health -= damage;

        ChangeDamageMaterial();

        if (health <= 0f) {
            // Send the player to the death realm
            LoadScene("DeathRealm");
        }
    }

    void ChangeDamageMaterial() {
        Color drawingPlaneColour = Color.Lerp(damageColour, baseColour, health / maxHealth);
        damageMaterial.color = drawingPlaneColour;
    }

    public void LoadScene(string sceneName) {
        health = 100;
        damageMaterial.color = baseColour;

        if (sceneName == "last") sceneName = previousScene;
        previousScene = SceneManager.GetActiveScene().name;
        DiscoverScene(sceneName);
        SceneManager.LoadScene(sceneName);
    }

    public void ReturnToLastScene() {
        health = 100;
        damageMaterial.color = baseColour;

        SceneManager.LoadScene(previousScene);
    }

    IEnumerator RegenHealth() {
        while (true) {
            yield return new WaitForSeconds(15);
            health = Mathf.Clamp(health+15, 0, 100);
            ChangeDamageMaterial();
        }
    }
}
