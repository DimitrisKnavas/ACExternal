using ACExternal.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ACExternal
{
    public class Player:IDisposable
    {
        private IntPtr Base =(IntPtr)Offsets.playerBase;
        private int playerAddress { get; set; }
        private IntPtr HealthAddress { get; set; }
        private IntPtr ArmorAddress { get; set; }
        private IntPtr ARAmmoAddress { get; set; }
        private IntPtr headAddress { get; set; }
        private IntPtr feetAddress { get; set; }
        private Vector3 head { get; set; }
        private Vector3 feet { get; set; }
        private Vector3 eyePosition { get; set; }
        public Thread godmodeThread { get; set; }

        public Player()
        {
            godmodeThread = new Thread(new ThreadStart(this.GodMode));
            playerAddress = Memory.Read(Base);
            HealthAddress = IntPtr.Add((IntPtr)playerAddress, Offsets.Health);
            ArmorAddress = IntPtr.Add((IntPtr)playerAddress, Offsets.Armor);
            ARAmmoAddress = IntPtr.Add((IntPtr)playerAddress, Offsets.ARAmmo);
            headAddress = IntPtr.Add((IntPtr)playerAddress, Offsets.vectorHead);
            feetAddress = IntPtr.Add((IntPtr)playerAddress, Offsets.vectorFeet);

        }

        public void updatePlayerPos()
        {
            head = Memory.ReadVector(headAddress);
            feet = Memory.ReadVector(feetAddress);
            eyePosition = Vector3.addVectors(head, feet);
            Console.WriteLine($"{eyePosition.ToString()}");
        }

        public void GodMode()
        {
            while (true)
            {
                Memory.Write(HealthAddress, 999);
                Memory.Write(ARAmmoAddress, 999);
                Memory.Write(ArmorAddress, 999);
                Thread.Sleep(500);
            }
        }

        public void Dispose()
        {
            godmodeThread.Abort();
        }
    }
}
