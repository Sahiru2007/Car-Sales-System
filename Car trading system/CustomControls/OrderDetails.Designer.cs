
namespace Car_trading_system.CustomControls
{
    partial class OrderDetails
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OrderDetails));
            this.labelOrderID = new System.Windows.Forms.Label();
            this.labelProductDetails = new System.Windows.Forms.Label();
            this.labelAmount = new System.Windows.Forms.Label();
            this.labelTotalAmount = new System.Windows.Forms.Label();
            this.labelOrderDate = new System.Windows.Forms.Label();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.line1 = new System.Windows.Forms.Label();
            this.line2 = new System.Windows.Forms.Label();
            this.line3 = new System.Windows.Forms.Label();
            this.status = new System.Windows.Forms.Label();
            this.deliveredBtn = new System.Windows.Forms.Button();
            this.shippedBtn = new System.Windows.Forms.Button();
            this.processingBtn = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.ProductImage = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.ProductImage)).BeginInit();
            this.SuspendLayout();
            // 
            // labelOrderID
            // 
            this.labelOrderID.AutoSize = true;
            this.labelOrderID.Font = new System.Drawing.Font("Poppins SemiBold", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelOrderID.Location = new System.Drawing.Point(252, 10);
            this.labelOrderID.Name = "labelOrderID";
            this.labelOrderID.Size = new System.Drawing.Size(87, 34);
            this.labelOrderID.TabIndex = 2;
            this.labelOrderID.Text = "#00001";
            // 
            // labelProductDetails
            // 
            this.labelProductDetails.AutoSize = true;
            this.labelProductDetails.Font = new System.Drawing.Font("Poppins", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelProductDetails.Location = new System.Drawing.Point(252, 82);
            this.labelProductDetails.Name = "labelProductDetails";
            this.labelProductDetails.Size = new System.Drawing.Size(81, 34);
            this.labelProductDetails.TabIndex = 3;
            this.labelProductDetails.Text = "Tesla Y";
            // 
            // labelAmount
            // 
            this.labelAmount.AutoSize = true;
            this.labelAmount.Font = new System.Drawing.Font("Poppins", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAmount.Location = new System.Drawing.Point(952, 10);
            this.labelAmount.Name = "labelAmount";
            this.labelAmount.Size = new System.Drawing.Size(146, 34);
            this.labelAmount.TabIndex = 4;
            this.labelAmount.Text = "Rs. 1000.00 x 2";
            // 
            // labelTotalAmount
            // 
            this.labelTotalAmount.AutoSize = true;
            this.labelTotalAmount.Font = new System.Drawing.Font("Poppins SemiBold", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTotalAmount.Location = new System.Drawing.Point(952, 62);
            this.labelTotalAmount.Name = "labelTotalAmount";
            this.labelTotalAmount.Size = new System.Drawing.Size(123, 34);
            this.labelTotalAmount.TabIndex = 5;
            this.labelTotalAmount.Text = "Rs. 2000.00";
            // 
            // labelOrderDate
            // 
            this.labelOrderDate.AutoSize = true;
            this.labelOrderDate.Font = new System.Drawing.Font("Poppins", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelOrderDate.Location = new System.Drawing.Point(258, 32);
            this.labelOrderDate.Name = "labelOrderDate";
            this.labelOrderDate.Size = new System.Drawing.Size(81, 34);
            this.labelOrderDate.TabIndex = 6;
            this.labelOrderDate.Text = "Tesla Y";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "processg");
            this.imageList1.Images.SetKeyName(1, "moneyg");
            this.imageList1.Images.SetKeyName(2, "moneya");
            this.imageList1.Images.SetKeyName(3, "processa");
            this.imageList1.Images.SetKeyName(4, "checkg");
            this.imageList1.Images.SetKeyName(5, "checka");
            this.imageList1.Images.SetKeyName(6, "shippedg");
            this.imageList1.Images.SetKeyName(7, "shippeda");
            // 
            // line1
            // 
            this.line1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(207)))), ((int)(((byte)(116)))));
            this.line1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.line1.Location = new System.Drawing.Point(460, 72);
            this.line1.Name = "line1";
            this.line1.Size = new System.Drawing.Size(80, 4);
            this.line1.TabIndex = 11;
            // 
            // line2
            // 
            this.line2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(184)))), ((int)(((byte)(184)))));
            this.line2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.line2.Location = new System.Drawing.Point(597, 71);
            this.line2.Name = "line2";
            this.line2.Size = new System.Drawing.Size(80, 4);
            this.line2.TabIndex = 12;
            // 
            // line3
            // 
            this.line3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(184)))), ((int)(((byte)(184)))));
            this.line3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.line3.Location = new System.Drawing.Point(734, 72);
            this.line3.Name = "line3";
            this.line3.Size = new System.Drawing.Size(80, 4);
            this.line3.TabIndex = 13;
            // 
            // status
            // 
            this.status.AutoSize = true;
            this.status.Font = new System.Drawing.Font("Poppins", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.status.Location = new System.Drawing.Point(601, 10);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(75, 34);
            this.status.TabIndex = 14;
            this.status.Text = "Status";
            // 
            // deliveredBtn
            // 
            this.deliveredBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.deliveredBtn.FlatAppearance.BorderSize = 0;
            this.deliveredBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.deliveredBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.deliveredBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.deliveredBtn.Font = new System.Drawing.Font("Poppins", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deliveredBtn.ImageIndex = 5;
            this.deliveredBtn.ImageList = this.imageList1;
            this.deliveredBtn.Location = new System.Drawing.Point(796, 26);
            this.deliveredBtn.Margin = new System.Windows.Forms.Padding(0);
            this.deliveredBtn.Name = "deliveredBtn";
            this.deliveredBtn.Size = new System.Drawing.Size(94, 91);
            this.deliveredBtn.TabIndex = 10;
            this.deliveredBtn.Text = "delivered";
            this.deliveredBtn.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.deliveredBtn.UseVisualStyleBackColor = true;
            // 
            // shippedBtn
            // 
            this.shippedBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.shippedBtn.FlatAppearance.BorderSize = 0;
            this.shippedBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.shippedBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.shippedBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.shippedBtn.Font = new System.Drawing.Font("Poppins", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.shippedBtn.ImageIndex = 7;
            this.shippedBtn.ImageList = this.imageList1;
            this.shippedBtn.Location = new System.Drawing.Point(660, 26);
            this.shippedBtn.Margin = new System.Windows.Forms.Padding(0);
            this.shippedBtn.Name = "shippedBtn";
            this.shippedBtn.Size = new System.Drawing.Size(94, 91);
            this.shippedBtn.TabIndex = 9;
            this.shippedBtn.Text = "Shipped";
            this.shippedBtn.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.shippedBtn.UseVisualStyleBackColor = true;
            // 
            // processingBtn
            // 
            this.processingBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.processingBtn.FlatAppearance.BorderSize = 0;
            this.processingBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.processingBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.processingBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.processingBtn.Font = new System.Drawing.Font("Poppins", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.processingBtn.ImageIndex = 3;
            this.processingBtn.ImageList = this.imageList1;
            this.processingBtn.Location = new System.Drawing.Point(522, 26);
            this.processingBtn.Margin = new System.Windows.Forms.Padding(0);
            this.processingBtn.Name = "processingBtn";
            this.processingBtn.Size = new System.Drawing.Size(94, 91);
            this.processingBtn.TabIndex = 8;
            this.processingBtn.Text = "Processing";
            this.processingBtn.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.processingBtn.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Poppins", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ImageIndex = 1;
            this.button1.ImageList = this.imageList1;
            this.button1.Location = new System.Drawing.Point(385, 25);
            this.button1.Margin = new System.Windows.Forms.Padding(0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(94, 91);
            this.button1.TabIndex = 7;
            this.button1.Text = "payment ";
            this.button1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ProductImage
            // 
            this.ProductImage.Location = new System.Drawing.Point(13, 10);
            this.ProductImage.Name = "ProductImage";
            this.ProductImage.Size = new System.Drawing.Size(224, 111);
            this.ProductImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ProductImage.TabIndex = 1;
            this.ProductImage.TabStop = false;
            // 
            // OrderDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(70)))), ((int)(((byte)(105)))));
            this.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.BorderRadius = 20;
            this.Controls.Add(this.status);
            this.Controls.Add(this.line3);
            this.Controls.Add(this.line2);
            this.Controls.Add(this.line1);
            this.Controls.Add(this.deliveredBtn);
            this.Controls.Add(this.shippedBtn);
            this.Controls.Add(this.processingBtn);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.labelOrderDate);
            this.Controls.Add(this.labelTotalAmount);
            this.Controls.Add(this.labelAmount);
            this.Controls.Add(this.labelProductDetails);
            this.Controls.Add(this.labelOrderID);
            this.Controls.Add(this.ProductImage);
            this.Name = "OrderDetails";
            this.Size = new System.Drawing.Size(1135, 135);
            this.Load += new System.EventHandler(this.OrderDetails_Load);
            this.Click += new System.EventHandler(this.OrderDetails_Click);
            ((System.ComponentModel.ISupportInitialize)(this.ProductImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox ProductImage;
        private System.Windows.Forms.Label labelOrderID;
        private System.Windows.Forms.Label labelProductDetails;
        private System.Windows.Forms.Label labelAmount;
        private System.Windows.Forms.Label labelTotalAmount;
        private System.Windows.Forms.Label labelOrderDate;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button processingBtn;
        private System.Windows.Forms.Button shippedBtn;
        private System.Windows.Forms.Button deliveredBtn;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Label line1;
        private System.Windows.Forms.Label line2;
        private System.Windows.Forms.Label line3;
        private System.Windows.Forms.Label status;
    }
}
