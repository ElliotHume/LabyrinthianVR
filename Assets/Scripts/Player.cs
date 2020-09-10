using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Player : MonoBehaviour
{

    public GlyphRecognition glyphRecognition;

    public float spellVelocity = 20;
    public string rightHandSpell, leftHandSpell;
    public string lastSpellCast;
    public GameObject rightHand, leftHand, drawingAnchor;
    public XRController rightHandController, leftHandController;
    private SkinnedMeshRenderer rightHandRenderer, leftHandRenderer;
    public Material baseMaterial, spellHandGlow, movementCooldownGlow;
    public ParticleSystem fireParticles, lightningParticles, windParticles, royalParticles, iceParticles, damageParticles;
    public GameObject shieldSphere, arcanoSphere, missileEmitter;
    private Dictionary<string, Color> handColours = new Dictionary<string, Color>();

    // SPELL PREFABS
    public GameObject fireball;
    public GameObject shield;
    public GameObject windslash;
    public GameObject lightningChargeObj;
    public GameObject lightning;
    public GameObject arcanePulse;
    public GameObject iceSpikeProjectile, iceSpray;
    public GameObject fizzle;
    public GameObject royalFireball;
    public GameObject magicMissile;
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
        if(rightHandRenderer && leftHandRenderer && spellHandGlow){
            Debug.Log("Setting hand colour to: "+handColours[spell]);
            spellHandGlow.SetColor("Color_682024A", handColours[spell]);
            // Debug.Log("Set hand material to "+spell);

            if (hand == "right") {
                rightHandRenderer.material = spellHandGlow;
            } else {
                leftHandRenderer.material = spellHandGlow;
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
                        CastHeldFireball(castingHand);
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
                        CastHeldArcanoPulse();
                        break;
                    case "magicmissile":
                        CastHeldMagicMissile(castingHand);
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
    public virtual void CastFireball(string hand) {
        if (hand == "right") {
            rightHandSpell = "fireball";
        } else {
            leftHandSpell = "fireball";
        }
        
        SetHandGlow("fireball", hand);
        if (fireParticles) {
            //print("fire particles play");
            fireParticles.Play();
        }
        // Debug.Log("PLAYERFIRECAST");
    }

    public void CastHeldFireball(GameObject castingHand) {
        GameObject newFireball = Instantiate(fireball, castingHand.transform.position, castingHand.transform.rotation);
        if (fireParticles) fireParticles.Stop();
    }








    //  ------------- SHIELD ------------------
    public void CastShieldBack(string hand) {
        if (hand == "right") {
            rightHandSpell = "shield";
        } else {
            leftHandSpell = "shield";
        }
        SetHandGlow("shield", hand);
        if (shieldSphere) {
            //print("activate shield sphere");
            shieldSphere.SetActive(true);
        }
        // Debug.Log("PLAYERSHIELDCAST");
    }

    public void CastHeldShield(GameObject castingHand) {
        GameObject newShield = Instantiate(shield, castingHand.transform.position + (castingHand.transform.forward * 0.5f), castingHand.transform.rotation * Quaternion.Euler(90f, 0f, 90f));

        shields.Add(newShield);
        if (shields.Count > maxShields) {
            GameObject oldShield = shields[0];
            shields.RemoveAt(0);
            Destroy(oldShield);
        }

        if (shieldSphere) shieldSphere.SetActive(false);
    }







    //  ------------- WIND SLASH ------------------
    public void CastWindForward(string hand) {
        if (hand == "right") {
            rightHandSpell = "windslash";
        } else {
            leftHandSpell = "windslash";
        }
        SetHandGlow("windslash", hand);
        if (windParticles) {
            //print("wind particles play");
            windParticles.Play();
        }
        // Debug.Log("PLAYERWINDSLASHCAST");
    }

    public void CastHeldWindSlash(GameObject castingHand){
        GameObject newWindSlash = Instantiate(windslash, castingHand.transform.position, castingHand.transform.rotation);
        newWindSlash.GetComponent<WindSlash>().SetOwner(castingHand, gameObject);
        newWindSlash.GetComponent<WindSlash>().SetDirection(castingHand.transform.forward);

        if (windParticles) windParticles.Stop();
    }










    //  ------------- LIGHTNING ------------------
    public void CastLightningNeutral(string hand) {
        if (hand == "right") {
            rightHandSpell = "lightning";
        } else {
            leftHandSpell = "lightning";
        }
        SetHandGlow("lightning", hand);
        if (lightningParticles) {
            lightningParticles.Play();
        }
        // Debug.Log("PLAYERLIGHTNINGCAST");
    }

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

        if (lightningParticles) lightningParticles.Stop();
    }







    //  ------------- ARCANOPULSE ------------------
    public void CastArcanePulse(string hand) {
        if (hand == "right") {
            rightHandSpell = "arcanopulse";
        } else {
            leftHandSpell = "arcanopulse";
        }
        SetHandGlow("arcanopulse", hand);
        if (arcanoSphere) {
            //print("activate arcano sphere");
            arcanoSphere.SetActive(true);
        }
        // Debug.Log("PLAYERARCANOPULSECAST");
    }

    public void CastHeldArcanoPulse() {
        //Arcane Pulse should spawn at the feet
        GameObject newPulse = Instantiate(arcanePulse, new Vector3(transform.position.x, 0f, transform.position.z), transform.rotation);
        newPulse.GetComponent<ArcanePulse>().SetOwner(gameObject);

        if (arcanoSphere) arcanoSphere.SetActive(false);
    }









    //  ------------- ICE SPIKES ------------------
    public void CastIceSpikes(string hand) {
        if (hand == "right") {
            rightHandSpell = "icespikes";
        } else {
            leftHandSpell = "icespikes";
        }
        SetHandGlow("icespikes", hand);
        if (iceParticles) {
            //print("ice particles play");
            iceParticles.Play();
        }
    }

    public void CastHeldIceSpikes(GameObject castingHand) {
        //Ice spikes should spawn at the feet
        Quaternion dir = Quaternion.Euler(0, castingHand.transform.rotation.eulerAngles.y, 0);
        Vector3 startPos = new Vector3(castingHand.transform.position.x, 0f, castingHand.transform.position.z) + castingHand.transform.forward*2f;
        GameObject newIceSpikes = Instantiate(iceSpikeProjectile, startPos, dir);

        if (iceParticles) iceParticles.Stop();
    }




    //  ------------- ICE SPRAY ------------------
    public void CastIceSpray(string hand) {
        if (hand == "right") {
            rightHandSpell = "icespray";
        } else {
            leftHandSpell = "icespray";
        }
        SetHandGlow("icespray", hand);
        if (iceParticles) {
            //print("ice particles play");
            iceParticles.Play();
        }
    }

    public void CastHeldIceSpray(GameObject castingHand) {
        GameObject newIceSpray = Instantiate(iceSpray, castingHand.transform.position, castingHand.transform.rotation);
        newIceSpray.transform.SetParent(castingHand.transform);

        if (iceParticles) iceParticles.Stop();
    }






    //  ------------- ROYAL FIRE ------------------
    public void CastRoyalFire(string hand) {
        if (hand == "right") {
            rightHandSpell = "royalfire";
        } else {
            leftHandSpell = "royalfire";
        }
        SetHandGlow("royalfire", hand);
        if (royalParticles) {
            //print("royal particles play");
            royalParticles.Play();
        }
        // Debug.Log("PLAYERROYALFIRECAST");
    }

    public void CastHeldRoyalFire(GameObject castingHand){
        GameObject newRoyalFireball = Instantiate(royalFireball, castingHand.transform.position, transform.rotation);

        // Get position of the casting projectile ray target hit
        RaycastHit raycastHit;
        Vector3 target = castingHand.transform.position + ( 50f * castingHand.transform.forward );
        if( Physics.Raycast( castingHand.transform.position, castingHand.transform.forward, out raycastHit, 50f ) ) {
            target = raycastHit.point;
        }

        newRoyalFireball.GetComponent<Royalfireball>().SetTarget(target);

        if (royalParticles) royalParticles.Stop();
    }








    //  ------------- Magic Missile ------------------
    public void CastMagicMissile(string hand) {
        if (hand == "right") {
            rightHandSpell = "magicmissile";
        } else {
            leftHandSpell = "magicmissile";
        }
        SetHandGlow("magicmissile", hand);
        if (missileEmitter) missileEmitter.SetActive(true);
    }

    public void CastHeldMagicMissile(GameObject castingHand){
        GameObject newMagicMissile = Instantiate(magicMissile, castingHand.transform.position, castingHand.transform.rotation);
        if (missileEmitter) missileEmitter.SetActive(false);
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
