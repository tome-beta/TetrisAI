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
            DispTest();
        }


        private void DispTest()
        {
            //描画先とするImageオブジェクトを作成する
            Bitmap canvas = new Bitmap(this.pictureBoxField1P.Width, this.pictureBoxField1P.Height);
            //ImageオブジェクトのGraphicsオブジェクトを作成する
            Graphics g = Graphics.FromImage(canvas);

            //画像ファイルを読み込んで、Imageオブジェクトとして取得する
            Image img = Image.FromFile(@"D:\myprog\tetris_work\issue1\TetrisAI\tetris\resouce\mino.bmp");
            //画像をcanvasの座標(20, 10)の位置に描画する
            g.DrawImage(img, 20, 10, img.Width, img.Height);
            //Imageオブジェクトのリソースを解放する
            img.Dispose();

            //Graphicsオブジェクトのリソースを解放する
            g.Dispose();
            //PictureBox1に表示する
            this.pictureBoxField1P.Image = canvas;
        }

        BlockInfo tmp_block = new BlockInfo();

    }
}
