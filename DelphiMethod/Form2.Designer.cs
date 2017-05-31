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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.nextTourButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.indicatorComboBox = new System.Windows.Forms.ComboBox();
            this.exportButton = new System.Windows.Forms.Button();
            this.ratingScaleTextBox = new System.Windows.Forms.TextBox();
            this.tourNumberLabel = new System.Windows.Forms.Label();
            this.calculateButton = new System.Windows.Forms.Button();
            this.clearButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.isConsensusReachedLabel = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.alphaComboBox = new System.Windows.Forms.ComboBox();
            this.showResultButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToDeleteRows = false;
            this.dataGridView2.BackgroundColor = System.Drawing.Color.PaleGoldenrod;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle17.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle17.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle17.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle17.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle17.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle17.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView2.DefaultCellStyle = dataGridViewCellStyle17;
            this.dataGridView2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dataGridView2.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.dataGridView2.GridColor = System.Drawing.Color.DimGray;
            this.dataGridView2.Location = new System.Drawing.Point(0, 281);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            dataGridViewCellStyle18.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dataGridView2.RowsDefaultCellStyle = dataGridViewCellStyle18;
            this.dataGridView2.RowTemplate.Height = 24;
            this.dataGridView2.Size = new System.Drawing.Size(1262, 472);
            this.dataGridView2.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(514, 246);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(196, 32);
            this.label1.TabIndex = 14;
            this.label1.Text = "Матрица рангов";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.FileName = "data.bin";
            this.saveFileDialog1.Filter = "Файлы конфигурации|*.bin|Все файлы|*.*";
            // 
            // nextTourButton
            // 
            this.nextTourButton.Font = new System.Drawing.Font("Segoe UI", 10.8F);
            this.nextTourButton.Location = new System.Drawing.Point(822, 92);
            this.nextTourButton.Name = "nextTourButton";
            this.nextTourButton.Size = new System.Drawing.Size(159, 58);
            this.nextTourButton.TabIndex = 15;
            this.nextTourButton.Text = "Следующий тур";
            this.nextTourButton.UseVisualStyleBackColor = true;
            this.nextTourButton.Click += new System.EventHandler(this.nextTourButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(6, 156);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(175, 32);
            this.label3.TabIndex = 24;
            this.label3.Text = "Шкала оценок";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(12, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(141, 32);
            this.label2.TabIndex = 22;
            this.label2.Text = "Показатель";
            // 
            // indicatorComboBox
            // 
            this.indicatorComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.indicatorComboBox.DropDownWidth = 700;
            this.indicatorComboBox.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.indicatorComboBox.FormattingEnabled = true;
            this.indicatorComboBox.Location = new System.Drawing.Point(405, 21);
            this.indicatorComboBox.Name = "indicatorComboBox";
            this.indicatorComboBox.Size = new System.Drawing.Size(370, 39);
            this.indicatorComboBox.TabIndex = 21;
            // 
            // exportButton
            // 
            this.exportButton.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.exportButton.Location = new System.Drawing.Point(1129, 93);
            this.exportButton.Name = "exportButton";
            this.exportButton.Size = new System.Drawing.Size(121, 58);
            this.exportButton.TabIndex = 18;
            this.exportButton.Text = "Экспорт";
            this.exportButton.UseVisualStyleBackColor = true;
            this.exportButton.Click += new System.EventHandler(this.exportButton_Click);
            // 
            // ratingScaleTextBox
            // 
            this.ratingScaleTextBox.Enabled = false;
            this.ratingScaleTextBox.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ratingScaleTextBox.Location = new System.Drawing.Point(405, 153);
            this.ratingScaleTextBox.Name = "ratingScaleTextBox";
            this.ratingScaleTextBox.Size = new System.Drawing.Size(151, 38);
            this.ratingScaleTextBox.TabIndex = 25;
            // 
            // tourNumberLabel
            // 
            this.tourNumberLabel.AutoSize = true;
            this.tourNumberLabel.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tourNumberLabel.Location = new System.Drawing.Point(605, 96);
            this.tourNumberLabel.Name = "tourNumberLabel";
            this.tourNumberLabel.Size = new System.Drawing.Size(170, 32);
            this.tourNumberLabel.TabIndex = 20;
            this.tourNumberLabel.Text = "Номер тура: 1";
            // 
            // calculateButton
            // 
            this.calculateButton.Font = new System.Drawing.Font("Segoe UI", 11.8F);
            this.calculateButton.Location = new System.Drawing.Point(822, 21);
            this.calculateButton.Name = "calculateButton";
            this.calculateButton.Size = new System.Drawing.Size(159, 58);
            this.calculateButton.TabIndex = 26;
            this.calculateButton.Text = "Посчитать";
            this.calculateButton.UseVisualStyleBackColor = true;
            this.calculateButton.Click += new System.EventHandler(this.calculateButton_Click_1);
            // 
            // clearButton
            // 
            this.clearButton.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.clearButton.Location = new System.Drawing.Point(987, 93);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(136, 58);
            this.clearButton.TabIndex = 27;
            this.clearButton.Text = "Начать заново";
            this.clearButton.UseVisualStyleBackColor = true;
            this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.button1.Location = new System.Drawing.Point(987, 156);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(263, 58);
            this.button1.TabIndex = 28;
            this.button1.Text = "Заполнить случ. значениями";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(430, 208);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(296, 32);
            this.label4.TabIndex = 29;
            this.label4.Text = "Согласованность мнений";
            // 
            // isConsensusReachedLabel
            // 
            this.isConsensusReachedLabel.AutoSize = true;
            this.isConsensusReachedLabel.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.isConsensusReachedLabel.Location = new System.Drawing.Point(712, 208);
            this.isConsensusReachedLabel.Name = "isConsensusReachedLabel";
            this.isConsensusReachedLabel.Size = new System.Drawing.Size(30, 32);
            this.isConsensusReachedLabel.TabIndex = 30;
            this.isConsensusReachedLabel.Text = "...";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(12, 92);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(379, 32);
            this.label5.TabIndex = 31;
            this.label5.Text = "Уровень значимости критерия α";
            // 
            // alphaComboBox
            // 
            this.alphaComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.alphaComboBox.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.alphaComboBox.FormattingEnabled = true;
            this.alphaComboBox.Location = new System.Drawing.Point(405, 93);
            this.alphaComboBox.Name = "alphaComboBox";
            this.alphaComboBox.Size = new System.Drawing.Size(151, 39);
            this.alphaComboBox.TabIndex = 28;
            // 
            // showResultButton
            // 
            this.showResultButton.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.showResultButton.Location = new System.Drawing.Point(987, 21);
            this.showResultButton.Name = "showResultButton";
            this.showResultButton.Size = new System.Drawing.Size(263, 58);
            this.showResultButton.TabIndex = 33;
            this.showResultButton.Text = "Вывод результатов";
            this.showResultButton.UseVisualStyleBackColor = true;
            this.showResultButton.Click += new System.EventHandler(this.showResultButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.showResultButton);
            this.groupBox1.Controls.Add(this.alphaComboBox);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.isConsensusReachedLabel);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.clearButton);
            this.groupBox1.Controls.Add(this.calculateButton);
            this.groupBox1.Controls.Add(this.tourNumberLabel);
            this.groupBox1.Controls.Add(this.ratingScaleTextBox);
            this.groupBox1.Controls.Add(this.exportButton);
            this.groupBox1.Controls.Add(this.indicatorComboBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.nextTourButton);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1262, 243);
            this.groupBox1.TabIndex = 27;
            this.groupBox1.TabStop = false;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1262, 753);
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Name = "Form2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Групповая оценка альтернатив";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button nextTourButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox indicatorComboBox;
        private System.Windows.Forms.Button exportButton;
        private System.Windows.Forms.TextBox ratingScaleTextBox;
        private System.Windows.Forms.Label tourNumberLabel;
        private System.Windows.Forms.Button calculateButton;
        private System.Windows.Forms.Button clearButton;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label isConsensusReachedLabel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox alphaComboBox;
        private System.Windows.Forms.Button showResultButton;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}