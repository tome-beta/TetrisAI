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
    //盤面評価の特徴量を表示するウインドウ
    public partial class EvaluateDispForm : Form
    {
        public EvaluateDispForm()
        {
            InitializeComponent();
        }

        public  void SetFeatureData(FeatureData input,int player)
        {
            if( player == 0)
            {
                this.feature1P = input;
            }
            else
            {
                this.feature2P = input;
            }
        }

        public void UpdateDisp()
        {
            //1P側の表示
            this.textBox1PFeature1.Text = this.feature1P.last_block_height.ToString();
            this.textBox1PFeature2.Text = this.feature1P.eraseline_and_block.ToString();
            this.textBox1PFeature3.Text = this.feature1P.horizon_change.ToString();
            this.textBox1PFeature4.Text = this.feature1P.veritical_change.ToString();
            this.textBox1PFeature5.Text = this.feature1P.hole.ToString();
            this.textBox1PFeature6.Text = this.feature1P.well_total.ToString();
            this.textBox1PFeature7.Text = this.feature1P.hole_on_block_total.ToString();
            this.textBox1PFeature8.Text = this.feature1P.hole_row.ToString();

            //2P側の表示
            this.textBox2PFeature1.Text = this.feature2P.last_block_height.ToString();
            this.textBox2PFeature2.Text = this.feature2P.eraseline_and_block.ToString();
            this.textBox2PFeature3.Text = this.feature2P.horizon_change.ToString();
            this.textBox2PFeature4.Text = this.feature2P.veritical_change.ToString();
            this.textBox2PFeature5.Text = this.feature2P.hole.ToString();
            this.textBox2PFeature6.Text = this.feature2P.well_total.ToString();
            this.textBox2PFeature7.Text = this.feature2P.hole_on_block_total.ToString();
            this.textBox2PFeature8.Text = this.feature2P.hole_row.ToString();

        }

        //特徴量を受け取るため
        FeatureData feature1P;
        FeatureData feature2P;
    }
}
