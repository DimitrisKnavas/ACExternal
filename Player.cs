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
        protected IntPtr Base =(IntPtr)Offsets.playerBase;
        protected int playerAddress;
        protected IntPtr HealthAddress;
        protected IntPtr ArmorAddress;
        protected IntPtr ARAmmoAddress;
        protected IntPtr headAddress;
        protected IntPtr feetAddress;
        protected Vector3 head;
        protected Vector3 feet;
        protected Vector3 eyePosition;
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

        public virtual void updatePlayerPos()
        {
            head = Memory.ReadVector(headAddress);
            feet = Memory.ReadVector(feetAddress);
            eyePosition = Vector3.addVectors(head, feet);
            Console.WriteLine($"My position: {eyePosition.ToString()}");
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
