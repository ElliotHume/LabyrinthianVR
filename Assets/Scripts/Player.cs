using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Player : MonoBehaviour
{

    public GlyphRecognition glyphRecognition;

    public float spellVelocity = 20;
    public string heldSpell;
    public string lastSpellCast;
    private Vector3 startGripMove, releaseGripMove;
    public int dragMoveSpeed = 10;
    private float movementCooldown = 0f;
    public GameObject castingHand, movingHand, drawingAnchor;
    public XRController castingHandController, movingHandController;
    private SkinnedMeshRenderer castingHandRenderer, movementHandRenderer;
    public Material baseMaterial, spellHandGlow, movementCooldownGlow;
    public ParticleSystem fireParticles, lightningParticles, windParticles, royalParticles, iceParticles, damageParticles;
    public GameObject shieldSphere, arcanoSphere;
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


        castingHand = GameObject.Find("RightHand Controller");
        Debug.Log("Casting Hand GameObject: "+ castingHand);

        

        movingHand = GameObject.Find("LeftHand Controller");
        Debug.Log("Moving Hand GameObject: "+ movingHand);

        try {
            castingHandRenderer = GameObject.Find("hands:Rhand").GetComponent<SkinnedMeshRenderer>();
            Debug.Log("Casting Hand render: " + GameObject.Find("hands:Rhand"));

            movementHandRenderer = GameObject.Find("hands:Lhand").GetComponent<SkinnedMeshRenderer>();
            Debug.Log("Movement Hand render: " + GameObject.Find("hands:Lhand"));
        } catch {
            Debug.Log("Could not find hand renderers, will try again on FixedUpdate");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!castingHandRenderer) {
            try {
                castingHandRenderer = GameObject.Find("hands:Rhand").GetComponent<SkinnedMeshRenderer>();
                Debug.Log("Casting Hand render: " + GameObject.Find("hands:Rhand"));
            } catch {
                //do nothing
            }
            
        }
        if (!movementHandRenderer) {
            try {
                movementHandRenderer = GameObject.Find("hands:Lhand").GetComponent<SkinnedMeshRenderer>();
                Debug.Log("Movement Hand render: " + GameObject.Find("hands:Lhand"));
            } catch {
                // do nothing
            }
        }
        if (!drawingAnchor) {
            drawingAnchor = GameObject.Find("DrawingAnchor");
        }
    }

    void FixedUpdate() {
        if (castingHandController == null) {
            castingHandController = castingHand.GetComponent<XRController>();
        }

        if (movingHandController == null) {
            movingHandController = movingHand.GetComponent<XRController>();
        }

        if (heldSpell != null && heldSpell != "") {
            castingHandController.inputDevice.SendHapticImpulse(0, 0.5f, 0.1f);
        }
    }

    public void StartGripMove(Vector3 startPos){
        startGripMove = new Vector3(startPos.x, 0f, startPos.z);
        //Debug.Log("StartGripMove:  "+startGripMove.ToString());
        movingHandController.inputDevice.SendHapticImpulse(0, 0.5f, 0.3f);
    }

    public void ReleaseGripMove(Vector3 endPos) {
        releaseGripMove = new Vector3(endPos.x, 0f, endPos.z);

        if (movementCooldown == 0) {
            Vector3 movement = startGripMove - releaseGripMove;
            //Debug.Log("Movement: "+movement.ToString()+ "   Magnitude: "+movement.magnitude);

            // Move player in direction of pull * speed
            transform.position += movement * dragMoveSpeed;

            // Set hand cooldown material
            //movementHandRenderer.material = movementCooldownGlow;

            // Set movement Cooldown
            //movementCooldown = 2.5f;

            
            try {
                movingHandController.inputDevice.SendHapticImpulse(0, 2f, 0.1f);
                //movingHand.GetComponent<AudioSource>().Play();
            } catch {
                Debug.Log("Failure to send haptics or play sound effect");
            }
            
        } else {
            Debug.Log("Movement on cooldown");
        }
    }

    public void SetHandGlow(string spell){
        if(castingHandRenderer && spellHandGlow){
            Debug.Log("Setting hand colour to: "+handColours[spell]);
            spellHandGlow.SetColor("Color_682024A", handColours[spell]);
            // Debug.Log("Set hand material to "+spell);
            castingHandRenderer.material = spellHandGlow;
        }
    }

    public void ReleaseSpellCast() {
        if (heldSpell != null) {
            Debug.Log("Player Cast Held Spell: "+heldSpell);
            try {
                switch (heldSpell) {
                    case "fireball":
                        CastHeldFireball();
                        break;
                    case "shield":
                        CastHeldShield();
                        break;
                    case "lightning":
                        CastHeldLightning();
                        break;
                    case "windslash":
                        CastHeldWindSlash();
                        break;
                    case "royalfire":
                        CastHeldRoyalFire();
                        break;
                    case "icespikes":
                        CastHeldIceSpikes();
                        break;
                    case "icespray":
                        CastHeldIceSpray();
                        break;
                    case "arcanopulse":
                        CastHeldArcanoPulse();
                        break;
                }
            } catch (System.Exception e) {
                Debug.Log("Error casting spell, error: "+e);
            }
            
            lastSpellCast = heldSpell;
            heldSpell = null;
            castingHandRenderer.material = baseMaterial;

            // Reset movement cooldown when a spell is cast
            // movementCooldown = 0f;
            // movementHandRenderer.material = baseMaterial;
        }
    }









    /* SPELLS
    --------------------------------------------------------------------------------------------------------
        --------------------------------------------------------------------------------------------------------
            --------------------------------------------------------------------------------------------------------
                -------------------------------------------------------------------------------------------------------- */



    //  ------------- FIREBALL ------------------
    public virtual void CastFireball() {
        heldSpell = "fireball";
        SetHandGlow(heldSpell);
        if (fireParticles) {
            //print("fire particles play");
            fireParticles.Play();
        }
        // Debug.Log("PLAYERFIRECAST");
    }

    public void CastHeldFireball() {
        GameObject newFireball = Instantiate(fireball, castingHand.transform.position, castingHand.transform.rotation);
        if (fireParticles) fireParticles.Stop();
    }








    //  ------------- SHIELD ------------------
    public void CastShieldBack() {
        heldSpell = "shield";
        SetHandGlow(heldSpell);
        if (shieldSphere) {
            //print("activate shield sphere");
            shieldSphere.SetActive(true);
        }
        // Debug.Log("PLAYERSHIELDCAST");
    }

    public void CastHeldShield() {
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
    public void CastWindForward() {
        heldSpell = "windslash";
        SetHandGlow(heldSpell);
        if (windParticles) {
            //print("wind particles play");
            windParticles.Play();
        }
        // Debug.Log("PLAYERWINDSLASHCAST");
    }

    public void CastHeldWindSlash(){
        GameObject newWindSlash = Instantiate(windslash, castingHand.transform.position, castingHand.transform.rotation);
        newWindSlash.GetComponent<WindSlash>().SetOwner(castingHand, gameObject);
        newWindSlash.GetComponent<WindSlash>().SetDirection(castingHand.transform.forward);

        if (windParticles) windParticles.Stop();
    }










    //  ------------- LIGHTNING ------------------
    public void CastLightningNeutral() {
        heldSpell = "lightning";
        SetHandGlow("lightning");
        if (lightningParticles) {
            lightningParticles.Play();
        }
        // Debug.Log("PLAYERLIGHTNINGCAST");
    }

    public void CastHeldLightning() {

        // Get position of the casting projectile ray target hit
        RaycastHit raycastHit;
        Vector3 target = castingHand.transform.position + ( 50f * castingHand.transform.forward );
        if( Physics.Raycast( castingHand.transform.position, castingHand.transform.forward, out raycastHit, 50f ) ) {
            target = raycastHit.point;
        }

        GameObject newLightning = Instantiate(lightning, castingHand.transform.position, transform.rotation);
        newLightning.GetComponent<Lightning>().SetOwner(gameObject);
        newLightning.GetComponent<Lightning>().SetTarget(target);

        if (lightningParticles) lightningParticles.Stop();
    }







    //  ------------- ARCANOPULSE ------------------
    public void CastArcanePulse() {
        heldSpell = "arcanopulse";
        SetHandGlow(heldSpell);
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
    public void CastIceSpikes() {
        heldSpell = "icespikes";
        SetHandGlow(heldSpell);
        if (iceParticles) {
            //print("ice particles play");
            iceParticles.Play();
        }
    }

    public void CastHeldIceSpikes() {
        //Ice spikes should spawn at the feet
        Quaternion dir = Quaternion.Euler(0, castingHand.transform.rotation.eulerAngles.y, 0);
        Vector3 startPos = new Vector3(castingHand.transform.position.x, 0f, castingHand.transform.position.z) + castingHand.transform.forward*2f;
        GameObject newIceSpikes = Instantiate(iceSpikeProjectile, startPos, dir);

        if (iceParticles) iceParticles.Stop();
    }




    //  ------------- ICE SPRAY ------------------
    public void CastIceSpray() {
        heldSpell = "icespray";
        SetHandGlow(heldSpell);
        if (iceParticles) {
            //print("ice particles play");
            iceParticles.Play();
        }
    }

    public void CastHeldIceSpray() {
        GameObject newIceSpray = Instantiate(iceSpray, castingHand.transform.position, castingHand.transform.rotation);
        newIceSpray.transform.SetParent(castingHand.transform);

        if (iceParticles) iceParticles.Stop();
    }






    //  ------------- ROYAL FIRE ------------------
    public void CastRoyalFire() {
        heldSpell = "royalfire";
        SetHandGlow(heldSpell);
        if (royalParticles) {
            //print("royal particles play");
            royalParticles.Play();
        }
        // Debug.Log("PLAYERROYALFIRECAST");
    }

    public void CastHeldRoyalFire(){
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






    // ------ FIZZLE ------
    public void CastFizzle()
    {
        GameObject newFizzle = Instantiate(fizzle, new Vector3(castingHand.transform.position.x, 0f, castingHand.transform.position.z), transform.rotation);
    }
}
