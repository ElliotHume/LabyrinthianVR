using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AdVd.GlyphRecognition;

/// <summary>
/// Utility monobehaviour to draw glyphs and strokes. The user may re-implement this class
/// in order to draw the strokes in a custom way.
/// </summary>
public class GlyphRecognition : MonoBehaviour {

	public string hand;
    public GlyphDrawInput glyphInput;

	public StrokeGraphic targetGlyphGraphic, castedGlyphGraphic, currentGlyphGraphic, currentStrokeGraphic, storedGlyphGraphic;

	public float costThreshold = 0.175f;
	public float CostThreshold {get{ return costThreshold; } set{
		costThreshold = value;
		//Debug.Log(value);
	}}

	public Stroke[] storedGlyph;

	public Player player;

	public GlyphMatch lastCast;

	// Variables for if the glyph is on a ground plane
	public bool groundCast = false;
	GroundDrawingPlane groundPlane;

	private enum CastDirection {Right, Left, Forward};

	private CastDirection currentCast;

	private bool stopStoredMorph = false;
	UnityEngine.UI.Image img;

    // This should potentially be in a constants file, though I'm not sure how to do importing in c#
    private Dictionary<string, Color> glyphColours = new Dictionary<string, Color>();

	void Start () {
		glyphInput = GetComponent<GlyphDrawInput>();
        glyphInput.OnGlyphCast.AddListener(this.OnGlyphCast);

		if (glyphInput.OnStrokeDraw!=this.OnStrokeDraw) glyphInput.OnStrokeDraw+=this.OnStrokeDraw;
		if (glyphInput.OnPointDraw!=this.OnPointDraw) glyphInput.OnPointDraw+=this.OnPointDraw;

		//StartCoroutine(CleanScreen());

        // Add Glyph Colour
        glyphColours.Add("fireball", new Color(1f, 0, 0));
		glyphColours.Add("shield", new Color(113/255f, 199/255f, 1f));
		glyphColours.Add("windslash", new Color(26/255f, 1f, 0));
		glyphColours.Add("lightning", new Color(1f, 247/255f, 103/255f));
		glyphColours.Add("arcanopulse", new Color(214/255f, 135/255f, 1f));
		glyphColours.Add("icespikes", new Color(127/255f, 126/255f, 253/255f));
		glyphColours.Add("icespray", new Color(127/255f, 126/255f, 253/255f));
		glyphColours.Add("royalfire", new Color(143/255f, 111/255f, 1f));
		glyphColours.Add("magicmissile", new Color(143/255f, 111/255f, 1f));
		glyphColours.Add("hammer", new Color(1f, 186/255f, 60/255f));
        glyphColours.Add("earthwall", new Color(165/255f, 145/255f, 0f));
		glyphColours.Add("default", new Color(191 / 255f, 110 / 255f, 54 / 255f, 64 / 255f));

	}

	void FixedUpdate() {
		if (player == null) {
			try {
				player = GameObject.Find("TestPlayer(Clone)").GetComponent<Player>();
			} catch {
				//Debug.Log("Cannot find player for Glyph Recognition");
			}
		}

		if (Input.GetKey("1")) {
            Cast();
        } else if (Input.GetKey("tab")) {
            glyphInput.EndCustomDrag(glyphInput.GetPrevPos());
        }

		// Clean screen
		if (!groundCast) {
			if (img == null) img = GetComponent<UnityEngine.UI.Image>();
			if (img.color.a > 0f) {
				float newAlpha = (img.color.a - 0.05f);
				img.color = new Color(img.color.r, img.color.g, img.color.b, newAlpha);
			}
		}
	}

	public void InitCleanScreen(){
		//StartCoroutine(CleanScreen());
	}

	IEnumerator CleanScreen() {
		UnityEngine.UI.Image img = GetComponent<UnityEngine.UI.Image>();
		while (true) {
			if (img.color.a > 0f) {
				float newAlpha = (img.color.a - 0.05f);
				img.color = new Color(img.color.r, img.color.g, img.color.b, newAlpha);
			}
			yield return new WaitForFixedUpdate();
		}
	}

	public GlyphMatch Match(Stroke[] strokes) {
		Glyph drawnGlyph = Glyph.CreateGlyph(strokes, glyphInput.sampleDistance);
        if (glyphInput.targetGlyphSet != null)
        {
			GlyphMatch match;
			int index = glyphInput.method.MultiMatch(drawnGlyph, glyphInput.targetGlyphSet, out match);
			return match;
		}
		return null;

	}

	Stroke Clone(Stroke stroke, out Vector2[] points) {
		points = new Vector2[stroke.Length];
		for(int i = 0; i < stroke.Length; i ++) {
			points[i] = stroke[i];
		}
		Stroke clone = new Stroke(points);
		return clone;
	}

	void Set(StrokeGraphic strokeGraphic, Glyph glyph)
    {
		if (strokeGraphic != null) strokeGraphic.SetStrokes(glyph);
	}
	void Set(StrokeGraphic strokeGraphic, Stroke[] strokes)
    {
		if (strokeGraphic != null) strokeGraphic.SetStrokes(strokes);
	}
	void Clear(StrokeGraphic strokeGraphic)
    {
		if (strokeGraphic != null) strokeGraphic.ClearStrokes();
	}
	bool IsClear(StrokeGraphic strokeGraphic)
    {
		return strokeGraphic == null || strokeGraphic.IsClear;
	}

	public void ClearAll(){
		if (targetGlyphGraphic != null) targetGlyphGraphic.ClearStrokes();
		if (castedGlyphGraphic != null) castedGlyphGraphic.ClearStrokes();
		if (currentGlyphGraphic != null) currentGlyphGraphic.ClearStrokes();
		if (currentStrokeGraphic != null) currentStrokeGraphic.ClearStrokes();
	}


	void OnGlyphCast(int index, GlyphMatch match){

		lastCast = match;
		
		// Reset casted glyph transparency
		targetGlyphGraphic.color = new Color(1f, 1f, 1f, 1f);

		Clear(currentGlyphGraphic);
		if (match == null || match.Cost > costThreshold) {
			Clear(targetGlyphGraphic);
			Clear(castedGlyphGraphic);
			if (match != null) print("Error cost of "+match.Cost+" too high for match: "+match.target.ToString());
			player.CastFizzle(hand);
			return;
		}
		
		Debug.Log("Match found:  "+ match.target.ToString()+"   Cost: "+match.Cost);
		// Debug.Log("Player:   "+player.name);
		// Make sure glyph recognition finishes and clears the stroke list
		// through any possible errors.
		try {
			StartCoroutine(Morph(match));
			// glyphColours[match.target.ToString()])
			if (!groundCast) {
				player.PrepareSpell(match.target.ToString(), hand, 1f - match.Cost/costThreshold);
			} else {
				groundPlane.CastSpell(match.target.ToString(), 1f - match.Cost/costThreshold);
			}
		}
		catch (System.Exception e) {
			Debug.LogError("Glyph recognition " + e + " occured. Clearing strokes.");
		}
		ClearAll();
	}

	const float step=0.01f;

	IEnumerator Morph(GlyphMatch match){
		//targetGlyphGraphic.color = new Color(1f, 1f, 1f, 1f);

		Clear(castedGlyphGraphic);
		Stroke[] strokes = null;
		/*
		for (float t=0;t<1;t+=0.05f){
			match.SetLerpStrokes(t, ref strokes);
			Set(targetGlyphGraphic,strokes);
			yield return new WaitForSeconds(step);
		}
		*/

		float t = 0f;
		while (t < 0.99f) {
			match.SetLerpStrokes(t, ref strokes);
			Set(targetGlyphGraphic, strokes);
			//targetGlyphGraphic.material.color = Color.Lerp(glyphColours["default"], color, t);
         	t += (1 - t) * 0.1f;
			yield return new WaitForSeconds(step);
		}

		Set(targetGlyphGraphic,match.target);
		if (IsClear(currentStrokeGraphic) && IsClear(currentGlyphGraphic)){
			Set(castedGlyphGraphic,match.source);
		}
	}

	IEnumerator MorphStored(GlyphMatch match) {
		Clear(castedGlyphGraphic);
		Stroke[] strokes = null;
		/*
		for (float t=0;t<1;t+=0.05f){
			match.SetLerpStrokes(t, ref strokes);
			Set(targetGlyphGraphic,strokes);
			yield return new WaitForSeconds(step);
		}
		*/
		float t = 0f;
		while (t < 0.99f && !stopStoredMorph) {
			match.SetLerpStrokes(t, ref strokes);
			Set(storedGlyphGraphic, strokes);
			storedGlyph = strokes;
			t += (1 - t) * 0.1f;
			yield return new WaitForSeconds(step);
		}
		if(!stopStoredMorph) Set(storedGlyphGraphic,match.target);
		stopStoredMorph = false;
	}

	void Cast(Stroke[] strokes, CastDirection dir) {
		Glyph newGlyph=Glyph.CreateGlyph(
			strokes,
			glyphInput.sampleDistance
		);
		currentCast = dir;
		glyphInput.Cast(newGlyph);
		glyphInput.ClearInput();
	}

	public bool Cast(){
		glyphInput.CommitStrokes();
		bool castSuccessful = glyphInput.Cast();
		glyphInput.ClearInput();
		return castSuccessful;
	}

	void getStored() {
		Set(currentGlyphGraphic, storedGlyph);
		Clear(storedGlyphGraphic);
		glyphInput.strokeList = new List<Stroke>(storedGlyph);
		storedGlyph = null;
		stopStoredMorph = true;
	}

	void OnStrokeDraw(Stroke[] strokes){
		Clear(currentStrokeGraphic);
		if (strokes == null) {
			Debug.Log("Null Stroke");
			return;
		}
		Stroke[] latestStroke = new Stroke[1];
		Stroke[] previousGlyph = new List<Stroke>(strokes).GetRange(0, strokes.Length - 1).ToArray();
		Vector2[] points;
		latestStroke[0] = Clone(strokes[strokes.Length - 1], out points);
		Vector2 vectorStroke = points[points.Length-1] - points[0];
		GlyphMatch castGlyph = Match(latestStroke);

		//Debug.Log(castGlyph.target.ToString());
        //Debug.Log(castGlyph.Cost);
		//Debug.Log("Stroke Drawn");
		Set(currentGlyphGraphic,strokes);
	}

	void OnPointDraw(Vector2[] points){
		Clear(castedGlyphGraphic);
		if (points!=null) Set(currentStrokeGraphic,new Stroke[]{ new Stroke(points) });
		else Clear(currentStrokeGraphic);
	}

	public void ChangePlayer(GameObject p) {
		player = p.GetComponent<Player>();
	}

	public void ActivateGroundDraw(GameObject go) {
		Debug.Log("Activate ground draw for "+gameObject);
		groundCast = true;
		groundPlane = go.GetComponent<GroundDrawingPlane>();
	}
}

