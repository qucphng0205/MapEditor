using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MapEditor
{
    class ImageTile
    {
        private Image image;
        private Size size;
        HashSet<Bitmap> tiles;
        int tileWidth;
        int tileHeight;
        public Image GetImage() { return image; }
        public ImageTile(string inputFile, int rows, int columns)
        {
            if (!File.Exists(inputFile))
            {
                MessageBox.Show("Image not found");
                return;
            }
            image = Image.FromFile(inputFile);
            size = new Size(rows, columns);
            tiles = new HashSet<Bitmap>();
        }
        public void GenerateTiles(string outputPath)
        {
            int xMax = image.Width;
            int yMax = image.Height;
            tileWidth = xMax / size.Width;
            tileHeight = yMax / size.Height;
            if (!Directory.Exists(outputPath))
                Directory.CreateDirectory(outputPath);
            //lay ra tung cuc 
            for (int y = 0; y < size.Height; y++)
            {
                for (int x = 0; x < size.Width; x++)
                {
                    string outputFileName = Path.Combine(outputPath, string.Format("{0}_{1}.png", x, y));

                    Rectangle tileBounds = new Rectangle(x * tileWidth, y * tileHeight, tileWidth, tileHeight);
                    Bitmap target = new Bitmap(tileWidth, tileHeight);
                    //ve len target
                    using (Graphics graphics = Graphics.FromImage(target))
                    {
                        graphics.DrawImage(
                            image,
                            new Rectangle(0, 0, tileWidth, tileHeight),
                            tileBounds,
                            GraphicsUnit.Pixel);
                    }
                    //so sanh voi cac cuc co r
                    bool flag = true;
                    for (int id = 0; id < tiles.Count; id++)
                        if (IsBitMapsSimilar(tiles.ElementAt(id), target))
                        {
                            flag = false;
                            break;
                        }
                    if (!flag)
                        continue;
                    //ve len image to
                    //target.Save(outputFileName, ImageFormat.Png);
                    tiles.Add(target);
                }
            }
            DrawToAnotherImage(80, 1, 32, 32, outputPath);
        }
        public void ConvertTilesetToID(Image image, string outputPath)
        {
            string fileName = "Output.txt";
            string outputFileName = Path.Combine(outputPath, fileName);
            StreamWriter writer = new StreamWriter(outputFileName, false);

            writer.WriteLine(tiles.Count + " " + this.image.Width / tileWidth + " " + this.image.Height / tileHeight);

            int rows = image.Size.Height / tileHeight;
            int columns = image.Size.Width / tileWidth;
            //lay ra tung cuc
            for (int j = 0; j < rows; j++)
            {
                bool firstLine = true;
                for (int i = 0; i < columns; i++)
                {
                    Rectangle tileBounds = new Rectangle(i * tileWidth, j * tileHeight, tileWidth, tileHeight);
                    Bitmap ori = new Bitmap(tileWidth, tileHeight);
                    using (Graphics graphics = Graphics.FromImage(ori))
                    {
                        graphics.DrawImage(
                            image,
                            new Rectangle(0, 0, tileWidth, tileHeight),
                            tileBounds,
                            GraphicsUnit.Pixel);
                    }
                    //so sanh voi cuc co r de lay id
                    for (int id = 0; id < tiles.Count; id++)
                        if (IsBitMapsSimilar(tiles.ElementAt(id), ori))
                        {
                            if (firstLine)
                                writer.Write(id.ToString());
                            else
                                writer.Write(" " + id.ToString());
                            firstLine = false;
                        }
                }
                writer.WriteLine();
            }
            writer.Close();
        }
        private bool IsBitMapsSimilar(Bitmap a, Bitmap b)
        {
            for (int i = 0; i < a.Width; i++)
                for (int j = 0; j < a.Height; j++)
                    if (a.GetPixel(i, j) != b.GetPixel(i, j))
                        return false;
            return true;
        }
        private void DrawToAnotherImage(int width, int height, int tileWidth, int tileHeight, string outputPath)
        {
            //ve voi width (column) height (row)
            //ez game
            string outputFileName = Path.Combine(outputPath, string.Format("bigImage.png"));
            Bitmap bigImage = new Bitmap(width * tileWidth, height * tileHeight);
            Graphics g = Graphics.FromImage(bigImage);
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                {
                    g.DrawImage(
                        tiles.ElementAt(i * height + j),
                        i * tileWidth,
                        j * tileHeight);
                }
            bigImage.Save(outputFileName, ImageFormat.Png);
        }
    }
}
