namespace ArbMaker
{
    partial class ParserFrm
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
            this.webPageContentTb = new System.Windows.Forms.TextBox();
            this.readBtn = new System.Windows.Forms.Button();
            this.urlTb = new System.Windows.Forms.TextBox();
            this.footballBetclicBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // webPageContentTb
            // 
            this.webPageContentTb.AllowDrop = true;
            this.webPageContentTb.Location = new System.Drawing.Point(12, 29);
            this.webPageContentTb.Multiline = true;
            this.webPageContentTb.Name = "webPageContentTb";
            this.webPageContentTb.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.webPageContentTb.Size = new System.Drawing.Size(730, 535);
            this.webPageContentTb.TabIndex = 0;
            // 
            // readBtn
            // 
            this.readBtn.Location = new System.Drawing.Point(667, 3);
            this.readBtn.Name = "readBtn";
            this.readBtn.Size = new System.Drawing.Size(75, 23);
            this.readBtn.TabIndex = 1;
            this.readBtn.Text = "Read page";
            this.readBtn.UseVisualStyleBackColor = true;
            this.readBtn.Click += new System.EventHandler(this.readBtn_Click);
            // 
            // urlTb
            // 
            this.urlTb.Location = new System.Drawing.Point(12, 3);
            this.urlTb.Name = "urlTb";
            this.urlTb.Size = new System.Drawing.Size(649, 20);
            this.urlTb.TabIndex = 2;
            this.urlTb.Text = "https://www.betclic.fr/football/ligue-1-e4";
            // 
            // footballBetclicBtn
            // 
            this.footballBetclicBtn.Location = new System.Drawing.Point(748, 3);
            this.footballBetclicBtn.Name = "footballBetclicBtn";
            this.footballBetclicBtn.Size = new System.Drawing.Size(106, 23);
            this.footballBetclicBtn.TabIndex = 3;
            this.footballBetclicBtn.Text = "Betclic football";
            this.footballBetclicBtn.UseVisualStyleBackColor = true;
            this.footballBetclicBtn.Click += new System.EventHandler(this.footballBetclicBtn_Click);
            // 
            // ParserFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(866, 586);
            this.Controls.Add(this.footballBetclicBtn);
            this.Controls.Add(this.urlTb);
            this.Controls.Add(this.readBtn);
            this.Controls.Add(this.webPageContentTb);
            this.Name = "ParserFrm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox webPageContentTb;
        private System.Windows.Forms.Button readBtn;
        private System.Windows.Forms.TextBox urlTb;
        private System.Windows.Forms.Button footballBetclicBtn;
    }
}

