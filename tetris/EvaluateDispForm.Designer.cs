﻿namespace tetris
{
    partial class EvaluateDispForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelFeature1 = new System.Windows.Forms.Label();
            this.labelFeature2 = new System.Windows.Forms.Label();
            this.labelFeature3 = new System.Windows.Forms.Label();
            this.labelFeature4 = new System.Windows.Forms.Label();
            this.labelFeature5 = new System.Windows.Forms.Label();
            this.labelFeature6 = new System.Windows.Forms.Label();
            this.labelFeature7 = new System.Windows.Forms.Label();
            this.labelFeature8 = new System.Windows.Forms.Label();
            this.textBox1PFeature1 = new System.Windows.Forms.TextBox();
            this.textBox1PFeature2 = new System.Windows.Forms.TextBox();
            this.textBox1PFeature3 = new System.Windows.Forms.TextBox();
            this.textBox1PFeature4 = new System.Windows.Forms.TextBox();
            this.textBox1PFeature5 = new System.Windows.Forms.TextBox();
            this.textBox1PFeature6 = new System.Windows.Forms.TextBox();
            this.textBox1PFeature7 = new System.Windows.Forms.TextBox();
            this.textBox1PFeature8 = new System.Windows.Forms.TextBox();
            this.label1P = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox2PFeature8 = new System.Windows.Forms.TextBox();
            this.textBox2PFeature7 = new System.Windows.Forms.TextBox();
            this.textBox2PFeature6 = new System.Windows.Forms.TextBox();
            this.textBox2PFeature5 = new System.Windows.Forms.TextBox();
            this.textBox2PFeature4 = new System.Windows.Forms.TextBox();
            this.textBox2PFeature3 = new System.Windows.Forms.TextBox();
            this.textBox2PFeature2 = new System.Windows.Forms.TextBox();
            this.textBox2PFeature1 = new System.Windows.Forms.TextBox();
            this.labelScore = new System.Windows.Forms.Label();
            this.textBox1PScore = new System.Windows.Forms.TextBox();
            this.textBox2PScore = new System.Windows.Forms.TextBox();
            this.labelGAScore = new System.Windows.Forms.Label();
            this.textBoxGAScore1 = new System.Windows.Forms.TextBox();
            this.textBoxGAScore2 = new System.Windows.Forms.TextBox();
            this.textBoxGAScore3 = new System.Windows.Forms.TextBox();
            this.textBoxGAScore4 = new System.Windows.Forms.TextBox();
            this.textBox2PFeature9 = new System.Windows.Forms.TextBox();
            this.textBox1PFeature9 = new System.Windows.Forms.TextBox();
            this.labelAverageHeight = new System.Windows.Forms.Label();
            this.textBox2PFeature10 = new System.Windows.Forms.TextBox();
            this.textBox1PFeature10 = new System.Windows.Forms.TextBox();
            this.labelDeviation = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelFeature1
            // 
            this.labelFeature1.AutoSize = true;
            this.labelFeature1.Font = new System.Drawing.Font("メイリオ", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelFeature1.Location = new System.Drawing.Point(23, 74);
            this.labelFeature1.Name = "labelFeature1";
            this.labelFeature1.Size = new System.Drawing.Size(221, 28);
            this.labelFeature1.TabIndex = 0;
            this.labelFeature1.Text = "直前に置いたミノの高さ";
            // 
            // labelFeature2
            // 
            this.labelFeature2.AutoSize = true;
            this.labelFeature2.Font = new System.Drawing.Font("メイリオ", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelFeature2.Location = new System.Drawing.Point(23, 115);
            this.labelFeature2.Name = "labelFeature2";
            this.labelFeature2.Size = new System.Drawing.Size(445, 28);
            this.labelFeature2.TabIndex = 1;
            this.labelFeature2.Text = "消えたラインの数×ミノの中で消えたブロックの数";
            // 
            // labelFeature3
            // 
            this.labelFeature3.AutoSize = true;
            this.labelFeature3.Font = new System.Drawing.Font("メイリオ", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelFeature3.Location = new System.Drawing.Point(23, 157);
            this.labelFeature3.Name = "labelFeature3";
            this.labelFeature3.Size = new System.Drawing.Size(468, 28);
            this.labelFeature3.TabIndex = 2;
            this.labelFeature3.Text = "横方向にスキャンした時にセルの内容が変化する回数";
            // 
            // labelFeature4
            // 
            this.labelFeature4.AutoSize = true;
            this.labelFeature4.Font = new System.Drawing.Font("メイリオ", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelFeature4.Location = new System.Drawing.Point(23, 199);
            this.labelFeature4.Name = "labelFeature4";
            this.labelFeature4.Size = new System.Drawing.Size(468, 28);
            this.labelFeature4.TabIndex = 3;
            this.labelFeature4.Text = "縦方向にスキャンした時にセルの内容が変化する回数";
            // 
            // labelFeature5
            // 
            this.labelFeature5.AutoSize = true;
            this.labelFeature5.Font = new System.Drawing.Font("メイリオ", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelFeature5.Location = new System.Drawing.Point(23, 241);
            this.labelFeature5.Name = "labelFeature5";
            this.labelFeature5.Size = new System.Drawing.Size(69, 28);
            this.labelFeature5.TabIndex = 4;
            this.labelFeature5.Text = "穴の数";
            // 
            // labelFeature6
            // 
            this.labelFeature6.AutoSize = true;
            this.labelFeature6.Font = new System.Drawing.Font("メイリオ", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelFeature6.Location = new System.Drawing.Point(23, 283);
            this.labelFeature6.Name = "labelFeature6";
            this.labelFeature6.Size = new System.Drawing.Size(202, 28);
            this.labelFeature6.TabIndex = 5;
            this.labelFeature6.Text = "井戸の高さの階和の和";
            // 
            // labelFeature7
            // 
            this.labelFeature7.AutoSize = true;
            this.labelFeature7.Font = new System.Drawing.Font("メイリオ", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelFeature7.Location = new System.Drawing.Point(23, 325);
            this.labelFeature7.Name = "labelFeature7";
            this.labelFeature7.Size = new System.Drawing.Size(240, 28);
            this.labelFeature7.TabIndex = 6;
            this.labelFeature7.Text = "穴の上のブロックの数の和";
            // 
            // labelFeature8
            // 
            this.labelFeature8.AutoSize = true;
            this.labelFeature8.Font = new System.Drawing.Font("メイリオ", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelFeature8.Location = new System.Drawing.Point(23, 367);
            this.labelFeature8.Name = "labelFeature8";
            this.labelFeature8.Size = new System.Drawing.Size(126, 28);
            this.labelFeature8.TabIndex = 7;
            this.labelFeature8.Text = "穴のある行数";
            // 
            // textBox1PFeature1
            // 
            this.textBox1PFeature1.Font = new System.Drawing.Font("メイリオ", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBox1PFeature1.Location = new System.Drawing.Point(575, 74);
            this.textBox1PFeature1.Name = "textBox1PFeature1";
            this.textBox1PFeature1.Size = new System.Drawing.Size(100, 36);
            this.textBox1PFeature1.TabIndex = 8;
            // 
            // textBox1PFeature2
            // 
            this.textBox1PFeature2.Font = new System.Drawing.Font("メイリオ", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBox1PFeature2.Location = new System.Drawing.Point(575, 115);
            this.textBox1PFeature2.Name = "textBox1PFeature2";
            this.textBox1PFeature2.Size = new System.Drawing.Size(100, 36);
            this.textBox1PFeature2.TabIndex = 9;
            // 
            // textBox1PFeature3
            // 
            this.textBox1PFeature3.Font = new System.Drawing.Font("メイリオ", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBox1PFeature3.Location = new System.Drawing.Point(575, 157);
            this.textBox1PFeature3.Name = "textBox1PFeature3";
            this.textBox1PFeature3.Size = new System.Drawing.Size(100, 36);
            this.textBox1PFeature3.TabIndex = 10;
            // 
            // textBox1PFeature4
            // 
            this.textBox1PFeature4.Font = new System.Drawing.Font("メイリオ", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBox1PFeature4.Location = new System.Drawing.Point(575, 199);
            this.textBox1PFeature4.Name = "textBox1PFeature4";
            this.textBox1PFeature4.Size = new System.Drawing.Size(100, 36);
            this.textBox1PFeature4.TabIndex = 11;
            // 
            // textBox1PFeature5
            // 
            this.textBox1PFeature5.Font = new System.Drawing.Font("メイリオ", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBox1PFeature5.Location = new System.Drawing.Point(575, 241);
            this.textBox1PFeature5.Name = "textBox1PFeature5";
            this.textBox1PFeature5.Size = new System.Drawing.Size(100, 36);
            this.textBox1PFeature5.TabIndex = 12;
            // 
            // textBox1PFeature6
            // 
            this.textBox1PFeature6.Font = new System.Drawing.Font("メイリオ", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBox1PFeature6.Location = new System.Drawing.Point(575, 283);
            this.textBox1PFeature6.Name = "textBox1PFeature6";
            this.textBox1PFeature6.Size = new System.Drawing.Size(100, 36);
            this.textBox1PFeature6.TabIndex = 13;
            // 
            // textBox1PFeature7
            // 
            this.textBox1PFeature7.Font = new System.Drawing.Font("メイリオ", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBox1PFeature7.Location = new System.Drawing.Point(575, 325);
            this.textBox1PFeature7.Name = "textBox1PFeature7";
            this.textBox1PFeature7.Size = new System.Drawing.Size(100, 36);
            this.textBox1PFeature7.TabIndex = 14;
            // 
            // textBox1PFeature8
            // 
            this.textBox1PFeature8.Font = new System.Drawing.Font("メイリオ", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBox1PFeature8.Location = new System.Drawing.Point(575, 367);
            this.textBox1PFeature8.Name = "textBox1PFeature8";
            this.textBox1PFeature8.Size = new System.Drawing.Size(100, 36);
            this.textBox1PFeature8.TabIndex = 15;
            // 
            // label1P
            // 
            this.label1P.AutoSize = true;
            this.label1P.Font = new System.Drawing.Font("メイリオ", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1P.Location = new System.Drawing.Point(570, 18);
            this.label1P.Name = "label1P";
            this.label1P.Size = new System.Drawing.Size(35, 28);
            this.label1P.TabIndex = 16;
            this.label1P.Text = "1P";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("メイリオ", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(720, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 28);
            this.label1.TabIndex = 17;
            this.label1.Text = "2P";
            // 
            // textBox2PFeature8
            // 
            this.textBox2PFeature8.Font = new System.Drawing.Font("メイリオ", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBox2PFeature8.Location = new System.Drawing.Point(725, 367);
            this.textBox2PFeature8.Name = "textBox2PFeature8";
            this.textBox2PFeature8.Size = new System.Drawing.Size(100, 36);
            this.textBox2PFeature8.TabIndex = 25;
            // 
            // textBox2PFeature7
            // 
            this.textBox2PFeature7.Font = new System.Drawing.Font("メイリオ", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBox2PFeature7.Location = new System.Drawing.Point(725, 325);
            this.textBox2PFeature7.Name = "textBox2PFeature7";
            this.textBox2PFeature7.Size = new System.Drawing.Size(100, 36);
            this.textBox2PFeature7.TabIndex = 24;
            // 
            // textBox2PFeature6
            // 
            this.textBox2PFeature6.Font = new System.Drawing.Font("メイリオ", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBox2PFeature6.Location = new System.Drawing.Point(725, 283);
            this.textBox2PFeature6.Name = "textBox2PFeature6";
            this.textBox2PFeature6.Size = new System.Drawing.Size(100, 36);
            this.textBox2PFeature6.TabIndex = 23;
            // 
            // textBox2PFeature5
            // 
            this.textBox2PFeature5.Font = new System.Drawing.Font("メイリオ", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBox2PFeature5.Location = new System.Drawing.Point(725, 241);
            this.textBox2PFeature5.Name = "textBox2PFeature5";
            this.textBox2PFeature5.Size = new System.Drawing.Size(100, 36);
            this.textBox2PFeature5.TabIndex = 22;
            // 
            // textBox2PFeature4
            // 
            this.textBox2PFeature4.Font = new System.Drawing.Font("メイリオ", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBox2PFeature4.Location = new System.Drawing.Point(725, 200);
            this.textBox2PFeature4.Name = "textBox2PFeature4";
            this.textBox2PFeature4.Size = new System.Drawing.Size(100, 36);
            this.textBox2PFeature4.TabIndex = 21;
            // 
            // textBox2PFeature3
            // 
            this.textBox2PFeature3.Font = new System.Drawing.Font("メイリオ", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBox2PFeature3.Location = new System.Drawing.Point(725, 158);
            this.textBox2PFeature3.Name = "textBox2PFeature3";
            this.textBox2PFeature3.Size = new System.Drawing.Size(100, 36);
            this.textBox2PFeature3.TabIndex = 20;
            // 
            // textBox2PFeature2
            // 
            this.textBox2PFeature2.Font = new System.Drawing.Font("メイリオ", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBox2PFeature2.Location = new System.Drawing.Point(725, 116);
            this.textBox2PFeature2.Name = "textBox2PFeature2";
            this.textBox2PFeature2.Size = new System.Drawing.Size(100, 36);
            this.textBox2PFeature2.TabIndex = 19;
            // 
            // textBox2PFeature1
            // 
            this.textBox2PFeature1.Font = new System.Drawing.Font("メイリオ", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBox2PFeature1.Location = new System.Drawing.Point(725, 74);
            this.textBox2PFeature1.Name = "textBox2PFeature1";
            this.textBox2PFeature1.Size = new System.Drawing.Size(100, 36);
            this.textBox2PFeature1.TabIndex = 18;
            // 
            // labelScore
            // 
            this.labelScore.AutoSize = true;
            this.labelScore.Font = new System.Drawing.Font("メイリオ", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelScore.Location = new System.Drawing.Point(23, 503);
            this.labelScore.Name = "labelScore";
            this.labelScore.Size = new System.Drawing.Size(69, 28);
            this.labelScore.TabIndex = 26;
            this.labelScore.Text = "スコア";
            // 
            // textBox1PScore
            // 
            this.textBox1PScore.Font = new System.Drawing.Font("メイリオ", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBox1PScore.Location = new System.Drawing.Point(575, 503);
            this.textBox1PScore.Name = "textBox1PScore";
            this.textBox1PScore.Size = new System.Drawing.Size(100, 36);
            this.textBox1PScore.TabIndex = 27;
            // 
            // textBox2PScore
            // 
            this.textBox2PScore.Font = new System.Drawing.Font("メイリオ", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBox2PScore.Location = new System.Drawing.Point(725, 503);
            this.textBox2PScore.Name = "textBox2PScore";
            this.textBox2PScore.Size = new System.Drawing.Size(100, 36);
            this.textBox2PScore.TabIndex = 28;
            // 
            // labelGAScore
            // 
            this.labelGAScore.AutoSize = true;
            this.labelGAScore.Font = new System.Drawing.Font("メイリオ", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelGAScore.Location = new System.Drawing.Point(33, 550);
            this.labelGAScore.Name = "labelGAScore";
            this.labelGAScore.Size = new System.Drawing.Size(134, 28);
            this.labelGAScore.TabIndex = 29;
            this.labelGAScore.Text = "GA評価スコア";
            // 
            // textBoxGAScore1
            // 
            this.textBoxGAScore1.Font = new System.Drawing.Font("メイリオ", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBoxGAScore1.Location = new System.Drawing.Point(28, 587);
            this.textBoxGAScore1.Name = "textBoxGAScore1";
            this.textBoxGAScore1.Size = new System.Drawing.Size(100, 36);
            this.textBoxGAScore1.TabIndex = 30;
            // 
            // textBoxGAScore2
            // 
            this.textBoxGAScore2.Font = new System.Drawing.Font("メイリオ", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBoxGAScore2.Location = new System.Drawing.Point(163, 587);
            this.textBoxGAScore2.Name = "textBoxGAScore2";
            this.textBoxGAScore2.Size = new System.Drawing.Size(100, 36);
            this.textBoxGAScore2.TabIndex = 31;
            // 
            // textBoxGAScore3
            // 
            this.textBoxGAScore3.Font = new System.Drawing.Font("メイリオ", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBoxGAScore3.Location = new System.Drawing.Point(308, 587);
            this.textBoxGAScore3.Name = "textBoxGAScore3";
            this.textBoxGAScore3.Size = new System.Drawing.Size(100, 36);
            this.textBoxGAScore3.TabIndex = 32;
            // 
            // textBoxGAScore4
            // 
            this.textBoxGAScore4.Font = new System.Drawing.Font("メイリオ", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBoxGAScore4.Location = new System.Drawing.Point(445, 587);
            this.textBoxGAScore4.Name = "textBoxGAScore4";
            this.textBoxGAScore4.Size = new System.Drawing.Size(100, 36);
            this.textBoxGAScore4.TabIndex = 33;
            // 
            // textBox2PFeature9
            // 
            this.textBox2PFeature9.Font = new System.Drawing.Font("メイリオ", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBox2PFeature9.Location = new System.Drawing.Point(725, 409);
            this.textBox2PFeature9.Name = "textBox2PFeature9";
            this.textBox2PFeature9.Size = new System.Drawing.Size(100, 36);
            this.textBox2PFeature9.TabIndex = 36;
            // 
            // textBox1PFeature9
            // 
            this.textBox1PFeature9.Font = new System.Drawing.Font("メイリオ", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBox1PFeature9.Location = new System.Drawing.Point(575, 409);
            this.textBox1PFeature9.Name = "textBox1PFeature9";
            this.textBox1PFeature9.Size = new System.Drawing.Size(100, 36);
            this.textBox1PFeature9.TabIndex = 35;
            // 
            // labelAverageHeight
            // 
            this.labelAverageHeight.AutoSize = true;
            this.labelAverageHeight.Font = new System.Drawing.Font("メイリオ", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelAverageHeight.Location = new System.Drawing.Point(23, 412);
            this.labelAverageHeight.Name = "labelAverageHeight";
            this.labelAverageHeight.Size = new System.Drawing.Size(107, 28);
            this.labelAverageHeight.TabIndex = 34;
            this.labelAverageHeight.Text = "高さの平均";
            // 
            // textBox2PFeature10
            // 
            this.textBox2PFeature10.Font = new System.Drawing.Font("メイリオ", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBox2PFeature10.Location = new System.Drawing.Point(725, 449);
            this.textBox2PFeature10.Name = "textBox2PFeature10";
            this.textBox2PFeature10.Size = new System.Drawing.Size(100, 36);
            this.textBox2PFeature10.TabIndex = 39;
            // 
            // textBox1PFeature10
            // 
            this.textBox1PFeature10.Font = new System.Drawing.Font("メイリオ", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBox1PFeature10.Location = new System.Drawing.Point(575, 449);
            this.textBox1PFeature10.Name = "textBox1PFeature10";
            this.textBox1PFeature10.Size = new System.Drawing.Size(100, 36);
            this.textBox1PFeature10.TabIndex = 38;
            // 
            // labelDeviation
            // 
            this.labelDeviation.AutoSize = true;
            this.labelDeviation.Font = new System.Drawing.Font("メイリオ", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelDeviation.Location = new System.Drawing.Point(23, 452);
            this.labelDeviation.Name = "labelDeviation";
            this.labelDeviation.Size = new System.Drawing.Size(145, 28);
            this.labelDeviation.TabIndex = 37;
            this.labelDeviation.Text = "高さの標準偏差";
            // 
            // EvaluateDispForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(964, 635);
            this.Controls.Add(this.textBox2PFeature10);
            this.Controls.Add(this.textBox1PFeature10);
            this.Controls.Add(this.labelDeviation);
            this.Controls.Add(this.textBox2PFeature9);
            this.Controls.Add(this.textBox1PFeature9);
            this.Controls.Add(this.labelAverageHeight);
            this.Controls.Add(this.textBoxGAScore4);
            this.Controls.Add(this.textBoxGAScore3);
            this.Controls.Add(this.textBoxGAScore2);
            this.Controls.Add(this.textBoxGAScore1);
            this.Controls.Add(this.labelGAScore);
            this.Controls.Add(this.textBox2PScore);
            this.Controls.Add(this.textBox1PScore);
            this.Controls.Add(this.labelScore);
            this.Controls.Add(this.textBox2PFeature8);
            this.Controls.Add(this.textBox2PFeature7);
            this.Controls.Add(this.textBox2PFeature6);
            this.Controls.Add(this.textBox2PFeature5);
            this.Controls.Add(this.textBox2PFeature4);
            this.Controls.Add(this.textBox2PFeature3);
            this.Controls.Add(this.textBox2PFeature2);
            this.Controls.Add(this.textBox2PFeature1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label1P);
            this.Controls.Add(this.textBox1PFeature8);
            this.Controls.Add(this.textBox1PFeature7);
            this.Controls.Add(this.textBox1PFeature6);
            this.Controls.Add(this.textBox1PFeature5);
            this.Controls.Add(this.textBox1PFeature4);
            this.Controls.Add(this.textBox1PFeature3);
            this.Controls.Add(this.textBox1PFeature2);
            this.Controls.Add(this.textBox1PFeature1);
            this.Controls.Add(this.labelFeature8);
            this.Controls.Add(this.labelFeature7);
            this.Controls.Add(this.labelFeature6);
            this.Controls.Add(this.labelFeature5);
            this.Controls.Add(this.labelFeature4);
            this.Controls.Add(this.labelFeature3);
            this.Controls.Add(this.labelFeature2);
            this.Controls.Add(this.labelFeature1);
            this.Name = "EvaluateDispForm";
            this.Text = "EvaluateDispForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.EvaluateDispForm_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelFeature1;
        private System.Windows.Forms.Label labelFeature2;
        private System.Windows.Forms.Label labelFeature3;
        private System.Windows.Forms.Label labelFeature4;
        private System.Windows.Forms.Label labelFeature5;
        private System.Windows.Forms.Label labelFeature6;
        private System.Windows.Forms.Label labelFeature7;
        private System.Windows.Forms.Label labelFeature8;
        private System.Windows.Forms.TextBox textBox1PFeature1;
        private System.Windows.Forms.TextBox textBox1PFeature2;
        private System.Windows.Forms.TextBox textBox1PFeature3;
        private System.Windows.Forms.TextBox textBox1PFeature4;
        private System.Windows.Forms.TextBox textBox1PFeature5;
        private System.Windows.Forms.TextBox textBox1PFeature6;
        private System.Windows.Forms.TextBox textBox1PFeature7;
        private System.Windows.Forms.TextBox textBox1PFeature8;
        private System.Windows.Forms.Label label1P;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox2PFeature8;
        private System.Windows.Forms.TextBox textBox2PFeature7;
        private System.Windows.Forms.TextBox textBox2PFeature6;
        private System.Windows.Forms.TextBox textBox2PFeature5;
        private System.Windows.Forms.TextBox textBox2PFeature4;
        private System.Windows.Forms.TextBox textBox2PFeature3;
        private System.Windows.Forms.TextBox textBox2PFeature2;
        private System.Windows.Forms.TextBox textBox2PFeature1;
        private System.Windows.Forms.Label labelScore;
        private System.Windows.Forms.TextBox textBox1PScore;
        private System.Windows.Forms.TextBox textBox2PScore;
        private System.Windows.Forms.Label labelGAScore;
        private System.Windows.Forms.TextBox textBoxGAScore1;
        private System.Windows.Forms.TextBox textBoxGAScore2;
        private System.Windows.Forms.TextBox textBoxGAScore3;
        private System.Windows.Forms.TextBox textBoxGAScore4;
        private System.Windows.Forms.TextBox textBox2PFeature9;
        private System.Windows.Forms.TextBox textBox1PFeature9;
        private System.Windows.Forms.Label labelAverageHeight;
        private System.Windows.Forms.TextBox textBox2PFeature10;
        private System.Windows.Forms.TextBox textBox1PFeature10;
        private System.Windows.Forms.Label labelDeviation;
    }
}