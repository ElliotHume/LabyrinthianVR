using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Player : MonoBehaviour
{

    public GlyphRecognition glyphRecognition;

    public string rightHandSpell = null, leftHandSpell = null;
    public GameObject rightHand, leftHand, drawingAnchor;
    public XRController rightHandController, leftHandController;
    private SkinnedMeshRenderer rightHandRenderer, leftHandRenderer;
    public Material baseMaterial, spellHandGlow_r, spellHandGlow_l, goldMaterial;
    public ParticleSystem r_fireParticles, r_lightningParticles, r_windParticles, r_arcaneParticles, r_iceParticles, r_mortalParticles, r_planarParticles, r_flightParticles, r_basicParticles;
    public ParticleSystem l_fireParticles, l_lightningParticles, l_windParticles, l_arcaneParticles, l_iceParticles, l_mortalParticles, l_planarParticles, l_flightParticles, l_basicParticles;
    public GameObject r_shieldSphere, r_arcanoSphere, r_missileEmitter;
    public GameObject l_shieldSphere, l_arcanoSphere, l_missileEmitter;

    // SPELL PREFABS
    public GameObject groundCaster;
    public GameObject fireball;
    public GameObject bluefire;
    public GameObject shield;
    public GameObject windslash;
    public GameObject lightning;
    public GameObject arcanePulse;
    public GameObject iceSpikes, iceSpray;
    public GameObject fizzle;
    public GameObject royalFireball;
    public GameObject magicMissile;
    public GameObject hammer;
    public GameObject earthWall;
    public GameObject farseer;
    public GameObject returnSpell;
    public GameObject defy;
    public GameObject midasTouch;
    public GameObject seeDead;
    public GameObject skeleton;
    public GameObject metalFan;
    public GameObject magnoset;
    public GameObject drainSphere;
    public GameObject binding;
    public GameObject marker;
    public GameObject sword;
    public GameObject flight;
    public GameObject wargodswand;

    // Spell Component prefabs
    public GameObject spellComponent;

    public List<GameObject> hammers;
    public int maxHammers = 1;
    public List<GameObject> shields;
    public int maxShields = 2;
    public List<GameObject> swords;
    public int maxSwords = 2;
    public AudioSource damagedSound;
    public LayerMask groundMask;

    private Dictionary<string, Color> handColours = new Dictionary<string, Color>();
    bool flight1=false, flight2=false, flightTimerRunning=false;
    float spellAccuracy = 0.5f;

    private PlayerManager playerManager;



    // Start is called before the first frame update
    void Start() {
        Color arcaneColor = new Color(156/255f, 0f, 1f);
        Color mortalColor = new Color(165/255f, 145/255f, 0f);
        Color planarColor = new Color(1f, 1f, 1f);
        Color basicColor = new Color(54/255f, 99/255f, 78/255f);


        // hand colours
        handColours.Add("fireball", new Color(1f, 0, 0));
        handColours.Add("fireballdirected", new Color(1f, 0, 0));
        handColours.Add("bluefire", new Color(1f, 0.1f, 0.2f));
		handColours.Add("shield", new Color(113/255f, 199/255f, 1f));
		handColours.Add("windslash", new Color(26/255f, 1f, 0));
		handColours.Add("lightning", new Color(1f, 247/255f, 103/255f));
		handColours.Add("arcanopulse", new Color(214/255f, 135/255f, 1f));
		handColours.Add("icespikes", new Color(50/255f, 50/255f, 1f));
        handColours.Add("icespray", new Color(50/255f, 50/255f, 1f));
		handColours.Add("royalfire", arcaneColor);
        handColours.Add("magicmissile", arcaneColor);
        handColours.Add("magicmissiledirected", new Color(214/255f, 135/255f, 1f));
        handColours.Add("hammer", mortalColor);
        handColours.Add("defy", mortalColor);
        handColours.Add("earthwall", mortalColor);
        handColours.Add("midastouch", mortalColor);
        handColours.Add("metalfan", mortalColor);
        handColours.Add("magnoset", mortalColor);
        handColours.Add("sword", basicColor);
        handColours.Add("farseer", planarColor);
        handColours.Add("seedead", planarColor);
        handColours.Add("death", planarColor);
        handColours.Add("raiseskeleton", planarColor);
        handColours.Add("drainsphere", planarColor);
        handColours.Add("binding", planarColor);
        handColours.Add("return", arcaneColor);
        handColours.Add("marker", basicColor);
        handColours.Add("flight1", planarColor);
        handColours.Add("flight2", planarColor);
        handColours.Add("groundcaster", planarColor);
        handColours.Add("wargodswand", new Color(1f, 0, 0.3f));

        // spell component colours
        handColours.Add("activate", basicColor);

        rightHand = GameObject.Find("RightHand Controller");
        Debug.Log("right Hand GameObject: "+ rightHand);

        leftHand = GameObject.Find("LeftHand Controller");
        Debug.Log("left Hand GameObject: "+ leftHand);

        try {
            rightHandRenderer = GameObject.Find("hands:Rhand").GetComponent<SkinnedMeshRenderer>();
            Debug.Log("Right Hand render: " + GameObject.Find("hands:Rhand"));

            leftHandRenderer = GameObject.Find("hands:Lhand").GetComponent<SkinnedMeshRenderer>();
            Debug.Log("Left Hand render: " + GameObject.Find("hands:Lhand"));
        } catch {
            Debug.Log("Could not find hand renderers, will try again on FixedUpdate");
        }

        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();

        if (damagedSound == null) damagedSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // Debug Stuff
        if (Input.GetKeyDown("space")) {
            Instantiate(fireball, Camera.main.gameObject.transform.position, Camera.main.gameObject.transform.rotation);
        } else if (Input.GetKeyDown("e")) {
            Instantiate(arcanePulse, Camera.main.gameObject.transform.position, Camera.main.gameObject.transform.rotation);
        } else if (Input.GetKeyDown("q")) {
            GameObject go = Instantiate(hammer, Camera.main.gameObject.transform.position, Camera.main.gameObject.transform.rotation);
            go.GetComponent<Rigidbody>().useGravity = true;
        } else if (Input.GetKeyDown("r")) {
            Instantiate(magicMissile, Camera.main.gameObject.transform.position, Camera.main.gameObject.transform.rotation);
        } else if (Input.GetKeyDown("t")) {
            Instantiate(drainSphere, Camera.main.gameObject.transform.position, Camera.main.gameObject.transform.rotation);
        } else if (Input.GetKeyDown("m")) {
            Instantiate(magnoset, Camera.main.gameObject.transform.position, Camera.main.gameObject.transform.rotation);
        } else if (Input.GetKeyDown("y")) {
            GameObject go = Instantiate(royalFireball, Camera.main.gameObject.transform.position, Camera.main.gameObject.transform.rotation);
            go.GetComponent<Rigidbody>().useGravity = true;
        } else if (Input.GetKeyDown("g")) {
            CastHeldBinding(rightHand);
        } if (Input.GetKeyDown("3")) {
            ReleaseSpellCast("right");
        }
    }

    void FixedUpdate() {
        if (rightHandController == null) {
            rightHandController = rightHand.GetComponent<XRController>();
        }
        if (leftHandController == null) {
            leftHandController = leftHand.GetComponent<XRController>();
        }
        if (!rightHandRenderer) {
            try {
                rightHandRenderer = GameObject.Find("hands:Rhand").GetComponent<SkinnedMeshRenderer>();
                Debug.Log("Right Hand render: " + GameObject.Find("hands:Rhand"));
            } catch {
                //do nothing
            }
            
        }
        if (!leftHandRenderer) {
            try {
                leftHandRenderer = GameObject.Find("hands:Lhand").GetComponent<SkinnedMeshRenderer>();
                Debug.Log("Left Hand render: " + GameObject.Find("hands:Lhand"));
            } catch {
                // do nothing
            }
        }
        if (!drawingAnchor) {
            drawingAnchor = GameObject.Find("DrawingAnchor");
        }

        if (rightHandSpell != null && rightHandSpell != "") {
            rightHandController.SendHapticImpulse(spellAccuracy, 0.1f);
        }

        if (leftHandSpell != null && leftHandSpell != "") {
            leftHandController.SendHapticImpulse(spellAccuracy, 0.1f);
        }
    }

    void SetHandGlow(string spell, string hand){
        if(rightHandRenderer && leftHandRenderer){
            //Debug.Log("Setting hand colour to: "+handColours[spell]);
            if (hand == "right") {
                spellHandGlow_r.SetColor("Color_682024A", handColours[spell]);
                rightHandRenderer.material = spellHandGlow_r;
            } else {
                spellHandGlow_l.SetColor("Color_682024A", handColours[spell]);
                leftHandRenderer.material = spellHandGlow_l;
            }
        }
    }

    void ToggleSpellParticles(bool rightHand, string spell, bool turnOn = true) {
        ParticleSystem ps = null;
        GameObject handObject = null;
        switch (spell) {
            case "fireball":
            case "fireballdirected":
            case "wargodswand":
                ps = rightHand ? r_fireParticles : l_fireParticles;
                break;
            case "shield":
                handObject = rightHand ? r_shieldSphere : l_shieldSphere;
                break;
            case "lightning":
                ps = rightHand ? r_lightningParticles : l_lightningParticles;
                break;
            case "windslash":
                ps = rightHand ? r_windParticles : l_windParticles;
                break;
            case "flight1":
            case "flight2":
                ps = rightHand ? r_flightParticles : l_flightParticles;
                break;
            case "arcanopulse":
                handObject = rightHand ? r_arcanoSphere : l_arcanoSphere;
                break;
            case "royalfire":
            case "return":
            case "binding":
                ps = rightHand ? r_arcaneParticles : l_arcaneParticles;
                break;
            case "icespikes":
            case "icespray":
            case "bluefire":
                ps = rightHand ? r_iceParticles : l_iceParticles;
                break;
            case "magicmissile":
            case "magicmissiledirected":
                handObject = rightHand ? r_missileEmitter : l_missileEmitter;
                break;
            case "hammer":
            case "earthwall":
            case "defy":
            case "midastouch":
            case "metalfan":
                ps = rightHand ? r_mortalParticles : l_mortalParticles;
                break;
            case "farseer":
            case "seedead":
            case "death":
            case "raiseskeleton":
            case "drainsphere":
                ps = rightHand ? r_planarParticles : l_planarParticles;
                break;
            case "sword":
            case "marker":
            case "magnoset":
            case "activate":
            case "groundcaster":
                ps = rightHand ? r_basicParticles : l_basicParticles;
                break;
        }
        if (ps != null){
            if (ps.isPlaying || !turnOn) {
                ps.Stop();
            } else {
                ps.Play();
            }
        }
        if (handObject != null) {
            //print("handObject: "+handObject+"     toggle active from: "+ handObject.activeInHierarchy+" to: "+!handObject.activeInHierarchy);
            handObject.SetActive(turnOn);
        }
    }

    public void ReleaseSpellCast(string hand) {
        string heldSpell = (hand == "right") ? rightHandSpell : leftHandSpell;
        GameObject castingHand = (hand == "right") ? rightHand : leftHand;

        if (heldSpell != null) {
            //Debug.Log("Player Cast Held Spell: "+heldSpell+"    from hand: "+hand);
            try {
                switch (heldSpell) {
                    case "fireball":
                        CastHeldFireball(castingHand);
                        break;
                    case "fireballdirected":
                        CastHeldFireball(castingHand, true);
                        break;
                    case "bluefire":
                        CastHeldBlueFire(castingHand);
                        break;
                    case "shield":
                        CastHeldShield(castingHand);
                        break;
                    case "lightning":
                        CastHeldLightning(castingHand);
                        break;
                    case "windslash":
                        CastHeldWindSlash(castingHand);
                        break;
                    case "royalfire":
                        CastHeldRoyalFire(castingHand);
                        break;
                    case "icespikes":
                        CastHeldIceSpikes(castingHand);
                        break;
                    case "icespray":
                        CastHeldIceSpray(castingHand);
                        break;
                    case "arcanopulse":
                        CastHeldArcanoPulse(castingHand);
                        break;
                    case "magicmissile":
                        CastHeldMagicMissile(castingHand, false);
                        break;
                    case "magicmissiledirected":
                        CastHeldMagicMissile(castingHand, true);
                        break;
                    case "hammer":
                        CastHeldHammer(castingHand);
                        break;
                    case "earthwall":
                        CastHeldEarthWall(castingHand);
                        break;
                    case "farseer":
                        CastHeldFarseer(castingHand);
                        break;
                    case "defy":
                        CastHeldDefyGravity(castingHand);
                        break;
                    case "return":
                        CastHeldReturn(castingHand);
                        break;
                    case "midastouch":
                        CastHeldMidasTouch(castingHand, hand);
                        break;
                    case "seedead":
                        CastHeldSeeDead(castingHand);
                        break;
                    case "death":
                        CastHeldDeath(castingHand);
                        break;
                    case "raiseskeleton":
                        CastHeldRaiseSkeleton(castingHand);
                        break;
                    case "metalfan":
                        CastHeldMetalFan(castingHand);
                        break;
                    case "magnoset":
                        CastHeldMagnoset(castingHand);
                        break;
                    case "drainsphere":
                        CastHeldDrainSphere(castingHand);
                        break;
                    case "binding":
                        CastHeldBinding(castingHand);
                        break;
                    case "marker":
                        CastHeldMarker(castingHand);
                        break;
                    case "sword":
                        CastHeldSword(castingHand);
                        break;
                    case "groundcaster":
                        CastHeldGroundCaster(castingHand);
                        break;
                    case "activate":
                        CastHeldSpellComponent(heldSpell, castingHand);
                        break;
                    case "flight1":
                        CastHeldFlight(true);
                        break;
                    case "flight2":
                        CastHeldFlight(false);
                        break;
                    case "wargodswand":
                        CastHeldWarGodsWand(castingHand);
                        break;
                }
            } catch (System.Exception e) {
                Debug.Log("Error casting spell, error: "+e);
            }
            
            if (hand == "right") {
                ToggleSpellParticles(true, heldSpell, false);
                rightHandSpell = null;
                rightHandRenderer.material = baseMaterial;
            } else {
                ToggleSpellParticles(false, heldSpell, false);
                leftHandSpell = null;
                leftHandRenderer.material = baseMaterial;
            }
        }
    }

    public void NullifySpellCast(string hand) {
        string heldSpell = (hand == "right") ? rightHandSpell : leftHandSpell;
        if (hand == "right") {
            ToggleSpellParticles(true, heldSpell, false);
            rightHandSpell = null;
            rightHandRenderer.material = baseMaterial;
        } else {
            ToggleSpellParticles(false, heldSpell, false);
            leftHandSpell = null;
            leftHandRenderer.material = baseMaterial;
        }
    }

    public void WeaponHit(float damage) {
        if (damagedSound != null && !damagedSound.isPlaying) damagedSound.Play();
        playerManager.Damage(damage);
    }








    /* SPELLS
    --------------------------------------------------------------------------------------------------------
        --------------------------------------------------------------------------------------------------------
            --------------------------------------------------------------------------------------------------------
                -------------------------------------------------------------------------------------------------------- */

    public void PrepareSpell(string spell, string hand, float accuracy) {
        if (hand == "right") {
            rightHandSpell = spell;
        } else {
            leftHandSpell = spell;
        }

        // Round accuracy results for more consistent spell scaling effects
        print("Accuracy: "+accuracy);
        if (accuracy >= 0.74f) {
            accuracy = 1f;
        } else if (accuracy >= 0.64f) {
            accuracy = 0.75f;
        } else if (accuracy >= 0.44f ) {
            accuracy = 0.5f;
        }

        spellAccuracy = accuracy;

        ToggleSpellParticles((hand=="right"), spell);
        SetHandGlow(spell, hand);
    }

    //  ------------- GROUND CASTER ------------------
    public void CastHeldGroundCaster(GameObject castingHand) {
        // Should spawn at the feet
        // Get position from a ray that aims straight down
        RaycastHit raycastHit;
        Vector3 target = new Vector3(castingHand.transform.position.x, 0f, castingHand.transform.position.z);
        if( Physics.Raycast( castingHand.transform.position, Vector3.down, out raycastHit, 50f, LayerMask.GetMask("Ground") ) ) {
            print("Snapping to position: "+raycastHit.point+"   object: "+ raycastHit.collider.gameObject);
            target = raycastHit.point+new Vector3(0f,0.05f,0f);
        }

        GameObject newGroundCaster = Instantiate(groundCaster, target, transform.rotation);
    }

    //  ------------- SPELL COMPONENT ------------------
    public void CastHeldSpellComponent(string componentName, GameObject castingHand) {
        GameObject newComponent = Instantiate(spellComponent, castingHand.transform.position, castingHand.transform.rotation);
        SpellComponent sp = newComponent.GetComponent<SpellComponent>();
        if (sp != null) sp.Create(componentName);
    }



    //  ------------- FIREBALL ------------------
    public void CastHeldFireball(GameObject castingHand, bool directed=false) {
        GameObject newFireball = Instantiate(fireball, castingHand.transform.position, castingHand.transform.rotation);
        if (directed) newFireball.GetComponent<BasicProjectile>().LinkCastingHand(castingHand);
        //newFireball.GetComponent<Fireball>().Scale(spellAccuracy);
    }



    //  ------------- BLUE FIRE ------------------
    public void CastHeldBlueFire(GameObject castingHand) {
        GameObject newBlueFire = Instantiate(bluefire, castingHand.transform.position, castingHand.transform.rotation);
        newBlueFire.GetComponent<BasicProjectile>().SetOwner(gameObject);
        newBlueFire.GetComponent<BasicProjectile>().Scale(spellAccuracy);
    }







    //  ------------- SHIELD ------------------
    public void CastHeldShield(GameObject castingHand) {
        GameObject newShield = Instantiate(shield, castingHand.transform.position + (castingHand.transform.forward * 0.5f), castingHand.transform.rotation);

        shields.Add(newShield);
        if (shields.Count > maxShields) {
            GameObject oldShield = shields[0];
            shields.RemoveAt(0);
            Destroy(oldShield);
        }  
    }







    //  ------------- WIND SLASH ------------------
    public void CastHeldWindSlash(GameObject castingHand){
        GameObject newWindSlash = Instantiate(windslash, castingHand.transform.position, castingHand.transform.rotation);
        newWindSlash.GetComponent<WindSlash>().SetOwner(castingHand, gameObject);
        newWindSlash.GetComponent<WindSlash>().SetDirection(castingHand.transform.forward);
    }










    //  ------------- LIGHTNING ------------------
    public void CastHeldLightning(GameObject castingHand) {

        // Get position of the casting projectile ray target hit
        RaycastHit raycastHit;
        Vector3 target = castingHand.transform.position + ( 50f * castingHand.transform.forward );
        if( Physics.Raycast( castingHand.transform.position, castingHand.transform.forward, out raycastHit, 50f ) ) {
            target = raycastHit.point;
        }

        GameObject newLightning = Instantiate(lightning, castingHand.transform.position, castingHand.transform.rotation);
        newLightning.GetComponent<Lightning>().SetOwner(castingHand);
        newLightning.GetComponent<Lightning>().SetTarget(target);
    }







    //  ------------- ARCANOPULSE ------------------
    public void CastHeldArcanoPulse(GameObject castingHand) {
        GameObject newPulse = Instantiate(arcanePulse, castingHand.transform.position, castingHand.transform.rotation);
        newPulse.GetComponent<ArcanePulse>().SetOwner(gameObject);
    }









    //  ------------- ICE SPIKES ------------------
    public void CastHeldIceSpikes(GameObject castingHand) {
        //Ice spikes should spawn at the feet
        // Get position of the casting ray that aims straight down
        RaycastHit raycastHit;
        Vector3 target = new Vector3(castingHand.transform.position.x, 0f, castingHand.transform.position.z);
        if( Physics.Raycast( castingHand.transform.position, Vector3.down, out raycastHit, 50f, LayerMask.GetMask("Ground") ) ) {
            print("Snapping to position: "+raycastHit.point+"   object: "+ raycastHit.collider.gameObject);
            target = raycastHit.point;
        }

        GameObject newIceSpikes = Instantiate(iceSpikes, target, transform.rotation);
    }




    //  ------------- ICE SPRAY ------------------
    public void CastHeldIceSpray(GameObject castingHand) {
        GameObject newIceSpray = Instantiate(iceSpray, castingHand.transform.position, castingHand.transform.rotation);
        newIceSpray.transform.SetParent(castingHand.transform);
    }






    //  ------------- ROYAL FIRE ------------------
    public void CastHeldRoyalFire(GameObject castingHand){
        GameObject newRoyalFireball = Instantiate(royalFireball, castingHand.transform.position + (castingHand.transform.forward * 0.3f), transform.rotation);
    }








    //  ------------- Magic Missile ------------------
    public void CastHeldMagicMissile(GameObject castingHand, bool directed){
        GameObject newMagicMissile = Instantiate(magicMissile, castingHand.transform.position, castingHand.transform.rotation);
        // TODO newMagicMissile.GetComponent<MagicMissile>().Scale(spellAccuracy);
        if (directed) newMagicMissile.GetComponent<BasicProjectile>().LinkCastingHand(castingHand);
    }










     //  ------------- Summon Hammer ------------------
    public void CastHeldHammer(GameObject castingHand){
        GameObject newHammer = Instantiate(hammer, castingHand.transform.position, castingHand.transform.rotation);

        hammers.Add(newHammer);
        if (hammers.Count > maxHammers) {
            GameObject oldestHammer = hammers[0];
            hammers.RemoveAt(0);
            Destroy(oldestHammer);
        }
    }









    //  ------------- Earth Wall ------------------
    public void CastHeldEarthWall(GameObject castingHand) {
        // Get position of the casting projectile ray target hit
        RaycastHit raycastHit;
        if( Physics.Raycast( castingHand.transform.position, castingHand.transform.forward, out raycastHit, 50f, groundMask ) ) {
            Quaternion dir = Quaternion.Euler(0, castingHand.transform.rotation.eulerAngles.y, 0);
            GameObject newEarthWall = Instantiate(earthWall, raycastHit.point, dir);
        }

        
    }







    //  ------------- Farseer ------------------
    public void CastHeldFarseer(GameObject castingHand) {
        GameObject newFarseer = Instantiate(farseer, castingHand.transform.position+Vector3.down, transform.rotation);
        newFarseer.GetComponent<Farseer>().SetOwner(gameObject, castingHand);
    }





    //  ------------- See Dead ------------------
    public void CastHeldSeeDead(GameObject castingHand) {
        GameObject newSeeDead = Instantiate(seeDead, castingHand.transform.position, castingHand.transform.rotation);
        newSeeDead.transform.SetParent(castingHand.transform);
    }






    //  ------------- Defy Gravity ------------------
    public void CastHeldDefyGravity(GameObject castingHand) {
        GameObject newDefy = Instantiate(defy, castingHand.transform.position, castingHand.transform.rotation);
        newDefy.transform.SetParent(castingHand.transform);
        
        gameObject.GetComponent<MovementProvider>().ToggleDefyGravity(true);
        StartCoroutine(Defy());
    }
    IEnumerator Defy() {
        yield return new WaitForSeconds(5);
        gameObject.GetComponent<MovementProvider>().ToggleDefyGravity(false);
    }









    //  ------------- Return ------------------
    public void CastHeldReturn(GameObject castingHand) {
        GameObject newReturn = Instantiate(returnSpell, castingHand.transform.position, castingHand.transform.rotation);
        newReturn.GetComponent<Return>().SetOwner(gameObject, castingHand);
    }







    
    //  ------------- Midas Touch ------------------
    public void CastHeldMidasTouch(GameObject castingHand, string hand) {
        GameObject newMidasTouch = Instantiate(midasTouch, castingHand.transform.position, castingHand.transform.rotation);
        newMidasTouch.transform.SetParent(castingHand.transform);

        StartCoroutine(MidasHand(hand));
    }
   
    IEnumerator MidasHand(string hand) {
        if (hand == "right") {
            print("Try set midas hand right");
            SetHandGlow("midastouch", hand);
            yield return new WaitForSeconds(5f);
            rightHandRenderer.material = baseMaterial;
        } else {
            print("Try set midas hand left");
            SetHandGlow("midastouch", hand);
            yield return new WaitForSeconds(5f);
            leftHandRenderer.material = baseMaterial;
        }
    }



    //  ------------- Death ------------------
    public void CastHeldDeath(GameObject castingHand) {
        WeaponHit(999f);
    }



    //  ------------- Metal Fan ------------------
    public void CastHeldMetalFan(GameObject castingHand) {
        GameObject newMetalFan = Instantiate(metalFan, castingHand.transform.position, castingHand.transform.rotation);
    }

    //  ------------- Magnoset ------------------
    public void CastHeldMagnoset(GameObject castingHand) {
        GameObject newMagnoset = Instantiate(magnoset, castingHand.transform.position, castingHand.transform.rotation);
    }



    //  ------------- Marker ------------------
    public void CastHeldMarker(GameObject castingHand) {
        GameObject newMarker = Instantiate(marker, castingHand.transform.position, castingHand.transform.rotation);
    }


    //  ------------- Sword Summon ------------------
    public void CastHeldSword(GameObject castingHand) {
        GameObject newSword = Instantiate(sword, castingHand.transform.position, castingHand.transform.rotation);

        swords.Add(newSword);
        if (swords.Count > maxSwords) {
            GameObject oldestSword = swords[0];
            swords.RemoveAt(0);
            Destroy(oldestSword);
        }
    }



    //  ------------- Raise Skeleton ------------------
    public void CastHeldRaiseSkeleton(GameObject castingHand) {
        // Get position of the casting projectile ray target hit
        RaycastHit raycastHit;
        if( Physics.Raycast( castingHand.transform.position, castingHand.transform.forward, out raycastHit, 50f, groundMask ) ) {
            Quaternion dir = Quaternion.Euler(0, -castingHand.transform.rotation.eulerAngles.y, 0);
            GameObject newSkeleton = Instantiate(skeleton, raycastHit.point+(Vector3.up*3f), dir);
        }        
    }

    //  ------------- Drain Sphere ------------------
    public void CastHeldDrainSphere(GameObject castingHand) {
        GameObject newDrainSphere = Instantiate(drainSphere, castingHand.transform.position + (castingHand.transform.forward * 3f), castingHand.transform.rotation);
        newDrainSphere.GetComponent<DrainSphere>().LinkCastingHand(castingHand);
    }

    //  ------------- Binding ------------------
    public void CastHeldBinding(GameObject castingHand) {
        GameObject newBinding = Instantiate(binding, castingHand.transform.position, castingHand.transform.rotation);
        newBinding.GetComponent<Binding>().LinkCastingHand(castingHand);
        newBinding.transform.SetParent(castingHand.transform);
    }



    //  ------------- Flight ------------------
    public void CastHeldFlight(bool first) {
        if (!flightTimerRunning) {
            flightTimerRunning = true;
            StartCoroutine(FlightTimer());
        }

        if (first) {
            flight1 = true;
        } else {
            flight2 = true;
        }

        if (flight1 && flight2) {
            gameObject.GetComponent<MovementProvider>().ToggleFlight(true);
            GameObject rightHandFlight = Instantiate(flight, rightHand.transform.position, rightHand.transform.rotation);
            GameObject leftHandFlight = Instantiate(flight, leftHand.transform.position, leftHand.transform.rotation);
            rightHandFlight.transform.SetParent(rightHand.transform);
            leftHandFlight.transform.SetParent(leftHand.transform);
            StartCoroutine(Flight());
            flight1 = false;
            flight2 = false;
        }
    }

    IEnumerator FlightTimer() {
        yield return new WaitForSeconds(1);
        flight1 = false;
        flight2 = false;
        flightTimerRunning = false;
    }

    IEnumerator Flight() {
        yield return new WaitForSeconds(30);
        ToggleSpellParticles(true,"flight2", false);
        ToggleSpellParticles(false,"flight1", false);
        gameObject.GetComponent<MovementProvider>().ToggleFlight(false);
    }

    //  ------------- War Gods Wand ------------------
    public void CastHeldWarGodsWand(GameObject castingHand) {
        GameObject newWGW = Instantiate(wargodswand, castingHand.transform.position, castingHand.transform.rotation);
    }



    // ------ FIZZLE ------
    public void CastFizzle(string hand)
    {
        if (hand =="right") {
            Instantiate(fizzle, rightHand.transform.position, rightHand.transform.rotation);
        } else {
            Instantiate(fizzle, leftHand.transform.position, leftHand.transform.rotation);
        }
    }
}
