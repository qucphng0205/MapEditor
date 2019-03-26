using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MapEditor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ImageTile x = new ImageTile(@"C:\Users\ThinkKING\Downloads\P04-1\lv1.png", 48, 12);
            x.GenerateTiles(@"C:\Users\ThinkKING\Downloads\P04-1");
            x.ConvertTilesetToID(x.GetImage(), @"C:\Users\ThinkKING\Downloads\P04-1");
        }
    }
}
