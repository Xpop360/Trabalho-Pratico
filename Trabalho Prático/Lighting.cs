using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Trabalho_Prático
{
    class Lighting
    {
        BasicEffect effect;
        public Lighting(BasicEffect effect)
        {
            this.effect = effect;
        }

        public void DefaultLighting()
        {
            effect.EnableDefaultLighting();
        }

        public void CustomLighting()
        {
            effect.LightingEnabled = true;
            effect.DirectionalLight0.DiffuseColor = Color.SandyBrown.ToVector3();
            effect.DirectionalLight0.Direction = new Vector3(1, -0.5f, 0);  // coming along the x-axis
            //effect.DirectionalLight0.SpecularColor = Color.White.ToVector3();

            effect.AmbientLightColor = new Vector3(0.5f, 0.5f, 0.5f);
            //effect.EmissiveColor = Color.White.ToVector3();
           
        }
    }
}
