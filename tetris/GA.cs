using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace tetris
{
    public struct GA_UNIT
    {
        public Colony colony;
        public GAManager manager;
    };

    /*
            世代数と遺伝子数の設定が必要
            使い方
            {
                Colony colon = new Colony();

                colon.Initialize(4 * 5);

                //初期の親を登録
                GenomManager manage = new GenomManager();

                manage.AddGenom(colon.RandomGetGenom());
                manage.AddGenom(colon.RandomGetGenom());

                const int GENALATION = 10;

                //世代数だけくりかえす
                for (int i = 0; i < GENALATION; i++)
                {
                    //親をつくる
                    manage.MakeParentGenom();

                    //交叉で子をつくる
                    manage.CrossExec();

                    //突然変異
                    manage.Mutation();

                    //実行

                    //結果による順位をつける
                    int[] ranking = { 0, 1, 2, 3 };


                    //判定にしたがってColnyにGenomを戻す
                    manage.SelectExec(ranking);

                }

                //100個の遺伝子を作成
                GAManager mane = new GAManager();
                mane.Init(GAManager.ALL_GENOM_NUM);

                for (int genaraion = 0; genaraion < 10; genaraion++)
                {
                    //評価
                    int[] score = new int[GAManager.ALL_GENOM_NUM];
                    for (int i = 0; i < GAManager.ALL_GENOM_NUM; i++)
                    {
                        score[i] = Common.rand.Next(0, 500);
                    }


                    //次の世代の遺伝子を作成
                    mane.GenerationUpdate(score);
                }
            }
    */

    //全探索空間　このクラスが必要かは要検討
    public class Colony
    {
        //全探索空間に持っておく遺伝子の数
        readonly int GENOM_MAX = 100;


        public void Initialize(int DnaSize)
        {
            GenomList.Clear();
            for (int i = 0; i < GENOM_MAX; i++)
            {
                Genom genom = new Genom(DnaSize);
                GenomList.Add(genom);
            }
        }

        //ランダムに登録されている遺伝子を作り出す
        public Genom RandomGetGenom()
        {

            int num = Common.MyRandom.Next(GENOM_MAX);

            if (GenomList.Count <= num)
            {
                //だめ
                Debug.Assert(false);
            }

            return GenomList[num];
        }

        //遺伝子リストを持っとく
       private List<Genom> GenomList = new List<Genom>();
    }

    //遺伝子
    public class Genom
    {
        //デフォルトコンストラクタ
        public Genom()
        {
            this.DNA = new double[this.DNA_SIZE];
            SettingDefaultDNA();
        }

        //DNAのサイズを指定して作成
        public Genom(int DNA_size)
        {
            this.DNA = new double[DNA_size];
            SettingDefaultDNA();
        }

        //遺伝子配列を指定して作成。
        public Genom(double[] input_dna)
        {
            this.DNA = (double[])input_dna.Clone();
        }

        public int GetDNASize()
        {
            return DNA.Length;
        }

        //初期値をランダムで決める
        private void SettingDefaultDNA()
        {
            for (int i = 0; i < DNA.Length; i++)
            {
                this.DNA[i] = Common.MyRandom.Next(DNA_VALUE_MIN, DNA_VALUE_MAX);
                this.DNA[i] /= 100.0;
            }
        }


        //持っているパラメータ
        public double[] DNA;

        public readonly int DNA_SIZE = 8;//デフォルト　まず使わない
        public readonly int DNA_VALUE_MAX = 100;
        public readonly int DNA_VALUE_MIN = -100;

    }

    //遺伝子操作クラス(局所空間)
    public class GenomManager
    {
        public Genom GetGenom(int no)
        {
            if( no >= 4)
            {
                Debug.Assert(false);
            }

            if(no == 0)
            {
                return ParentList[0];
            }
            if (no == 1)
            {
                return ParentList[1];
            }
            if (no == 2)
            {
                return ChildList[0];
            }
            if (no == 3)
            {
                return ChildList[1];
            }

            return null;
        }

        //順位にしたがって残す遺伝子を決める
        public void SelectExec(int[] ranking)
        {
            //ranking 親１ 親２ 子１ 子２　の順番

            String result = @""; //上位２つを表す文字列

            for (int i = 0; i < ranking.Length; i++)
            {
                if (ranking[i] == 0)
                {
                    if (i >= 2)
                    {
                        result = result.Insert(0, "C");
                    }
                    else
                    {
                        result = result.Insert(0, "P");
                    }
                }
                if (ranking[i] == 1)
                {
                    if (i >= 2)
                    {
                        result += @"C";
                    }
                    else
                    {
                        result += @"P";
                    }

                }
            }

            //親の上位を決める
            int parent_up = -1;
            if (ranking[0] < ranking[1])
            {
                parent_up = 0;
            }
            else
            {
                parent_up = 1;
            }

            //子の上位を決める
            int child_up = -1;
            if (ranking[2] < ranking[3])
            {
                child_up = 0;
            }
            else
            {
                child_up = 1;
            }

            //局所集団にもどす遺伝子をきめる
            switch (result)
            {
                case "CC": // type A 子供 2人と 親1人 コロニー ＋1
                    AddGenom(this.ChildList[0]);
                    AddGenom(this.ChildList[1]);
                    AddGenom(this.ParentList[parent_up]);
                    break;
                case "PP": // type B 親1人 コロニー -1 人口 1人なら、異邦人要求
                    AddGenom(this.ParentList[parent_up]);
                    if (this.AllGenomList.Count < 2)
                    {
                        Genom n = new Genom(DNA_size);
                        AddGenom(n);
                    }
                    break;
                case "PC": // type c 親1人 子供1人 コロニー 増減なし
                    AddGenom(this.ParentList[parent_up]);
                    AddGenom(this.ChildList[child_up]);
                    break;
                case "CP": //type D 子供 1人 異邦人要求 コロニー 増減なし
                    AddGenom(this.ChildList[child_up]);
                    Genom n2 = new Genom(DNA_size);
                    AddGenom(n2);
                    break;
            }
        }


        //局所集団リストに加える
        public void AddGenom(Genom p)
        {
            this.AllGenomList.Add(p);
        }

        //局所集団リストから親をつくる
        public void MakeParentGenom()
        {
            this.ParentList.Clear();

            int num = Common.MyRandom.Next(this.AllGenomList.Count);
            this.ParentList.Add(this.AllGenomList[num]);
            this.AllGenomList.RemoveAt(num);

            num = Common.MyRandom.Next(this.AllGenomList.Count);
            this.ParentList.Add(this.AllGenomList[num]);
            this.AllGenomList.RemoveAt(num);

        }

        //親リストに加える
        public void AddParentGenom(Genom p)
        {
            if (ParentList.Count > 2)
            {
                //だめ
                Debug.Assert(false);
            }

            ParentList.Add(p);
        }

        //親２つで交叉を行う
        public void CrossExec()
        {
            double[] p1 = ParentList[0].DNA;
            double[] p2 = ParentList[1].DNA;

            //交叉点の作成
            int[] cutPoint = MakeCutPoint(Common.MyRandom.Next(1, p1.Length), 0, p1.Length);

            //分割点の最後に配列のサイズを入れる
            Array.Resize(ref cutPoint, cutPoint.Length + 1);
            cutPoint[cutPoint.Length - 1] = p1.Length;

            //交叉実行
            double[] ch1 = new double[p1.Length];
            double[] ch2 = new double[p1.Length];
            int IX = 0, st = 0;
            foreach (int pt in cutPoint)
            {
                if (IX++ % 2 == 0)
                {
                    Array.Copy(p1, st, ch1, st, pt - st);
                    Array.Copy(p2, st, ch2, st, pt - st);
                }
                else
                {
                    Array.Copy(p1, st, ch2, st, pt - st);
                    Array.Copy(p2, st, ch1, st, pt - st);
                }
                st = pt;
            }

            //子をつくる
            Genom ch_genom1 = new Genom(ch1);
            Genom ch_genom2 = new Genom(ch2);

            //子供リストをクリア
            ChildList.Clear();

            ChildList.Add(ch_genom1);
            ChildList.Add(ch_genom2);
        }

        /// <summary>
        /// 突然変異
        /// </summary>
        public void Mutation()
        {
            //子のどちらかを突然変異させる
            int num = Common.MyRandom.Next(2);
            Genom ch = ChildList[num];

            HashSet<int> hsTable = new HashSet<int>();
            while (true)
            {
                int point = Common.MyRandom.Next(ch.DNA_SIZE);
                if (hsTable.Add(point) == false)
                {
                    break;
                }

                ch.DNA[point] = Common.MyRandom.Next(ch.DNA_VALUE_MIN, ch.DNA_VALUE_MAX);
            }
        }

        /// <summary>
        /// 交叉するときに入れ替え点をつくる
        /// </summary>
        /// <param name="cut_num">交叉点の数</param>
        /// <param name="start"></param>
        /// <param name="enc"></param>
        /// <returns></returns>
        private int[] MakeCutPoint(int cut_num, int start, int end)
        {
            SortedSet<int> ssTable = new SortedSet<int>();
            while (ssTable.Count() < cut_num)
            {
                ssTable.Add(Common.MyRandom.Next(start + 1, end));
            }
            int[] ans = new int[cut_num];
            ssTable.CopyTo(ans);
            return (ans);
        }

        //親１親２　子１子２のGenomを持つ
        List<Genom> ParentList = new List<Genom>();
        List<Genom> ChildList = new List<Genom>();

        //局所集団にあるGenom全て
        List<Genom> AllGenomList = new List<Genom>();   //この中から２つをランダムに選んで親とする


        //何世代目を実行しているか。
        public void SetGeneration(int generation)
        {
            GenerationMAX = generation;
            GenerationCount = 0;
        }

        public void SetDNASize(int size)
        {
            this.DNA_size = size;
        }

        public int GenerationMAX = 0;
        public int GenerationCount = 0;
        public int DNA_size = 0;
    }

    //ノーマルGA
    public class GAManager
    {
        public const int ALL_GENOM_NUM = 100;
        public const int RANKING_GENOM = 50;

        //遺伝子の初期設定
        public void Init(int genom_num,int genom_size)
        {
            for (int i = 0; i < genom_num; i++)
            {
                AllGenomList.Add(new Genom(genom_size));
            }
        }

        public Genom GetGenom(int no)
        {
            return AllGenomList[no];
        }
            /// <summary>
            /// 結果による世代交代処理
            /// </summary>
            /// <param name="score_array"></param>
            public void GenerationUpdate(double[] score_array)
        {
            //スコアによってランキングをつくる
            List<Tuple<double, int>> sort_list = new List<Tuple<double, int>>();
            for (int i = 0; i < ALL_GENOM_NUM; i++)
            {
                Tuple<double, int> t = new Tuple<double, int>(score_array[i], i);
                sort_list.Add(t);
            }
            sort_list.Sort();
            sort_list.Reverse();

            //上位ランキングのみを採用
            List<Genom> tmp_list = new List<Genom>();
            int count = 0;
            foreach (var data in sort_list)
            {
                int num = data.Item2;
                tmp_list.Add(AllGenomList[num]);
                count++;
                if (count >= RANKING_GENOM)
                {
                    break;
                }
            }
            while (tmp_list.Count < ALL_GENOM_NUM)
            {
                int p1_num = Common.MyRandom.Next(0, RANKING_GENOM);
                int p2_num = Common.MyRandom.Next(0, RANKING_GENOM);
                while (p2_num == p1_num)
                {
                    p2_num = Common.MyRandom.Next(0, RANKING_GENOM);
                }

                Genom c1 = new Genom();
                Genom c2 = new Genom();

                //交叉、突然変異を遺伝子が１００個になるまで繰り替えす
                CrossExec(tmp_list[p1_num], tmp_list[p2_num], ref c1, ref c2);
                Mutation(ref c1, ref c2);
                tmp_list.Add(c1);
                tmp_list.Add(c2);
            }

            //遺伝子リストを更新
            this.AllGenomList.Clear();

            this.AllGenomList = tmp_list;


        }

        //親２つで交叉を行う
        //作成した２つの子を受け取る
        private void CrossExec(Genom p1, Genom p2, ref Genom c1, ref Genom c2)
        {
            double[] p1_DNA = (double[])p1.DNA.Clone();
            double[] p2_DNA = (double[])p2.DNA.Clone();

            //交叉点の作成
            int[] cutPoint = MakeCutPoint(Common.MyRandom.Next(1, p1_DNA.Length), 0, p1_DNA.Length);

            //分割点の最後に配列のサイズを入れる
            Array.Resize(ref cutPoint, cutPoint.Length + 1);
            cutPoint[cutPoint.Length - 1] = p1_DNA.Length;

            //交叉実行
            double[] ch1 = new double[p1_DNA.Length];
            double[] ch2 = new double[p1_DNA.Length];
            int IX = 0, st = 0;
            foreach (int pt in cutPoint)
            {
                if (IX++ % 2 == 0)
                {
                    Array.Copy(p1_DNA, st, ch1, st, pt - st);
                    Array.Copy(p2_DNA, st, ch2, st, pt - st);
                }
                else
                {
                    Array.Copy(p1_DNA, st, ch2, st, pt - st);
                    Array.Copy(p2_DNA, st, ch1, st, pt - st);
                }
                st = pt;
            }

            //子をつくる
            c1.DNA = (double[])ch1.Clone();
            c2.DNA = (double[])ch2.Clone();
        }

        /// <summary>
        /// 交叉するときに入れ替え点をつくる
        /// </summary>
        /// <param name="cut_num">交叉点の数</param>
        /// <param name="start"></param>
        /// <param name="enc"></param>
        /// <returns></returns>
        private int[] MakeCutPoint(int cut_num, int start, int end)
        {
            SortedSet<int> ssTable = new SortedSet<int>();
            while (ssTable.Count() < cut_num)
            {
                ssTable.Add(Common.MyRandom.Next(start + 1, end));
            }
            int[] ans = new int[cut_num];
            ssTable.CopyTo(ans);
            return (ans);
        }

        /// <summary>
        /// 突然変異
        /// </summary>
        private void Mutation(ref Genom c1, ref Genom c2)
        {
            //子のどちらかを突然変異させる
            int num = Common.MyRandom.Next(2);
            Genom ch;

            if (num == 0)
            {
                ch = c1;
            }
            else
            {
                ch = c2;
            }

            HashSet<int> hsTable = new HashSet<int>();
            while (true)
            {
                int point = Common.MyRandom.Next(ch.DNA_SIZE);
                if (hsTable.Add(point) == false)
                {
                    break;
                }

                ch.DNA[point] = Common.MyRandom.Next(ch.DNA_VALUE_MIN, ch.DNA_VALUE_MAX);
            }
        }

        //何世代目まで実行するか
        public void SetGeneration(int generation)
        {
            GenerationMAX = generation;
            GenerationCount = 0;
        }

        List<Genom> AllGenomList = new List<Genom>();
        public int GenerationMAX = 0;
        public int GenerationCount = 0;
        public int DNA_size = 0;
    }

}


