using System;

namespace TestPlatformer
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (TestPlatformer game = new TestPlatformer())
            {
                game.Run();
            }
        }
    }
#endif
}

