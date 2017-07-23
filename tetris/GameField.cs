using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tetris
{
    public partial class GameField : Form
    {
        public GameField()
        {
            InitializeComponent();

            Init();
            DispTest();
        }

        private void Init()
        {
        }
        private void DispTest()
        {
            //描画先とするImageオブジェクトを作成する
            Bitmap canvas = new Bitmap(this.pictureBoxField1P.Width, this.pictureBoxField1P.Height);
            //ImageオブジェクトのGraphicsオブジェクトを作成する
            Graphics g = Graphics.FromImage(canvas);


            BlockInfo I_mino = blockControle.GetBlock(BlockInfo.BlockType.MINO_Z);
            I_mino.Draw(g, blockControle.BlockSourceImage);

            //Imageオブジェクトのリソースを解放する
           //Graphicsオブジェクトのリソースを解放する
            g.Dispose();
            //PictureBox1に表示する
            this.pictureBoxField1P.Image = canvas;
        }

        BlockControle blockControle = new BlockControle();
    }
}
