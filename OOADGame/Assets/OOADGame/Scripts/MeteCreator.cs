using UnityEngine;
using System.Collections;

public class MeteCreator : MonoBehaviour {
	public Material[] materials;
	public GameObject refParticle;
	public GameObject[] mete;//niz meta
	private float cooldown = -2f;//cooldown za kreiranje mete
	public float cooldownAmount = 2;
	// Use this for initialization
	void Start () {
		refParticle = GameObject.Find("Efekat");//pronadjemo taj objekat u sceni
		refParticle = refParticle.transform.GetChild(0).gameObject;//spasi se referenca na particle unutar efekta
	}
	// Update is called once per frame
	void Update () {
		if(cooldown<0){
			createMeta();
			cooldown = cooldownAmount;
		}
		cooldown -= Time.deltaTime;
	}
	public void createMeta(){
		//Random meta na random x poziciji se kreira
		Vector3 createPosition = new Vector3(Random.Range(-4,4),14,0);
		GameObject metaObjekat = (GameObject) Instantiate(mete[(int)Random.Range(0,mete.Length)], createPosition, transform.rotation);
		metaObjekat.GetComponent<MetaBehavior>().refParticle = refParticle;//referenca na objekat za particle
		int boja = (int)Random.Range(0,3);
		metaObjekat.GetComponent<MetaBehavior>().boja = boja;
		metaObjekat.GetComponent<Renderer>().material = materials[boja];
	}
}
