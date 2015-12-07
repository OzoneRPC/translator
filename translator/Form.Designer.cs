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
      this.SuspendLayout();
      // 
      // textbox
      // 
      this.textbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.textbox.Location = new System.Drawing.Point(12, 147);
      this.textbox.Name = "textbox";
      this.textbox.Size = new System.Drawing.Size(354, 227);
      this.textbox.TabIndex = 0;
      this.textbox.Text = "";
      this.textbox.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
      // 
      // button
      // 
      this.button.Location = new System.Drawing.Point(372, 335);
      this.button.Name = "button";
      this.button.Size = new System.Drawing.Size(108, 39);
      this.button.TabIndex = 1;
      this.button.Text = "Отправить";
      this.button.UseVisualStyleBackColor = true;
      this.button.Click += new System.EventHandler(this.button1_Click);
      // 
      // outputLabel
      // 
      this.outputLabel.AutoSize = true;
      this.outputLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.outputLabel.Location = new System.Drawing.Point(12, 32);
      this.outputLabel.Name = "outputLabel";
      this.outputLabel.Size = new System.Drawing.Size(0, 20);
      this.outputLabel.TabIndex = 2;
      // 
      // Translator
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(717, 386);
      this.Controls.Add(this.outputLabel);
      this.Controls.Add(this.button);
      this.Controls.Add(this.textbox);
      this.Name = "Translator";
      this.Text = "Translator";
      this.Load += new System.EventHandler(this.Form1_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.RichTextBox textbox;
    private System.Windows.Forms.Button button;
    private System.Windows.Forms.Label outputLabel;
  }
}

