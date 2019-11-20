namespace EepromVisualAccess
{
	partial class AskModelPopup
	{
		public int selectedModel {get; set;}
		
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AskModelPopup));
			this.modelSelector = new System.Windows.Forms.ComboBox();
			this.btnAccept = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// modelSelector
			// 
			this.modelSelector.CausesValidation = false;
			this.modelSelector.FormattingEnabled = true;
			this.modelSelector.Items.AddRange(new object[] {
            "GWF-A-20/30/40TR",
            "GWF-A-80TR",
			"GWF-W-90TR"});
			this.modelSelector.Location = new System.Drawing.Point(36, 21);
			this.modelSelector.Margin = new System.Windows.Forms.Padding(2);
			this.modelSelector.Name = "modelSelector";
			this.modelSelector.Size = new System.Drawing.Size(135, 21);
			this.modelSelector.TabIndex = 10;
			this.modelSelector.Text = "Seleccione modelo";
			this.modelSelector.SelectedIndexChanged += new System.EventHandler(this.modelSelector_SelectedIndexChanged);
			// 
			// btnAccept
			// 
			this.btnAccept.Location = new System.Drawing.Point(36, 63);
			this.btnAccept.Name = "btnAccept";
			this.btnAccept.Size = new System.Drawing.Size(123, 32);
			this.btnAccept.TabIndex = 11;
			this.btnAccept.Text = "Aceptar";
			this.btnAccept.UseVisualStyleBackColor = true;
			this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(191, 63);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(123, 32);
			this.btnCancel.TabIndex = 12;
			this.btnCancel.Text = "Cancelar";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// AskModelPopup
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(347, 123);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnAccept);
			this.Controls.Add(this.modelSelector);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "AskModelPopup";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Por favor ingrese modelo de la máquina:";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ComboBox modelSelector;
		private System.Windows.Forms.Button btnAccept;
		private System.Windows.Forms.Button btnCancel;
	}
}