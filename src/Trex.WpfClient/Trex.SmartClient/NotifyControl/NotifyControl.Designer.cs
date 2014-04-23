namespace Trex.SmartClient.NotifyControl
{
    partial class NotifyControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NotifyControl));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.startNewTaskAltF12ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startPauseActiveTaskAltF11ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopActiveTaskAltF10ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.assignActiveTaskAltF9ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toggleWindowAltF5ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toggleWindowHotkey = new CodeProject.SystemHotkey.SystemHotkey(this.components);
            this.startNewTaskHotkey = new CodeProject.SystemHotkey.SystemHotkey(this.components);
            this.toggleActiveTaskHotkey = new CodeProject.SystemHotkey.SystemHotkey(this.components);
            this.systemHotkey1 = new CodeProject.SystemHotkey.SystemHotkey(this.components);
            this.assignTaskHotkey = new CodeProject.SystemHotkey.SystemHotkey(this.components);
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon1.BalloonTipText = "Hejsa";
            this.notifyIcon1.BalloonTipTitle = "Test";
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "T.Rex SmartClient";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startNewTaskAltF12ToolStripMenuItem,
            this.startPauseActiveTaskAltF11ToolStripMenuItem,
            this.stopActiveTaskAltF10ToolStripMenuItem,
            this.assignActiveTaskAltF9ToolStripMenuItem,
            this.toggleWindowAltF5ToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(256, 136);
            // 
            // startNewTaskAltF12ToolStripMenuItem
            // 
            this.startNewTaskAltF12ToolStripMenuItem.Name = "startNewTaskAltF12ToolStripMenuItem";
            this.startNewTaskAltF12ToolStripMenuItem.Size = new System.Drawing.Size(255, 22);
            this.startNewTaskAltF12ToolStripMenuItem.Text = "Start New Task (Alt + F11)";
            this.startNewTaskAltF12ToolStripMenuItem.Click += new System.EventHandler(this.startNewTaskAltF12ToolStripMenuItem_Click);
            // 
            // startPauseActiveTaskAltF11ToolStripMenuItem
            // 
            this.startPauseActiveTaskAltF11ToolStripMenuItem.Name = "startPauseActiveTaskAltF11ToolStripMenuItem";
            this.startPauseActiveTaskAltF11ToolStripMenuItem.Size = new System.Drawing.Size(255, 22);
            this.startPauseActiveTaskAltF11ToolStripMenuItem.Text = "Start/Pause Active Task (Alt + F12)";
            this.startPauseActiveTaskAltF11ToolStripMenuItem.Click += new System.EventHandler(this.startPauseActiveTaskAltF11ToolStripMenuItem_Click);
            // 
            // stopActiveTaskAltF10ToolStripMenuItem
            // 
            this.stopActiveTaskAltF10ToolStripMenuItem.Name = "stopActiveTaskAltF10ToolStripMenuItem";
            this.stopActiveTaskAltF10ToolStripMenuItem.Size = new System.Drawing.Size(255, 22);
            this.stopActiveTaskAltF10ToolStripMenuItem.Text = "Stop Active Task (Alt F10)";
            this.stopActiveTaskAltF10ToolStripMenuItem.Click += new System.EventHandler(this.stopActiveTaskAltF10ToolStripMenuItem_Click);
            // 
            // assignActiveTaskAltF9ToolStripMenuItem
            // 
            this.assignActiveTaskAltF9ToolStripMenuItem.Name = "assignActiveTaskAltF9ToolStripMenuItem";
            this.assignActiveTaskAltF9ToolStripMenuItem.Size = new System.Drawing.Size(255, 22);
            this.assignActiveTaskAltF9ToolStripMenuItem.Text = "Assign Active Task( Alt + F9)";
            this.assignActiveTaskAltF9ToolStripMenuItem.Click += new System.EventHandler(this.assignActiveTaskAltF9ToolStripMenuItem_Click);
            // 
            // toggleWindowAltF5ToolStripMenuItem
            // 
            this.toggleWindowAltF5ToolStripMenuItem.Name = "toggleWindowAltF5ToolStripMenuItem";
            this.toggleWindowAltF5ToolStripMenuItem.Size = new System.Drawing.Size(255, 22);
            this.toggleWindowAltF5ToolStripMenuItem.Text = "Toggle Window (Alt+F5)";
            this.toggleWindowAltF5ToolStripMenuItem.Click += new System.EventHandler(this.toggleWindowAltF5ToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(255, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toggleWindowHotkey
            // 
            this.toggleWindowHotkey.Shortcut = System.Windows.Forms.Shortcut.AltF5;
            this.toggleWindowHotkey.Pressed += new System.EventHandler(this.toggleWindowAltF5ToolStripMenuItem_Click);
            // 
            // startNewTaskHotkey
            // 
            this.startNewTaskHotkey.Shortcut = System.Windows.Forms.Shortcut.AltF11;
            this.startNewTaskHotkey.Pressed += new System.EventHandler(this.startNewTaskAltF12ToolStripMenuItem_Click);
            // 
            // toggleActiveTaskHotkey
            // 
            this.toggleActiveTaskHotkey.Shortcut = System.Windows.Forms.Shortcut.AltF12;
            this.toggleActiveTaskHotkey.Pressed += new System.EventHandler(this.startPauseActiveTaskAltF11ToolStripMenuItem_Click);
            // 
            // systemHotkey1
            // 
            this.systemHotkey1.Shortcut = System.Windows.Forms.Shortcut.AltF10;
            this.systemHotkey1.Pressed += new System.EventHandler(this.stopActiveTaskAltF10ToolStripMenuItem_Click);
            // 
            // assignTaskHotkey
            // 
            this.assignTaskHotkey.Pressed += new System.EventHandler(this.assignActiveTaskAltF9ToolStripMenuItem_Click);
            // 
            // NotifyControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "NotifyControl";
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem startPauseActiveTaskAltF11ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toggleWindowAltF5ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startNewTaskAltF12ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private CodeProject.SystemHotkey.SystemHotkey toggleWindowHotkey;
        private CodeProject.SystemHotkey.SystemHotkey startNewTaskHotkey;
        private CodeProject.SystemHotkey.SystemHotkey toggleActiveTaskHotkey;
        private System.Windows.Forms.ToolStripMenuItem stopActiveTaskAltF10ToolStripMenuItem;
        private CodeProject.SystemHotkey.SystemHotkey systemHotkey1;
        private System.Windows.Forms.ToolStripMenuItem assignActiveTaskAltF9ToolStripMenuItem;
        private CodeProject.SystemHotkey.SystemHotkey assignTaskHotkey;
    }
}
