using System;

namespace GameJam1
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            Game1.Instance.Run();
        }
    }
#endif
}

