namespace DelphiMethod
{
    partial class Form2
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
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.nextTourButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.exportButton = new System.Windows.Forms.Button();
            this.tourNumberLabel = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ratingScaleTextBox = new System.Windows.Forms.TextBox();
            this.calculateButton = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToDeleteRows = false;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dataGridView2.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.dataGridView2.Location = new System.Drawing.Point(0, 173);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dataGridView2.RowTemplate.Height = 24;
            this.dataGridView2.Size = new System.Drawing.Size(1262, 500);
            this.dataGridView2.TabIndex = 12;
            // 
            // nextTourButton
            // 
            this.nextTourButton.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.nextTourButton.Location = new System.Drawing.Point(870, 14);
            this.nextTourButton.Name = "nextTourButton";
            this.nextTourButton.Size = new System.Drawing.Size(237, 58);
            this.nextTourButton.TabIndex = 15;
            this.nextTourButton.Text = "Следующий тур →";
            this.nextTourButton.UseVisualStyleBackColor = true;
            this.nextTourButton.Click += new System.EventHandler(this.nextTourButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(483, 135);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 32);
            this.label1.TabIndex = 14;
            this.label1.Text = "Оценки";
            // 
            // exportButton
            // 
            this.exportButton.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.exportButton.Location = new System.Drawing.Point(998, 123);
            this.exportButton.Name = "exportButton";
            this.exportButton.Size = new System.Drawing.Size(109, 38);
            this.exportButton.TabIndex = 18;
            this.exportButton.Text = "Экспорт";
            this.exportButton.UseVisualStyleBackColor = true;
            this.exportButton.Click += new System.EventHandler(this.exportButton_Click);
            // 
            // tourNumberLabel
            // 
            this.tourNumberLabel.AutoSize = true;
            this.tourNumberLabel.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tourNumberLabel.Location = new System.Drawing.Point(5, 14);
            this.tourNumberLabel.Name = "tourNumberLabel";
            this.tourNumberLabel.Size = new System.Drawing.Size(145, 32);
            this.tourNumberLabel.TabIndex = 20;
            this.tourNumberLabel.Text = "Номер тура";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(342, 25);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(289, 39);
            this.comboBox1.TabIndex = 21;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(195, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(141, 32);
            this.label2.TabIndex = 22;
            this.label2.Text = "Показатель";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(5, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(175, 32);
            this.label3.TabIndex = 24;
            this.label3.Text = "Шкала оценок";
            // 
            // ratingScaleTextBox
            // 
            this.ratingScaleTextBox.Enabled = false;
            this.ratingScaleTextBox.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ratingScaleTextBox.Location = new System.Drawing.Point(185, 102);
            this.ratingScaleTextBox.Name = "ratingScaleTextBox";
            this.ratingScaleTextBox.Size = new System.Drawing.Size(151, 38);
            this.ratingScaleTextBox.TabIndex = 25;
            // 
            // calculateButton
            // 
            this.calculateButton.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.calculateButton.Location = new System.Drawing.Point(702, 14);
            this.calculateButton.Name = "calculateButton";
            this.calculateButton.Size = new System.Drawing.Size(162, 58);
            this.calculateButton.TabIndex = 26;
            this.calculateButton.Text = "Посчитать";
            this.calculateButton.UseVisualStyleBackColor = true;
            this.calculateButton.Click += new System.EventHandler(this.calculateButton_Click_1);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.FileName = "data.csv";
            this.saveFileDialog1.Filter = "Файлы конфигурации|*.bin|Все файлы|*.*";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.calculateButton);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.tourNumberLabel);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.ratingScaleTextBox);
            this.groupBox1.Controls.Add(this.exportButton);
            this.groupBox1.Controls.Add(this.comboBox1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.nextTourButton);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1262, 167);
            this.groupBox1.TabIndex = 27;
            this.groupBox1.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(6, 28);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(145, 32);
            this.label4.TabIndex = 20;
            this.label4.Text = "Номер тура";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1262, 673);
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Групповая оценка альтернатив";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Button nextTourButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button exportButton;
        private System.Windows.Forms.Label tourNumberLabel;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox ratingScaleTextBox;
        private System.Windows.Forms.Button calculateButton;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
    }
}