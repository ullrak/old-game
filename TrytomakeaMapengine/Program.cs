using System;

namespace TrytomakeaMapengine
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (GameTry2 game = new GameTry2())
            {
                game.Run();
            }
        }
    }
#endif
}

