using ACExternal.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ACExternal
{
    public class Entity:Player,IDisposable
    {
        private int entityBaseAddress;
        private int num;
        private const int hopOffset = 0x4;
        private int health;
        private string printHealth;

        public Entity()
        {
        }

        public override void updatePlayerPos()
        {
            entityBaseAddress = Memory.Read((IntPtr)0x50F4F8);
            num = Memory.Read(IntPtr.Add(Offsets.baseGame, Offsets.numOfPlayers));
            Console.WriteLine($"Number of players: {num}");

             for(int i=1;i<num;i++)
            {
                playerAddress = Memory.Read(IntPtr.Add((IntPtr)entityBaseAddress, (hopOffset * i)));
                headAddress = IntPtr.Add((IntPtr)(playerAddress), Offsets.vectorHead);
                feetAddress = IntPtr.Add((IntPtr)(playerAddress), Offsets.vectorFeet);
                head = Memory.ReadVector(headAddress);
                feet = Memory.ReadVector(feetAddress);
                eyePosition = Vector3.addVectors(head, feet);
                health = Memory.Read(IntPtr.Add((IntPtr)playerAddress, Offsets.Health));
                if (health > 0)
                    printHealth = $"HEALTH == {health.ToString()}";
                else
                    printHealth = "DEAD";
                Console.WriteLine($"INFO ==> PLAYER{i + 1} ---- {printHealth}");
                Console.WriteLine($"{eyePosition.ToString()}");
            }

        }

    }
}
