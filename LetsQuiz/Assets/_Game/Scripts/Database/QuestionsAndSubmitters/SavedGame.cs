using System.Collections;
using UnityEngine;

namespace _LetsQuiz
{
	public class SavedGame
	{

		public WWW _submitRequest;


		public SavedGame(WWW submitRequest) {

			_submitRequest = submitRequest;

		}

		public WWW getRequest() {

			return _submitRequest;
		}



	}



}