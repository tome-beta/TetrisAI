using System;
using System.Diagnostics;

namespace tetris
{
/*
    class Program
    {
        static void Main(string[] args)
        {
            CoreBass core = new CoreBass();
            core.Init(8, 5, 1);
            core.SettinWeight();

            for (int i = 0; i < core.InputData.Length; i++)
            {
                core.InputData[i] = i;

            }


            core.ForwardPropagation();
        }
*/
    public class NN
    {
        public const int INPUT_CORE = 8;    //入力層の数
        public const int HIDDIN_CORE = 5;   //中間層の数
        public const int OUTPUT_CORE = 1;   //出力層の数



        //入力層、隠れ層、出力層の設定
        public void Init(int InputLayer, int HiddenLayer, int Outputlayer)
        {
            //ニューロンの数設定
            this.InputLayerNum = InputLayer;
            this.HiddenLayerNum = HiddenLayer;
            this.OutputlayerNum = Outputlayer;

            //重み
            this.WeightInputToHidden = new double[this.HiddenLayerNum, this.InputLayerNum];
            this.WeightHiddenToOutput = new double[this.OutputlayerNum, this.HiddenLayerNum];

            //バイアス
            this.BiasHidden = new double[this.HiddenLayerNum];
            this.BiasOut = new double[this.OutputlayerNum];

            this.InputData = new double[this.InputLayerNum];

            this.ToHiddenValue = new double[this.HiddenLayerNum];
            this.ToOutputValue = new double[this.OutputlayerNum];
        }


        //重みの設定
        public void SettinWeight()
        {
            // 入力層->隠れ層の重みを乱数で初期化
            for (int i = 0; i < this.HiddenLayerNum; i++)
            {
                for (int j = 0; j < this.InputLayerNum; j++)
                {
                    this.WeightInputToHidden[i, j] = Math.Sign(Common.MyRandom.NextDouble() - 0.5) * Common.MyRandom.NextDouble();
                }
                this.BiasHidden[i] = Math.Sign(Common.MyRandom.NextDouble() - 0.5) * Common.MyRandom.NextDouble();
            }

            // 隠れ層→出力層の重みを乱数で初期化
            for (int i = 0; i < this.OutputlayerNum; i++)
            {
                for (int j = 0; j < this.HiddenLayerNum; j++)
                {
                    this.WeightHiddenToOutput[i, j] = Math.Sign(Common.MyRandom.NextDouble() - 0.5) * Common.MyRandom.NextDouble();
                }
                this.BiasOut[i] = Math.Sign(Common.MyRandom.NextDouble() - 0.5) * Common.MyRandom.NextDouble();
            }
        }

        /// <summary>
        /// 重みを入力するとき
        /// </summary>
        /// <param name="weight_array"></param>
        public void SettingWeight(double[] weight_array)
        {
            //8*5+5になるはず
            if(weight_array.Length != NN.INPUT_CORE*NN.HIDDIN_CORE + NN.HIDDIN_CORE * NN.OUTPUT_CORE)
            {
                Debug.Assert(false);
            }

            int count = 0;

            // 入力層->隠れ層の重みを乱数で初期化
            for (int i = 0; i < this.HiddenLayerNum; i++)
            {
                for (int j = 0; j < this.InputLayerNum; j++)
                {
                    this.WeightInputToHidden[i, j] = weight_array[count];
                    count++;
                }
                this.BiasHidden[i] = 0.01;
            }

            // 隠れ層→出力層の重みを乱数で初期化
            for (int i = 0; i < this.OutputlayerNum; i++)
            {
                for (int j = 0; j < this.HiddenLayerNum; j++)
                {
                    this.WeightHiddenToOutput[i, j] = weight_array[count];
                    count++;
                }
                this.BiasOut[i] = 0.01;
            }

        }


        /// <summary>
        /// 入力層->隠れ層の信号伝播
        /// </summary>
        /// <param name="dataIndex">訓練データのインデックス</param>
        public void ForwardPropagation()
        {
            double sum = 0.0;

            // 入力層->隠れ層への信号伝播
            for (int i = 0; i < this.HiddenLayerNum; i++)
            {
                sum = 0.0;
                for (int j = 0; j < this.InputLayerNum; j++)
                {
                    // 重みと入力層j番目のユニットの出力値をかけて足し合わせる
                    sum += this.WeightInputToHidden[i, j] * InputData[j];
                }
                // バイアスを加えたsumを伝達関数に与えたものが隠れ層i番目のユニットの出力
                this.ToHiddenValue[i] = sigmoid(sum + this.BiasHidden[i]);
            }

            // 隠れ層->出力層への信号伝播
            for (int i = 0; i < this.OutputlayerNum; i++)
            {
                sum = 0.0;
                for (int j = 0; j < this.HiddenLayerNum; j++)
                {
                    sum += this.WeightHiddenToOutput[i, j] * this.ToHiddenValue[j];
                }
                this.ToOutputValue[i] = sigmoid(sum + this.BiasOut[i]);
            }
        }


        private double sigmoid(double x)
        {
            return 1.0 / (1.0 + Math.Exp(-x));
        }

        public double tanh(double x)
        {
            return Math.Tanh(x);
        }

        //最終的な計算結果を取得
        public double GetOutput()
        {
            return ToOutputValue[0];
        }
        //重み
        private double[,] WeightInputToHidden;  //入力層から隠れ層
        private double[,] WeightHiddenToOutput; //隠れ層から出力層

        //バイアス
        private double[] BiasHidden;            //隠れ層に追加するバイアス
        private double[] BiasOut;               //出力層に追加するバイアス

        //伝搬
        private double[] ToHiddenValue;         //隠れ層に送られる結果
        private double[] ToOutputValue;         //出力層に送られる結果

        //
        public double[] InputData;             //入力データ

        //ニューロン数
        private int InputLayerNum;
        private int HiddenLayerNum;
        private int OutputlayerNum;

    }

    //入力層、中間層、出力層

}
