using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace plasmaeffect.Engine
{
    public enum ColorRampEnum {
        GRAY_SCALE,
        RAINBOW
    }

    public class VoronoiPoint
    {
        public Color Color { get; set; }

        public Point Point { get; set; }
    }

    public class VoronoiEngine
    {
        private Dictionary<ColorRampEnum, Color[]> _colorRamp;

        public VoronoiEngine()
        {
            this._colorRamp = new Dictionary<ColorRampEnum, Color[]>();
            this._colorRamp[ColorRampEnum.GRAY_SCALE] = this.GenerateColorRamp(ColorRampEnum.GRAY_SCALE);
            this._colorRamp[ColorRampEnum.RAINBOW] = this.GenerateColorRamp(ColorRampEnum.RAINBOW);
        }

        /// <summary>
        /// Generate a color ramp according to provided enum
        /// </summary>
        /// <param name="ramp"></param>
        /// <returns></returns>
        private Color[] GenerateColorRamp(ColorRampEnum ramp)
        {
            Color[] res = new Color[256];

            if(ramp == ColorRampEnum.GRAY_SCALE)
            {
                for(int i = 0; i<256; i++)
                {
                    res[i] = new Color(i, i, i);
                }
            }
            else if (ramp == ColorRampEnum.RAINBOW)
            {
                for (int i = 0; i < 256; i++)
                {
                    res[i] = Toolkit.FromHsl((float)i/256f,1f,0.5f);
                    var d = res[i];
                }
            }

            return res;
        }

        /// <summary>
        /// Return distance between two points
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private double DistanceBetween(Point a, Point b)
        {
            return Math.Pow((b.X - a.X), 2) + Math.Pow((b.Y - a.Y), 2);
        }

        /// <summary>
        /// Return the closest from a list of point at a given position
        /// </summary>
        /// <param name="fromPoints"></param>
        /// <param name="at"></param>
        /// <returns></returns>
        private VoronoiPoint GetClosestPoint(List<VoronoiPoint> fromPoints, Point at)
        {
            return fromPoints.OrderBy(vPoint =>  DistanceBetween(vPoint.Point,at)).First();
        }

        /// <summary>
        /// Generate plasma effect on a device (as a Texture2D) with given width and height
        /// <returns></returns>
        public Texture2D GenerateVoronoi(GraphicsDevice device,int width,int height, List<VoronoiPoint> points, ColorRampEnum colorRamp = ColorRampEnum.GRAY_SCALE, int ratio = 50)
        {
            Texture2D rect = new Texture2D(device, width, height);

            Color[] data = new Color[width * height];
            var stepX = ratio;
            var stepY = ratio;
            for (int x = 0; x < width;x+=stepX)
            {
                for (int y = 0; y < height;y+=stepY)
                {
                    var closestPoint = this.GetClosestPoint(points, new Point(x, y));
                    var topA = Math.Min(x + stepX, width);
                    var topB = Math.Min(y + stepY, height);
                    for (int a = x; a < topA; a++)
                    {
                        for (int b = y; b < topB; b++)
                        {
                            data[((b) * width) + (a)] = closestPoint.Color;
                        }
                    }
                }
            }

            rect.SetData(data);

            return rect;
        }
    }
}
