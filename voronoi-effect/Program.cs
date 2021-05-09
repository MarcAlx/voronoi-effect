using System;

namespace plasma_effect
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new VoronoiEffect())
                game.Run();
        }
    }
}
