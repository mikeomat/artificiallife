using System;
using System.Drawing;
using System.Windows.Forms;
using HeuristicLab.Core.Views;
using HeuristicLab.MainForm;

namespace ArtificialLifePlugin {
  partial class SolutionView : NamedItemView {
    private PictureBox pictureBox;
    private System.Windows.Forms.GroupBox groupBox;


    private void InitializeComponent() {
      this.groupBox = new System.Windows.Forms.GroupBox();
      this.pictureBox = new System.Windows.Forms.PictureBox();
      ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
      this.groupBox.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
      this.SuspendLayout();
      // 
      // nameTextBox
      // 
      this.errorProvider.SetIconAlignment(this.nameTextBox, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
      this.errorProvider.SetIconPadding(this.nameTextBox, 2);
      this.nameTextBox.Size = new System.Drawing.Size(270, 20);
      // 
      // infoLabel
      // 
      this.infoLabel.Location = new System.Drawing.Point(334, 3);
      // 
      // groupBox
      // 
      this.groupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBox.Controls.Add(this.pictureBox);
      this.groupBox.Location = new System.Drawing.Point(3, 26);
      this.groupBox.Name = "groupBox";
      this.groupBox.Size = new System.Drawing.Size(347, 232);
      this.groupBox.TabIndex = 3;
      this.groupBox.TabStop = false;
      this.groupBox.Text = "Clever creature solution";
      // 
      // pictureBox
      // 
      this.pictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
      this.pictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
      this.pictureBox.Location = new System.Drawing.Point(6, 19);
      this.pictureBox.Name = "pictureBox";
      this.pictureBox.Size = new System.Drawing.Size(335, 207);
      this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
      this.pictureBox.TabIndex = 0;
      this.pictureBox.TabStop = false;
      // 
      // SolutionView
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.Controls.Add(this.groupBox);
      this.Name = "SolutionView";
      this.Size = new System.Drawing.Size(353, 261);
      this.Controls.SetChildIndex(this.groupBox, 0);
      this.Controls.SetChildIndex(this.nameLabel, 0);
      this.Controls.SetChildIndex(this.nameTextBox, 0);
      this.Controls.SetChildIndex(this.infoLabel, 0);
      ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
      this.groupBox.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }
  }
}
