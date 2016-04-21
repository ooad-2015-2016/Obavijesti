using UnityEngine;
using System.Collections;

public class MetaBehavior : MonoBehaviour {
	public AudioClip explozijaSound;
	public float brzina;//brzina padanja
	public GameObject refParticle;
	public int boja =0;
    public GameObject gameLogic;
	// Use this for initialization
	void Start () {
        gameLogic = GameObject.Find("GameLogic");

    }
	// Update is called once per frame
	void Update () {
		//svaki frame pomjerimo po y osi za zadanu brzinu
		transform.position = transform.position - new Vector3(0,brzina * Time.deltaTime,0);
		if(transform.position.y<-2){
            gameLogic.GetComponent<LevelLogic>().displayMenu();
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.gameObject.tag == "Metak")
		{
			if(collision.collider.gameObject.GetComponent<MetakBehavior>().boja==boja){
				Destroy(collision.collider.gameObject);//unisti metak
				//postaviti efekat na poziciju objekta
				refParticle.transform.position=transform.position;
				//Pozovi emitovanje cestica Particle Emit
				refParticle.gameObject.transform.GetComponent<ParticleSystem>().startColor = GetComponent<Renderer>().material.color;
				refParticle.gameObject.transform.GetComponent<ParticleSystem>().Emit(20);
				AudioSource.PlayClipAtPoint(explozijaSound, gameLogic.transform.position,1f);
				Destroy(gameObject);//unisti ovaj objekat (objekat koji ima skriptu)
                gameLogic.GetComponent<LevelLogic>().dodajScore();
			}
		}
	}
}
