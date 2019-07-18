using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kmeans
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public static int[,] xy = new int[10010,2];
        public static int[,] clusters = new int[10, 2];
        public static int[,] cluster2 = new int[10, 2];
        public static int[] dis = new int[50];
        public static int[] nocp = new int[50];
        static int np = 0;
        static int nc = 0;
        static int mindisindex=0;
        static int count = 0;
        Random r = new Random();

        Brush[] b = { Brushes.Chartreuse, Brushes.DeepPink, Brushes.MidnightBlue, Brushes.OrangeRed, Brushes.Gold, Brushes.Indigo, Brushes.DodgerBlue,Brushes.Lime,Brushes.MediumTurquoise, Brushes.Fuchsia};

        public void drawpoint(int x, int y)
        {
            Graphics g = panel1.CreateGraphics();         
            g.FillEllipse(Brushes.Black, x, y, 10, 10);                      
        }

        public void drawpoint2(int x, int y,Brush brush)
        {
            Graphics g = panel1.CreateGraphics();           
            g.FillEllipse(brush,x,y, 10, 10);
        }

        public void drawpointw(int x, int y)
        {
            Graphics g = panel1.CreateGraphics();            
            g.FillEllipse(Brushes.DimGray, x, y, 10, 10);
        }
        public void drawpointw2(int x, int y)
        {
            Graphics g = panel1.CreateGraphics();
            g.FillRectangle(Brushes.DimGray, x, y, 10, 10);
        }
        public int distance(int x1, int y1, int x2, int y2)
        {
            double x = (double)(x1 - x2);
            double y = (double)(y1 - y2);

            return (int)Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));           
        }

        public void eql()
        {
            for (int i = 0; i < nc; i++)
            {
                clusters[i, 0] = cluster2[i, 0]/nocp[i];
                clusters[i, 1] = cluster2[i, 1]/nocp[i];
                cluster2[i, 0] = 0;
                cluster2[i, 1] = 0;
            }
            for (int i = 0; i < nc; i++)
            {
                nocp[i] = 0;
            }
        }
           
        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (rb1.Checked)
            {
                xy[np, 0] = (int)e.X;
                xy[np, 1] = (int)e.Y;
                drawpoint(e.X, e.Y);
                listBox1.Items.Add(e.X.ToString() + "\t" + (panel1.Height - e.Y).ToString());
                np += 1;
                if (np == 9999)
                {
                   // MessageBox.Show("one more to input");
                }
            }   
            else if(rb2.Checked){

                clusters[nc, 0] = (int)e.X;
                clusters[nc, 1] = (int)e.Y;
                // drawpoint(e.X, e.Y);
                //int N = xy.Count;
                xy[np, 0] = clusters[nc, 0];
                xy[np, 1] = clusters[nc, 1];
                np++;
                Graphics g = panel1.CreateGraphics();
                g.FillRectangle(b[nc], clusters[nc, 0], clusters[nc, 1], 10, 10);
                listBox2.Items.Add(e.X.ToString() + "\t" + (panel1.Height - e.Y).ToString());
                nc += 1;
                if (nc == 9)
                {
                    MessageBox.Show("one more cluster to input");
                }
            }                   
        }

        public void show()
        {
            label1.Text = xy[np - 1, 0].ToString();
            label2.Text = xy[np - 1, 1].ToString();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //label2.Text = listBox1.GetItemText(listBox1.SelectedItem);
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = listBox1.Items.IndexOf(listBox1.SelectedItem);           
            clusters[nc, 0] = xy[index, 0];
            clusters[nc, 1] = xy[index, 1];

            listBox2.Items.Add(clusters[nc, 0] + "\t" + (clusters[nc, 1]-panel1.Height));
            nc++;          
        }
       
        private void button1_Click(object sender, EventArgs e)
        {
            Graphics g = panel1.CreateGraphics();
            g.Clear(Color.DimGray);
            listBox1.Items.Clear();
            listBox2.Items.Clear();
        }

        public void calculate()
        {
            for (int i = 0; i < np; i++)
            {
                for (int j = 0; j < nc; j++)
                {
                    dis[j] = distance(xy[i, 0], xy[i, 1], clusters[j, 0], clusters[j, 1]);                   
                }

                mindisindex = 0;
                for (int k = 0; k < nc - 1; k++)
                {
                    if (dis[k + 1] <= dis[mindisindex])
                    {
                        mindisindex = k + 1;
                    }                  
                }                
                drawpoint2(xy[i, 0], xy[i, 1], b[mindisindex]);
                cluster2[mindisindex, 0] += xy[i, 0];
                cluster2[mindisindex, 1] += xy[i, 1];
                nocp[mindisindex] += 1;                
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {        
            if (count == 0)
            {
                calculate();
                for (int i = 0; i < nc; i++)
                {                   
                    Graphics g = panel1.CreateGraphics();
                    g.FillRectangle(b[i], cluster2[i, 0] / nocp[i], cluster2[i, 1] / nocp[i], 10, 10);
                    
                    drawpointw2(clusters[i, 0], clusters[i, 1]);
                    g.FillEllipse(b[i], clusters[i, 0], clusters[i, 1], 10, 10);

                    listBox2.Items.Clear();                   
                }
                count++;
            }
            else
            {
                for (int i = 0; i < nc; i++) {               
                    Graphics g = panel1.CreateGraphics();
                    g.FillRectangle(Brushes.DimGray, cluster2[i, 0] / nocp[i], cluster2[i, 1] / nocp[i], 10, 10);
                }
                eql();
                calculate();
                for (int i = 0; i < nc; i++)
                {
                    Graphics g = panel1.CreateGraphics();
                    g.FillRectangle(b[i], cluster2[i, 0] / nocp[i], cluster2[i, 1] / nocp[i], 10, 10);
                }
            }                        
        }

        private int randy()
        {         
            return r.Next(0, 620);
        }

        private int randx()
        {
            return r.Next(0, 520);
        }
        private void btnRanGen_Click(object sender, EventArgs e)
        {
            int n = Convert.ToInt32(txtNop.Text);
            for(int i=0;i<n;i++)
            {
                int x = randx();
                int y = randy();
                //int y = 20;
                drawpoint(x, y);
                xy[np, 0] = x;
                xy[np, 1] = y;
                listBox1.Items.Add(x.ToString() + "\t" + y.ToString());
                np += 1;
                if (np == 999)
                {
                    MessageBox.Show("one more to input");
                }
            }
        }

        private void rb1_CheckedChanged(object sender, EventArgs e)
        {
            if (rb1.Checked)
            {
                string ms1 = "Do you wish to genarate points automatically?(<10,000)";
                string ms2 = "Generate Points";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                //DialogResult result = MessageBox.Show(ms1, ms2, buttons);
                if (MessageBox.Show(ms1, ms2, buttons) == DialogResult.Yes)
                {
                    pnlP.Visible = true;
                    pnlP.Enabled = true;
                   //rb1.Enabled = false;
                    rb1.Checked = false;
                }
            }
           
        }

        private void rb2_CheckedChanged(object sender, EventArgs e)
        {
            MessageBox.Show("You can enter 10 points as clusters (^.^)");
            pnlP.Visible = false;
        }
    }
}
