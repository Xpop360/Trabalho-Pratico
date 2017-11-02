using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Trabalho_Prático
{
    class ClsCamera
    {
        Vector3 position;
        public Matrix view;
        public Matrix projection;
        Vector3 LookAt = new Vector3(1f, 0f, 0f);
        int limitX;
        int limitZ;
        float ScreenX;
        float ScreenY;
        Vector3 up = Vector3.UnitY;

        public ClsCamera(Vector3 position, GraphicsDevice device, Terrain terrain)
        {
            this.position = position;
            view = Matrix.Identity;
            this.limitX = terrain.terrainWidth;
            this.limitZ = terrain.terrainHeight;
            ScreenX = device.Viewport.Width;
            ScreenY = device.Viewport.Height;
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, device.Viewport.AspectRatio, 0.1f, 1000.0f);
        }

        public void Update()
        {
            KeyboardState keyboard = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();

            if(keyboard.IsKeyDown(Keys.NumPad6)) //direita
                position += Vector3.Cross(LookAt, up);

            if (mouse.Position.X > 3 * ScreenX / 4)//ver direita
                LookAt = Vector3.Transform(LookAt, Matrix.CreateRotationY(-0.025f));

            if (keyboard.IsKeyDown(Keys.NumPad4)) //esquerda
                position -= Vector3.Cross(LookAt, up);

            if (mouse.Position.X < ScreenX / 4)//ver esquerda
                LookAt = Vector3.Transform(LookAt, Matrix.CreateRotationY(0.025f));

            if (keyboard.IsKeyDown(Keys.NumPad8)) //Frente
                if (!(position.X + LookAt.X > (limitX - 1) || position.Z + LookAt.Z > (limitZ - 1) ||
                    position.X + LookAt.X < -(limitX - 1) || position.Z + LookAt.Z < -(limitZ - 1)))
                    position += LookAt;

            //if (mouse.Position.Y < ScreenY / 4)//Baixo fix
            //{
            //    LookAt = Vector3.Transform(LookAt, Matrix.CreateRotationZ(0.025f));
            //    up = Vector3.Transform(up, Matrix.CreateRotationZ(0.025f));
            //}

            if (keyboard.IsKeyDown(Keys.NumPad5)) //Trás
                if (!(position.X - LookAt.X > (limitX - 1) || position.Z - LookAt.Z > (limitZ - 1) ||
                    position.X - LookAt.X < -(limitX - 1) || position.Z - LookAt.Z < -(limitZ - 1)))
                    position -= LookAt;

            //if (mouse.Position.Y > 3 * ScreenY / 4)//Cima fix
            //{
            //    LookAt = Vector3.Transform(LookAt, Matrix.CreateRotationZ(-0.025f));
            //    up = Vector3.Transform(up, Matrix.CreateRotationZ(-0.025f));
            //}

            if (keyboard.IsKeyDown(Keys.NumPad7))
                position.Y += 1;

            if (keyboard.IsKeyDown(Keys.NumPad1))
                position.Y -= 1;
            
            Matrix rotation = Matrix.Identity;
            Vector3 dir = LookAt;
            dir.Normalize();
            rotation.Forward = dir;
            rotation.Up = up;
            rotation.Right = Vector3.Cross(dir, up);
            view = rotation * Matrix.CreateTranslation(position);

            view = Matrix.CreateLookAt(position, position + LookAt, view.Up);
        }
    }
}