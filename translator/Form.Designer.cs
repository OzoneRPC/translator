namespace translator {
  partial class Translator {
    /// <summary>
    /// Обязательная переменная конструктора.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Освободить все используемые ресурсы.
    /// </summary>
    /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Код, автоматически созданный конструктором форм Windows

    /// <summary>
    /// Требуемый метод для поддержки конструктора — не изменяйте 
    /// содержимое этого метода с помощью редактора кода.
    /// </summary>
    private void InitializeComponent() {
      this.textbox = new System.Windows.Forms.RichTextBox();
      this.button = new System.Windows.Forms.Button();
      this.outputLabel = new System.Windows.Forms.Label();
      this.bnf = new System.Windows.Forms.Label();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.groupBox1.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.SuspendLayout();
      // 
      // textbox
      // 
      this.textbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.textbox.Location = new System.Drawing.Point(13, 325);
      this.textbox.Name = "textbox";
      this.textbox.Size = new System.Drawing.Size(354, 241);
      this.textbox.TabIndex = 0;
      this.textbox.Text = "";
      this.textbox.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
      // 
      // button
      // 
      this.button.Location = new System.Drawing.Point(13, 572);
      this.button.Name = "button";
      this.button.Size = new System.Drawing.Size(354, 29);
      this.button.TabIndex = 1;
      this.button.Text = "Отправить";
      this.button.UseVisualStyleBackColor = true;
      this.button.Click += new System.EventHandler(this.button1_Click);
      // 
      // outputLabel
      // 
      this.outputLabel.AutoEllipsis = true;
      this.outputLabel.AutoSize = true;
      this.outputLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.outputLabel.Location = new System.Drawing.Point(6, 19);
      this.outputLabel.MaximumSize = new System.Drawing.Size(295, 355);
      this.outputLabel.Name = "outputLabel";
      this.outputLabel.Size = new System.Drawing.Size(0, 20);
      this.outputLabel.TabIndex = 2;
      // 
      // bnf
      // 
      this.bnf.AutoEllipsis = true;
      this.bnf.AutoSize = true;
      this.bnf.Font = new System.Drawing.Font("MS Reference Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.bnf.Location = new System.Drawing.Point(6, 16);
      this.bnf.MaximumSize = new System.Drawing.Size(425, 0);
      this.bnf.Name = "bnf";
      this.bnf.Size = new System.Drawing.Size(0, 16);
      this.bnf.TabIndex = 3;
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.outputLabel);
      this.groupBox1.Location = new System.Drawing.Point(12, 13);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(355, 295);
      this.groupBox1.TabIndex = 4;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Вывод";
      this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
      // 
      // groupBox2
      // 
      this.groupBox2.Controls.Add(this.bnf);
      this.groupBox2.Location = new System.Drawing.Point(380, 13);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(425, 588);
      this.groupBox2.TabIndex = 5;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "БНФ";
      // 
      // Translator
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(817, 613);
      this.Controls.Add(this.groupBox2);
      this.Controls.Add(this.groupBox1);
      this.Controls.Add(this.button);
      this.Controls.Add(this.textbox);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Name = "Translator";
      this.Text = "Translator";
      this.Load += new System.EventHandler(this.Form1_Load);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.RichTextBox textbox;
    private System.Windows.Forms.Button button;
    private System.Windows.Forms.Label outputLabel;
    private System.Windows.Forms.Label bnf;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.GroupBox groupBox2;
  }
}

