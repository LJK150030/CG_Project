﻿using System.Collections;
using UnityEngine;

namespace Script
{
	public class Shoot : MonoBehaviour 
	{
		public Camera Cam;
		public int Radius;
		public int ConeRadius;

		private void Start()
		{
			Cam = transform.GetComponent<Camera>();
		}

		private void Update()
		{
			if (!Input.GetMouseButton(0))
				return;

			RaycastHit hit;

			var angle = Random.Range(0.0f, 1.0f) * Mathf.PI * 2.0f;
			var radius = Mathf.Sqrt(Random.Range(0.0f, 1.0f)) * ConeRadius;
			var newCenterX = Input.mousePosition.x + radius * Mathf.Cos(angle);
			var newCenterY = Input.mousePosition.y + radius * Mathf.Sin(angle);
			
			if (!Physics.Raycast(Cam.ScreenPointToRay(new Vector2(newCenterX, newCenterY)), out hit))
				return;
			
			var rend = hit.transform.GetComponent<Renderer>();
			var meshCollider = hit.collider as MeshCollider;

			if (rend == null || rend.sharedMaterial == null || rend.sharedMaterial.mainTexture == null || meshCollider == null)
				return;

			var tex = rend.material.mainTexture as Texture2D;
			var pixelUv = hit.textureCoord;
			if (tex == null) return;
			
			pixelUv.x *= tex.width;
			pixelUv.y *= tex.height;

			StartCoroutine(DrawCircle(tex, new Vector2(pixelUv.x, pixelUv.y), hit.distance));
		}

		private IEnumerator DrawCircle(Texture2D tex, Vector2 center, float distance)
		{
			int x;

			for (x = 0; x <= Radius; x++)
			{
				var d = (int)Mathf.Ceil(Mathf.Sqrt(Radius * Radius - x * x));
				int y;
				for (y = 0; y <= d; y++)
				{
					var px = Mathf.FloorToInt(center.x + x + 0.5f);
					var nx = Mathf.FloorToInt(center.x - x + 0.5f);
					var py = Mathf.FloorToInt(center.y + y + 0.5f);
					var ny = Mathf.FloorToInt(center.y - y + 0.5f);
 
					tex.SetPixel(px, py, Color.white);
					tex.SetPixel(nx, py, Color.white);
  
					tex.SetPixel(px, ny, Color.white);
					tex.SetPixel(nx, ny, Color.white);
				}
			}    
			
			tex.Apply ();
			yield return 0;
		}
	}
}
