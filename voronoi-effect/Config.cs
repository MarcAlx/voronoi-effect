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
    }
}
