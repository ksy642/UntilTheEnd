using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RFX.Demo
{

	public class Trigger : MonoBehaviour
	{
		[SerializeField] private Transform[] objects;


		private void OnTriggerEnter(Collider other)
		{
			foreach (var obj in objects)
				Destroy(obj.gameObject);
		}
	}

}
