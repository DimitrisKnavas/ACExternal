using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ACExternal.GFX;

namespace ACExternal
{
    class Program
    {
        static void Main(string[] args)
        {
            WaitProcess:
            int procID = Memory.IsProcessRunning();
            
            while (procID == 0)
            {
                //Wait for the game to open
                Console.WriteLine("The game is not running...");
                procID = Memory.IsProcessRunning();
                Thread.Sleep(1000);
            }

            Player self = new Player();
            self.godmodeThread.Start();

            Entity entities = new Entity();

            Overlay ov = new Overlay();
            ov.Window.Visible = true;

            while (true)
            {
                if (Memory.IsProcessRunning() == 0)
                {
                    Console.WriteLine("The game closed...");
                    self.Dispose();
                    ov.Window.Close();
                    //Environment.Exit(0);
                    goto WaitProcess;
                }
                self.updatePlayerPos();
                entities.updatePlayerPos();
                Window.printRect();
                ov.drawRect(Window.rect);
                Thread.Sleep(1500);
            }
        }

    }
}
