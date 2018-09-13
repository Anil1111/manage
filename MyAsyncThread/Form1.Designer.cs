namespace MyAsyncThread
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnTask = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnTask
            // 
            this.btnTask.Location = new System.Drawing.Point(97, 62);
            this.btnTask.Name = "btnTask";
            this.btnTask.Size = new System.Drawing.Size(138, 42);
            this.btnTask.TabIndex = 6;
            this.btnTask.Text = "Task";
            this.btnTask.UseVisualStyleBackColor = true;
            this.btnTask.Click += new System.EventHandler(this.btnTask_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(364, 169);
            this.Controls.Add(this.btnTask);
            this.Name = "Form1";
            this.Text = "异步多线程";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnTask;
    }
}

