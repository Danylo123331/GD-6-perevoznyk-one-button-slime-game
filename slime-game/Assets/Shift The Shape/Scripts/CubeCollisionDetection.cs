using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCollisionDetection : MonoBehaviour {

	public GameObject explosion;

	void OnCollisionEnter(Collision collision)
	{
		GameOver();
		Destroy(collision.gameObject);
	}
	
	private void GameOver() {
		GameObject.Find("explosionSound").GetComponent<AudioSource> ().Play();
		GameObject.Find("Canvas").GetComponent<MenuSelect>().GameOver();
		explosion.transform.parent = null;
		explosion.SetActive(true);
		Camera.main.transform.parent = null;
		Destroy(transform.parent.gameObject);
	}
}
