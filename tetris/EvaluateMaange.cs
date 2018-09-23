using System;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;

namespace tetris
{

    //フィールドの特徴量　外部でも扱えるように外に出しておく　微妙？
    [Serializable]
    public struct FeatureData
    {
        public int last_block_height;      //１．直前に置いたミノの高さ
        public int eraseline_and_block;    //２．消えたラインの数×ミノの中で消えたブロックの数
        public int horizon_change;         //３．横方向にスキャンした時にセルの内容が変化する回数
        public int veritical_change;       //４．縦方向にスキャンした時にセルの内容が変化する回数
        public int hole;                   //５．穴の数(空白の上にブロックが存在）
        public int well_total;             //６．井戸の高さの階和(例４の階和4+3+2+1)の和
        public int hole_on_block_total;    //７．穴の上のブロックの数の和
        public int hole_row;               //８．穴のある行数
                                           //９．各列の高さの平均値
                                           //１０．各列の高さの標準偏差
    };

    //特徴量の算出と盤面の評価点を出す
    class EvaluateManage
    {
        public const int FEATURE_NUM = 8;
        public readonly int NN_WEIGHT_NUM;


        //最後に操作したブロック
        [Serializable]
        public struct LAST_BLOCK_INFO
        {
            public Point pos;
            public BlockInfo.BlockRot rot;
            public BlockInfo.BlockType type;
        };

        //コンストラクタ
        public EvaluateManage()
        {
            LastBlockInfo = new LAST_BLOCK_INFO();
            LastBlockInfo.pos = new Point();

            NN_WEIGHT_NUM = NN.INPUT_CORE * NN.HIDDIN_CORE + NN.HIDDIN_CORE * NN.OUTPUT_CORE;

            //仮設定　TODO
            EvaluateWeight = new double[NN_WEIGHT_NUM];
            for (int i = 0; i < NN_WEIGHT_NUM; i++)
            {
                EvaluateWeight[i] = Common.MyRandom.Next(-100,100);
            }


            //ニューラルネットワークの設定
            Nnetwork = new NN();
            Nnetwork.Init(NN.INPUT_CORE, NN.HIDDIN_CORE, NN.OUTPUT_CORE);
            Nnetwork.SettinWeight();


        }

        /// <summary>
        /// 重み付け設定
        /// </summary>
        /// <param name="weight"></param>
        public void SetWeightData(int[] weight)
        {
            if( weight.Length != NN_WEIGHT_NUM)
            {
                Debug.Assert(false);
            }

            for (int i = 0; i < NN_WEIGHT_NUM; i++)
            {
                this.EvaluateWeight[i] = weight[i];
            }

            Nnetwork.SettingWeight(this.EvaluateWeight);
        }

        /// <summary>
        /// 外部から呼び出す実行関数
        /// </summary>
        /// <param name="block_controle"></param>
        /// <param name="field_manage"></param>
        public FeatureData Exec(BlockControle block_controle, FieldManage field_manage,ref double score)
        {
            FeatureData feature = CalcFeature(block_controle, field_manage);

            score = CalcScore(feature, this.EvaluateWeight);

            return feature;
        }

        /// <summary>
        /// 盤面の評価点を求める
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private double CalcScore(FeatureData data,double[] weight_data)
        {
/*
            score += (int)(data.last_block_height * weight_data[0]);
            score += (int)(data.eraseline_and_block * weight_data[1]);
            score += (int)(data.horizon_change * weight_data[2]);
            score += (int)(data.veritical_change * weight_data[3]);
            score += (int)(data.hole * weight_data[4]);
            score += (int)(data.well_total * weight_data[5]);
            score += (int)(data.hole_on_block_total * weight_data[6]);
            score += (int)(data.hole_row * weight_data[7]);
*/
            Nnetwork.InputData[0] = data.last_block_height;
            Nnetwork.InputData[1] = data.eraseline_and_block;
            Nnetwork.InputData[2] = data.horizon_change;
            Nnetwork.InputData[3] = data.veritical_change;
            Nnetwork.InputData[4] = data.hole;
            Nnetwork.InputData[5] = data.well_total;
            Nnetwork.InputData[6] = data.hole_on_block_total;
            Nnetwork.InputData[7] = data.hole_row;

            //計算
            Nnetwork.ForwardPropagation();

            return Nnetwork.GetOutput();
        }

        //特徴量を計算する
        private FeatureData CalcFeature(BlockControle block_controle, FieldManage field_manage)
        {
            FeatureData feature_data = new FeatureData();

            //特徴量の作成
            feature_data.last_block_height = CalcLastBlockHeight(block_controle, field_manage.BlockField);
            feature_data.eraseline_and_block = CalcEraseAndBlock(block_controle, field_manage);
            feature_data.horizon_change = CalcHorizonChange(field_manage);
            feature_data.veritical_change = CalcVerticalChange(field_manage);
            feature_data.hole = CalcHole(field_manage);
            feature_data.well_total =  CalcWellTotal(field_manage);
            feature_data.hole_on_block_total = CalcHoleOnBlock(field_manage);
            feature_data.hole_row = CalcHoleRow(field_manage);


            return feature_data;
        }

        //特徴量１：最後に置いたブロックの高さを計算
        private int CalcLastBlockHeight(BlockControle ai_controle, int[,] ai_field)
        {
            int height = 0;

            LAST_BLOCK_INFO last_block = ai_controle.LastBlockInfo;

            //位置を基準に4*4を検索して最初に空きじゃ無いところ
            for (int y = 0; y < BlockInfo.BLOCK_CELL_HEIGHT; y++)
            {
                for (int x = 0; x < BlockInfo.BLOCK_CELL_WIDTH; x++)
                {
                    //フィールドチェック
                    int chk_x = last_block.pos.X + x;
                    int chk_y = last_block.pos.Y + y;

                    int fence = (int)(BlockInfo.BlockType.MINO_FENCE | BlockInfo.BlockType.MINO_IN_FIELD);

                    //Iブロックはみ出し対策
                    if(chk_x < 0)
                    {
                        break;
                    }


                    if (ai_field[chk_y, chk_x] != 0 &&
                        ai_field[chk_y, chk_x] != fence)
                    {
                        //ブロック発見
                        height = chk_y;
                        break;
                    }

                }

                if (height != 0)
                {
                    break;
                }

            }


            return height;
        }

        //特徴量２：消えたラインの数×ミノの中で消えたブロックの数
        private int CalcEraseAndBlock(BlockControle ai_controle, FieldManage ai_field_manage)
        {
            //最後に置いたブロックの範囲で消えるライン数を調べる
            LAST_BLOCK_INFO last_block = ai_controle.LastBlockInfo; //TODO これが更新されてない

            int erase_and_block = 0;

            //消えるライン数をチェック
            int erase_line_num = ai_field_manage.CheckEraseLine();

            //最後に置いたブロックがいくつ消えたか
            int currennt_block_erase = 0;
            int current_block_type = (int)(last_block.type) + (int)(BlockInfo.BlockType.MINO_IN_FIELD);

            //位置を基準に4*4を検索して空きが無いところ
            List<int> erase_list = ai_field_manage.EraseLine;

            //消えたラインの中から最後に落としたブロックがいくつあるかを数える
            foreach (int erase_y_pos in erase_list)
            {
                //壁の所は見ない
                for (int x = 1; x < FieldManage.FIELD_WIDTH - 1; x++)
                {
                    //最後に設置したブロックの探索する
                    int type = ai_field_manage.BlockField[erase_y_pos, x] % (int)(BlockInfo.BlockType.MINO_VANISH);
                    if (type == current_block_type)
                    {
                        currennt_block_erase++;
                    }
                }
            }

            //最終的な値
            erase_and_block = erase_line_num * currennt_block_erase;
            return erase_and_block;
        }

        //特徴量３：横方向にスキャンした時にセルの内容が変化する回数
        private int CalcHorizonChange(FieldManage ai_field_manage)
        {
            int horizeon_change = 0;

            for (int y = 0; y < FieldManage.FIELD_HEIGHT; y++)
            {
                int chk_block = ai_field_manage.BlockField[y, 1];

                //壁の所は見ない
                for (int x = 1; x < FieldManage.FIELD_WIDTH - 1; x++)
                {
                    int now = ai_field_manage.BlockField[y, x];

                    //変化があったら
                    if ((chk_block == 0 && now != 0) ||
                       (chk_block != 0 && now == 0)
                    )
                    {
                        horizeon_change++;
                        chk_block = now;
                    }
                }
            }
            return horizeon_change;
        }

        //特徴量４：横方向にスキャンした時にセルの内容が変化する回数
        private int CalcVerticalChange(FieldManage ai_field_manage)
        {
            int vertical_change = 0;

            //壁の所は見ない
            for (int x = 1; x < FieldManage.FIELD_WIDTH - 1; x++)
            {
                int chk_block = ai_field_manage.BlockField[0, x];

                for (int y = 0; y < FieldManage.FIELD_HEIGHT - 1; y++)
                {
                    int now = ai_field_manage.BlockField[y, x];

                    //変化があったら
                    if ((chk_block == 0 && now != 0) ||
                       (chk_block != 0 && now == 0)
                    )
                    {
                        vertical_change++;
                        chk_block = now;
                    }
                }
            }
            return vertical_change;
        }

        /// <summary>
        /// 特徴量５　穴の数をカウント
        /// 穴・空白の上がブロックで埋まっている箇所
        /// </summary>
        /// <param name="data"></param>
        private int CalcHole(FieldManage field_manage)
        {
            int hole_num = 0;

            List<int[]> PosList = SearchHole(field_manage);

            hole_num = PosList.Count;
            return hole_num;
        }

        /// <summary>
        /// 特徴量６　井戸の高さの階和の総数
        /// </summary>
        /// <param name="ai_field_manage"></param>
        /// <returns></returns>
        private int CalcWellTotal(FieldManage ai_field_manage)
        {
            //井戸の深さ・両側との高さの差をとれば分かる

            int well_total = 0;

            List<int> HeightList = new List<int>();
            for (int x = 1; x < FieldManage.FIELD_WIDTH - 1; x++)
            {
                bool well = false;
                int height = 0;
                for (int y = 1; y < FieldManage.FIELD_HEIGHT; y++)
                {
                    int now = ai_field_manage.BlockField[y, x];

                    if (x == 1)
                    {
                        //左端
                        int chk = ai_field_manage.BlockField[y, x + 1];

                        if (!well)
                        {
                            //井戸かどうかを調べる
                            if (now == 0 && chk != 0)
                            {
                                well = true;
                            }
                            if( now != 0)
                            {
                                //ブロックが見つかったら終わり
                                break;
                            }
                        }
                        else
                        {
                            //井戸の深さを調べる
                            height++;
                            if (now != 0 || chk == 0)
                            {
                                HeightList.Add(height);
                                break;
                            }
                        }

                    }
                    else if (x == FieldManage.FIELD_HEIGHT - 1)
                    {
                        //右端
                        int chk = ai_field_manage.BlockField[y, x - 1];
                        if (!well)
                        {
                            if (now == 0 && chk != 0)
                            {
                                well = true;
                            }
                            if (now != 0)
                            {
                                //ブロックが見つかったら終わり
                                break;
                            }
                        }
                        else
                        {
                            height++;
                            if (now != 0 || chk == 0)
                            {
                                HeightList.Add(height);
                                break;
                            }
                        }
                    }
                    else
                    {
                        //右端
                        int chk1 = ai_field_manage.BlockField[y, x - 1];
                        int chk2 = ai_field_manage.BlockField[y, x + 1];
                        if (!well)
                        {
                            if (now == 0 && chk1 != 0 && chk2 != 0)
                            {
                                well = true;
                            }
                            if (now != 0)
                            {
                                //ブロックが見つかったら終わり
                                break;
                            }
                        }
                        else
                        {
                            height++;
                            if (now != 0 || chk1 == 0 || chk2 == 0)
                            {
                                HeightList.Add(height);
                                break;
                            }
                        }
                    }
                }
            }

            //階和
            foreach(int va in HeightList)
            {
                well_total += (va * (va + 1)) / 2;
            }


            return well_total;
        }

        /// <summary>
        /// 特徴量７　穴の上にあるブロックの総数
        /// </summary>
        /// <param name="ai_field_manage"></param>
        /// <returns></returns>
        private int CalcHoleOnBlock(FieldManage field_manage)
        {
            List<int[]> PosList = SearchHole(field_manage);

            int total = 0;
            //穴の上のブロックを検索
            foreach (int[] pos in PosList)
            {
                int start_x = pos[0];
                int start_y = pos[1];

                for (int y = start_y-1; y >= 0; y--)
                {
                    int chk = field_manage.BlockField[y, start_x];

                    if( chk != 0)
                    {
                        total++;
                    }
                    else
                    {
                        break;
                    }
                }

            }

            return total;
        }

        /// <summary>
        /// 特徴量８　穴のある行数
        /// </summary>
        /// <param name="field_manage"></param>
        /// <returns></returns>
        private int CalcHoleRow(FieldManage field_manage)
        {
            int total = 0;
            List<int[]>PosList = SearchHole(field_manage);

            for (int x = 1; x < FieldManage.FIELD_WIDTH - 1; x++)
            {
                foreach(int[] pos in PosList)
                {
                    //行数一致
                    if( x == pos[0])
                    {
                        total++;
                        break;
                    }
                }
            }

            return total;
        }

        /// <summary>
        /// 穴のある座標を取得
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        private List<int[]> SearchHole(FieldManage field_manage)
        {
            List<int[]> PosList = new List<int[]>();
            for (int x = 1; x < FieldManage.FIELD_WIDTH - 1; x++)
            {
                for (int y = FieldManage.FIELD_HEIGHT - 1; y >= 0; y--)
                {
                    int now = field_manage.BlockField[y, x];

                    //空き空間があれば
                    if (now == 0)
                    {
                        //空きの上部を探索してブロックがあれば穴とする
                        for (int chk_y = y; chk_y >= 0; chk_y--)
                        {
                            int chk = field_manage.BlockField[chk_y, x];
                            if (chk != 0)
                            {
                                //このとき左右が空間でないこと
                                int chk2 = field_manage.BlockField[y, x - 1];
                                int chk3 = field_manage.BlockField[y, x + 1];

                                if (chk2 != 0 && chk3 != 0)
                                {
                                    //穴の座標を記録
                                    int[] pos = { x, y };
                                    PosList.Add(pos);
                                    y = chk_y;//チェック場所を更新
                                    break;
                                }
                            }
                        }
                    }

                }
            }

            return PosList;
        }


        public LAST_BLOCK_INFO LastBlockInfo;
        public double[] EvaluateWeight;

        //ニューラルネットワーク
        private NN Nnetwork;
    }
}
