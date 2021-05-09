using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace plasmaeffect.Engine
{
    public enum ColorRampEnum {
        GRAY_SCALE,
        RAINBOW
    }

    /// <summary>
    /// Represents a point (and the color of its area)
    /// </summary>
    public class VoronoiPoint
    {
        /// <summary>
        /// Color of the point
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Coordinate are relative to adapt to screen size
        /// </summary>
        public Vector2 RelativeCoordinate { get; set; }

        /// <summary>
        /// Get real coordinate of point
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public Point GetAbsolutePoint(int width, int height)
        {
            return new Point(
                (int)Math.Round(this.RelativeCoordinate.X * width),
                (int)Math.Round(this.RelativeCoordinate.Y * height));
        }
    }

    public class VoronoiEngine
    {
        private Dictionary<ColorRampEnum, Color[]> _colorRamp;

        private List<VoronoiPoint> _points;

        private Random _random;

        private VoronoiEngine()
        {
            this._colorRamp = new Dictionary<ColorRampEnum, Color[]>();
            this._colorRamp[ColorRampEnum.GRAY_SCALE] = this.GenerateColorRamp(ColorRampEnum.GRAY_SCALE);
            this._colorRamp[ColorRampEnum.RAINBOW] = this.GenerateColorRamp(ColorRampEnum.RAINBOW);
            this._points = new List<VoronoiPoint>();
            this._random = new Random();
        }

        /// <summary>
        /// Init a new Voronoi engine
        /// </summary>
        /// <param name="nbOfPoints"></param>
        public VoronoiEngine(int nbOfPoints) : this()
        {
            for (var i = 0; i < nbOfPoints; i++)
            {
                this._points.Add(new VoronoiPoint
                {
                    Color = new Color(
                        this._random.Next(0, 255),
                        this._random.Next(0, 255),
                        this._random.Next(0, 255)
                    ),
                    RelativeCoordinate = new Vector2
                    {
                        X = this._random.Next(0, 100) / 100.0f,
                        Y = this._random.Next(0, 100) / 100.0f
                    }
                });
            }
        }

        /// <summary>
        /// Init a new VoronoiEngine with a list of points
        /// </summary>
        /// <param name="points"></param>
        public VoronoiEngine(List<VoronoiPoint> points)
        {
            this._points = points;
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
        private double DistanceBetween(float x1,float y1, float x2, float y2)
        {
            return Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2);
        }

        /// <summary>
        /// Return the closest from a list of point at a given position
        /// </summary>
        /// <param name="fromPoints"></param>
        /// <param name="at"></param>
        /// <returns></returns>
        private VoronoiPoint GetClosestPoint(List<VoronoiPoint> fromPoints, Point at,int inWidth, int inHeight)
        {
            return fromPoints.OrderBy(vPoint => {
                return DistanceBetween(
                    vPoint.RelativeCoordinate.X * inWidth,
                    vPoint.RelativeCoordinate.Y * inHeight,
                    at.X,
                    at.Y);
            }).First();
        }

        /// <summary>
        /// Generate plasma effect on a device (as a Texture2D) with given width and height
        /// <returns></returns>
        public Texture2D UpdateVoronoi(GraphicsDevice device,int width,int height,int ratio = 50)
        {
            Texture2D rect = new Texture2D(device, width, height);

            Color[] data = new Color[width * height];
            var stepX = ratio;
            var stepY = ratio;
            for (int x = 0; x < width;x+=stepX)
            {
                var topA = Math.Min(x + stepX, width);
                for (int y = 0; y < height;y+=stepY)
                {
                    var closestPoint = this.GetClosestPoint(this._points, new Point(x, y),width,height);
                    var topB = Math.Min(y + stepY, height);
                    for (int a = x; a < topA; ++a)
                    {
                        for (int b = y; b < topB; ++b)
                        {
                            data[((b) * width) + (a)] = closestPoint.Color;
                        }
                    }
                }
            }

            rect.SetData(data);

            return rect;
        }

        /// <summary>
        /// Return points as readonly
        /// </summary>
        public IReadOnlyList<VoronoiPoint> Points
        {
            get
            {
                return new ReadOnlyCollection<VoronoiPoint>(this._points);
            }
        }
    }
}
