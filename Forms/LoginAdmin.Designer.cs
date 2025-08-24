namespace Mobile_app_shoppe.Forms
{
    partial class LoginAdmin
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
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.lnkForget = new System.Windows.Forms.LinkLabel();
            this.btnLogin = new System.Windows.Forms.Button();
            this.lnkBack = new System.Windows.Forms.LinkLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(153, 140);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(130, 22);
            this.textBox2.TabIndex = 13;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(153, 93);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(130, 22);
            this.textBox1.TabIndex = 12;
            // 
            // lnkForget
            // 
            this.lnkForget.AutoSize = true;
            this.lnkForget.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lnkForget.LinkColor = System.Drawing.Color.Black;
            this.lnkForget.Location = new System.Drawing.Point(27, 206);
            this.lnkForget.Name = "lnkForget";
            this.lnkForget.Size = new System.Drawing.Size(161, 22);
            this.lnkForget.TabIndex = 11;
            this.lnkForget.TabStop = true;
            this.lnkForget.Text = "Forgot Password";
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(208, 208);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(75, 23);
            this.btnLogin.TabIndex = 10;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            // 
            // lnkBack
            // 
            this.lnkBack.AutoSize = true;
            this.lnkBack.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lnkBack.LinkColor = System.Drawing.Color.Black;
            this.lnkBack.Location = new System.Drawing.Point(27, 35);
            this.lnkBack.Name = "lnkBack";
            this.lnkBack.Size = new System.Drawing.Size(54, 22);
            this.lnkBack.TabIndex = 9;
            this.lnkBack.TabStop = true;
            this.lnkBack.Text = "Back";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(64, 143);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 16);
            this.label2.TabIndex = 8;
            this.label2.Text = "Password:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(64, 93);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 16);
            this.label1.TabIndex = 7;
            this.label1.Text = "Username:";
            // 
            // LoginAdmin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(373, 332);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.lnkForget);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.lnkBack);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "LoginAdmin";
            this.Text = "Login_Admin";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.LinkLabel lnkForget;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.LinkLabel lnkBack;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}