using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Trabalho_Prático
{
    class ClsTank
    {
        Model model;
        Matrix world;
        float scale;

        ModelBone turretBone;
        ModelBone cannonBone;

        Matrix cannonTransform;
        Matrix turretTransform;

        float turretAngle = 0f;
        float cannonAngle = 0f;

        Matrix[] bonetransforms;

        Vector3 tankpos;
        Terrain Ground;
        Vector3 speed = new Vector3(1f, 0f, 0f);
        Vector3 up = Vector3.UnitY;
        float limitX, limitZ;

        public ClsTank(GraphicsDevice graphics, ContentManager content, Terrain Ground)
        {
           
            model = content.Load<Model>("tank");

            scale = 0.005f;

            turretBone = model.Bones["turret_geo"];
            cannonBone = model.Bones["canon_geo"]; //um n desgraça :V

            turretTransform = turretBone.Transform;
            cannonTransform = cannonBone.Transform;

            bonetransforms = new Matrix[model.Bones.Count];
            tankpos = new Vector3(120f, Ground.vertices[0].Position.Y, -120f);
            this.Ground = Ground;

            limitX = Ground.terrainHeight;
            limitZ = Ground.terrainWidth;
        }

        public void Update()
        {
            KeyboardState keyboard = Keyboard.GetState();

            //cannon + turret
            if (keyboard.IsKeyDown(Keys.Left))
            {
                turretAngle -= 0.05f;
            }
            if (keyboard.IsKeyDown(Keys.Right))
            {
                turretAngle += 0.05f;
            }
            if (keyboard.IsKeyDown(Keys.Up))
            {
                cannonAngle -= 0.02f;
            }
            if (keyboard.IsKeyDown(Keys.Down))
            {
                cannonAngle += 0.02f;
            }

            //Movement
            if (keyboard.IsKeyDown(Keys.D)) //rodar direita
            {
                speed = Vector3.Transform(speed, Matrix.CreateRotationY(0.025f));
            }
            if (keyboard.IsKeyDown(Keys.A)) //rodar esquerda
            {
                speed = Vector3.Transform(speed, Matrix.CreateRotationY(-0.025f));
            }
            if (keyboard.IsKeyDown(Keys.S)) //Baixo
            {
                if (!(tankpos.X + speed.X > (limitX - 1) || tankpos.Z + speed.Z < -(limitZ - 1) || tankpos.X + speed.X < 0 || tankpos.Z + speed.Z > 0))
                {
                    tankpos += speed;
                }
            }
            if (keyboard.IsKeyDown(Keys.W)) //Cima
            {
                if (!(tankpos.X - speed.X > (limitX - 1) || tankpos.Z - speed.Z < -(limitZ - 1) || tankpos.X - speed.X < 0 || tankpos.Z - speed.Z > 0))
                {
                    tankpos -= speed;
                }
            }

            tankpos.Y = Ground.vertices[(int)tankpos.Z * (-1) * Ground.terrainWidth + (int)tankpos.X].Position.Y;

            Matrix rotation = Matrix.Identity;
            Vector3 dir = speed;
            dir.Normalize();
            rotation.Forward = dir;
            rotation.Up = Ground.vertices[(int)tankpos.Z * (-1) * Ground.terrainWidth + (int)tankpos.Z].Normal;
            rotation.Right = Vector3.Cross(dir, rotation.Up);
            world = rotation * Matrix.CreateTranslation(tankpos);

            model.Root.Transform = Matrix.CreateScale(scale) * world;
            turretBone.Transform = Matrix.CreateRotationY(turretAngle) * turretTransform;
            cannonBone.Transform = Matrix.CreateRotationX(cannonAngle) * cannonTransform;

            model.CopyAbsoluteBoneTransformsTo(bonetransforms);
        }

        public void Draw(ClsCamera camera)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = bonetransforms[mesh.ParentBone.Index];
                    effect.View = camera.view;
                    effect.Projection = camera.projection;
                    effect.EnableDefaultLighting();
                }
                // Draw each mesh of the model
                mesh.Draw();
            }
        }
    }
}
