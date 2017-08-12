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
        public const int FIELD_HEIGHT = 20 + 1;// + 3; //ミノ領域＋床＋出現位置
        public const int FIELD_WIDTH = 10 + 2;     //ミノ領域 + 壁＊２

        public const int NEXT_BLOCK_MAX = 14;   //７種類＊２で表示は５個。
        public const int BLOCK_TYPE_NUM = 7;   //ミノは７種類

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

            fps = 0;
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
            this.BlockField = new int[GameField.FIELD_HEIGHT, GameField.FIELD_WIDTH];
            //壁と床を設置
            for (int w = 0; w < GameField.FIELD_WIDTH; w++)
            {
                for (int h = 0; h < GameField.FIELD_HEIGHT; h++)
                {
                    if (w == 0 || w == GameField.FIELD_WIDTH - 1 ||
                        h == GameField.FIELD_HEIGHT - 1)
                    {
                        this.BlockField[h, w] = (int)BlockInfo.BlockType.MINO_FENCE + (int)BlockInfo.BlockType.MINO_IN_FIELD;
                    }
                    else
                    {
                        this.BlockField[h, w] = 0;
                    }
                }
            }

            //NEXTブロックを収める配列
            UpdateNextBlock();
        }

        public void Exec()
        {
            Console.WriteLine(@"EXEC");

            switch (Mode)
            {
                //次のブロックを決める
                case GANME_MODE.MODE_SET_BLOCK:
                    {
                        //次のブロックを取り出す
                        UpdateNextBlock();
                        int type = GetNextBlock();

                        blockControle.SetCurrentBlock((BlockInfo.BlockType)type);
                        Mode = GANME_MODE.MODE_MOVE_BLOCK;
                    }
                    break;

                //ブロックを設置させるまでの操作
                case GANME_MODE.MODE_MOVE_BLOCK:
                    {
                        if (this.HardDrop)
                        {
                            //ハードドロップ
                            this.blockControle.HardDropCurrentBlock(BlockField);
                            this.HardDrop = false;

                            this.Mode = GANME_MODE.MODE_ERASE_CHECK;
                            this.blockControle.SetBlockInField(this.BlockField);
                        }
                    }
                    break;

                //ブロックが消えるかチェック
                case GANME_MODE.MODE_ERASE_CHECK:
                    {
                        int line_num = 0;
                        //床から見ていく
                        for (int h = GameField.FIELD_HEIGHT-2; h >=0; h--)
                        {
                            bool erase_line = true;
                            //壁の所は見ない
                            for (int w =1; w < GameField.FIELD_WIDTH-1; w++)
                            {
                                //設置されていないか
                                if( this.BlockField[h, w] < (int)BlockInfo.BlockType.MINO_IN_FIELD)
                                {
                                    //空きがあれば飛ばす
                                    erase_line = false;
                                    break;
                                }
                            }

                            //消すラインを予約する
                            if (erase_line)
                            {
                                //消す
                                line_num++;
                                this.EraseLine.Add(h);
                            }
                        }

                        //TODO 
                        //ここでTスピン　BtoB　RENのチェックする

                         this.Mode = GANME_MODE.MODE_ERASE_BLOCK;

                    }
                    break;

                //ブロックを消す処理
                case GANME_MODE.MODE_ERASE_BLOCK:
                    {
                        //ブロックを実際に消す処理
                        //アニメーションをそのうちつける
                        foreach (int line in this.EraseLine)
                        {
                            for (int w = 1; w < GameField.FIELD_WIDTH - 1; w++)
                            {
                                this.BlockField[line, w] = 0;
                            }

                            //上のラインをずらす
                            for (int h = line; h > 0; h--)
                            {
                                for (int w = 1; w < GameField.FIELD_WIDTH - 1; w++)
                                {
                                    this.BlockField[h, w] = this.BlockField[h - 1, w];
                                }
                            }
                        }

                        //一番上のラインを埋める
                        for (int w = 1; w < GameField.FIELD_WIDTH - 1; w++)
                        {
                            this.BlockField[0, w] = 0;
                        }

                        this.EraseLine.Clear();
                        this.Mode = GANME_MODE.MODE_SET_BLOCK;
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

            this.labelFPS.Text = @"FPS: " + this.fps.ToString();

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
            DrawGameField();

            //操作中のブロックを描画 TODO これもfieldクラスの中でやるべき
            blockControle.DrawCurrentBlock(g, blockControle.BlockSourceImage);

            //PictureBox1に表示する
            this.pictureBoxField1P.Image = canvas;

            //デバッグ用、操作ブロックの位置を表示
            int pos_x = blockControle.CurrentPos.X;
            int pos_y = blockControle.CurrentPos.Y;
            this.labelCurrentPos.Text = @"CurrectPos :(" + pos_x.ToString() + "," + pos_y.ToString() + @")";

            //フォームの書き換え
            Invalidate();
        }

        //===========================================
        //  private
        //===========================================

        //NEXTブロックを取得
        private int GetNextBlock()
        {
            //一つ取り出す
            int type = this.NextBlock[0];
            this.NextBlock.RemoveAt(0);
            return type;
        }

        //ネクストブロックを更新
        private void UpdateNextBlock()
        {
            //NEXTブロックの数をカウントする
            int count = this.NextBlock.Count();

            if( count <= NEXT_BLOCK_MAX)
            {
                //追加で７つのブロックを選び出す。
                //１から７の入った配列をランダムでシャッフルして追加する
                int[] array = { 1, 2, 3, 4, 5, 6, 7 };

                //Fisher–Yatesアルゴリズム
                for(int i = array.Length - 1; i > 0; i-- )
                {
                    int a = i - 1;
                    int b = MyRandom.Next(array.Length) % i;
                    var tmp = array[a];
                    array[a] = array[b];
                    array[b] = tmp;
                }

                foreach( int a  in array)
                {
                    this.NextBlock.Add(a);
                }
            }
        }

        //フィールドに置かれたブロックを描く
        private void DrawGameField()
        {
            //壁と設置されているブロックを描く
            for (int y = 0; y < GameField.FIELD_HEIGHT; y++)
            {
                for (int x = 0; x < GameField.FIELD_WIDTH; x++)
                {
                    if (this.BlockField[y, x] >= (int)BlockInfo.BlockType.MINO_IN_FIELD)
                    {
                        int y_pos = (this.BlockField[y, x] % (int)BlockInfo.BlockType.MINO_IN_FIELD) * BlockInfo.BLOCK_HEIGHT;
                        Rectangle srcRect = new Rectangle(0, y_pos, BlockInfo.BLOCK_WIDTH, BlockInfo.BLOCK_HEIGHT);
                        Rectangle desRect = new Rectangle(0, 0, srcRect.Width, srcRect.Height);

                        desRect.X = (x) * BlockInfo.BLOCK_WIDTH;
                        desRect.Y = (y) * BlockInfo.BLOCK_HEIGHT;
                        g.DrawImage(blockControle.BlockSourceImage, desRect, srcRect, GraphicsUnit.Pixel);

                    }
                }
            }
        }

        //消去するラインを調べる
        private void CheckEraseLine()
        {
            //地面から順に上に向かって調べる

            //すべて空のラインがあれば終了


        }


        //キー入力
        private void GameField_KeyDown(object sender, KeyEventArgs e)
        {
            //TODO ハードドロップ
            if (e.KeyData == Keys.Space)
            {
                Console.WriteLine(@"HARD_DROP");
                //         this.blockControle.CurrentPos.Y -= 1;
                HardDrop = true;
            }

            if (e.KeyData == Keys.Up)
            {
                Console.WriteLine(@"UP");
                this.blockControle.CurrentPos.Y -= 1;
            }
            if (e.KeyData == Keys.Down)
            {
                Console.WriteLine(@"DOWN");
                //                this.blockControle.CurrentPos.Y += 1;
                this.blockControle.MoveCurrentBlockDown(this.BlockField);
            }
            if (e.KeyData == Keys.Right)
            {
                Console.WriteLine(@"RIGHT");
                //                this.blockControle.CurrentPos.X += 1;
                this.blockControle.MoveCurrentBlockRight(this.BlockField);
            }
            if (e.KeyData == Keys.Left)
            {
                Console.WriteLine(@"LEFT");
                //                this.blockControle.CurrentPos.X -= 1;
                this.blockControle.MoveCurrentBlockLeft(this.BlockField);
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
        public int[,] BlockField { get; set; }
        public int fps {get;set;}

        private List<int> NextBlock = new List<int>();

        private GANME_MODE Mode;
        private bool HardDrop = false;
        private List<int> EraseLine = new List<int>();
        System.Random MyRandom = new Random();


        //フィールドの描画用
        Bitmap canvas;
        Graphics g;
    }
}
