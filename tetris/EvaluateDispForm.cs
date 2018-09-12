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

        public  void SetFeatureData(FeatureData input)
        {
            this.feature = input;
        }

        public void UpdateDisp()
        {
            this.textBoxFeature1.Text = this.feature.last_block_height.ToString();
            this.textBoxFeature2.Text = this.feature.eraseline_and_block.ToString();
            this.textBoxFeature3.Text = this.feature.horizon_change.ToString();
            this.textBoxFeature4.Text = this.feature.veritical_change.ToString();
            this.textBoxFeature5.Text = this.feature.hole.ToString();
            this.textBoxFeature6.Text = this.feature.well_total.ToString();
            this.textBoxFeature7.Text = this.feature.hole_on_block_total.ToString();
            this.textBoxFeature8.Text = this.feature.hole_row.ToString();

        }

        //特徴量を受け取るため
        FeatureData feature;
    }
}
