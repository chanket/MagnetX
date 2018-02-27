using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MagnetX.List
{
    class ListView : System.Windows.Forms.ListView
    {
        public ListView()
        {
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();
        }

        public void UniqueItemClear()
        {
            Items.Clear();
            Hash.Clear();
        }

        public bool UniqueItemAdd(ListViewItem item, string magnet)
        {
            if (Hash.Add(new Magnet(magnet)))
            {
                Items.Add(item);
                return true;
            }
            else
            {
                return false;
            }
        }

        protected struct Magnet
        {
            byte b0; byte b1;  byte b2;  byte b3;  byte b4;  byte b5;  byte b6;  byte b7;  byte b8;  byte b9;  byte b10;  byte b11;  byte b12;  byte b13;  byte b14;  byte b15;  byte b16;  byte b17;  byte b18;  byte b19;  byte b20;  byte b21;  byte b22;  byte b23;  byte b24;  byte b25;  byte b26;  byte b27;  byte b28;  byte b29;  byte b30;  byte b31;  byte b32;  byte b33;  byte b34;  byte b35;  byte b36;  byte b37;  byte b38;  byte b39;  byte b40;  byte b41;  byte b42;  byte b43;  byte b44;  byte b45;  byte b46;  byte b47;  byte b48;  byte b49;  byte b50;  byte b51;  byte b52;  byte b53;  byte b54;  byte b55;  byte b56;  byte b57;  byte b58;  byte b59;
            public Magnet(string magnet)
            {
                if (magnet.Length > 0) b0 = (byte)magnet[0]; else b0 = 0;
                if (magnet.Length > 1) b1 = (byte)magnet[1]; else b1 = 0;
                if (magnet.Length > 2) b2 = (byte)magnet[2]; else b2 = 0;
                if (magnet.Length > 3) b3 = (byte)magnet[3]; else b3 = 0;
                if (magnet.Length > 4) b4 = (byte)magnet[4]; else b4 = 0;
                if (magnet.Length > 5) b5 = (byte)magnet[5]; else b5 = 0;
                if (magnet.Length > 6) b6 = (byte)magnet[6]; else b6 = 0;
                if (magnet.Length > 7) b7 = (byte)magnet[7]; else b7 = 0;
                if (magnet.Length > 8) b8 = (byte)magnet[8]; else b8 = 0;
                if (magnet.Length > 9) b9 = (byte)magnet[9]; else b9 = 0;
                if (magnet.Length > 10) b10 = (byte)magnet[10]; else b10 = 0;
                if (magnet.Length > 11) b11 = (byte)magnet[11]; else b11 = 0;
                if (magnet.Length > 12) b12 = (byte)magnet[12]; else b12 = 0;
                if (magnet.Length > 13) b13 = (byte)magnet[13]; else b13 = 0;
                if (magnet.Length > 14) b14 = (byte)magnet[14]; else b14 = 0;
                if (magnet.Length > 15) b15 = (byte)magnet[15]; else b15 = 0;
                if (magnet.Length > 16) b16 = (byte)magnet[16]; else b16 = 0;
                if (magnet.Length > 17) b17 = (byte)magnet[17]; else b17 = 0;
                if (magnet.Length > 18) b18 = (byte)magnet[18]; else b18 = 0;
                if (magnet.Length > 19) b19 = (byte)magnet[19]; else b19 = 0;
                if (magnet.Length > 20) b20 = (byte)magnet[20]; else b20 = 0;
                if (magnet.Length > 21) b21 = (byte)magnet[21]; else b21 = 0;
                if (magnet.Length > 22) b22 = (byte)magnet[22]; else b22 = 0;
                if (magnet.Length > 23) b23 = (byte)magnet[23]; else b23 = 0;
                if (magnet.Length > 24) b24 = (byte)magnet[24]; else b24 = 0;
                if (magnet.Length > 25) b25 = (byte)magnet[25]; else b25 = 0;
                if (magnet.Length > 26) b26 = (byte)magnet[26]; else b26 = 0;
                if (magnet.Length > 27) b27 = (byte)magnet[27]; else b27 = 0;
                if (magnet.Length > 28) b28 = (byte)magnet[28]; else b28 = 0;
                if (magnet.Length > 29) b29 = (byte)magnet[29]; else b29 = 0;
                if (magnet.Length > 30) b30 = (byte)magnet[30]; else b30 = 0;
                if (magnet.Length > 31) b31 = (byte)magnet[31]; else b31 = 0;
                if (magnet.Length > 32) b32 = (byte)magnet[32]; else b32 = 0;
                if (magnet.Length > 33) b33 = (byte)magnet[33]; else b33 = 0;
                if (magnet.Length > 34) b34 = (byte)magnet[34]; else b34 = 0;
                if (magnet.Length > 35) b35 = (byte)magnet[35]; else b35 = 0;
                if (magnet.Length > 36) b36 = (byte)magnet[36]; else b36 = 0;
                if (magnet.Length > 37) b37 = (byte)magnet[37]; else b37 = 0;
                if (magnet.Length > 38) b38 = (byte)magnet[38]; else b38 = 0;
                if (magnet.Length > 39) b39 = (byte)magnet[39]; else b39 = 0;
                if (magnet.Length > 40) b40 = (byte)magnet[40]; else b40 = 0;
                if (magnet.Length > 41) b41 = (byte)magnet[41]; else b41 = 0;
                if (magnet.Length > 42) b42 = (byte)magnet[42]; else b42 = 0;
                if (magnet.Length > 43) b43 = (byte)magnet[43]; else b43 = 0;
                if (magnet.Length > 44) b44 = (byte)magnet[44]; else b44 = 0;
                if (magnet.Length > 45) b45 = (byte)magnet[45]; else b45 = 0;
                if (magnet.Length > 46) b46 = (byte)magnet[46]; else b46 = 0;
                if (magnet.Length > 47) b47 = (byte)magnet[47]; else b47 = 0;
                if (magnet.Length > 48) b48 = (byte)magnet[48]; else b48 = 0;
                if (magnet.Length > 49) b49 = (byte)magnet[49]; else b49 = 0;
                if (magnet.Length > 50) b50 = (byte)magnet[50]; else b50 = 0;
                if (magnet.Length > 51) b51 = (byte)magnet[51]; else b51 = 0;
                if (magnet.Length > 52) b52 = (byte)magnet[52]; else b52 = 0;
                if (magnet.Length > 53) b53 = (byte)magnet[53]; else b53 = 0;
                if (magnet.Length > 54) b54 = (byte)magnet[54]; else b54 = 0;
                if (magnet.Length > 55) b55 = (byte)magnet[55]; else b55 = 0;
                if (magnet.Length > 56) b56 = (byte)magnet[56]; else b56 = 0;
                if (magnet.Length > 57) b57 = (byte)magnet[57]; else b57 = 0;
                if (magnet.Length > 58) b58 = (byte)magnet[58]; else b58 = 0;
                if (magnet.Length > 59) b59 = (byte)magnet[59]; else b59 = 0;

            }
        }

        protected HashSet<Magnet> Hash { get; set; } = new HashSet<Magnet>();
    }
}
