using System;
using System.Drawing;
using System.Collections.Generic;

namespace tetris
{
    //  フィールドの評価を行うクラス
    class EvaluateManage
    {
        //最後に操作したブロック
        public struct LAST_BLOCK_INFO
        {
            public Point pos;
            public BlockInfo.BlockRot rot;
            public BlockInfo.BlockType type;
        };

        /// <summary>
        /// 評価のための入力
        /// </summary>
        public struct EvaluateInputData
        {
            public int[,] field;       //フィールドのデータ
            public int[] nextBlock;   //NEXTブロック
            public LAST_BLOCK_INFO last_info;
        };

        //フィールドの特徴量
        public struct FeatureData
        {
            int last_block_height;      //１．直前に置いたミノの高さ
            int eraseline_and_block;    //２．消えたラインの数×ミノの中で消えたブロックの数
            int horizon_change;         //３．横方向にスキャンした時にセルの内容が変化する回数
            int veritical_change;       //４．縦方向にスキャンした時にセルの内容が変化する回数
            int hole;                   //５．穴の数
            int well_total;             //６．井戸の高さの階和(例４の階和4+3+2+1)の和
            int hole_on_block_total;    //７．穴の上のブロックの数の和
            int hole_row;               //８．穴のある行数
            //９．各列の高さの平均値
            //１０．各列の高さの標準偏差
        };


        /// <summary>
        //フィールドを評価して点数をつける関数
        /// </summary>
        /// <param name="field">フィールド配列</param>
        /// <param name="nextBlock">NEXTブロック情報</param>
        /// <param name="block_ctrl">Nブロック操作</param>
        /// <returns></returns>
        public int EvaluateField(int[,] field, NextBlockManage nextManage,BlockControle block_ctrl)
        {
            int score = 0;
            

            //ここでは全て擬似的にフィールドの操作を行う




            //擬似的に次のブロックを取り出す

            List<int> tmp = new List<int>(nextManage.NextBlock);
            AINextBlockManage.NextBlock = tmp;

            AINextBlockManage.UpdateNextBlock();
            int type = AINextBlockManage.GetNextBlock();

//            block_ctrl.SetCurrentBlock((BlockInfo.BlockType)type);


            //置く場所を決める

            //仮に置いた時のフィールドを評価する

            //フィールドから特徴量を作る

            //計算した特徴量からフィールドのスコアを求める


            return score;
            
        }

        //特徴量を計算する
        private FeatureData CalcFeature(EvaluateInputData input_data)
        {
            FeatureData feature_data = new FeatureData();

            return feature_data;
        }

        /// <summary>
        /// 特徴量５　穴の数をカウント
        /// 穴・・四方を囲まれているセル
        /// </summary>
        /// <param name="data"></param>
        private void CalcHole(int[,] field,ref FeatureData data)
        {

        }

        BlockInfo block_info = new BlockInfo(); //先読み用

        //AI用に擬似的にフィールドを操作できるようにコピー先を用意する
        NextBlockManage AINextBlockManage = new NextBlockManage();
    }
}
