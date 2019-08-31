using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACExternal
{
    public static class Offsets
    {
        //https://www.unknowncheats.me/forum/other-games/331273-assaultcube-addresses.html
        public static IntPtr baseGame = (IntPtr)0x0050F4E8;
        public static int playerBase = 0x00509B74;
        public static int entityBase = 0x50F4F4;
        public static int numOfPlayers = 0x18;
        public static int Health = 0xF8;
        public static int ARAmmo = 0x150;
        public static int Armor = 0xFC;
        public static int vectorHead = 0x4;
        public static int vectorFeet = 0x34;
    }
}
