using System;
namespace plasmaeffect
{
    public static class Config
    {
        /// <summary>
        /// Window title
        /// </summary>
        public static readonly string WINDOW_TITLE = "Voronoi effect";

        /// <summary>
        /// Tells if FPS should be displayed on screen
        /// </summary>
        public static readonly bool DISPLAY_FPS = false;

        /// <summary>
        /// Tells if points should be drawn
        /// </summary>
        public static readonly bool DRAW_POINTS = false;

        /// <summary>
        /// FPS cap
        /// </summary>
        public static readonly int FPS_CAP = 60;

        /// <summary>
        /// Pixel ratio
        /// </summary>
        public static readonly int PIXEL_RATIO = 15;

        /// <summary>
        /// Points move every ms represented by this value
        /// </summary>
        public static readonly int MOVE_FREQUENCY = 0_100;

        /// <summary>
        /// Points move at this percentage of screen every MOVE_FREQUENCY
        /// </summary>
        public static readonly double POINT_RELATIVE_SPEED = 0.05;

        /// <summary>
        /// Points move inside this relative amplitude (around their initial position)
        /// </summary>
        public static readonly double POINT_MOVE_RELATIVE_AMPLITUDE = 0.1;
    }
}
