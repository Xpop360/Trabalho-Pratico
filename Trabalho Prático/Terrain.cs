using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trabalho_Prático
{
    class Terrain
    {
        VertexPositionTexture[] vertices;
        BasicEffect effect;
        Matrix worldMatrix = Matrix.Identity;
        short[] indices;

        public Terrain(GraphicsDevice device, ContentManager content)
        {
            Texture2D terrain = content.Load<Texture2D>("terrain");
            Texture2D texture = content.Load<Texture2D>("ground");
            
            effect = new BasicEffect(device);

            effect.TextureEnabled = true;
            effect.Texture = texture;

            CreateGrid(device, terrain);
        }

        private void CreateGrid(GraphicsDevice device, Texture2D heightMap)
        {
            int terrainWidth = heightMap.Width;
            int terrainHeight = heightMap.Height;

            Color[] heightMapColors = new Color[terrainWidth * terrainHeight];
            heightMap.GetData(heightMapColors);

            float[,] heightData = new float[terrainWidth, terrainHeight];

            Vector2 texturePosition;
            vertices = new VertexPositionTexture[terrainWidth * terrainHeight];

            for (int x = 0; x < terrainWidth; x++)
                for (int y = 0; y < terrainHeight; y++)
                {
                    heightData[x, y] = heightMapColors[x + y * terrainWidth].R / 25.5f;
                    texturePosition = new Vector2((float)x / 25.5f, (float)y / 25.5f);
                    vertices[x + y * terrainWidth] = new VertexPositionTexture(new Vector3(x, heightData[x, y], -y), texturePosition);
                }

            indices = new short[(terrainWidth - 1) * (terrainHeight - 1) * 6];
            int counter = 0;

            for (int y = 0; y < terrainHeight - 1; y++)
                for (int x = 0; x < terrainWidth - 1; x++)
                {
                    indices[counter++] = (short)(x + (y + 1) * terrainWidth);
                    indices[counter++] = (short)((x + 1) + y * terrainWidth);
                    indices[counter++] = (short)(x + y * terrainWidth);
                    indices[counter++] = (short)(x + (y + 1) * terrainWidth);
                    indices[counter++] = (short)((x + 1) + (y + 1) * terrainWidth);
                    indices[counter++] = (short)((x + 1) + y * terrainWidth);
                }
        }

        public void Draw(GraphicsDevice device)
        {
            effect.World = worldMatrix;

            effect.CurrentTechnique.Passes[0].Apply();
            device.DrawUserIndexedPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, vertices, 0, vertices.Length, indices, 0, indices.Length / 3);
        }
    }
}
