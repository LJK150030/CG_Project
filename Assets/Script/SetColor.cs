using System.Collections;
using UnityEngine;

namespace Script
{
	public class SetColor : MonoBehaviour
	{

		public Texture2D Blank;
		
		private Renderer _childRender;
		private Texture2D _newBlack;
		private float _timeLeft;
		private float _numObjects;
		

		// Use this for initialization
		private void Start()
		{
			_timeLeft = 0.0f;
			_numObjects = 0.0f;
			print("percentage done: " + _timeLeft);
			RecursiveCount(transform);
			_timeLeft += 1.0f / _numObjects;
			StartCoroutine(RecursiveColor(transform));
		}

		private void RecursiveCount(Transform t)
		{
			if (t.childCount == 0)
			{
				_numObjects += 1.0f;
				
			}
			else
			{
				for (var i = 0; i < t.childCount; i++)
				{
					RecursiveCount(t.GetChild(i));
				}
			}
		}

		private IEnumerator RecursiveColor(Transform t)
		{
			//base case
			if (t.childCount == 0)
			{
				_childRender = t.GetComponent<Renderer>();
				
				foreach (var VARIABLE in _childRender.materials)
				{
					_newBlack = new Texture2D(Blank.width, Blank.height);

					for (var i = 0; i < Blank.width; i++)
					{
						for (var j = 0; j < Blank.height; j++)
						{
							_newBlack.SetPixel(i, j, Color.black);
						}
					}
				
					_newBlack.Apply();
					VARIABLE.mainTexture = _newBlack;
					print("percentage done: " + _timeLeft);
				}
				
				_timeLeft += 1.0f / _numObjects;
				
				yield return 0;
			}
			else
			{
				for (var i = 0; i < t.childCount; i++)
				{
					//yield return StartCoroutine(RecursiveColor(t.GetChild(i)));
					yield return RecursiveColor(t.GetChild(i));
				}
			}
		}
	}
}
