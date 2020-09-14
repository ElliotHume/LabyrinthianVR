using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Player : MonoBehaviour
{

    public GlyphRecognition glyphRecognition;

    public float spellVelocity = 20;
    public string rightHandSpell = null, leftHandSpell = null;
    public string lastSpellCast;
    public GameObject rightHand, leftHand, drawingAnchor;
    public XRController rightHandController, leftHandController;
    private SkinnedMeshRenderer rightHandRenderer, leftHandRenderer;
    public Material baseMaterial, spellHandGlow_r, spellHandGlow_l;
    public ParticleSystem r_fireParticles, r_lightningParticles, r_windParticles, r_arcaneParticles, r_iceParticles;
    public ParticleSystem l_fireParticles, l_lightningParticles, l_windParticles, l_arcaneParticles, l_iceParticles;
    public GameObject r_shieldSphere, r_arcanoSphere, r_missileEmitter;
    public GameObject l_shieldSphere, l_arcanoSphere, l_missileEmitter;
    private Dictionary<string, Color> handColours = new Dictionary<string, Color>();

    // SPELL PREFABS
    public GameObject fireball;
    public GameObject shield;
    public GameObject windslash;
    public GameObject lightning;
    public GameObject arcanePulse;
    public GameObject iceSpikeProjectile, iceSpray;
    public GameObject fizzle;
    public GameObject royalFireball;
    public GameObject magicMissile;
    public GameObject hammer;
    public GameObject earthWall;
    public List<GameObject> shields;
    public int maxShields = 3;




    // Start is called before the first frame update
    void Start() {
        // hand colours
        handColours.Add("fireball", new Color(1f, 0, 0));
		handColours.Add("shield", new Color(113/255f, 199/255f, 1f));
		handColours.Add("windslash", new Color(26/255f, 1f, 0));
		handColours.Add("lightning", new Color(1f, 247/255f, 103/255f));
		handColours.Add("arcanopulse", new Color(214/255f, 135/255f, 1f));
		handColours.Add("icespikes", new Color(50/255f, 50/255f, 1f));
        handColours.Add("icespray", new Color(50/255f, 50/255f, 1f));
		handColours.Add("royalfire", new Color(156/255f, 0f, 1f));
        handColours.Add("magicmissile", new Color(156/255f, 0f, 1f));
        handColours.Add("hammer", new Color(1f, 186/255f, 60/255f));
        handColours.Add("earthwall", new Color(165/255f, 145/255f, 0f));

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
    }

    // Update is called once per frame
    void Update()
    {
        
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
    }

    void FixedUpdate() {
        if (rightHandController == null) {
            rightHandController = rightHand.GetComponent<XRController>();
        }

        if (leftHandController == null) {
            leftHandController = leftHand.GetComponent<XRController>();
        }

        if (rightHandSpell != null && rightHandSpell != "") {
            rightHandController.inputDevice.SendHapticImpulse(0, 0.5f, 0.1f);
        }

        if (leftHandSpell != null && leftHandSpell != "") {
            leftHandController.inputDevice.SendHapticImpulse(0, 0.5f, 0.1f);
        }
    }

    public void SetHandGlow(string spell, string hand){
        if(rightHandRenderer && leftHandRenderer){
            Debug.Log("Setting hand colour to: "+handColours[spell]);
            if (hand == "right") {
                spellHandGlow_r.SetColor("Color_682024A", handColours[spell]);
                rightHandRenderer.material = spellHandGlow_r;
            } else {
                spellHandGlow_l.SetColor("Color_682024A", handColours[spell]);
                leftHandRenderer.material = spellHandGlow_l;
            }
        }
    }

    public void ReleaseSpellCast(string hand) {
        string heldSpell = (hand == "right") ? rightHandSpell : leftHandSpell;
        GameObject castingHand = (hand == "right") ? rightHand : leftHand;

        if (heldSpell != null) {
            Debug.Log("Player Cast Held Spell: "+heldSpell+"    from hand: "+hand);
            try {
                switch (heldSpell) {
                    case "fireball":
                        CastHeldFireball(castingHand, hand);
                        break;
                    case "shield":
                        CastHeldShield(castingHand, hand);
                        break;
                    case "lightning":
                        CastHeldLightning(castingHand, hand);
                        break;
                    case "windslash":
                        CastHeldWindSlash(castingHand, hand);
                        break;
                    case "royalfire":
                        CastHeldRoyalFire(castingHand, hand);
                        break;
                    case "icespikes":
                        CastHeldIceSpikes(castingHand, hand);
                        break;
                    case "icespray":
                        CastHeldIceSpray(castingHand, hand);
                        break;
                    case "arcanopulse":
                        CastHeldArcanoPulse(hand);
                        break;
                    case "magicmissile":
                        CastHeldMagicMissile(castingHand, hand);
                        break;
                    case "hammer":
                        CastHeldHammer(castingHand, hand);
                        break;
                    case "earthwall":
                        CastHeldEarthWall(castingHand, hand);
                        break;
                }
            } catch (System.Exception e) {
                Debug.Log("Error casting spell, error: "+e);
            }
            
            if (hand == "right") {
                rightHandSpell = null;
                rightHandRenderer.material = baseMaterial;
            } else {
                leftHandSpell = null;
                leftHandRenderer.material = baseMaterial;
            }
        }
    }









    /* SPELLS
    --------------------------------------------------------------------------------------------------------
        --------------------------------------------------------------------------------------------------------
            --------------------------------------------------------------------------------------------------------
                -------------------------------------------------------------------------------------------------------- */



    //  ------------- FIREBALL ------------------
    public void CastFireball(string hand) {
        if (hand == "right") {
            rightHandSpell = "fireball";
            if (r_fireParticles) r_fireParticles.Play();
        } else {
            leftHandSpell = "fireball";
            if (l_fireParticles) l_fireParticles.Play();
        }
        SetHandGlow("fireball", hand);
        // Debug.Log("PLAYERFIRECAST");
    }

    public void CastHeldFireball(GameObject castingHand, string hand) {
        GameObject newFireball = Instantiate(fireball, castingHand.transform.position, castingHand.transform.rotation);
        if (hand == "right") {
            if (r_fireParticles) r_fireParticles.Stop();
        } else {
            if (l_fireParticles) l_fireParticles.Stop();
        } 
    }








    //  ------------- SHIELD ------------------
    public void CastShieldBack(string hand) {
        if (hand == "right") {
            rightHandSpell = "shield";
            if (r_shieldSphere) r_shieldSphere.SetActive(true);
        } else {
            leftHandSpell = "shield";
            if (l_shieldSphere) l_shieldSphere.SetActive(true);
        }
        SetHandGlow("shield", hand);
        // Debug.Log("PLAYERSHIELDCAST");
    }

    public void CastHeldShield(GameObject castingHand, string hand) {
        GameObject newShield = Instantiate(shield, castingHand.transform.position + (castingHand.transform.forward * 0.5f), castingHand.transform.rotation * Quaternion.Euler(90f, 0f, 90f));

        shields.Add(newShield);
        if (shields.Count > maxShields) {
            GameObject oldShield = shields[0];
            shields.RemoveAt(0);
            Destroy(oldShield);
        }

        if (hand == "right") {
            if (r_shieldSphere) r_shieldSphere.SetActive(false);
        } else {
            if (l_shieldSphere) l_shieldSphere.SetActive(false);
        } 
        
    }







    //  ------------- WIND SLASH ------------------
    public void CastWindForward(string hand) {
        if (hand == "right") {
            rightHandSpell = "windslash";
            if (r_windParticles) r_windParticles.Play();
        } else {
            leftHandSpell = "windslash";
            if (l_windParticles) l_windParticles.Play();
        }
        SetHandGlow("windslash", hand);
        // Debug.Log("PLAYERWINDSLASHCAST");
    }

    public void CastHeldWindSlash(GameObject castingHand, string hand){
        GameObject newWindSlash = Instantiate(windslash, castingHand.transform.position, castingHand.transform.rotation);
        newWindSlash.GetComponent<WindSlash>().SetOwner(castingHand, gameObject);
        newWindSlash.GetComponent<WindSlash>().SetDirection(castingHand.transform.forward);

        if (hand == "right") {
            if (r_windParticles) r_windParticles.Stop();
        } else {
            if (l_windParticles) l_windParticles.Stop();
        } 
    }










    //  ------------- LIGHTNING ------------------
    public void CastLightningNeutral(string hand) {
        if (hand == "right") {
            rightHandSpell = "lightning";
            if (r_lightningParticles) r_lightningParticles.Play();
        } else {
            leftHandSpell = "lightning";
            if (l_lightningParticles) l_lightningParticles.Play();
        }
        SetHandGlow("lightning", hand);
        // Debug.Log("PLAYERLIGHTNINGCAST");
    }

    public void CastHeldLightning(GameObject castingHand, string hand) {

        // Get position of the casting projectile ray target hit
        RaycastHit raycastHit;
        Vector3 target = castingHand.transform.position + ( 50f * castingHand.transform.forward );
        if( Physics.Raycast( castingHand.transform.position, castingHand.transform.forward, out raycastHit, 50f ) ) {
            target = raycastHit.point;
        }

        GameObject newLightning = Instantiate(lightning, castingHand.transform.position, castingHand.transform.rotation);
        newLightning.GetComponent<Lightning>().SetOwner(castingHand);
        newLightning.GetComponent<Lightning>().SetTarget(target);

        if (hand == "right") {
            if (r_lightningParticles) r_lightningParticles.Stop();
        } else {
            if (l_lightningParticles) l_lightningParticles.Stop();
        } 
    }







    //  ------------- ARCANOPULSE ------------------
    public void CastArcanePulse(string hand) {
        if (hand == "right") {
            rightHandSpell = "arcanopulse";
            if (r_arcanoSphere) r_arcanoSphere.SetActive(true);
        } else {
            leftHandSpell = "arcanopulse";
            if (l_arcanoSphere) l_arcanoSphere.SetActive(true);
        }
        SetHandGlow("arcanopulse", hand);
        // Debug.Log("PLAYERARCANOPULSECAST");
    }

    public void CastHeldArcanoPulse(string hand) {
        //Arcane Pulse should spawn at the feet
        GameObject newPulse = Instantiate(arcanePulse, new Vector3(transform.position.x, 0f, transform.position.z), transform.rotation);
        newPulse.GetComponent<ArcanePulse>().SetOwner(gameObject);

        if (hand == "right") {
            if (r_arcanoSphere) r_arcanoSphere.SetActive(false);
        } else {
            if (l_arcanoSphere) l_arcanoSphere.SetActive(false);
        } 
    }









    //  ------------- ICE SPIKES ------------------
    public void CastIceSpikes(string hand) {
        if (hand == "right") {
            rightHandSpell = "icespikes";
            if (r_iceParticles) r_iceParticles.Play();
        } else {
            leftHandSpell = "icespikes";
            if (l_iceParticles) l_iceParticles.Play();
        }
        SetHandGlow("icespikes", hand);
    }

    public void CastHeldIceSpikes(GameObject castingHand, string hand) {
        //Ice spikes should spawn at the feet
        Quaternion dir = Quaternion.Euler(0, castingHand.transform.rotation.eulerAngles.y, 0);
        Vector3 startPos = new Vector3(castingHand.transform.position.x, 0f, castingHand.transform.position.z) + castingHand.transform.forward*2f;
        GameObject newIceSpikes = Instantiate(iceSpikeProjectile, startPos, dir);

        if (hand == "right") {
            if (r_iceParticles) r_iceParticles.Stop();
        } else {
            if (l_iceParticles) l_iceParticles.Stop();
        }
    }




    //  ------------- ICE SPRAY ------------------
    public void CastIceSpray(string hand) {
        if (hand == "right") {
            rightHandSpell = "icespray";
            if (r_iceParticles) r_iceParticles.Play();
        } else {
            leftHandSpell = "icespray";
            if (l_iceParticles) l_iceParticles.Play();
        }
        SetHandGlow("icespray", hand);
    }

    public void CastHeldIceSpray(GameObject castingHand, string hand) {
        GameObject newIceSpray = Instantiate(iceSpray, castingHand.transform.position, castingHand.transform.rotation);
        newIceSpray.transform.SetParent(castingHand.transform);

        if (hand == "right") {
            if (r_iceParticles) r_iceParticles.Stop();
        } else {
            if (l_iceParticles) l_iceParticles.Stop();
        }
    }






    //  ------------- ROYAL FIRE ------------------
    public void CastRoyalFire(string hand) {
        if (hand == "right") {
            rightHandSpell = "royalfire";
            if (r_arcaneParticles) r_arcaneParticles.Play();
        } else {
            leftHandSpell = "royalfire";
            if (l_arcaneParticles) l_arcaneParticles.Play();
        }
        SetHandGlow("royalfire", hand);
        // Debug.Log("PLAYERROYALFIRECAST");
    }

    public void CastHeldRoyalFire(GameObject castingHand, string hand){
        GameObject newRoyalFireball = Instantiate(royalFireball, castingHand.transform.position, transform.rotation);

        // Get position of the casting projectile ray target hit
        RaycastHit raycastHit;
        Vector3 target = castingHand.transform.position + ( 50f * castingHand.transform.forward );
        if( Physics.Raycast( castingHand.transform.position, castingHand.transform.forward, out raycastHit, 50f ) ) {
            target = raycastHit.point;
        }

        newRoyalFireball.GetComponent<Royalfireball>().SetTarget(target);

        if (hand == "right") {
            if (r_arcaneParticles) r_arcaneParticles.Stop();
        } else {
            if (l_arcaneParticles) l_arcaneParticles.Stop();
        }
    }








    //  ------------- Magic Missile ------------------
    public void CastMagicMissile(string hand) {
        if (hand == "right") {
            rightHandSpell = "magicmissile";
            if (r_missileEmitter) r_missileEmitter.SetActive(true);
        } else {
            leftHandSpell = "magicmissile";
            if (l_missileEmitter) l_missileEmitter.SetActive(true);
        }
        SetHandGlow("magicmissile", hand);
    }

    public void CastHeldMagicMissile(GameObject castingHand, string hand){
        GameObject newMagicMissile = Instantiate(magicMissile, castingHand.transform.position, castingHand.transform.rotation);
        if (hand == "right") {
            if (r_missileEmitter) r_missileEmitter.SetActive(false);
        } else {
            if (l_missileEmitter) l_missileEmitter.SetActive(false);
        } 
    }










     //  ------------- Summon Hammer ------------------
    public void CastHammer(string hand) {
        if (hand == "right") {
            rightHandSpell = "hammer";
            // if (r_missileEmitter) r_missileEmitter.SetActive(true);
        } else {
            leftHandSpell = "hammer";
            // if (l_missileEmitter) l_missileEmitter.SetActive(true);
        }
        SetHandGlow("hammer", hand);
    }

    public void CastHeldHammer(GameObject castingHand, string hand){
        GameObject newHammer = Instantiate(hammer, castingHand.transform.position, castingHand.transform.rotation);
        // if (hand == "right") {
        //     if (r_missileEmitter) r_missileEmitter.SetActive(false);
        // } else {
        //     if (l_missileEmitter) l_missileEmitter.SetActive(false);
        // } 
    }









    //  ------------- Earth Wall ------------------
    public void CastEarthWall(string hand) {
        if (hand == "right") {
            rightHandSpell = "earthwall";
            //if (r_fireParticles) r_fireParticles.Play();
        } else {
            leftHandSpell = "earthwall";
            //if (l_fireParticles) l_fireParticles.Play();
        }
        SetHandGlow("earthwall", hand);
        // Debug.Log("PLAYERFIRECAST");
    }

    public void CastHeldEarthWall(GameObject castingHand, string hand) {
        // Get position of the casting projectile ray target hit
        RaycastHit raycastHit;
        Vector3 target = castingHand.transform.position + ( 50f * castingHand.transform.forward );
        if( Physics.Raycast( castingHand.transform.position, castingHand.transform.forward, out raycastHit, 50f ) ) {
            target = raycastHit.point;
        }

        Quaternion dir = Quaternion.Euler(0, castingHand.transform.rotation.eulerAngles.y, 0);

        GameObject newEarthWall = Instantiate(earthWall, target, dir);

        // if (hand == "right") {
        //     if (r_fireParticles) r_fireParticles.Stop();
        // } else {
        //     if (l_fireParticles) l_fireParticles.Stop();
        // } 
    }






    // ------ FIZZLE ------
    public void CastFizzle(string hand)
    {
        if (hand =="right") {
            Instantiate(fizzle, new Vector3(rightHand.transform.position.x, 0f, rightHand.transform.position.z), transform.rotation);
        } else {
            Instantiate(fizzle, new Vector3(leftHand.transform.position.x, 0f, leftHand.transform.position.z), transform.rotation);
        }
    }
}
