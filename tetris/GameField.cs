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
        const int FIELD_HEIGHT = 20;
        const int FIELD_WIDTH = 10;


        public GameField()
        {
            InitializeComponent();

            Init();
            DispTest();
        }

        //=============================================================
        //  private 


       private void Init()
       {
            //フィールド情報を初期化
            BlockFieldInit();
       }

        private void BlockFieldInit()
        {
            //フィールドを作る
            //フィールドは１０＊２０の両サイドに壁を表す９９を入れる。
            //ブロックのスタート位置のために上に３行加える。
            //床にも１行追加
            //全体としては１２＊２4
            this.BlockField = new int[GameField.FIELD_WIDTH + 2, GameField.FIELD_HEIGHT + 4];
            //壁と床を設置
            for (int w = 0; w < GameField.FIELD_WIDTH + 2; w++)
            {
                for (int h = 0; h < GameField.FIELD_HEIGHT + 4; h++)
                {
                    if (w == 0 || w == GameField.FIELD_WIDTH + 1 ||
                        h == GameField.FIELD_HEIGHT + 3)
                    {
                        this.BlockField[w, h] = 99;
                    }
                    else
                    {
                        this.BlockField[w, h] = 0;
                    }
                }
            }
        }
   

        private void DispTest()
        {
            //描画先とするImageオブジェクトを作成する
            Bitmap canvas = new Bitmap(this.pictureBoxField1P.Width, this.pictureBoxField1P.Height);
            //ImageオブジェクトのGraphicsオブジェクトを作成する
            Graphics g = Graphics.FromImage(canvas);


            BlockInfo I_mino = blockControle.GetBlock(BlockInfo.BlockType.MINO_I);
            I_mino.Draw(g, blockControle.BlockSourceImage);

            //Imageオブジェクトのリソースを解放する
           //Graphicsオブジェクトのリソースを解放する
            g.Dispose();
            //PictureBox1に表示する
            this.pictureBoxField1P.Image = canvas;
        }

        BlockControle blockControle = new BlockControle();

        public int[,] BlockField { get; set;}

        //キー入力
        private void GameField_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyData == Keys.Up)
            {
                Console.WriteLine(@"UP");
            }
            if (e.KeyData == Keys.Down)
            {
                Console.WriteLine(@"DOWN");
            }
            if (e.KeyData == Keys.Right)
            {
                Console.WriteLine(@"RIGHT");
            }
            if (e.KeyData == Keys.Left)
            {
                Console.WriteLine(@"LEFT");
            }
        }

        private void GameField_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.X)
            {
                Console.WriteLine(@"ROTATE_R");
            }
            if (e.KeyData == Keys.Z)
            {
                Console.WriteLine(@"ROTATE_L");
            }
        }
    }
}
