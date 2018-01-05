using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(ParticleSystem))]
[RequireComponent(typeof(BeamParam))]

public class GetColor_Particle : MonoBehaviour {

	// Use this for initialization
	void Start () {
		ParticleSystem ps = this.gameObject.GetComponent<ParticleSystem>();
		ParticleSystem.MainModule newMain = ps.main;

		newMain.startColor = this.transform.root.gameObject.GetComponent<BeamParam>().BeamColor;
		newMain.startSize = new ParticleSystem.MinMaxCurve(this.transform.root.gameObject.GetComponent<BeamParam>().Scale, newMain.startSize.curve);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
