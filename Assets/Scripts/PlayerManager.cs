using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public List<string> discoveredSpells;
    public List<string> discoveredScenes;
    public float health = 100f;
    float maxHealth;
    public string previousScene;

    public Color baseColour, damageColour;
    public Material damageMaterial;

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        // StartCoroutine(RegenHealth());
        maxHealth = health;
        health = 100f;

        // SINGLETON
        if (instance != null && instance != this) {
            Destroy(gameObject);
        } else {
            instance = this;
        }
    }

    void FixedUpdate() {
        health = Mathf.Clamp(health+(0.33f * Time.deltaTime), 0f, maxHealth);
        ChangeDamageMaterial();
    }

    public void DiscoverScene(string scene) {
        if (!discoveredScenes.Contains(scene)) discoveredScenes.Add(scene);
    }

    public void DiscoverSpell(string spell) {
        if (!discoveredSpells.Contains(spell)) discoveredSpells.Add(spell);
    }

    public void Damage(float damage) {
        health -= damage;

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
        health = 100f;
        damageMaterial.color = baseColour;

        if (sceneName == "last") sceneName = previousScene;
        previousScene = SceneManager.GetActiveScene().name;
        DiscoverScene(sceneName);
        SceneManager.LoadScene(sceneName);
    }

    public void ReturnToLastScene() {
        health = 100f;
        damageMaterial.color = baseColour;

        SceneManager.LoadScene(previousScene);
    }

    IEnumerator RegenHealth() {
        while (true) {
            health = Mathf.Clamp(health+15f, 0f, maxHealth);
            Debug.Log("Health: "+health);
            ChangeDamageMaterial();
            yield return new WaitForSeconds(20f);
        }
    }
}
