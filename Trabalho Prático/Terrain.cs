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
        public VertexPositionNormalTexture[] vertices;
        public BasicEffect effect;
        public Matrix worldMatrix = Matrix.Identity;
        Texture2D terrain, texture;
        short[] indices;
        public int terrainWidth;
        public int terrainHeight;

        public Terrain(GraphicsDevice device, ContentManager content)
        {
            terrain = content.Load<Texture2D>("terrain");
            texture = content.Load<Texture2D>("ground");

            effect = new BasicEffect(device);

            CreateGrid(device);
        }

        private void CreateGrid(GraphicsDevice device)
        {
            terrainWidth = terrain.Width;
            terrainHeight = terrain.Height;

            Color[] heightMapColors = new Color[terrainWidth * terrainHeight];
            terrain.GetData(heightMapColors);

            float[,] heightData = new float[terrainWidth, terrainHeight];

            Vector2 texturePosition;
            vertices = new VertexPositionNormalTexture[terrainWidth * terrainHeight];

            for (int x = 0; x < terrainWidth; x++)
                for (int y = 0; y < terrainHeight; y++)
                {
                    heightData[x, y] = heightMapColors[x + y * terrainWidth].R / 25.5f;
                    texturePosition = new Vector2((float)x / 25.5f, (float)y / 25.5f);

                    vertices[x + y * terrainWidth] = new VertexPositionNormalTexture(new Vector3(x, heightData[x, y], -y),Vector3.UnitY ,texturePosition);
                }

            for (int y = 0; y < terrainHeight; y++)//calculo das normais
            {
                for (int x = 0; x < terrainWidth; x++)//up left to up right
                {
                    if (x == 0 && y == 0)//up left
                    {
                        Vector3 vetor1 = vertices[1].Position - vertices[0].Position;
                        Vector3 vetor2 = vertices[terrainWidth + 1].Position - vertices[0].Position;
                        Vector3 vetor3 = vertices[terrainWidth].Position - vertices[0].Position;
                        Vector3 n1 = Vector3.Cross(vetor1, vetor2);
                        Vector3 n2 = Vector3.Cross(vetor2, vetor3);
                        Vector3 n3 = Vector3.Cross(vetor3, vetor1);
                        Vector3 median = (n1 + n2 + n3) / 3;
                        vertices[0].Normal = median;
                    }
                    else if (x == terrainWidth - 1 && y == 0)//up right
                    {
                        Vector3 vetor1 = vertices[x + terrainWidth].Position - vertices[x].Position;
                        Vector3 vetor2 = vertices[x + terrainWidth - 1].Position - vertices[x].Position;
                        Vector3 vetor3 = vertices[x - 1].Position - vertices[x].Position;
                        Vector3 n1 = Vector3.Cross(vetor1, vetor2);
                        Vector3 n2 = Vector3.Cross(vetor2, vetor3);
                        Vector3 n3 = Vector3.Cross(vetor3, vetor1);
                        Vector3 median = (n1 + n2 + n3) / 3;
                        vertices[terrainWidth - 1].Normal = median;
                    }
                    else if (x == 0 && y == terrainHeight - 1)//Bottom left
                    {
                        Vector3 vetor1 = vertices[(y - 1) * terrainHeight].Position - vertices[y * terrainHeight].Position;
                        Vector3 vetor2 = vertices[(y - 1) * terrainHeight + 1].Position - vertices[y * terrainHeight].Position;
                        Vector3 vetor3 = vertices[y * terrainHeight + 1].Position - vertices[y * terrainHeight].Position;
                        Vector3 n1 = Vector3.Cross(vetor1, vetor2);
                        Vector3 n2 = Vector3.Cross(vetor2, vetor3);
                        Vector3 n3 = Vector3.Cross(vetor3, vetor1);
                        Vector3 median = (n1 + n2 + n3) / 3;
                        vertices[(terrainHeight - 1) * terrainWidth].Normal = median;
                    }
                    else if (x == terrainWidth - 1 && y == terrainHeight - 1)//Bottom right
                    {
                        Vector3 vetor1 = vertices[y * terrainHeight + x - 1].Position - vertices[128 * 128 - 1].Position;
                        Vector3 vetor2 = vertices[(y - 1) * terrainHeight + x - 1].Position - vertices[128 * 128 - 1].Position;
                        Vector3 vetor3 = vertices[(y - 1) * terrainHeight + x].Position - vertices[128 * 128 - 1].Position;
                        Vector3 n1 = Vector3.Cross(vetor1, vetor2);
                        Vector3 n2 = Vector3.Cross(vetor2, vetor3);
                        Vector3 n3 = Vector3.Cross(vetor3, vetor1);
                        Vector3 median = (n1 + n2 + n3) / 3;
                        vertices[128 * 128 - 1].Normal = median;
                    }
                    else if (y == 0)//top
                    {
                        Vector3 vetor1 = vertices[x - 1].Position - vertices[x].Position;
                        Vector3 vetor2 = vertices[x - 1 + terrainWidth].Position - vertices[x].Position;
                        Vector3 vetor3 = vertices[x + terrainWidth].Position - vertices[x].Position;
                        Vector3 vetor4 = vertices[x + 1 + terrainWidth].Position - vertices[x].Position;
                        Vector3 vetor5 = vertices[x + 1].Position - vertices[x].Position;
                        Vector3 n1 = -Vector3.Cross(vetor1, vetor2);
                        Vector3 n2 = -Vector3.Cross(vetor2, vetor3);
                        Vector3 n3 = -Vector3.Cross(vetor3, vetor4);
                        Vector3 n4 = -Vector3.Cross(vetor4, vetor5);
                        Vector3 n5 = -Vector3.Cross(vetor5, vetor1);
                        Vector3 median = (n1 + n2 + n3 + n4 + n5) / 5;
                        vertices[x].Normal = median;
                    }
                    else if (x == 0)//left
                    {
                        Vector3 vetor1 = vertices[(y - 1) * terrainWidth].Position - vertices[y * terrainWidth].Position;
                        Vector3 vetor2 = vertices[(y - 1) * terrainWidth + 1].Position - vertices[y * terrainWidth].Position;
                        Vector3 vetor3 = vertices[y * terrainWidth + 1].Position - vertices[y * terrainWidth].Position;
                        Vector3 vetor4 = vertices[(y + 1) * terrainWidth + 1].Position - vertices[y * terrainWidth].Position;
                        Vector3 vetor5 = vertices[(y + 1) * terrainWidth].Position - vertices[y * terrainWidth].Position;
                        Vector3 n1 = Vector3.Cross(vetor1, vetor2);
                        Vector3 n2 = Vector3.Cross(vetor2, vetor3);
                        Vector3 n3 = Vector3.Cross(vetor3, vetor4);
                        Vector3 n4 = Vector3.Cross(vetor4, vetor5);
                        Vector3 n5 = Vector3.Cross(vetor5, vetor1);
                        Vector3 median = (n1 + n2 + n3 + n4 + n5) / 5;
                        vertices[y * terrainWidth].Normal = median;
                    }
                    else if (x == terrainWidth - 1)//Right
                    {
                        Vector3 vetor1 = vertices[(y - 1) * terrainWidth + x].Position - vertices[y * terrainHeight + x].Position;
                        Vector3 vetor2 = vertices[(y - 1) * terrainWidth + x - 1].Position - vertices[y * terrainHeight + x].Position;
                        Vector3 vetor3 = vertices[y * terrainWidth + x - 1].Position - vertices[y * terrainHeight + x].Position;
                        Vector3 vetor4 = vertices[(y + 1) * terrainWidth + x - 1].Position - vertices[y * terrainHeight + x].Position;
                        Vector3 vetor5 = vertices[(y + 1) * terrainWidth + x].Position - vertices[y * terrainHeight + x].Position;
                        Vector3 n1 = -Vector3.Cross(vetor1, vetor2);
                        Vector3 n2 = -Vector3.Cross(vetor2, vetor3);
                        Vector3 n3 = -Vector3.Cross(vetor3, vetor4);
                        Vector3 n4 = -Vector3.Cross(vetor4, vetor5);
                        Vector3 n5 = -Vector3.Cross(vetor5, vetor1);
                        Vector3 median = (n1 + n2 + n3 + n4 + n5) / 5;
                        vertices[y * terrainHeight + terrainWidth - 1].Normal = median;
                    }
                    else if (y == terrainHeight - 1)//Bottom
                    {
                        Vector3 vetor1 = vertices[y * terrainHeight + x - 1].Position - vertices[y * terrainHeight + x].Position;
                        Vector3 vetor2 = vertices[(y - 1) * terrainHeight + x - 1].Position - vertices[y * terrainHeight + x].Position;
                        Vector3 vetor3 = vertices[(y - 1) * terrainHeight + x].Position - vertices[y * terrainHeight + x].Position;
                        Vector3 vetor4 = vertices[(y - 1) * terrainHeight + x + 1].Position - vertices[y * terrainHeight + x].Position;
                        Vector3 vetor5 = vertices[y * terrainHeight + x + 1].Position - vertices[y * terrainHeight + x].Position;
                        Vector3 n1 = Vector3.Cross(vetor1, vetor2);
                        Vector3 n2 = Vector3.Cross(vetor2, vetor3);
                        Vector3 n3 = Vector3.Cross(vetor3, vetor4);
                        Vector3 n4 = Vector3.Cross(vetor4, vetor5);
                        Vector3 n5 = Vector3.Cross(vetor5, vetor1);
                        Vector3 median = (n1 + n2 + n3 + n4 + n5) / 5;
                        vertices[y * (terrainHeight - 1) + x].Normal = median;
                    }
                    else
                    {
                        Vector3 vetor1 = vertices[(y - 1) * terrainHeight + x].Position - vertices[y * terrainHeight + x].Position;
                        Vector3 vetor2 = vertices[(y - 1) * terrainHeight + x + 1].Position - vertices[y * terrainHeight + x].Position;
                        Vector3 vetor3 = vertices[y * terrainHeight + x + 1].Position - vertices[y * terrainHeight + x].Position;
                        Vector3 vetor4 = vertices[(y + 1) * terrainHeight + x + 1].Position - vertices[y * terrainHeight + x].Position;
                        Vector3 vetor5 = vertices[(y + 1) * terrainHeight + x].Position - vertices[y * terrainHeight + x].Position;
                        Vector3 vetor6 = vertices[(y + 1) * terrainHeight + x - 1].Position - vertices[y * terrainHeight + x].Position;
                        Vector3 vetor7 = vertices[y * terrainHeight + x - 1].Position - vertices[y * terrainHeight + x].Position;
                        Vector3 vetor8 = vertices[(y - 1) * terrainHeight + x - 1].Position - vertices[y * terrainHeight + x].Position;
                        Vector3 n1 = Vector3.Cross(vetor1, vetor2);
                        Vector3 n2 = Vector3.Cross(vetor2, vetor3);
                        Vector3 n3 = Vector3.Cross(vetor3, vetor4);
                        Vector3 n4 = Vector3.Cross(vetor4, vetor5);
                        Vector3 n5 = Vector3.Cross(vetor5, vetor6);
                        Vector3 n6 = Vector3.Cross(vetor6, vetor7);
                        Vector3 n7 = Vector3.Cross(vetor7, vetor8);
                        Vector3 n8 = Vector3.Cross(vetor8, vetor1);
                        Vector3 median = (n1 + n2 + n3 + n4 + n5 + n6 + n7 + n8) / 8;
                        vertices[y * terrainHeight + x].Normal = median;
                    }
                }
            }

            indices = new short[(terrainWidth - 1) * (terrainHeight - 1) * 6];
            int counter = 0;

            for (int y = 0; y < terrainHeight - 1; y++)
                for (int x = 0; x < terrainWidth - 1; x++)
                {
                    int lowerLeft = x + y * terrainWidth;
                    int lowerRight = (x + 1) + y * terrainWidth;
                    int topLeft = x + (y + 1) * terrainWidth;
                    int topRight = (x + 1) + (y + 1) * terrainWidth;

                    indices[counter++] = (short)topLeft;
                    indices[counter++] = (short)lowerRight;
                    indices[counter++] = (short)lowerLeft;

                    indices[counter++] = (short)topLeft;
                    indices[counter++] = (short)topRight;
                    indices[counter++] = (short)lowerRight;
                }
        }

        public void Draw(GraphicsDevice device, ClsCamera camera)
        {
            effect.TextureEnabled = true;
            effect.Texture = texture;
            effect.VertexColorEnabled = false;

            effect.World = worldMatrix;
            effect.View = camera.view;
            effect.Projection = camera.projection;

            effect.CurrentTechnique.Passes[0].Apply();
            device.DrawUserIndexedPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, vertices, 0, vertices.Length, indices, 0, indices.Length / 3);
        }
    }
}