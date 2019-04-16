using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TOLD
{
    public class PlankModel
    {
        Model m_model;
        Vector3 m_position;
        float m_angle;

        public PlankModel(ContentManager content)
        {
            m_model = content.Load<Model>("PlankModel");
            m_position = new Vector3(0, 0, -30);
            m_angle = 0;
        }

        public void Update(GameTime gameTime)
        {
            m_angle += 0.01f;
        }

        public void Draw(GraphicsDevice graphics)
        {
            // Enabling the depth buffer
            graphics.DepthStencilState = new DepthStencilState()
            {
                DepthBufferEnable = true
            };

            DrawModel(graphics, m_model, m_position, m_angle);

            graphics.DepthStencilState = new DepthStencilState()
            {
                DepthBufferEnable = false
            };
        }

        // Draws the model based on positions and camerapositions
        private void DrawModel(GraphicsDevice graphics, Model model, Vector3 position, float angle = 0)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();


                    effect.World = Matrix.CreateTranslation(position)  *  Matrix.CreateRotationX(MathHelper.ToRadians(245)) * Matrix.CreateRotationY(m_angle);

                    // Move the camera 150 units away from the origin:
                    var cameraPosition = new Vector3(150, 0, 150);
                    // Tell the camera to look at the origin:
                    var cameraLookAtVector = Vector3.Zero;
                    // Tell the camera that positive Y is up
                    var cameraUpVector = Vector3.UnitY;

                    effect.View = Matrix.CreateLookAt(cameraPosition, cameraLookAtVector, cameraUpVector);

                    float aspectRatio = graphics.Viewport.AspectRatio;
                    float fieldOfView = MathHelper.PiOver4;
                    float nearClipPlane = 1;
                    float farClipPlane = 2000;

                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, nearClipPlane, farClipPlane);
                }

                mesh.Draw();
            }
        }
    }
}
