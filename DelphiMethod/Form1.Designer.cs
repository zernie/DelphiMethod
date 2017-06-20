namespace DelphiMethod
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.configOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.label5 = new System.Windows.Forms.Label();
            this.indicatorsDataGridView = new System.Windows.Forms.DataGridView();
            this.Title = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Weight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Remove = new System.Windows.Forms.DataGridViewButtonColumn();
            this.label4 = new System.Windows.Forms.Label();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.importButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.alternativesLabel = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.expertsCountNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.alternativesCountNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.expertsRichTextBox = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.alternativesRichTextBox = new System.Windows.Forms.RichTextBox();
            this.alphaComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.indicatorsDataGridView)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.expertsCountNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.alternativesCountNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // configOpenFileDialog
            // 
            this.configOpenFileDialog.Filter = "Файлы конфигурации|*.bin|Все файлы|*.*";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(982, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(143, 32);
            this.label5.TabIndex = 14;
            this.label5.Text = "Показатели";
            // 
            // indicatorsDataGridView
            // 
            this.indicatorsDataGridView.BackgroundColor = System.Drawing.Color.PaleGoldenrod;
            this.indicatorsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.indicatorsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Title,
            this.Weight,
            this.Remove});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.indicatorsDataGridView.DefaultCellStyle = dataGridViewCellStyle5;
            this.indicatorsDataGridView.Location = new System.Drawing.Point(850, 78);
            this.indicatorsDataGridView.Name = "indicatorsDataGridView";
            this.indicatorsDataGridView.RowTemplate.Height = 24;
            this.indicatorsDataGridView.Size = new System.Drawing.Size(400, 400);
            this.indicatorsDataGridView.TabIndex = 17;
            this.indicatorsDataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // Title
            // 
            this.Title.HeaderText = "Имя";
            this.Title.Name = "Title";
            this.Title.Width = 200;
            // 
            // Weight
            // 
            this.Weight.HeaderText = "Вес";
            this.Weight.Name = "Weight";
            this.Weight.Width = 70;
            // 
            // Remove
            // 
            this.Remove.HeaderText = "";
            this.Remove.Name = "Remove";
            this.Remove.Text = "Удалить";
            this.Remove.UseColumnTextForButtonValue = true;
            this.Remove.Width = 80;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(196, 547);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(228, 32);
            this.label4.TabIndex = 11;
            this.label4.Text = "Шкала оценивания";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton1.Location = new System.Drawing.Point(564, 547);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(85, 36);
            this.radioButton1.TabIndex = 12;
            this.radioButton1.Text = "0..10";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Checked = true;
            this.radioButton2.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton2.Location = new System.Drawing.Point(701, 544);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(98, 36);
            this.radioButton2.TabIndex = 13;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "0..100";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // importButton
            // 
            this.importButton.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.importButton.Location = new System.Drawing.Point(671, 600);
            this.importButton.Name = "importButton";
            this.importButton.Size = new System.Drawing.Size(128, 46);
            this.importButton.TabIndex = 20;
            this.importButton.Text = "Импорт";
            this.importButton.UseVisualStyleBackColor = true;
            this.importButton.Click += new System.EventHandler(this.importButton_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(521, 600);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(128, 46);
            this.button1.TabIndex = 6;
            this.button1.Text = "Пуск";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // alternativesLabel
            // 
            this.alternativesLabel.AutoSize = true;
            this.alternativesLabel.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.alternativesLabel.Location = new System.Drawing.Point(119, 21);
            this.alternativesLabel.Name = "alternativesLabel";
            this.alternativesLabel.Size = new System.Drawing.Size(172, 32);
            this.alternativesLabel.TabIndex = 21;
            this.alternativesLabel.Text = "Альтернативы";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.expertsCountNumericUpDown);
            this.groupBox1.Controls.Add(this.alternativesCountNumericUpDown);
            this.groupBox1.Controls.Add(this.expertsRichTextBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.alternativesRichTextBox);
            this.groupBox1.Controls.Add(this.alternativesLabel);
            this.groupBox1.Controls.Add(this.indicatorsDataGridView);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1262, 484);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            // 
            // expertsCountNumericUpDown
            // 
            this.expertsCountNumericUpDown.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.expertsCountNumericUpDown.Location = new System.Drawing.Point(736, 79);
            this.expertsCountNumericUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.expertsCountNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.expertsCountNumericUpDown.Name = "expertsCountNumericUpDown";
            this.expertsCountNumericUpDown.Size = new System.Drawing.Size(90, 38);
            this.expertsCountNumericUpDown.TabIndex = 26;
            this.expertsCountNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.expertsCountNumericUpDown.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
            this.expertsCountNumericUpDown.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.expertsCountNumericUpDown.ValueChanged += new System.EventHandler(this.expertsCountNumericUpDown_ValueChanged);
            // 
            // alternativesCountNumericUpDown
            // 
            this.alternativesCountNumericUpDown.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.alternativesCountNumericUpDown.Location = new System.Drawing.Point(316, 79);
            this.alternativesCountNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.alternativesCountNumericUpDown.Name = "alternativesCountNumericUpDown";
            this.alternativesCountNumericUpDown.Size = new System.Drawing.Size(90, 38);
            this.alternativesCountNumericUpDown.TabIndex = 25;
            this.alternativesCountNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.alternativesCountNumericUpDown.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
            this.alternativesCountNumericUpDown.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.alternativesCountNumericUpDown.ValueChanged += new System.EventHandler(this.alternativesCountNumericUpDown_ValueChanged);
            // 
            // expertsRichTextBox
            // 
            this.expertsRichTextBox.BackColor = System.Drawing.Color.PaleGoldenrod;
            this.expertsRichTextBox.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.expertsRichTextBox.Location = new System.Drawing.Point(426, 78);
            this.expertsRichTextBox.Name = "expertsRichTextBox";
            this.expertsRichTextBox.Size = new System.Drawing.Size(400, 400);
            this.expertsRichTextBox.TabIndex = 24;
            this.expertsRichTextBox.Text = "";
            this.expertsRichTextBox.TextChanged += new System.EventHandler(this.expertsRichTextBox_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(569, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 32);
            this.label1.TabIndex = 23;
            this.label1.Text = "Эксперты";
            // 
            // alternativesRichTextBox
            // 
            this.alternativesRichTextBox.BackColor = System.Drawing.Color.PaleGoldenrod;
            this.alternativesRichTextBox.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.alternativesRichTextBox.Location = new System.Drawing.Point(6, 78);
            this.alternativesRichTextBox.Name = "alternativesRichTextBox";
            this.alternativesRichTextBox.Size = new System.Drawing.Size(400, 400);
            this.alternativesRichTextBox.TabIndex = 22;
            this.alternativesRichTextBox.Text = "";
            this.alternativesRichTextBox.TextChanged += new System.EventHandler(this.alternativesRichTextBox_TextChanged);
            // 
            // alphaComboBox
            // 
            this.alphaComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.alphaComboBox.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.alphaComboBox.FormattingEnabled = true;
            this.alphaComboBox.Location = new System.Drawing.Point(589, 499);
            this.alphaComboBox.Name = "alphaComboBox";
            this.alphaComboBox.Size = new System.Drawing.Size(151, 39);
            this.alphaComboBox.TabIndex = 32;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(196, 502);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(379, 32);
            this.label2.TabIndex = 33;
            this.label2.Text = "Уровень значимости критерия α";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1262, 673);
            this.Controls.Add(this.alphaComboBox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.importButton);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.radioButton2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Групповая оценка альтернатив";
            ((System.ComponentModel.ISupportInitialize)(this.indicatorsDataGridView)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.expertsCountNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.alternativesCountNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog configOpenFileDialog;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridView indicatorsDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn Title;
        private System.Windows.Forms.DataGridViewTextBoxColumn Weight;
        private System.Windows.Forms.DataGridViewButtonColumn Remove;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.Button importButton;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label alternativesLabel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox alternativesRichTextBox;
        private System.Windows.Forms.RichTextBox expertsRichTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox alphaComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown expertsCountNumericUpDown;
        private System.Windows.Forms.NumericUpDown alternativesCountNumericUpDown;
    }
}

