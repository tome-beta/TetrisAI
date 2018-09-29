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

        public const int BLOCK_TYPE_NUM = 7;        //ミノは７種類

        //描画先のpictureBoxの切り替え
        enum PLAYER_DEFINE
        {
            PLAYER_1 = 0,
            PLAYER_2 = 1,
            PLAYER_NUM = 2,
        };

        //１プレイヤーか２プレイヤーか
        enum PLAY_MODE
        {
            ONLY_1P = 0,    //１P
            VS = 1,      //対戦
        };

        enum GAME_MODE
        {
            MODE_WAIT,          //開始待ち
            MODE_SET_BLOCK,     //次のブロックを決める
            MODE_MOVE_BLOCK,    //ブロックを設置させるまでの操作
            MODE_ERASE_CHECK,   //ブロックが消えるかチェック
            MODE_ERASE_BLOCK,   //ブロックを消す処理
            MODE_ATTACK_BLOCK,  //攻撃ブロックの処理
            MODE_TURN_CHANGE,   //プレイヤーのターンを切り替える
            MODE_GAME_OVER,     //ゲームオーバー
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
            int player = (int)playerTurn;

            switch (Mode)
            {
                case GAME_MODE.MODE_WAIT:
                    {
                        if(this.GameStart)
                        {
                            //メニューで選択してるモードのチェック
                            ChkMenuPlayMode();

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
                        int type = nextManage[player].GetNextBlock();
                        blockControle[player].SetCurrentBlock((BlockInfo.BlockType)type);

                        //AIの番ならフィールドの評価を行う
                        if(this.PlayerAI[player])
                        {
                            EvaluateField();
                        }
                        
                        //ここでブロックを置くことができなければゲームオーバー
                        if (this.blockControle[player].CheckGameOver(this.fieldManage[player].BlockField))
                        {
                            Mode = GAME_MODE.MODE_GAME_OVER;
                            this.fieldManage[player].GameOverField();
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
                        //AIの番なら自動操作
                        if (this.PlayerAI[player])
                        {
                            //ハードドロップ
                            this.blockControle[player].HardDropCurrentBlock(this.fieldManage[player].BlockField);

                            this.blockControle[player].DoHold = false;
                            this.HardDrop = false;

                            this.Mode = GAME_MODE.MODE_ERASE_CHECK;
                            this.blockControle[player].SetBlockInField(this.fieldManage[player].BlockField);

                            //盤面の評価
                            double score = 0;
                            FeatureData data = this.evaluateManage.Exec(this.blockControle[player], this.fieldManage[player], ref score);
                            if (evaluateDispForm != null)
                            {
                                this.evaluateDispForm.SetFeatureData(data, (int)playerTurn);
                                this.evaluateDispForm.SetScore(score, (int)playerTurn);
                            }
                        }
                        else
                        {
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
                                this.blockControle[player].HardDropCurrentBlock(field);

                                this.blockControle[player].DoHold = false;
                                this.HardDrop = false;

                                this.Mode = GAME_MODE.MODE_ERASE_CHECK;
                                this.blockControle[player].SetBlockInField(field);

                                //盤面の評価
                                double score = 0;
                                FeatureData data = this.evaluateManage.Exec(this.blockControle[player], this.fieldManage[player], ref score);
                                if (evaluateDispForm != null)
                                {
                                    this.evaluateDispForm.SetFeatureData(data, (int)playerTurn);
                                    this.evaluateDispForm.SetScore(score, (int)playerTurn);
                                }

                            }
                        }

                    }
                    break;

                //ブロックが消えるかチェック
                case GAME_MODE.MODE_ERASE_CHECK:
                    {
                        //消えるラインのチェック
                        int line_num = this.fieldManage[player].CheckEraseLine();
                        bool perfect = this.fieldManage[player].CheckPerfect(line_num);
                        int tspin_type = this.blockControle[player].CheckTspin();

                        AttackLineManage.EraseLineResult result = new AttackLineManage.EraseLineResult();

                        int enemy = player == (int)PLAYER_DEFINE.PLAYER_1 ? (int)PLAYER_DEFINE.PLAYER_2 : (int)PLAYER_DEFINE.PLAYER_1;
                        int enemy_attack = this.attackLineManage[enemy].AttackLineNum;

                        this.attackLineManage[player].CalcAttackLine(
                            line_num, 
                            tspin_type,
                            perfect,
                            ref this.blockControle[player].BtoB,
                            ref result,
                            ref enemy_attack
                            );

                        //メッセージを作る
                        messageControle[player].MakeEraseLineMessage(result);

                        //スコアを記録
                        if ( line_num > 0)
                        {
                            int tspin = this.blockControle[player].CheckTspin();
                            this.scoreManage[player].SetEraseLine(line_num, (tspin == BlockControle.TSPIN));
                        }

                        //AI学習用
                        if (this.PlayerAI[player])
                        {
                            //一度に多く消したときの価値を大きくするため
                            AI_Score += (line_num * line_num);
                        }

                        this.Mode = GAME_MODE.MODE_ERASE_BLOCK;
                    }
                    break;

                //ブロックを消す処理
                case GAME_MODE.MODE_ERASE_BLOCK:
                    {
                        this.fieldManage[player].ExecEraseLine();

                        this.Mode = GAME_MODE.MODE_ATTACK_BLOCK;

                    }
                    break;

                case GAME_MODE.MODE_ATTACK_BLOCK:
                    {
                        //ここで攻撃ラインの処理を行う
                        int enemy = player == (int)PLAYER_DEFINE.PLAYER_1 ? (int)PLAYER_DEFINE.PLAYER_2 : (int)PLAYER_DEFINE.PLAYER_1;

                        int[,] field = this.fieldManage[player].BlockField;
                        this.attackLineManage[enemy].ExecAttacLine(ref field);

                        this.Mode = GAME_MODE.MODE_TURN_CHANGE;
                    }
                    break;

                //プレイヤー交代
                case GAME_MODE.MODE_TURN_CHANGE:
                    {
                        if (player_select != 0)
                        {
                            playerTurn = playerTurn == PLAYER_DEFINE.PLAYER_1 ? PLAYER_DEFINE.PLAYER_2 : PLAYER_DEFINE.PLAYER_1;
                        }
                        
                        this.Mode = GAME_MODE.MODE_SET_BLOCK;
                    }
                    break;
                //ゲームオーバー
                case GAME_MODE.MODE_GAME_OVER:
                    {
                        messageControle[player].SetMessage(@"Press F1 key to start", false);
                        this.Mode = GAME_MODE.MODE_WAIT;
                        GameOverFlag = true;

                        //AI学習モード
                        if(this.AILearningMode)
                        {
                            UpdateLearning();
                        }

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
            this.labelFPS.Text = @"FPS: " + this.fps.ToString();
            int p1 = (int)PLAYER_DEFINE.PLAYER_1;
            int p2 = (int)PLAYER_DEFINE.PLAYER_2;

            if (!描画OFFToolStripMenuItem.Checked)
            {
                for (int player = 0; player < (int)PLAYER_DEFINE.PLAYER_NUM; player++)
                {
                    //設置したブロックを描画
                    DrawGameField(player);

                    //NEXTブロックの描画
                    DrawNextBlock(player);

                    if (player == (int)this.playerTurn)
                    {
                        //操作中のブロックを描画
                        DrawCurrentBlock(player, GameOverFlag);
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



            //メッセージの表示
            this.messageControle[p1].DrawUpdate();
            this.messageControle[p2].DrawUpdate();


            //盤面評価ウインドウの表示更新
            if (this.evaluateDispForm != null)
            {
                evaluateDispForm.UpdateDisp();
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
            nextManage[0].InitNextBlock();
            nextManage[1].InitNextBlock();

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
            nextManage = new NextBlockManage[make_num];

            for (int i = 0; i < make_num; i++)
            {
                blockControle[i] = new BlockControle();
                messageControle[i] = new MessageControle();
                scoreManage[i] = new ScoreManage();
                attackLineManage[i] = new AttackLineManage();
                fieldManage[i] = new FieldManage();
                nextManage[i] = new NextBlockManage();
            }

            //メッセージ
            messageControle[p1].Message = this.labelMessage1P;
            messageControle[p1].SetMessage(@"Press F1 key to start", false);
            messageControle[p2].Message = this.labelMessage2P;
            messageControle[p2].SetMessage(@"Press F1 key to start", false);

            //学習パラメータ
            this.LearningSetting = new AIManage.LearningSetting[GAManager.ALL_GENOM_NUM];
            for(int i = 0; i < GAManager.ALL_GENOM_NUM; i++)
            {
                this.LearningSetting[i] = new AIManage.LearningSetting();
            }
            LearningTypeCount = 0;

            //GA
            GA_Unit = new GA_UNIT();
            GA_Unit.colony = new Colony();
            GA_Unit.colony.Initialize(evaluateManage.NN_WEIGHT_NUM);//遺伝子のサイズ
            GA_Unit.manager = new GAManager();
            GA_Unit.manager.Init(GAManager.ALL_GENOM_NUM,evaluateManage.NN_WEIGHT_NUM);
            GA_Unit.manager.SetGeneration(1000);
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

        /// <summary>
        /// 学習設定の更新
        /// </summary>
        private void UpdateLearning()
        {
            //スコアの集計
            this.LearningSetting[this.LearningTypeCount].EvaluateScore += AI_Score;
            AI_Score = 0;

            this.LearningSetting[this.LearningTypeCount].ExecNum--;
            if (this.LearningSetting[this.LearningTypeCount].ExecNum > 0)
            {
                //繰り返し
                this.GameStart = true;
                this.Mode = GAME_MODE.MODE_WAIT;
            }
            else
            {
                //GA評価平均スコアの出力
                double avre_score = (double)(this.LearningSetting[this.LearningTypeCount].EvaluateScore / (double)AIManage.LEARNING_EXEC);
                this.LearningSetting[this.LearningTypeCount].AverageScore = avre_score;

                //ログを出力
                LogManager.GAResultWrite(GA_Unit.manager.GenerationCount,
                    this.LearningTypeCount,
                    this.LearningSetting[this.LearningTypeCount].AverageScore,
                    evaluateManage.EvaluateWeight);

                //ログ表示
                String exec_log = @"";
                exec_log += @"世代_遺伝子: " + GA_Unit.manager.GenerationCount.ToString() + 
                    "_"+ this.LearningTypeCount.ToString() + " ";
                exec_log += @"スコア: " + this.LearningSetting[this.LearningTypeCount].AverageScore.ToString() + " ";

                if(evaluateDispForm != null)
                {
                    TextBoxLogger.GetInstance().log(exec_log);
                }

                //遺伝子１タイプの平均スコアが記録できたら次のタイプへ
                this.LearningTypeCount++;

                //100タイプの評価が終わったら
                if (this.LearningTypeCount >= GAManager.ALL_GENOM_NUM)
                {
                    double[] score = new double[GAManager.ALL_GENOM_NUM];
                    for (int i = 0; i < GAManager.ALL_GENOM_NUM; i++)
                    {
                        score[i] = this.LearningSetting[i].AverageScore;
                    }
                    //GAによって重みの平均スコアによる更新評価
                    GA_Unit.manager.GenerationUpdate(score);

                    //世代数をチェックしておわるかどうか決める
                    if (GA_Unit.manager.GenerationCount++ < GA_Unit.manager.GenerationMAX)
                    {
                        this.LearningTypeCount = 0;
                        //学習初期化
                        SettingLearn();
                        this.GameStart = true;
                        this.Mode = GAME_MODE.MODE_WAIT;

                    }
                    else
                    {
                        //学習終了
                        LogManager.EndLogOutput();
                    }

                    this.labelGAGeneration.Text = @"Generation : " + GA_Unit.manager.GenerationCount.ToString();

                }
                else
                {
                    //次のタイプで繰り返し
                    //重みをGAから取得して実行開始
                    Genom ge = GA_Unit.manager.GetGenom(this.LearningTypeCount);
                    this.evaluateManage.SetWeightData(ge.DNA);
                    //繰り返し
                    this.GameStart = true;
                    this.Mode = GAME_MODE.MODE_WAIT;
                }
            }

            this.labelGAType.Text = @"GAType : " + this.LearningTypeCount.ToString() + @"_" + this.LearningSetting[this.LearningTypeCount].ExecNum.ToString();
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

        private void ChkMenuPlayMode()
        {
            if(MenuItem1Ponly.Checked)
            {
                player_select = (int)PLAY_MODE.ONLY_1P;
                //TODO ２プレイヤーをAIプレイヤーにしておく
                this.PlayerAI[0] = false;
                this.PlayerAI[1] = true;

            }
            else if(MenuItemVS.Checked)
            {
                player_select = (int)PLAY_MODE.VS;
                //TODO ２プレイヤーをAIプレイヤーにしておく
                this.PlayerAI[0] = false;
                this.PlayerAI[1] = true;
            }
            else if( MenuItemComOnly.Checked)
            {
                player_select = (int)PLAY_MODE.ONLY_1P;
                this.PlayerAI[0] = true;
                this.PlayerAI[1] = true;

            }
        }

        //メニューから１Pを選択
        private void MenuItem1Ponly_Click(object sender, EventArgs e)
        {
           this.MenuItem1Ponly.Checked = true;
           this.MenuItemVS.Checked = false;
            this.MenuItemComOnly.Checked = false;

            this.AILearningMode = false;
        }

        //メニューからVSを選択
        private void MenuItemVS_Click(object sender, EventArgs e)
        {
            this.MenuItem1Ponly.Checked = false;
            this.MenuItemVS.Checked = true;
            this.MenuItemComOnly.Checked = false;

            this.AILearningMode = false;
        }

        //メニューからCOMONLYを選択
        private void MenuItemComOnly_Click(object sender, EventArgs e)
        {
            this.MenuItem1Ponly.Checked = false;
            this.MenuItemVS.Checked = false;
            this.MenuItemComOnly.Checked = true;

            this.AILearningMode = true;

            LogManager.StartLogOutput(@"AI_Log.csv");
            LogManager.WriteLine(@"世代,ダイプ,平均点,遺伝子");

            SettingLearn();

            this.labelGAType.Text = @"GAType : " + this.LearningTypeCount.ToString() + @"_" + this.LearningSetting[this.LearningTypeCount].ExecNum.ToString();
            this.labelGAGeneration.Text = @"Generation : " + GA_Unit.manager.GenerationCount.ToString();
        }

        //学習設定
        private void SettingLearn()
        {
            for(int i = 0; i < GAManager.ALL_GENOM_NUM;i++)
            {
                //学習設定
                LearningSetting[i].ExecNum = AIManage.LEARNING_EXEC;
                LearningSetting[i].EvaluateScore = 0;
                LearningSetting[i].AverageScore = 0;
                LearningSetting[i].EndConditionsBlock = 1000;
            }
            //ここで重みをGAから取得して保持しておく
            Genom ge = GA_Unit.manager.GetGenom(0);

            this.evaluateManage.SetWeightData(ge.DNA);
        }


        /// <summary>
        /// AIによるフィールドの評価
        /// </summary>
        private void EvaluateField()
        {
            int player = (int)playerTurn;
            aiManage.EvaluateField(this.nextManage[player],
                                         this.blockControle[player],
                                         this.fieldManage[player],
                                         this.evaluateManage);

        }

        public int fps {get;set;}

        //ゲーム管理
        private GAME_MODE Mode;
        BlockControle[] blockControle;
        MessageControle[] messageControle;
        ScoreManage[] scoreManage;
        AttackLineManage[] attackLineManage;
        FieldManage[] fieldManage;
        NextBlockManage[] nextManage;
        AIManage aiManage = new AIManage();

        //学習関係
        EvaluateManage evaluateManage = new EvaluateManage();  //盤面評価
        AIManage.LearningSetting[] LearningSetting;            //学習セッティング
        int LearningTypeCount = 0;
        GA_UNIT GA_Unit;
        int AI_Score = 0;
        LogManage LogManager = new LogManage();

        PLAYER_DEFINE playerTurn;

        //一人プレーか二人プレーがどうか
        private int player_select = 0;

        private bool GameOverFlag = false;
        private bool[] PlayerAI = new bool[(int)PLAYER_DEFINE.PLAYER_NUM];
        private bool AILearningMode = false;

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


        //追加ウインドウ
        EvaluateDispForm evaluateDispForm;

        private void 盤面評価ウインドウToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(this.evaluateDispForm == null || this.evaluateDispForm.IsDisposed)
            {
                evaluateDispForm = new EvaluateDispForm();
                evaluateDispForm.Show();
            }
        }

        //フォームが閉じる時
        private void GameField_FormClosing(object sender, FormClosingEventArgs e)
        {
            LogManager.EndLogOutput();
        }

        //描画OFFにチェック
        private void 描画OFFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(描画OFFToolStripMenuItem.Checked)
            {
                描画OFFToolStripMenuItem.Checked = false;
            }
            else
            {
                描画OFFToolStripMenuItem.Checked = true;
            }
        }
    }
}
