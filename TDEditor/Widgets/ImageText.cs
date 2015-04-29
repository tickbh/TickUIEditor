using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TDEditor.Data;

namespace TDEditor.Widgets
{
    public partial class ImageText : UserControl
    {
        public String showText
        {
            get
            {
                return this.label1.Text;
            }
            set
            {
                this.label1.Text = value;
            }
        }

        public String showImage
        {
            get
            {
                return this.pictureBox1.ImageLocation;
            }
            set 
            {
                this.pictureBox1.ImageLocation = value;
            }
        }
        public ImageText()
        {
            InitializeComponent();

            this.MouseDown += new MouseEventHandler(ImageText_MouseDown);
            this.MouseLeave += new EventHandler(ImageText_MouseLeave);
        }

        private void ImageText_Load(object sender, EventArgs e)
        {

        }
        private void ImageText_MouseDown(object sender, MouseEventArgs e)
        {
            ControlDDData data = new ControlDDData();
            data.controlType = (string)this.Tag;
            this.DoDragDrop(data, DragDropEffects.Copy);
        }

        private void ImageText_MouseLeave(object sender, EventArgs e)
        {
            Console.WriteLine("leave");
        }

    }
}
