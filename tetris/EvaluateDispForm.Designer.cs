namespace tetris
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
            this.textBoxFeature1 = new System.Windows.Forms.TextBox();
            this.textBoxFeature2 = new System.Windows.Forms.TextBox();
            this.textBoxFeature3 = new System.Windows.Forms.TextBox();
            this.textBoxFeature4 = new System.Windows.Forms.TextBox();
            this.textBoxFeature5 = new System.Windows.Forms.TextBox();
            this.textBoxFeature6 = new System.Windows.Forms.TextBox();
            this.textBoxFeature7 = new System.Windows.Forms.TextBox();
            this.textBoxFeature8 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // labelFeature1
            // 
            this.labelFeature1.AutoSize = true;
            this.labelFeature1.Font = new System.Drawing.Font("メイリオ", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelFeature1.Location = new System.Drawing.Point(26, 20);
            this.labelFeature1.Name = "labelFeature1";
            this.labelFeature1.Size = new System.Drawing.Size(221, 28);
            this.labelFeature1.TabIndex = 0;
            this.labelFeature1.Text = "直前に置いたミノの高さ";
            // 
            // labelFeature2
            // 
            this.labelFeature2.AutoSize = true;
            this.labelFeature2.Font = new System.Drawing.Font("メイリオ", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelFeature2.Location = new System.Drawing.Point(26, 61);
            this.labelFeature2.Name = "labelFeature2";
            this.labelFeature2.Size = new System.Drawing.Size(445, 28);
            this.labelFeature2.TabIndex = 1;
            this.labelFeature2.Text = "消えたラインの数×ミノの中で消えたブロックの数";
            // 
            // labelFeature3
            // 
            this.labelFeature3.AutoSize = true;
            this.labelFeature3.Font = new System.Drawing.Font("メイリオ", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelFeature3.Location = new System.Drawing.Point(26, 103);
            this.labelFeature3.Name = "labelFeature3";
            this.labelFeature3.Size = new System.Drawing.Size(468, 28);
            this.labelFeature3.TabIndex = 2;
            this.labelFeature3.Text = "横方向にスキャンした時にセルの内容が変化する回数";
            // 
            // labelFeature4
            // 
            this.labelFeature4.AutoSize = true;
            this.labelFeature4.Font = new System.Drawing.Font("メイリオ", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelFeature4.Location = new System.Drawing.Point(26, 145);
            this.labelFeature4.Name = "labelFeature4";
            this.labelFeature4.Size = new System.Drawing.Size(468, 28);
            this.labelFeature4.TabIndex = 3;
            this.labelFeature4.Text = "縦方向にスキャンした時にセルの内容が変化する回数";
            // 
            // labelFeature5
            // 
            this.labelFeature5.AutoSize = true;
            this.labelFeature5.Font = new System.Drawing.Font("メイリオ", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelFeature5.Location = new System.Drawing.Point(26, 187);
            this.labelFeature5.Name = "labelFeature5";
            this.labelFeature5.Size = new System.Drawing.Size(69, 28);
            this.labelFeature5.TabIndex = 4;
            this.labelFeature5.Text = "穴の数";
            // 
            // labelFeature6
            // 
            this.labelFeature6.AutoSize = true;
            this.labelFeature6.Font = new System.Drawing.Font("メイリオ", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelFeature6.Location = new System.Drawing.Point(26, 229);
            this.labelFeature6.Name = "labelFeature6";
            this.labelFeature6.Size = new System.Drawing.Size(202, 28);
            this.labelFeature6.TabIndex = 5;
            this.labelFeature6.Text = "井戸の高さの階和の和";
            // 
            // labelFeature7
            // 
            this.labelFeature7.AutoSize = true;
            this.labelFeature7.Font = new System.Drawing.Font("メイリオ", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelFeature7.Location = new System.Drawing.Point(26, 271);
            this.labelFeature7.Name = "labelFeature7";
            this.labelFeature7.Size = new System.Drawing.Size(240, 28);
            this.labelFeature7.TabIndex = 6;
            this.labelFeature7.Text = "穴の上のブロックの数の和";
            // 
            // labelFeature8
            // 
            this.labelFeature8.AutoSize = true;
            this.labelFeature8.Font = new System.Drawing.Font("メイリオ", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelFeature8.Location = new System.Drawing.Point(26, 313);
            this.labelFeature8.Name = "labelFeature8";
            this.labelFeature8.Size = new System.Drawing.Size(126, 28);
            this.labelFeature8.TabIndex = 7;
            this.labelFeature8.Text = "穴のある行数";
            // 
            // textBoxFeature1
            // 
            this.textBoxFeature1.Font = new System.Drawing.Font("メイリオ", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBoxFeature1.Location = new System.Drawing.Point(578, 20);
            this.textBoxFeature1.Name = "textBoxFeature1";
            this.textBoxFeature1.Size = new System.Drawing.Size(100, 36);
            this.textBoxFeature1.TabIndex = 8;
            // 
            // textBoxFeature2
            // 
            this.textBoxFeature2.Font = new System.Drawing.Font("メイリオ", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBoxFeature2.Location = new System.Drawing.Point(578, 61);
            this.textBoxFeature2.Name = "textBoxFeature2";
            this.textBoxFeature2.Size = new System.Drawing.Size(100, 36);
            this.textBoxFeature2.TabIndex = 9;
            // 
            // textBoxFeature3
            // 
            this.textBoxFeature3.Font = new System.Drawing.Font("メイリオ", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBoxFeature3.Location = new System.Drawing.Point(578, 103);
            this.textBoxFeature3.Name = "textBoxFeature3";
            this.textBoxFeature3.Size = new System.Drawing.Size(100, 36);
            this.textBoxFeature3.TabIndex = 10;
            // 
            // textBoxFeature4
            // 
            this.textBoxFeature4.Font = new System.Drawing.Font("メイリオ", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBoxFeature4.Location = new System.Drawing.Point(578, 145);
            this.textBoxFeature4.Name = "textBoxFeature4";
            this.textBoxFeature4.Size = new System.Drawing.Size(100, 36);
            this.textBoxFeature4.TabIndex = 11;
            // 
            // textBoxFeature5
            // 
            this.textBoxFeature5.Font = new System.Drawing.Font("メイリオ", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBoxFeature5.Location = new System.Drawing.Point(578, 187);
            this.textBoxFeature5.Name = "textBoxFeature5";
            this.textBoxFeature5.Size = new System.Drawing.Size(100, 36);
            this.textBoxFeature5.TabIndex = 12;
            // 
            // textBoxFeature6
            // 
            this.textBoxFeature6.Font = new System.Drawing.Font("メイリオ", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBoxFeature6.Location = new System.Drawing.Point(578, 229);
            this.textBoxFeature6.Name = "textBoxFeature6";
            this.textBoxFeature6.Size = new System.Drawing.Size(100, 36);
            this.textBoxFeature6.TabIndex = 13;
            // 
            // textBoxFeature7
            // 
            this.textBoxFeature7.Font = new System.Drawing.Font("メイリオ", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBoxFeature7.Location = new System.Drawing.Point(578, 271);
            this.textBoxFeature7.Name = "textBoxFeature7";
            this.textBoxFeature7.Size = new System.Drawing.Size(100, 36);
            this.textBoxFeature7.TabIndex = 14;
            // 
            // textBoxFeature8
            // 
            this.textBoxFeature8.Font = new System.Drawing.Font("メイリオ", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBoxFeature8.Location = new System.Drawing.Point(578, 313);
            this.textBoxFeature8.Name = "textBoxFeature8";
            this.textBoxFeature8.Size = new System.Drawing.Size(100, 36);
            this.textBoxFeature8.TabIndex = 15;
            // 
            // EvaluateDispForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.textBoxFeature8);
            this.Controls.Add(this.textBoxFeature7);
            this.Controls.Add(this.textBoxFeature6);
            this.Controls.Add(this.textBoxFeature5);
            this.Controls.Add(this.textBoxFeature4);
            this.Controls.Add(this.textBoxFeature3);
            this.Controls.Add(this.textBoxFeature2);
            this.Controls.Add(this.textBoxFeature1);
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
        private System.Windows.Forms.TextBox textBoxFeature1;
        private System.Windows.Forms.TextBox textBoxFeature2;
        private System.Windows.Forms.TextBox textBoxFeature3;
        private System.Windows.Forms.TextBox textBoxFeature4;
        private System.Windows.Forms.TextBox textBoxFeature5;
        private System.Windows.Forms.TextBox textBoxFeature6;
        private System.Windows.Forms.TextBox textBoxFeature7;
        private System.Windows.Forms.TextBox textBoxFeature8;
    }
}