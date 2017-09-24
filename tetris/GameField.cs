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
        public const int FILED_NON_DRAW = 3;    //見えない所
        public const int FIELD_HEIGHT = 20 + 3;     // + 3; //ミノ領域＋床＋出現位置
        public const int FIELD_WIDTH = 10;      //ミノ領域

        public const int NEXT_BLOCK_MAX = 14;       //７種類＊２で表示は５個。
        public const int BLOCK_TYPE_NUM = 7;        //ミノは７種類
        public const int NEXT_BLOCK_DISP_NUM = 5;   //表示は５個。

        //描画先のpictureBoxの切り替え
        enum PLAYER_DEFINE
        {
            PLAYER_1 = 0,
            PLAYER_2 = 1,
            PLAYER_NUM = 2,
        };

        enum GAME_MODE
        {
            MODE_WAIT,          //開始待ち
            MODE_SET_BLOCK,     //次のブロックを決める
            MODE_MOVE_BLOCK,    //ブロックを設置させるまでの操作
            MODE_ERASE_CHECK,   //ブロックが消えるかチェック
            MODE_ERASE_BLOCK,   //ブロックを消す処理
            MODE_TURN_CHANGE,   //プレイヤーのターンを切り替える
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
            string BlockImageFile = @"..\..\..\resource\mino.bmp";
            if (System.IO.File.Exists(BlockImageFile))
            {
                BlockSourceImage = Image.FromFile(BlockImageFile);
            }
            else
            {
                BlockImageFile = @"..\..\resource\mino.bmp";
                BlockSourceImage = Image.FromFile(BlockImageFile);
            }

            //マネージャー系のクラスを初期化
            CreateManager();

            //フィールド情報を初期化
            BlockFieldInit();

            //描画先とするImageオブジェクトを作成する
            CreateImageObject();

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

                            for( int i = 0; i < (int)PLAYER_DEFINE.PLAYER_NUM;i++)
                            {
                                this.messageControle[i].ClearMessage();
                                this.scoreManage[i].ClearScore();
                                this.attackLineManage[i].ClearAttackLine();
                            }

                            playerTurn = PLAYER_DEFINE.PLAYER_1;
                        }
                    }
                    break;
                //次のブロックを決める
                case GAME_MODE.MODE_SET_BLOCK:
                    {
                        //次のブロックを取り出す
                        UpdateNextBlock();

                        int player = (int)playerTurn;

                        int type = GetNextBlock(player);

                        blockControle[player].SetCurrentBlock((BlockInfo.BlockType)type);

                        int[,] field = this.fieldManage[player].BlockField;

                        //ここでブロックを置くことができなければゲームオーバー
                        if(this.blockControle[player].CheckGameOver(field))
                        {
                            Mode = GAME_MODE.MODE_GAME_OVER;

                            //置いているブロックをすべて灰色にする
                            //壁と設置されているブロックを描く
                            for (int y = 0; y < GameField.FIELD_HEIGHT; y++)
                            {
                                for (int x = 0; x < GameField.FIELD_WIDTH; x++)
                                {
                                    int field_block = field[y, x] % (int)BlockInfo.BlockType.MINO_IN_FIELD;

                                    if ((int)BlockInfo.BlockType.MINO_I <= field_block &&
                                        field_block <= (int)BlockInfo.BlockType.MINO_O)
                                    {
                                        field[y, x] = (int)BlockInfo.BlockType.MINO_ATTACK + (int)BlockInfo.BlockType.MINO_IN_FIELD;
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
                        int player = (int)playerTurn;

                        if (this.InputHold)
                        {
                            //HOLD
                            if (!this.blockControle[player].DoHold)
                            {
                                if (!this.blockControle[player].UpdateHold())
                                {
                                    //HOLDブロックが無かったとき
                                    Mode = GAME_MODE.MODE_SET_BLOCK;
                                }
                            }
                            this.InputHold = false;
                        }
                        else if (this.HardDrop)
                        {
                            int[,] field = this.fieldManage[player].BlockField;

                            //ハードドロップ
                            int y = this.blockControle[player].HardDropCurrentBlock(field);
                            this.blockControle[player].CurrentPos.Y += y;   //TODO 関数化

                            this.blockControle[player].DoHold = false;
                            this.HardDrop = false;

                            this.Mode = GAME_MODE.MODE_ERASE_CHECK;
                            this.blockControle[player].SetBlockInField(field);
                        }
                    }
                    break;

                //ブロックが消えるかチェック
                case GAME_MODE.MODE_ERASE_CHECK:
                    {
                        int player = (int)playerTurn;

                        //消えるラインのチェック
                        int line_num = CheckEraseLine(player);
                        bool perfect = CheckPerfect(line_num, player);
                        int tspin_type = CheckTspin(this.blockControle[player].status);

                        AttackLineManage.EraseLineResult result = new AttackLineManage.EraseLineResult();
                        this.attackLineManage[player].CalcAttackLine(
                            line_num, 
                            tspin_type,
                            perfect,
                            ref this.blockControle[player].BtoB,
                            ref result);

                        //メッセージを作る
                        MakeEraseLineMessage(result, player);

                        //スコアを記録
                        if ( line_num > 0)
                        {
                            int tspin = CheckTspin(this.blockControle[player].status);
                            this.scoreManage[player].SetEraseLine(line_num, (tspin == BlockControle.TSPIN));
                        }


                        this.Mode = GAME_MODE.MODE_ERASE_BLOCK;
                    }
                    break;

                //ブロックを消す処理
                case GAME_MODE.MODE_ERASE_BLOCK:
                    {
                        int player = (int)playerTurn;
                        ExecEraseLine(player);

                        //ここで攻撃ラインの処理を行う
                        this.Mode = GAME_MODE.MODE_TURN_CHANGE;
                    }
                    break;

                case GAME_MODE.MODE_TURN_CHANGE:
                    {
                        //プレイヤー交代
                        playerTurn = playerTurn == PLAYER_DEFINE.PLAYER_1 ? PLAYER_DEFINE.PLAYER_2 : PLAYER_DEFINE.PLAYER_1;
                        this.Mode = GAME_MODE.MODE_SET_BLOCK;
                    }
                    break;
                //ゲームオーバー
                case GAME_MODE.MODE_GAME_OVER:
                    {
                        messageControle[0].SetMessage(@"Press F1 key to start", false);
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

            for(int player = 0; player < (int)PLAYER_DEFINE.PLAYER_NUM;player++)
            {
                //設置したブロックを描画
                DrawGameField(player);

                //NEXTブロックの描画
                DrawNextBlock(player);

                if(player == (int)this.playerTurn)
                {
                    //操作中のブロックを描画
                    DrawCurrentBlock(player,GameOverFlag);
                    //落下位置ブロックを描画
                    DrawGuideBlock(player);
                }

                //HOLDブロックを描画
                DrawHoldBlock(player);

                //スコア表示の描画
                DrawScore(player);

                //攻撃ライン表示
                DrawAttackLine(player);

            }





            {
                int p1 = (int)PLAYER_DEFINE.PLAYER_1;
                int p2 = (int)PLAYER_DEFINE.PLAYER_2;

                //メッセージの表示
                this.messageControle[p1].DrawUpdate();
                this.messageControle[p2].DrawUpdate();

                //PictureBoxを更新
                this.pictureBoxField1P.Image = canvasFiled[p1];
                this.pictureBoxField2P.Image = canvasFiled[p2];

                this.pictureBoxNext1P.Image = canvasNextBlock[p1];
                this.pictureBoxNext2P.Image = canvasNextBlock[p2];

                this.pictureBoxHold1P.Image = canvasHoldBlock[p1];
                this.pictureBoxHold2P.Image = canvasHoldBlock[p2];

                this.pictureBoxAttackLine1P.Image = canvasAttackLine[p1];
                this.pictureBoxAttackLine2P.Image = canvasAttackLine[p2];
            }


            //デバッグ用、操作ブロックの位置を表示
            int pos_x = blockControle[0].CurrentPos.X;
            int pos_y = blockControle[0].CurrentPos.Y;
            this.labelCurrentPos.Text = @"CurrectPos :(" + pos_x.ToString() + "," + pos_y.ToString() + @")";

            //フォームの書き換え
            Invalidate();
        }

        //===========================================
        //  private
        //===========================================

        private void BlockFieldInit()
        {
            GameOverFlag = false;

            this.fieldManage[0].ClearField();
            this.fieldManage[1].ClearField();

            //NEXTブロックを収める配列
            UpdateNextBlock();
        }


        //NEXTブロックを取得
        private int GetNextBlock(int player)
        {
            //一つ取り出す
            List<int> next = this.NextBlock[player];
            int type = next[0];
            next.RemoveAt(0);
            return type;
        }

        //NEXTブロックを更新
        private void UpdateNextBlock()
        {
            for(int player = 0; player < (int)PLAYER_DEFINE.PLAYER_NUM; player++)
            {
                //NEXTブロックの数をカウントする
                int count = this.NextBlock[player].Count();

                if (count <= NEXT_BLOCK_MAX)
                {
                    //追加で７つのブロックを選び出す。
                    //１から７の入った配列をランダムでシャッフルして追加する
                    int[] array = { 1, 2, 3, 4, 5, 6, 7 };

                    //Fisher–Yatesアルゴリズム
                    for (int i = array.Length - 1; i > 0; i--)
                    {
                        int a = i - 1;
                        int b = MyRandom.Next(array.Length) % i;
                        var tmp = array[a];
                        array[a] = array[b];
                        array[b] = tmp;
                    }

                    foreach (int a in array)
                    {
                        this.NextBlock[player].Add(a);
                    }
                }
            }
        }


        //消去するラインを調べる
        private int  CheckEraseLine(int player)
        {
            int line_num = 0;
            int[,] field = this.fieldManage[player].BlockField;
            //床から見ていく
            for (int h = GameField.FIELD_HEIGHT - 2; h >= 0; h--)
            {
                bool erase_line = true;
                //壁の所は見ない
                for (int w = 1; w < GameField.FIELD_WIDTH - 1; w++)
                {
                    //設置されていないか
                    if (field[h, w] < (int)BlockInfo.BlockType.MINO_IN_FIELD)
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
                        field[h, w] += (int)(BlockInfo.BlockType.MINO_VANISH);
                    }
                    //消す
                    line_num++;
                    this.EraseLine[player].Add(h);
                }
            }

            return line_num;
        }

        /// <summary>
        /// パーフェクトチェック
        /// </summary>
        /// <param name="erase_line_num"></param>
        /// <returns></returns>
        private bool CheckPerfect(int erase_line_num,int player)
        {
            bool ok = false;

            int[,] field = this.fieldManage[player].BlockField;

            //パーフェクトチェック
            //床から見ていく
            int perfect_count = 0;
            const int PERFECT_LINE_CHECK = 5;
            for (int h = GameField.FIELD_HEIGHT - 2; h > GameField.FIELD_HEIGHT - 2 - PERFECT_LINE_CHECK; h--)
            {
                bool line_check = true;
                //壁の所は見ない
                for (int w = 1; w < GameField.FIELD_WIDTH - 2; w++)
                {
                    //消す予定になっているor何もない
                    int block_data = field[h, w];
                    if( block_data >= (int)BlockInfo.BlockType.MINO_VANISH ||
                        block_data == 0)
                    {

                    }
                    else
                    {
                        line_check = false;
                        break;
                    }
                }
                if (line_check)
                {
                    perfect_count++;
                }
            }

            //消したライン数と床から探索して消す予定ライン数が一致していたらパーフェクト
            if (perfect_count == PERFECT_LINE_CHECK)
            {
                ok = true;
            }

            return ok;
        }

        /// <summary>
        /// Tspinで有るかをチェック
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        private int CheckTspin(int status)
        {
            int ret = BlockControle.NUN;
            if ((status & BlockControle.TSPIN) == BlockControle.TSPIN)
            {
                ret = BlockControle.TSPIN;
            }
            else if ((status & BlockControle.TSPIN_MINI) == BlockControle.TSPIN_MINI)
            {
                ret = BlockControle.TSPIN_MINI;
            }

            return ret;
        }

        /// <summary>
        /// 消去したラインによってメッセージを作る
        /// </summary>
        /// <param name="result"></param>
        /// <param name="player">0 = 1P 1 = 2P</param>
        private void MakeEraseLineMessage(AttackLineManage.EraseLineResult result,int player)
        {
            if (result.Line <= 0)
            {
                return;
            }

            if (result.perfect)
            {
                //パーフェクトは他にメッセージを出さない
                this.messageControle[player].message_list.Add(MessageControle.MESSAGE_TYPE.PERFECT);
                this.messageControle[player].MakeMessage();
                return;
            }

            if (result.BtoB)
            {
                this.messageControle[player].message_list.Add(MessageControle.MESSAGE_TYPE.BACK_TO_BACK);
            }
            if (result.Tspin == BlockControle.TSPIN)
            {
                this.messageControle[player].message_list.Add(MessageControle.MESSAGE_TYPE.T_SPIN);
            }
            else if (result.Tspin == BlockControle.TSPIN_MINI)
            {
                this.messageControle[player].message_list.Add(MessageControle.MESSAGE_TYPE.T_SPIN_MINI);
            }

            switch(result.Line)
            {
                case 1: this.messageControle[player].message_list.Add(MessageControle.MESSAGE_TYPE.SINGLE);  break;
                case 2: this.messageControle[player].message_list.Add(MessageControle.MESSAGE_TYPE.DOUBLE); break;
                case 3: this.messageControle[player].message_list.Add(MessageControle.MESSAGE_TYPE.TRIPLE); break;
                case 4: this.messageControle[player].message_list.Add(MessageControle.MESSAGE_TYPE.TETRIS); break;
                default: break;
            }

            //REN
            if( result.Ren >= 3)
            {
                this.messageControle[player].ren_num = result.Ren;
                this.messageControle[player].message_list.Add(MessageControle.MESSAGE_TYPE.REN);
            }

            this.messageControle[player].MakeMessage();
        }

        //消去するラインを調べる
        private void ExecEraseLine(int player)
        {
            int[,] field = this.fieldManage[player].BlockField;

            //ブロックを実際に消す処理
            //アニメーションをそのうちつける
            for (int h = 0; h < GameField.FIELD_HEIGHT; h++)
            {
                //壁の所は見ない
                for (int w = 1; w < GameField.FIELD_WIDTH - 1; w++)
                {
                    if (field[h, w] >= (int)BlockInfo.BlockType.MINO_VANISH)
                    {
                        field[h, w] = 0;
                        for (int h2 = h; h2 > 0; h2--)
                        {
                            field[h2, w] = field[h2 - 1, w];
                        }
                    }
                }
            }

            //一番上のラインを埋める
            for (int w = 1; w < GameField.FIELD_WIDTH - 1; w++)
            {
                field[0, w] = 0;
            }

            this.EraseLine[player].Clear();
        }

        /// <summary>
        /// 管理クラスの初期化
        /// </summary>
        private void CreateManager()
        {
            const int p1 = (int)PLAYER_DEFINE.PLAYER_1;
            const int p2 = (int)PLAYER_DEFINE.PLAYER_2;
            const int make_num = (int)PLAYER_DEFINE.PLAYER_NUM;

            blockControle = new BlockControle[make_num];
            messageControle = new MessageControle[make_num];
            scoreManage = new ScoreManage[make_num];
            attackLineManage = new AttackLineManage[make_num];
            fieldManage = new FieldManage[make_num];

            NextBlock = new List<int>[make_num];
            EraseLine = new List<int>[make_num];

            for (int i = 0; i < make_num; i++)
            {
                blockControle[i] = new BlockControle();
                messageControle[i] = new MessageControle();
                scoreManage[i] = new ScoreManage();
                attackLineManage[i] = new AttackLineManage();
                NextBlock[i] = new List<int>();
                EraseLine[i] = new List<int>();
                fieldManage[i] = new FieldManage();
            }

            //メッセージ

            messageControle[p1].Message = this.labelMessage1P;
            messageControle[p1].SetMessage(@"Press F1 key to start", false);
            messageControle[p2].Message = this.labelMessage2P;
            messageControle[p2].SetMessage(@"Press F1 key to start", false);

        }

        private void CreateImageObject()
        {
            const int p1 = (int)PLAYER_DEFINE.PLAYER_1;
            const int p2 = (int)PLAYER_DEFINE.PLAYER_2;
            const int make_num = (int)PLAYER_DEFINE.PLAYER_NUM;
            canvasFiled = new Bitmap[make_num];
            gFiled = new Graphics[make_num];
            canvasNextBlock = new Bitmap[make_num];
            gNextBlock = new Graphics[make_num];
            canvasNextBlock = new Bitmap[make_num];
            gNextBlock = new Graphics[make_num];
            canvasHoldBlock = new Bitmap[make_num];
            gHoldBlock = new Graphics[make_num];
            canvasAttackLine = new Bitmap[make_num];
            gAttackLine = new Graphics[make_num];

            this.canvasFiled[p1] = new Bitmap(this.pictureBoxField1P.Width, this.pictureBoxField1P.Height);
            this.gFiled[p1] = Graphics.FromImage(canvasFiled[p1]);
            this.canvasFiled[p2] = new Bitmap(this.pictureBoxField2P.Width, this.pictureBoxField2P.Height);
            this.gFiled[p2] = Graphics.FromImage(canvasFiled[p2]);


            this.canvasNextBlock[p1] = new Bitmap(this.pictureBoxNext1P.Width, this.pictureBoxNext1P.Height);
            this.gNextBlock[p1] = Graphics.FromImage(canvasNextBlock[p1]);
            this.canvasNextBlock[p2] = new Bitmap(this.pictureBoxNext2P.Width, this.pictureBoxNext2P.Height);
            this.gNextBlock[p2] = Graphics.FromImage(canvasNextBlock[p2]);

            this.canvasHoldBlock[p1] = new Bitmap(this.pictureBoxHold1P.Width, this.pictureBoxHold1P.Height);
            this.gHoldBlock[p1] = Graphics.FromImage(canvasHoldBlock[p1]);
            this.canvasHoldBlock[p2] = new Bitmap(this.pictureBoxHold2P.Width, this.pictureBoxHold2P.Height);
            this.gHoldBlock[p2] = Graphics.FromImage(canvasHoldBlock[p2]);

            this.canvasAttackLine[p1] = new Bitmap(this.pictureBoxAttackLine1P.Width, this.pictureBoxAttackLine1P.Height);
            this.gAttackLine[p1] = Graphics.FromImage(canvasAttackLine[p1]);
            this.canvasAttackLine[p2] = new Bitmap(this.pictureBoxAttackLine2P.Width, this.pictureBoxAttackLine2P.Height);
            this.gAttackLine[p2] = Graphics.FromImage(canvasAttackLine[p2]);

        }

        //リセットする
        private void ResetGame()
        {
            this.GameStart = true;
            this.Mode = GAME_MODE.MODE_WAIT;
        }

        //キー入力
        private void GameField_KeyDown(object sender, KeyEventArgs e)
        {
            int player = (int)playerTurn;

            //TODO 
            int[,] field = this.fieldManage[player].BlockField;



            //GameStart
            if (e.KeyData == Keys.F1)
            {
                Console.WriteLine(@"GAME_START");
                this.GameStart = true;
                return;
            }
            //Reset
            if (e.KeyData == Keys.F2)
            {
                Console.WriteLine(@"RESET");
                ResetGame();
                return;
            }
            if ( this.Mode == GAME_MODE.MODE_MOVE_BLOCK)
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
                    this.blockControle[player].CurrentPos.Y -= 1;
                }
                if (e.KeyData == Keys.Down)
                {
                    Console.WriteLine(@"DOWN");
                    this.blockControle[player].MoveCurrentBlockDown(field);
                }
                if (e.KeyData == Keys.Right)
                {
                    Console.WriteLine(@"RIGHT");
                    this.blockControle[player].MoveCurrentBlockRight(field);
                }
                if (e.KeyData == Keys.Left)
                {
                    Console.WriteLine(@"LEFT");
                    this.blockControle[player].MoveCurrentBlockLeft(field);
                }

                //回転
                if (e.KeyData == Keys.X)
                {
                    Console.WriteLine(@"ROTATE_R");
                    this.blockControle[player].RotateCurrentBlock(true, field);
                }
                else if (e.KeyData == Keys.Z)
                {
                    Console.WriteLine(@"ROTATE_L");
                    this.blockControle[player].RotateCurrentBlock(false, field);
                }

            }
        }

        public int fps {get;set;}

        //ゲーム管理
        private GAME_MODE Mode;
        BlockControle[] blockControle;
        MessageControle[] messageControle;
        ScoreManage[] scoreManage;
        AttackLineManage[] attackLineManage;
        FieldManage[] fieldManage;
        PLAYER_DEFINE playerTurn;

        private bool GameOverFlag = false;

        //データ配列
        private List<int>[] NextBlock;
        private List<int>[] EraseLine;
        System.Random MyRandom = new Random();

        //キー入力持ち
        private bool HardDrop = false;
        private bool InputHold = false;
        private bool GameStart = false;


        //フィールドの描画用
        private Image BlockSourceImage;
        Bitmap[] canvasFiled;
        Graphics[] gFiled;
        Bitmap[] canvasNextBlock;
        Graphics[] gNextBlock;
        Bitmap[] canvasHoldBlock;
        Graphics[] gHoldBlock;
        Bitmap[] canvasAttackLine;
        Graphics[] gAttackLine;


    }
}
