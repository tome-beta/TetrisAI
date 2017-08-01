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

        enum GANME_MODE
        {
            MODE_SET_BLOCK,     //次のブロックを決める
            MODE_MOVE_BLOCK,    //ブロックを設置させるまでの操作
            MODE_ERASE_CHECK,   //ブロックが消えるかチェック
            MODE_ERASE_BLOCK,   //ブロックを消す処理
        };


        public GameField()
        {
            fps = 0;
            oldtime = System.Environment.TickCount;

            InitializeComponent();

            Init();
        }

        //=============================================================
        //  private 


       private void Init()
       {
            //フィールド情報を初期化
            BlockFieldInit();

            //描画先とするImageオブジェクトを作成する
            this.canvas = new Bitmap(this.pictureBoxField1P.Width, this.pictureBoxField1P.Height);
            //ImageオブジェクトのGraphicsオブジェクトを作成する
            this.g = Graphics.FromImage(canvas);

            Mode = GANME_MODE.MODE_SET_BLOCK;
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
        //メインループ
        public void MainLoop()
        {
            fps++;
            if (System.Environment.TickCount >= oldtime + 1000)
            {
                oldtime = System.Environment.TickCount;
                this.labelFPS.Text = fps.ToString();
                fps = 0;

                //演算処理
                Exec();
            }

            //描画処理
            DrawUpdate();
        }

        public void Exec()
        {
            Console.WriteLine(@"EXEC");

            switch (Mode)
            {
                //次のブロックを決める
                case GANME_MODE.MODE_SET_BLOCK:
                    {
                        blockControle.SetCurrentBlock(BlockInfo.BlockType.MINO_I);
                        Mode = GANME_MODE.MODE_MOVE_BLOCK;
                    }
                    break;

                //ブロックを設置させるまでの操作
                case GANME_MODE.MODE_MOVE_BLOCK:
                    {
                    }
                    break;

                //ブロックが消えるかチェック
                case GANME_MODE.MODE_ERASE_CHECK:
                    {

                    }
                    break;

                //ブロックを消す処理
                case GANME_MODE.MODE_ERASE_BLOCK:
                    {

                    }
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// 描画更新
        /// </summary>
        public void DrawUpdate()
        {
            Console.WriteLine(@"Disp");

            //フィールドのクリア
            g.Clear(Color.White);

            //デバッグ用にフィールドに線を引いておく
            using (Pen pen = new Pen(Color.Gray))
            {
                for (int x = 1; x < 11; x++)
                {
                    g.DrawLine(pen, new Point(x * BlockInfo.BLOCK_WIDTH, 0 * BlockInfo.BLOCK_HEIGHT),
                        new Point(x * BlockInfo.BLOCK_WIDTH, 20 * BlockInfo.BLOCK_HEIGHT));
                }
                for (int y = 1; y < 21; y++)
                {
                    g.DrawLine(pen, new Point(0 * BlockInfo.BLOCK_WIDTH, y * BlockInfo.BLOCK_HEIGHT),
                        new Point(20 * BlockInfo.BLOCK_WIDTH, y * BlockInfo.BLOCK_HEIGHT));
                }
            }

            //TODO 設置したブロックを描画

            //操作中のブロックを描画
            blockControle.DrawCurrentBlock(g, blockControle.BlockSourceImage);

            //PictureBox1に表示する
            this.pictureBoxField1P.Image = canvas;

            //デバッグ用、操作ブロックの位置を表示
            int pos_x = blockControle.CurrentPos.X;
            int pos_y = blockControle.CurrentPos.Y;
            this.labelCurrentPos.Text = @"CurrectPos :("+ pos_x.ToString()+ "," + pos_y.ToString() + @")";

            //フォームの書き換え
            Invalidate();
        }

        
        //キー入力
        private void GameField_KeyDown(object sender, KeyEventArgs e)
        {
            //TODO ハードドロップ
            if (e.KeyData == Keys.Up)
            {
                Console.WriteLine(@"UP");
                this.blockControle.CurrentPos.Y -= 1;
            }
            if (e.KeyData == Keys.Down)
            {
                Console.WriteLine(@"DOWN");
                this.blockControle.CurrentPos.Y += 1;
            }
            if (e.KeyData == Keys.Right)
            {
                Console.WriteLine(@"RIGHT");
                this.blockControle.CurrentPos.X += 1;
            }
            if (e.KeyData == Keys.Left)
            {
                Console.WriteLine(@"LEFT");
                this.blockControle.CurrentPos.X -= 1;
            }

            //回転
            if (e.KeyData == Keys.X)
            {
                Console.WriteLine(@"ROTATE_R");
                this.blockControle.RotateCurrentBlock(true);
            }
            else if (e.KeyData == Keys.Z)
            {
                Console.WriteLine(@"ROTATE_L");
                this.blockControle.RotateCurrentBlock(false);
            }
        }


        BlockControle blockControle = new BlockControle();
        public int[,] BlockField { get; set;}

        private GANME_MODE Mode;
        private int fps;
        private int oldtime;

        //フィールドの描画用
        Bitmap canvas;
        Graphics g;
    }
}
