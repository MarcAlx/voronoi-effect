using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace plasmaeffect.Engine
{
    /// <summary>
    /// A point moving randomly inside Width and height
    /// </summary>
    public class RandomMovingPoint
    {
        /// <summary>
        /// Position
        /// </summary>
        public Point Position { private set; get; } = new Point(0, 0);

        private int currentSpeed = 5;
        private int currentAngle = 45;

        private int boundWidth = 0;
        private int boundHeight = 0;

        private int textureWidth = 50;
        private int textureHeight = 50;

        private Random random = new Random();

        private Texture2D texture;

        public RandomMovingPoint()
        {
        }

        /// <summary>
        /// Update position of this point
        /// </summary>
        public void UpdatePosition(int boundWidth, int boundHeight)
        {
            this.boundWidth = boundWidth;
            this.boundHeight = boundHeight;
            var futureX = this.Position.X + Convert.ToInt32(this.currentSpeed * Math.Cos(Toolkit.ConvertToRadians(this.currentAngle)));
            var futureY = this.Position.Y + Convert.ToInt32(this.currentSpeed * Math.Sin(Toolkit.ConvertToRadians(this.currentAngle)));

            if (futureX <= 0
            ||  futureY <= 0
            ||  futureY > this.boundHeight
            ||  futureY > this.boundHeight)
            {
                this.currentAngle = random.Next(90, 270);
                this.currentSpeed = -this.currentSpeed;
            }

            futureX = Math.Max(Math.Min(this.boundWidth, futureX), 0);
            futureY = Math.Max(Math.Min(this.boundHeight, futureY), 0);

            //Console.WriteLine(futureX + " - " + futureY);
            this.Position = new Point(futureX, futureY);
        }

        /// <summary>
        /// Get texture for drawing
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public Texture2D GetTexture(GraphicsDevice device)
        {
            Texture2D rect = new Texture2D(device, textureWidth, textureHeight);

            Color[] data = new Color[textureWidth*textureHeight];
            for(var i = 0; i < data.Length; i++)
            {
                data[i] = Color.Black;
            }
            rect.SetData(data);

            return rect;
        }
    }
}
