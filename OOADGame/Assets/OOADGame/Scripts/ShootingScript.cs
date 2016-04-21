using UnityEngine;
using System.Collections;

public class ShootingScript : MonoBehaviour {
	private int boja =0;
	public Material[] materials;
	public AudioClip pucanjeSound;
	public GameObject metakPrefab;//metak objekat koji ce se instancirati
	private float cooldown=-2;//neka prvi put bude negativ tj. cooldown spreman
	private static float snaga = 20;//koliko jako ce se ispaliti metak
	private static float cooldownAmount = 0.3f;//koliki da bude cooldown inace
	// Use this for initialization
	void Start () {
		boja = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton(0) && cooldown<0) {//samo ako je cooldown gotov
			GameObject mojMetak = (GameObject) Instantiate(metakPrefab, transform.position, transform.rotation);
			Vector3 meta = Camera.main.ScreenToWorldPoint(Input.mousePosition);//iz koordinata 
			//ekrana gdje je mis nadji u 3d prostoru koja je to tacka
			meta.z = 0;//da ne bi metak otisao u trecu dimenziju
			mojMetak.GetComponent<Rigidbody>().velocity = meta.normalized*snaga - transform.position;//pocetno ubrzanje
			AudioSource.PlayClipAtPoint(pucanjeSound,transform.position,1.0f);
			cooldown = cooldownAmount;//reset cooldown
			mojMetak.GetComponent<MetakBehavior>().boja = boja;
			mojMetak.GetComponent<Renderer>().material = materials[boja];
		}
		if(Input.GetMouseButtonUp(1)){
			if(boja==materials.Length-1){
				boja = 0;
			}
			else{
				boja++; 
			}

		}
		cooldown -= Time.deltaTime;//smanji cooldown za kolicinu vremena koje je proslo
	}
}
