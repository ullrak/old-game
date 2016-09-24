using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TrytomakeaMapengine
{
    class Camera
    {
        private Matrix transform;

        public Matrix Transform
        {
            get { return transform; }
        }

        private Vector2 center;
        private Viewport viewport;
        private float zoom = 1;
        private float rotation = 0;

        public float X
        {
            get { return center.X; }
            set { center.X = value; }
        }

        public float Y
        {
            get { return center.Y; }
            set { center.Y = value; }
        }

        public float Zoom
        {
            get { return zoom; }
            set { zoom = value; if (zoom < 0.1f) zoom = 0.1f; }
        }

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public Camera(Viewport newViewport)
        {
            viewport = newViewport;
        }

        public void Update(Vector2 position)
        {
            center = new Vector2(position.X, position.Y);

            transform = Matrix.CreateTranslation(new Vector3(-center.X, -center.Y, 0)) * 
                                                Matrix.CreateRotationZ(Rotation) * 
                                                Matrix.CreateScale(new Vector3(Zoom,Zoom,0)) * 
                                                Matrix.CreateTranslation(new Vector3(viewport.Width / 2, viewport.Height / 2, 0));
        }
    }
}
