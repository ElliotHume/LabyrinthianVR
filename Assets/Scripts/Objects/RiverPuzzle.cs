using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Linq;

public class RiverPuzzle : MonoBehaviour
{
    public XRSocketInteractor tile1, tile2, tile3;
    public string answer1, answer2, answer3;
    private string guess1, guess2, guess3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaceTile() {
        try {
            IEnumerable<Collider> colls = tile1.selectTarget.colliders;
            List<Collider> listColls = colls.ToList();
            guess1 = listColls[0].gameObject.name;
        } catch {
            //do nothing for now
        }
        Debug.Log("guess1 : "+guess1);

        try {
            IEnumerable<Collider> colls = tile2.selectTarget.colliders;
            List<Collider> listColls = colls.ToList();
            guess2 = listColls[0].gameObject.name;
        } catch {
            //do nothing for now
        }
        Debug.Log("guess2 : "+guess2);

        try {
            IEnumerable<Collider> colls = tile3.selectTarget.colliders;
            List<Collider> listColls = colls.ToList();
            guess3 = listColls[0].gameObject.name;
        } catch {
            //do nothing for now
        }
        Debug.Log("guess3 : "+guess3);


        if (guess1+guess2+guess3 == answer1+answer2+answer3) {
            GetComponent<AudioSource>().Play();
            Destroy(gameObject, 0.5f);
        }
    }

}
