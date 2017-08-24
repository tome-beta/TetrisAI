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
        public const int FIELD_HEIGHT = 20 + 1;     // + 3; //ミノ領域＋床＋出現位置
        public const int FIELD_WIDTH = 10 + 2;      //ミノ領域 + 壁＊２

        public const int NEXT_BLOCK_MAX = 14;       //７種類＊２で表示は５個。
        public const int BLOCK_TYPE_NUM = 7;        //ミノは７種類
        public const int NEXT_BLOCK_DISP_NUM = 5;   //表示は５個。

        enum GAME_MODE
        {
            MODE_WAIT,          //開始待ち
            MODE_SET_BLOCK,     //次のブロックを決める
            MODE_MOVE_BLOCK,    //ブロックを設置させるまでの操作
            MODE_ERASE_CHECK,   //ブロックが消えるかチェック
            MODE_ERASE_BLOCK,   //ブロックを消す処理
            MODE_GAME_OVER,   //ゲームオーバー
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
            //ブロックの元画像を読み込んでおく
            BlockSourceImage = Image.FromFile(@"..\..\resource\mino.bmp");

            this.BlockField = new int[GameField.FIELD_HEIGHT, GameField.FIELD_WIDTH];

            //フィールド情報を初期化
            BlockFieldInit();

            //描画先とするImageオブジェクトを作成する
            this.canvasFiled1P = new Bitmap(this.pictureBoxField1P.Width, this.pictureBoxField1P.Height);
            this.gFiled1P = Graphics.FromImage(canvasFiled1P);

            this.canvasNextBlock1P = new Bitmap(this.pictureBoxNext1P.Width, this.pictureBoxNext1P.Height);
            this.gNextBlock1P = Graphics.FromImage(canvasNextBlock1P);

            this.canvasHoldBlock1P = new Bitmap(this.pictureBoxHold1P.Width, this.pictureBoxHold1P.Height);
            this.gHoldBlock1P = Graphics.FromImage(canvasHoldBlock1P);

            Mode = GAME_MODE.MODE_WAIT;
        }
        public void Exec()
        {
            Console.WriteLine(@"EXEC");

            switch (Mode)
            {
                case GAME_MODE.MODE_WAIT:
                    {
                        if(this.GameStart)
                        {
                            //フィールドの初期化をする
                            BlockFieldInit();
                            this.GameStart = false;
                            Mode = GAME_MODE.MODE_SET_BLOCK;
                        }
                    }
                    break;
                //次のブロックを決める
                case GAME_MODE.MODE_SET_BLOCK:
                    {
                        //次のブロックを取り出す
                        UpdateNextBlock();
                        int type = GetNextBlock();

                        blockControle.SetCurrentBlock((BlockInfo.BlockType)type);

                        //ここでブロックを置くことができなければゲームオーバー
                        if(this.blockControle.CheckGameOver(this.BlockField))
                        {
                            Mode = GAME_MODE.MODE_GAME_OVER;

                            //置いているブロックをすべて灰色にする
                            //壁と設置されているブロックを描く
                            for (int y = 0; y < GameField.FIELD_HEIGHT; y++)
                            {
                                for (int x = 0; x < GameField.FIELD_WIDTH; x++)
                                {
                                    int field_block = this.BlockField[y, x] % (int)BlockInfo.BlockType.MINO_IN_FIELD;

                                    if ((int)BlockInfo.BlockType.MINO_I <= field_block &&
                                        field_block <= (int)BlockInfo.BlockType.MINO_O)
                                    {
                                        this.BlockField[y, x] = (int)BlockInfo.BlockType.MINO_ATTACK + (int)BlockInfo.BlockType.MINO_IN_FIELD;
                                    }
                                }
                            }
                        }
                        else
                        {
                            Mode = GAME_MODE.MODE_MOVE_BLOCK;
                        }
                        
                    }
                    break;

                //ブロックを設置させるまでの操作
                case GAME_MODE.MODE_MOVE_BLOCK:
                    {
                        if (this.InputHold)
                        {
                            //HOLD
                            if (!this.blockControle.DoHold)
                            {
                                if (!this.blockControle.UpdateHold())
                                {
                                    //HOLDブロックが無かったとき
                                    Mode = GAME_MODE.MODE_SET_BLOCK;
                                }
                            }
                            this.InputHold = false;
                        }
                        else if (this.HardDrop)
                        {
                            //ハードドロップ
                            int y = this.blockControle.HardDropCurrentBlock(BlockField);
                            this.blockControle.CurrentPos.Y += y;   //TODO 関数化

                            this.blockControle.DoHold = false;
                            this.HardDrop = false;

                            this.Mode = GAME_MODE.MODE_ERASE_CHECK;
                            this.blockControle.SetBlockInField(this.BlockField);
                        }
                    }
                    break;

                //ブロックが消えるかチェック
                case GAME_MODE.MODE_ERASE_CHECK:
                    {
                        //消えるラインのチェック
                        CheckEraseLine();

                        this.Mode = GAME_MODE.MODE_ERASE_BLOCK;
                    }
                    break;

                //ブロックを消す処理
                case GAME_MODE.MODE_ERASE_BLOCK:
                    {
                        ExecEraseLine();

                        this.Mode = GAME_MODE.MODE_SET_BLOCK;
                    }
                    break;

                //ゲームオーバー
                case GAME_MODE.MODE_GAME_OVER:
                    {
                        this.Mode = GAME_MODE.MODE_WAIT;
                        GameOverFlag = true;
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

            //設置したブロックを描画
            DrawGameField();

            //NEXTブロックの描画
            DrawNextBlock();

            //操作中のブロックを描画
            DrawCurrentBlock();

            //落下位置ブロックを描画
            DrawGuideBlock();

            
            //HOLDブロックを描画
            DrawHoldBlock();

            //PictureBoxを更新
            this.pictureBoxField1P.Image = canvasFiled1P;
            this.pictureBoxNext1P.Image = canvasNextBlock1P;
            this.pictureBoxHold1P.Image = canvasHoldBlock1P;

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

        private void BlockFieldInit()
        {
            //フィールドを作る
            //フィールドは１０＊２０の両サイドに壁を表す９９を入れる。
            //ブロックのスタート位置のために上に３行加える。
            //床にも１行追加
            //全体としては１２＊２4
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

            GameOverFlag = false;

            //NEXTブロックを収める配列
            UpdateNextBlock();
        }


        //NEXTブロックを取得
        private int GetNextBlock()
        {
            //一つ取り出す
            int type = this.NextBlock[0];
            this.NextBlock.RemoveAt(0);
            return type;
        }

        //NEXTブロックを更新
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


        //消去するラインを調べる
        private void CheckEraseLine()
        {
            int line_num = 0;
            //床から見ていく
            for (int h = GameField.FIELD_HEIGHT - 2; h >= 0; h--)
            {
                bool erase_line = true;
                //壁の所は見ない
                for (int w = 1; w < GameField.FIELD_WIDTH - 1; w++)
                {
                    //設置されていないか
                    if (this.BlockField[h, w] < (int)BlockInfo.BlockType.MINO_IN_FIELD)
                    {
                        //空きがあれば飛ばす
                        erase_line = false;
                        break;
                    }
                }

                //消すラインを予約する
                if (erase_line)
                {
                    //消す予定の情報を加える
                    for (int w = 1; w < GameField.FIELD_WIDTH - 1; w++)
                    {
                        this.BlockField[h, w] += (int)(BlockInfo.BlockType.MINO_VANISH);
                    }
                    //消す
                    line_num++;
                    this.EraseLine.Add(h);
                }
            }

            //TODO 
            //ここでTスピン　BtoB　RENのチェックする

        }

        //消去するラインを調べる
        private void ExecEraseLine()
        {
            //ブロックを実際に消す処理
            //アニメーションをそのうちつける
            for (int h = 0; h < GameField.FIELD_HEIGHT; h++)
            {
                //壁の所は見ない
                for (int w = 1; w < GameField.FIELD_WIDTH - 1; w++)
                {
                    if (this.BlockField[h, w] >= (int)BlockInfo.BlockType.MINO_VANISH)
                    {
                        this.BlockField[h, w] = 0;
                        for (int h2 = h; h2 > 0; h2--)
                        {
                            this.BlockField[h2, w] = this.BlockField[h2 - 1, w];
                        }
                    }
                }
            }

            //一番上のラインを埋める
            for (int w = 1; w < GameField.FIELD_WIDTH - 1; w++)
            {
                this.BlockField[0, w] = 0;
            }

            this.EraseLine.Clear();
        }


        //キー入力
        private void GameField_KeyDown(object sender, KeyEventArgs e)
        {
            //GameStart
            if (e.KeyData == Keys.F1)
            {
                Console.WriteLine(@"GAME_START");
                this.GameStart = true;
                return;
            }

            if( this.Mode == GAME_MODE.MODE_MOVE_BLOCK)
            {
                //HOLD
                if (e.KeyData == Keys.C)
                {
                    Console.WriteLine(@"HOLD");
                    InputHold = true;
                    return;
                }

                //ハードドロップ
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
                    this.blockControle.RotateCurrentBlock(true, this.BlockField);
                }
                else if (e.KeyData == Keys.Z)
                {
                    Console.WriteLine(@"ROTATE_L");
                    this.blockControle.RotateCurrentBlock(false, this.BlockField);
                }

            }
        }


        BlockControle blockControle = new BlockControle();
        public int fps {get;set;}

        //ゲーム管理
        private GAME_MODE Mode;
        private bool GameOverFlag = false;

        //データ配列
        public int[,] BlockField { get; set; }
        private List<int> NextBlock = new List<int>();
        private List<int> EraseLine = new List<int>();
        System.Random MyRandom = new Random();

        //キー入力持ち
        private bool HardDrop = false;
        private bool InputHold = false;
        private bool GameStart = false;


        //フィールドの描画用
        private Image BlockSourceImage;
        Bitmap canvasFiled1P;
        Graphics gFiled1P;
        Bitmap canvasNextBlock1P;
        Graphics gNextBlock1P;

        Bitmap canvasHoldBlock1P;
        Graphics gHoldBlock1P;

    }
}
