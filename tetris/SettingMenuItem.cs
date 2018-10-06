using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
namespace tetris
{
    public partial class GameField : Form
    {
        //ここにメニューアイテムの操作を書く
        private void ChkMenuPlayMode()
        {
            if (MenuItem1Ponly.Checked)
            {
                player_select = (int)PLAY_MODE.ONLY_1P;

                if(this.MenuItem_1P_Human.Checked)
                {
                    this.PlayerAI[0] = false;
                    this.PlayerAI[1] = true;
                }
                else
                {
                    this.PlayerAI[0] = true;
                    this.PlayerAI[1] = false;
                }
            }
            else if (MenuItemVS.Checked)
            {
                player_select = (int)PLAY_MODE.VS;
                if (this.MenuItem_1P_Human.Checked)
                {
                    this.PlayerAI[0] = false;
                    this.PlayerAI[1] = true;
                }
                else
                {
                    this.PlayerAI[0] = true;
                    this.PlayerAI[1] = true;
                }

            }
            else if (MenuItemComOnly.Checked)
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

        //メニューからLearningを選択
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


        //1PメニューからHUMANを選択
        private void MenuItem_1P_Human_Click(object sender, EventArgs e)
        {
            this.MenuItem_1P_Human.Checked = true;
            this.MenuItem_1P_CPU_1Line.Checked = false;
            this.MenuItem_1P_CPU_4Line.Checked = false;
        }

        //1PメニューからCPU_１Lineを選択
        private void MenuItem_1P_CPU_1Line_Click(object sender, EventArgs e)
        {
            this.MenuItem_1P_Human.Checked = false;
            this.MenuItem_1P_CPU_1Line.Checked = true;
            this.MenuItem_1P_CPU_4Line.Checked = false;
        }

        //1PメニューからCPU_4Lineを選択
        private void MenuItem_1P_CPU_4Line_Click(object sender, EventArgs e)
        {
            this.MenuItem_1P_Human.Checked = false;
            this.MenuItem_1P_CPU_1Line.Checked = false;
            this.MenuItem_1P_CPU_4Line.Checked = true;
        }

        //2PメニューからCPU_1Lineを選択
        private void MenuItem_2P_CPU_1Line_Click(object sender, EventArgs e)
        {
            this.MenuItem_2P_CPU_1Line.Checked = true;
            this.MenuItem_2P_CPU_4Line.Checked = false;
        }

        //2PメニューからCPU_4Lineを選択
        private void MenuItem_2P_CPU_4Line_Click(object sender, EventArgs e)
        {
            this.MenuItem_2P_CPU_1Line.Checked = false;
            this.MenuItem_2P_CPU_4Line.Checked = true;
        }
    }
}
