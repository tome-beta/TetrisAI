﻿using System;
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
   

        public void MainLoop(int targetTimes)
        {
            this.labelFPS.Text = @"FPS: " + targetTimes.ToString();

            switch(Mode)
            {
                //次のブロックを決める
                case GANME_MODE.MODE_SET_BLOCK:
                    {

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


            //フォームの書き換え
            Invalidate();
        }

        private void DispTest()
        {
            //描画先とするImageオブジェクトを作成する
            Bitmap canvas = new Bitmap(this.pictureBoxField1P.Width, this.pictureBoxField1P.Height);
            //ImageオブジェクトのGraphicsオブジェクトを作成する
            Graphics g = Graphics.FromImage(canvas);


            //操作中のブロックをセット
            blockControle.SetCurrentBlock(BlockInfo.BlockType.MINO_I);
            blockControle.DrawCurrentBlock(g, blockControle.BlockSourceImage);

            //Imageオブジェクトのリソースを解放する
           //Graphicsオブジェクトのリソースを解放する
            g.Dispose();
            //PictureBox1に表示する
            this.pictureBoxField1P.Image = canvas;
        }

        BlockControle blockControle = new BlockControle();

        public int[,] BlockField { get; set;}

        GANME_MODE Mode;

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
