using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Game
{
	/// <summary>
	/// Usability wrapper for the mathf perlin noise function.
	/// </summary>
	public class PerlinNoiseMap
	{
		private PerlinNoiseMapData _Data;
		private float[,] _Map;

		public PerlinNoiseMapData Data => _Data;
		public float[,] GrayScaleMap => _Map;

		public PerlinNoiseMap(int width, int height, float xOrigin, float yOrigin, float scale = 1.0f)
		{
			Remap(new PerlinNoiseMapData(width, height, xOrigin, yOrigin, scale));
		}

		public PerlinNoiseMap(PerlinNoiseMapData data)
		{
			Remap(data);
		}

		/// <summary>
		/// Returns the float between 0 and 1 represented by the x and y position.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public float GetNormalValue(int x, int y)
		{
			float xCoord = _Data.XOrigin + ((float)x / _Data.Width * _Data.Scale);
			float yCoord = _Data.YOrigin + ((float)y / _Data.Height * _Data.Scale);
			return Mathf.PerlinNoise(xCoord, yCoord);
		}

		/// <summary>
		/// Remaps the normal values according to the current data. Call this function after you changed the current data.
		/// </summary>
		public void Remap()
		{
			_Map = new float[_Data.Width, _Data.Height];

			for (int y = 0; y < _Data.Height; y++)
			{
				for (int x = 0; x < _Data.Width; x++)
				{
					_Map[x, y] = GetNormalValue(x, y);
				}
			}
		}

		/// <summary>
		/// Remaps the normal values according to new data.
		/// </summary>
		/// <param name="data">The new data to use.</param>
		public void Remap(PerlinNoiseMapData data)
		{
			_Data = data;
			Remap();
		}

		/// <summary>
		/// Returns a new Texture2D containing the noisemap as a grayscale image.
		/// </summary>
		/// <returns></returns>
		public Texture2D ToTexture2D()
		{
			Texture2D texture = new Texture2D(_Data.Width, _Data.Height);
			Color[] pixels = new Color[_Data.Width * _Data.Height];

			for (int y = 0; y < texture.height; y++)
			{
				for (int x = 0; x < texture.width; x++)
				{
					float sample = _Map[x, y];
					pixels[(y * texture.width) + x] = new Color(sample, sample, sample);
				}
			}

			texture.SetPixels(pixels);
			texture.Apply();

			return texture;
		}
	}

	[System.Serializable]
	public struct PerlinNoiseMapData
	{
		[SerializeField] private int _Width;
		[SerializeField] private int _Height;
		[SerializeField] private float _XOrigin;
		[SerializeField] private float _YOrigin;
		[SerializeField] private float _Scale;

		public int Width { get => _Width; set => _Width = value; }
		public int Height { get => _Height; set => _Height = value; }
		public float XOrigin { get => _XOrigin; set => _XOrigin = value; }
		public float YOrigin { get => _YOrigin; set => _YOrigin = value; }
		public float Scale { get => _Scale; set => _Scale = value; }

		public PerlinNoiseMapData(int width, int height, float xOrigin, float yOrigin, float scale = 1.0f)
		{
			_Width = width;
			_Height = height;
			_XOrigin = xOrigin;
			_YOrigin = yOrigin;
			_Scale = scale;
		}
	}
}