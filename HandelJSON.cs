using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _LetsQuiz{
public class HandelJSON : MonoBehaviour {

	private GetAllQuestions _getallquestions;
	private string jsonString;

	// Use this for initialization
	void Start () {

		//trash timer to ensure the string has been downloaded from teh server
		float timer = 0;
		while (timer < 5) {
			timer += Time.deltaTime;
			Debug.Log (timer);
		}

		//this is probably stored in the datacontroller
		_getallquestions = FindObjectOfType<GetAllQuestions>();
		jsonString = _getallquestions.getJSON ();

		databaseArrayObject thing = JsonUtility.FromJson (jsonString);

		Debug.Log (thing.databaseThings.Length);



	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

}