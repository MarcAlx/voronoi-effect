using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace plasmaeffect.Engine
{
    /// <summary>
    /// Color ramp to use
    /// </summary>
    public enum ColorRampEnum {
        /// <summary>
        /// Equally distributed gray scale
        /// </summary>
        GRAY_SCALE,

        /// <summary>
        /// Equally distributed along rainbow
        /// </summary>
        RAINBOW,

        /// <summary>
        /// Equally distributed blue scale
        /// </summary>
        OCEAN,

        /// <summary>
        /// Equally distributed red scale
        /// </summary>
        LAVA,

        /// <summary>
        /// Equally distributed dark green scale
        /// </summary>
        DARK_LEAF,

        /// <summary>
        /// Random
        /// </summary>
        RANDOM
    }

    /// <summary>
    /// Direction in which the color ramp is applied
    /// </summary>
    public enum ApplyColorDirectionEnum
    {
        /// <summary>
        /// Along X axis of point
        /// </summary>
        HORIZONTAL,

        /// <summary>
        /// Along Y axis of poitn
        /// </summary>
        VERTICAL,

        /// <summary>
        /// No direction is applied
        /// </summary>
        NONE
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

        public Vector2 _InitialCoordinates { get; set; }
        /// <summary>
        /// Coordinate are relative to adapt to screen size
        /// </summary>
        public Vector2 InitialCoordinates {
            get
            {
                return this._InitialCoordinates;
            }
            set
            {
                this._InitialCoordinates = value;
                this.RelativeCoordinates = value;
            }
        }

        /// <summary>
        /// Coordinate are relative to adapt to screen size
        /// </summary>
        public Vector2 RelativeCoordinates { get; set; }

        /// <summary>
        /// Get real coordinate of point
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public Point GetAbsolutePoint(int width, int height)
        {
            return new Point(
                (int)Math.Round(this.RelativeCoordinates.X * width),
                (int)Math.Round(this.RelativeCoordinates.Y * height));
        }

        private RandomMovingPoint _rp;

        public VoronoiPoint()
        {
            this._rp = new RandomMovingPoint();
        }

        /// <summary>
        /// Update position
        /// </summary>
        public void UpdatePosition(double speed, double amplitude)
        {
            this._rp.UpdatePosition(
                speed,
                this.InitialCoordinates.X - amplitude,
                this.InitialCoordinates.Y - amplitude,
                this.InitialCoordinates.X + amplitude,
                this.InitialCoordinates.Y + amplitude
            );
            this.RelativeCoordinates = this._rp.Position;
        }
    }

    public class VoronoiEngine
    {
        private List<VoronoiPoint> _points;

        private Random _random;

        private VoronoiEngine()
        {
            this._points = new List<VoronoiPoint>();
            this._random = new Random();
        }

        /// <summary>
        /// Init a new Voronoi engine
        /// </summary>
        /// <param name="nbOfPoints"></param>
        public VoronoiEngine(int nbOfPoints, ColorRampEnum colorRamp = ColorRampEnum.RAINBOW, ApplyColorDirectionEnum colorDirection = ApplyColorDirectionEnum.NONE) : this()
        {
            for (var i = 0; i < nbOfPoints; i++)
            {
                this._points.Add(new VoronoiPoint
                {
                    Color = Color.Black,
                    InitialCoordinates = new Vector2
                    {
                        X = this._random.Next(0, 100) / 100.0f,
                        Y = this._random.Next(0, 100) / 100.0f
                    }
                });
            }
            this.ApplyColorRamp(colorRamp, colorDirection);
        }

        /// <summary>
        /// Init a new VoronoiEngine with a list of points
        /// </summary>
        /// <param name="points"></param>
        public VoronoiEngine(List<VoronoiPoint> points, ColorRampEnum colorRamp = ColorRampEnum.RAINBOW, ApplyColorDirectionEnum colorDirection = ApplyColorDirectionEnum.NONE)
        {
            this._points = points;
            this.ApplyColorRamp(colorRamp, colorDirection);
        }

        /// <summary>
        /// Apply a color ramp according to provided enum
        /// </summary>
        /// <param name="ramp"></param>
        /// <returns></returns>
        private void ApplyColorRamp(ColorRampEnum ramp, ApplyColorDirectionEnum colorDirection = ApplyColorDirectionEnum.NONE)
        {
            var pointCount = this._points.Count;
            if(colorDirection == ApplyColorDirectionEnum.HORIZONTAL)
            {
                this._points.Sort((a, b) => a.RelativeCoordinates.X.CompareTo(b.RelativeCoordinates.X));
            }
            else if (colorDirection == ApplyColorDirectionEnum.VERTICAL)
            {
                this._points.Sort((a, b) => a.RelativeCoordinates.Y.CompareTo(b.RelativeCoordinates.Y));
            }

            if (ramp == ColorRampEnum.GRAY_SCALE)
            {
                for(int i = 0; i< pointCount; i++)
                {
                    var comp = (int)(i * 256.0 / pointCount);
                    this._points[i].Color = new Color(comp, comp, comp);
                }
            }
            else if (ramp == ColorRampEnum.RAINBOW)
            {
                for (int i = 0; i < pointCount; i++)
                {
                    this._points[i].Color = Toolkit.FromHsl(i / 20.0f, 1f, 0.5f);
                }   
            }
            else if (ramp == ColorRampEnum.OCEAN)
            {
                var colors = Toolkit.GenerateRampBetween(Color.Blue, Color.White, this._points.Count);
                for (int i = 0; i < this._points.Count; i++)
                {
                    this._points[i].Color = colors[i];
                }
            }
            else if (ramp == ColorRampEnum.DARK_LEAF)
            {
                var colors = Toolkit.GenerateRampBetween(Color.Green, Color.Black, this._points.Count);
                for (int i = 0; i < this._points.Count; i++)
                {
                    this._points[i].Color = colors[i];
                }
            }
            else if (ramp == ColorRampEnum.LAVA)
            {
                var colors = Toolkit.GenerateRampBetween(Color.Yellow, Color.Red, this._points.Count);
                for (int i = 0; i < this._points.Count; i++)
                {
                    this._points[i].Color = colors[i];
                }
            }
            else if (ramp == ColorRampEnum.RANDOM)
            {
                for (int i = 0; i < pointCount; i++)
                {
                    this._points[i].Color = new Color(
                        this._random.Next(0, 255),
                        this._random.Next(0, 255),
                        this._random.Next(0, 255)
                    );
                }
            }
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
                    vPoint.RelativeCoordinates.X * inWidth,
                    vPoint.RelativeCoordinates.Y * inHeight,
                    at.X,
                    at.Y);
            }).First();
        }

        /// <summary>
        /// Generate voronoie effect on a device (as a Texture2D)
        /// <returns></returns>
        public Texture2D UpdateVoronoi(GraphicsDevice device,int width,int height,int ratio = 50, bool updatePoint = false, double pointRelativeSpeed = 0.1, double pointRelativeAmplitude = 0.1)
        {
            Texture2D rect = new Texture2D(device, width, height);

            if (updatePoint)
            {
                foreach (var vp in this._points)
                {
                    var p = vp.GetAbsolutePoint(width, height);
                    vp.UpdatePosition(pointRelativeSpeed, pointRelativeAmplitude);
                }
            }

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
