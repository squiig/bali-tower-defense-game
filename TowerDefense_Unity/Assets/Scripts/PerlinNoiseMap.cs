using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Game
{
	[System.Serializable]
	public struct PerlinNoiseMapData
	{
		[SerializeField] private int m_Height;
		[SerializeField] private float m_Scale;
		[SerializeField] private int m_Width;
		[SerializeField] private float m_XOrg;
		[SerializeField] private float m_YOrg;
        
		public int Height { get { return m_Height; } set { m_Height = value; } }
		public float Scale { get { return m_Scale; } set { m_Scale = value; } }
		public int Width { get { return m_Width; } set { m_Width = value; } }
		public float XOrigin { get { return m_XOrg; } set { m_XOrg = value; } }
		public float YOrigin { get { return m_YOrg; } set { m_YOrg = value; } }
        
		public PerlinNoiseMapData(int width, int height, float xOrigin, float yOrigin, float scale = 1.0f)
		{
			m_Width = width;
			m_Height = height;
			m_XOrg = xOrigin;
			m_YOrg = yOrigin;
			m_Scale = scale;
		}
	}

	public class PerlinNoiseMap
	{
		private PerlinNoiseMapData m_Data;
		private float[,] m_Map;

        public PerlinNoiseMapData Data => m_Data;
        public float[,] GrayScaleMap => m_Map;

        public PerlinNoiseMap(int width, int height, float xOrg, float yOrg, float scale = 1.0f)
		{
			Remap(new PerlinNoiseMapData(width, height, xOrg, yOrg, scale));
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
			float xCoord = m_Data.XOrigin + ((float) x / m_Data.Width * m_Data.Scale);
			float yCoord = m_Data.YOrigin + ((float) y / m_Data.Height * m_Data.Scale);
			return Mathf.PerlinNoise(xCoord, yCoord);
		}

		/// <summary>
		/// Remaps the normal values according to the current data. Call this function after you changed the current data.
		/// </summary>
		public void Remap()
		{
			m_Map = new float[m_Data.Width, m_Data.Height];

			for (int y = 0; y < m_Data.Height; y++)
			{
				for (int x = 0; x < m_Data.Width; x++)
				{
					m_Map[x, y] = GetNormalValue(x, y);
				}
			}
		}

		/// <summary>
		/// Remaps the normal values according to new data.
		/// </summary>
		/// <param name="data">The new data to use.</param>
		public void Remap(PerlinNoiseMapData data)
		{
			m_Data = data;
			Remap();
		}

		/// <summary>
		/// Returns a new Texture2D containing the noisemap as a grayscale image.
		/// </summary>
		/// <returns></returns>
		public Texture2D ToTexture2D()
		{
			Texture2D texture = new Texture2D(m_Data.Width, m_Data.Height);
			Color[] pixels = new Color[m_Data.Width * m_Data.Height];

			for (int y = 0; y < texture.height; y++)
			{
				for (int x = 0; x < texture.width; x++)
				{
					float sample = m_Map[x, y];
					pixels[((y * texture.width) + x)] = new Color(sample, sample, sample);
				}
			}

			texture.SetPixels(pixels);
			texture.Apply();

			return texture;
		}
	}
}