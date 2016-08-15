namespace AWAProtocolProjectClient
{
    partial class Form1
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
            this.ConnectButton = new System.Windows.Forms.Button();
            this.ConnectTextBox = new System.Windows.Forms.TextBox();
            this.ConnectLabel = new System.Windows.Forms.Label();
            this.ConnectPanel = new System.Windows.Forms.Panel();
            this.MessagePanel = new System.Windows.Forms.Panel();
            this.messageBox = new System.Windows.Forms.ListBox();
            this.messageButton = new System.Windows.Forms.Button();
            this.messageText = new System.Windows.Forms.TextBox();
            this.UsernamePanel = new System.Windows.Forms.Panel();
            this.UsernameLabel = new System.Windows.Forms.Label();
            this.UsernameButton = new System.Windows.Forms.Button();
            this.UsernameTextBox = new System.Windows.Forms.TextBox();
            this.GameFieldPanel = new System.Windows.Forms.Panel();
            this.GameFieldLabel = new System.Windows.Forms.Label();
            this.healthPanel = new System.Windows.Forms.Panel();
            this.ConnectPanel.SuspendLayout();
            this.MessagePanel.SuspendLayout();
            this.UsernamePanel.SuspendLayout();
            this.GameFieldPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ConnectButton
            // 
            this.ConnectButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConnectButton.Location = new System.Drawing.Point(25, 120);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(250, 80);
            this.ConnectButton.TabIndex = 0;
            this.ConnectButton.Text = "Connect to server";
            this.ConnectButton.UseVisualStyleBackColor = true;
            this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // ConnectTextBox
            // 
            this.ConnectTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConnectTextBox.Location = new System.Drawing.Point(25, 75);
            this.ConnectTextBox.Name = "ConnectTextBox";
            this.ConnectTextBox.Size = new System.Drawing.Size(250, 29);
            this.ConnectTextBox.TabIndex = 1;
            this.ConnectTextBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.ConnectTextBox_PreviewKeyDown);
            // 
            // ConnectLabel
            // 
            this.ConnectLabel.AutoSize = true;
            this.ConnectLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConnectLabel.Location = new System.Drawing.Point(25, 25);
            this.ConnectLabel.Name = "ConnectLabel";
            this.ConnectLabel.Size = new System.Drawing.Size(209, 24);
            this.ConnectLabel.TabIndex = 0;
            this.ConnectLabel.Text = "Enter server IP-Address";
            // 
            // ConnectPanel
            // 
            this.ConnectPanel.Controls.Add(this.ConnectLabel);
            this.ConnectPanel.Controls.Add(this.ConnectButton);
            this.ConnectPanel.Controls.Add(this.ConnectTextBox);
            this.ConnectPanel.Location = new System.Drawing.Point(50, 50);
            this.ConnectPanel.Name = "ConnectPanel";
            this.ConnectPanel.Size = new System.Drawing.Size(300, 250);
            this.ConnectPanel.TabIndex = 0;
            // 
            // MessagePanel
            // 
            this.MessagePanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.MessagePanel.Controls.Add(this.messageBox);
            this.MessagePanel.Controls.Add(this.messageButton);
            this.MessagePanel.Controls.Add(this.messageText);
            this.MessagePanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.MessagePanel.Location = new System.Drawing.Point(0, 658);
            this.MessagePanel.Name = "MessagePanel";
            this.MessagePanel.Size = new System.Drawing.Size(816, 185);
            this.MessagePanel.TabIndex = 12;
            // 
            // messageBox
            // 
            this.messageBox.Location = new System.Drawing.Point(10, 0);
            this.messageBox.Name = "messageBox";
            this.messageBox.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.messageBox.Size = new System.Drawing.Size(608, 134);
            this.messageBox.TabIndex = 0;
            this.messageBox.TabStop = false;
            this.messageBox.UseTabStops = false;
            this.messageBox.Click += new System.EventHandler(this.GameFieldPanel_Click);
            // 
            // messageButton
            // 
            this.messageButton.Location = new System.Drawing.Point(623, 150);
            this.messageButton.Name = "messageButton";
            this.messageButton.Size = new System.Drawing.Size(75, 30);
            this.messageButton.TabIndex = 1;
            this.messageButton.Text = "Send";
            this.messageButton.UseVisualStyleBackColor = true;
            this.messageButton.Click += new System.EventHandler(this.messageButton_Click);
            this.messageButton.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.Button_PreviewKeyDown);
            // 
            // messageText
            // 
            this.messageText.Location = new System.Drawing.Point(10, 157);
            this.messageText.Name = "messageText";
            this.messageText.Size = new System.Drawing.Size(608, 20);
            this.messageText.TabIndex = 0;
            this.messageText.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.messageText_PreviewKeyDown);
            // 
            // UsernamePanel
            // 
            this.UsernamePanel.Controls.Add(this.UsernameLabel);
            this.UsernamePanel.Controls.Add(this.UsernameButton);
            this.UsernamePanel.Controls.Add(this.UsernameTextBox);
            this.UsernamePanel.Location = new System.Drawing.Point(50, 50);
            this.UsernamePanel.Name = "UsernamePanel";
            this.UsernamePanel.Size = new System.Drawing.Size(300, 250);
            this.UsernamePanel.TabIndex = 11;
            this.UsernamePanel.Visible = false;
            // 
            // UsernameLabel
            // 
            this.UsernameLabel.AutoSize = true;
            this.UsernameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UsernameLabel.Location = new System.Drawing.Point(25, 25);
            this.UsernameLabel.Name = "UsernameLabel";
            this.UsernameLabel.Size = new System.Drawing.Size(171, 24);
            this.UsernameLabel.TabIndex = 0;
            this.UsernameLabel.Text = "Choose your name";
            // 
            // UsernameButton
            // 
            this.UsernameButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UsernameButton.Location = new System.Drawing.Point(25, 120);
            this.UsernameButton.Name = "UsernameButton";
            this.UsernameButton.Size = new System.Drawing.Size(250, 80);
            this.UsernameButton.TabIndex = 0;
            this.UsernameButton.Text = "Submit";
            this.UsernameButton.UseVisualStyleBackColor = true;
            this.UsernameButton.Click += new System.EventHandler(this.UsernameButton_Click);
            // 
            // UsernameTextBox
            // 
            this.UsernameTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UsernameTextBox.Location = new System.Drawing.Point(25, 75);
            this.UsernameTextBox.Name = "UsernameTextBox";
            this.UsernameTextBox.Size = new System.Drawing.Size(250, 29);
            this.UsernameTextBox.TabIndex = 1;
            this.UsernameTextBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.UsernameTextBox_PreviewKeyDown);
            // 
            // GameFieldPanel
            // 
            this.GameFieldPanel.BackColor = System.Drawing.SystemColors.Control;
            this.GameFieldPanel.BackgroundImage = global::AWAProtocolProjectClient.Properties.Resources.mobile_game_background_by_disnie_d4jmrye;
            this.GameFieldPanel.Controls.Add(this.GameFieldLabel);
            this.GameFieldPanel.Location = new System.Drawing.Point(10, 10);
            this.GameFieldPanel.Name = "GameFieldPanel";
            this.GameFieldPanel.Size = new System.Drawing.Size(640, 640);
            this.GameFieldPanel.TabIndex = 10;
            this.GameFieldPanel.Click += new System.EventHandler(this.GameFieldPanel_Click);
            // 
            // GameFieldLabel
            // 
            this.GameFieldLabel.AutoSize = true;
            this.GameFieldLabel.BackColor = System.Drawing.Color.Transparent;
            this.GameFieldLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GameFieldLabel.Location = new System.Drawing.Point(79, 103);
            this.GameFieldLabel.Name = "GameFieldLabel";
            this.GameFieldLabel.Size = new System.Drawing.Size(172, 26);
            this.GameFieldLabel.TabIndex = 0;
            this.GameFieldLabel.Text = "GameFieldLabel";
            // 
            // healthPanel
            // 
            this.healthPanel.Location = new System.Drawing.Point(670, 10);
            this.healthPanel.Name = "healthPanel";
            this.healthPanel.Size = new System.Drawing.Size(130, 640);
            this.healthPanel.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(816, 843);
            this.Controls.Add(this.healthPanel);
            this.Controls.Add(this.ConnectPanel);
            this.Controls.Add(this.UsernamePanel);
            this.Controls.Add(this.MessagePanel);
            this.Controls.Add(this.GameFieldPanel);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.Button_PreviewKeyDown);
            this.ConnectPanel.ResumeLayout(false);
            this.ConnectPanel.PerformLayout();
            this.MessagePanel.ResumeLayout(false);
            this.MessagePanel.PerformLayout();
            this.UsernamePanel.ResumeLayout(false);
            this.UsernamePanel.PerformLayout();
            this.GameFieldPanel.ResumeLayout(false);
            this.GameFieldPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ConnectButton;
        private System.Windows.Forms.TextBox ConnectTextBox;
        private System.Windows.Forms.Label ConnectLabel;
        private System.Windows.Forms.Panel ConnectPanel;
        private System.Windows.Forms.Panel MessagePanel;
        private System.Windows.Forms.ListBox messageBox;
        private System.Windows.Forms.Button messageButton;
        private System.Windows.Forms.TextBox messageText;
        private System.Windows.Forms.Panel UsernamePanel;
        private System.Windows.Forms.Label UsernameLabel;
        private System.Windows.Forms.Button UsernameButton;
        private System.Windows.Forms.TextBox UsernameTextBox;
        private System.Windows.Forms.Panel GameFieldPanel;
        private System.Windows.Forms.Label GameFieldLabel;
        private System.Windows.Forms.Panel healthPanel;
    }
}

