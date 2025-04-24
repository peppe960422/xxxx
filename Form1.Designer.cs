namespace xxxx
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.DatabaseTxtBox = new System.Windows.Forms.TextBox();
            this.buttonExecuteGenerationClass = new System.Windows.Forms.Button();
            this.ausgabeTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // DatabaseTxtBox
            // 
            this.DatabaseTxtBox.Location = new System.Drawing.Point(76, 38);
            this.DatabaseTxtBox.Name = "DatabaseTxtBox";
            this.DatabaseTxtBox.Size = new System.Drawing.Size(100, 20);
            this.DatabaseTxtBox.TabIndex = 0;
            // 
            // buttonExecuteGenerationClass
            // 
            this.buttonExecuteGenerationClass.Location = new System.Drawing.Point(201, 35);
            this.buttonExecuteGenerationClass.Name = "buttonExecuteGenerationClass";
            this.buttonExecuteGenerationClass.Size = new System.Drawing.Size(75, 23);
            this.buttonExecuteGenerationClass.TabIndex = 1;
            this.buttonExecuteGenerationClass.Text = "button1";
            this.buttonExecuteGenerationClass.UseVisualStyleBackColor = true;
            this.buttonExecuteGenerationClass.Click += new System.EventHandler(this.buttonExecuteGenerationClass_Click<object>);
            // 
            // ausgabeTextBox
            // 
            this.ausgabeTextBox.Location = new System.Drawing.Point(321, 35);
            this.ausgabeTextBox.Multiline = true;
            this.ausgabeTextBox.Name = "ausgabeTextBox";
            this.ausgabeTextBox.Size = new System.Drawing.Size(311, 285);
            this.ausgabeTextBox.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.ausgabeTextBox);
            this.Controls.Add(this.buttonExecuteGenerationClass);
            this.Controls.Add(this.DatabaseTxtBox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox DatabaseTxtBox;
        private System.Windows.Forms.Button buttonExecuteGenerationClass;
        private System.Windows.Forms.TextBox ausgabeTextBox;
    }
}

