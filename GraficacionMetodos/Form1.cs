using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraficacionMetodos
{
    public partial class Form1 : Form
    {
        int zoom = 10;
        ArrayList ar = new ArrayList();
        ArrayList arban = new ArrayList();
        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private void btnCalcular_Click(object sender, EventArgs e)
        {
            if (txtX1.Text == "" || txtX2.Text == "" || txtY1.Text == "" || txtY2.Text == "")
            {
                MessageBox.Show("Debe llenar todos los campos");
            }
            else
            {
                dtgDatos.DataSource = null;
                dtgDatos.Rows.Clear();
                dtgDatos.Columns.Clear();
                dtgDatos.Refresh();
                repintar();
                ar.Clear();
                arban.Clear();
                if (rbBasico.Checked == true)
                {
                    metodoBasico();
                    labelmetodo.Text = "Algoritmo: " + rbBasico.Text;
                }
                if (rbDDA.Checked == true)
                {
                    dda();
                    labelmetodo.Text = "Algoritmo: " + rbDDA.Text;
                }
                if (rbBresenham.Checked == true)
                {
                    Bresenham();
                    labelmetodo.Text = "Algoritmo: " + rbBresenham.Text;
                }
                pixel();
                txtX1.Clear();
                txtX2.Clear();
                txtY1.Clear();
                txtY2.Clear();
                txtX1.Focus();
                foreach (Puntos item in ar)
                {
                    arban.Add(new Puntos(item.x,item.y));
                }
                foreach (Puntos item in arban)
                {
                    item.x = item.x / zoom;
                    item.y = item.y / zoom;
                }
                dtgDatos.DataSource = arban;

            }
            
        }


        private void pictureBox1_LoadCompleted(object sender, AsyncCompletedEventArgs e)
        {
        }


        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            SolidBrush brochablack = new SolidBrush(Color.Black);
            int ejex = panelpintar.Height / 2;
            int ejey = panelpintar.Width / 2;
            planocartesiano(g);
            ejes(g, ejex, ejey);
            g.FillRectangle(brochablack, ejey, ejex, 0, 0); //centro
        }

        private void repintar()
        {
            panelpintar.Refresh();
            Graphics g = panelpintar.CreateGraphics();
            SolidBrush brochablack = new SolidBrush(Color.Black);
            int ejex = panelpintar.Height / 2;
            int ejey = panelpintar.Width / 2;
            planocartesiano(g);
            ejes(g, ejex, ejey);
            g.FillRectangle(brochablack, ejey, ejex, 0, 0); //centro
        }


        private void planocartesiano(Graphics g)
        {
            Pen penGray = new Pen(Color.Gray);
            int cont = 0;
            int w = panelpintar.Width;
            int w2 = panelpintar.Width / 2;
            int h = panelpintar.Height;
            int h2 = panelpintar.Height / 2;

            for (int i = 0; i <= panelpintar.Width/2; i += zoom)
            {
                g.DrawLine(penGray,w2 - i, 0, w2 - i, h);
                g.DrawLine(penGray,w2 + i, 0, w2 + i, h);
            }

            for (int i = 0; i <= h2; i += zoom)
            {
                g.DrawLine(penGray,0, h2 - i, w, h2 - i);
                g.DrawLine(penGray,0, h2 + i, w, h2 + i);
            }
        }

        private void ejes(Graphics g, int ejex, int ejey)
        {
            Pen penRoja = new Pen(Color.Red, zoom);
            g.DrawLine(penRoja, 0, ejex + zoom/2, panelpintar.Width, ejex + zoom/2);
            g.DrawLine(penRoja, ejey + zoom/2, 0, ejey + zoom/2, panelpintar.Height);
        }


        private void pixel()
        {
            Graphics g;
            g = panelpintar.CreateGraphics();
            int ejex = panelpintar.Height / 2;
            int ejey = panelpintar.Width / 2;
            SolidBrush brochablack = new SolidBrush(Color.Black);
            foreach (Puntos item in ar)
            {
                int x = ejey + item.x;
                int y = ejex - item.y;
                g.FillRectangle(brochablack, x, y, zoom, zoom);
            }
        }


        private void metodoBasico()
        {
            int x1 = Convert.ToInt32(txtX1.Text);
            int y1 = Convert.ToInt32(txtY1.Text);
            int x2 = Convert.ToInt32(txtX2.Text);
            int y2 = Convert.ToInt32(txtY2.Text);
            double arriba, abajo, m = 0;
            arriba = y2 - y1;
            abajo = x2 - x1;
            m = arriba / abajo;
            txtM.Text = arriba+ " / "+ abajo;
            txtDy.Text = Convert.ToString(y2 - y1);
            txtDx.Text = Convert.ToString(x2 - x1);
            if (x2 - x1 >= y2 - y1)
            {
                if (x1 < 0 && x2 < 0)
                {
                    for (int i = x1; i >= x2; i--)
                    {
                        int x = i;
                        int y = Convert.ToInt32(Math.Round(((m * (x - x1) + y1))));
                        ar.Add(new Puntos(x * zoom, y * zoom));
                    }
                }
                else
                {
                    for (int i = x1; i <= x2; i++)
                    {
                        int x = i;
                        int y = Convert.ToInt32(Math.Round(((m * (x - x1) + y1))));
                        ar.Add(new Puntos(x * zoom, y * zoom));
                    }
                }
            }
            else
            {
                for (int i = y1; i <= y2; i++)
                {
                    int y = i;
                    int x = Convert.ToInt32(Math.Round(((y - y1) / m) + x1));
                    ar.Add(new Puntos(x * zoom, y * zoom));
                }
            }
        }


        private void dda()
        {
            int x1 = Convert.ToInt32(txtX1.Text);
            int y1 = Convert.ToInt32(txtY1.Text);
            int x2 = Convert.ToInt32(txtX2.Text);
            int y2 = Convert.ToInt32(txtY2.Text);
            int dx = x2 - x1, dy = y2 - y1, pasos, k, xfinal, yfinal;
            txtM.Text = (y2 - y1) + " / " + (x2 - x1) ;
            txtDy.Text = Convert.ToString(y2 - y1);
            txtDx.Text = Convert.ToString(x2 - x1);
            float incrementoX, incrementoY, x = x1, y = y1;
            if (Math.Abs(dx) > Math.Abs(dy)) pasos = Math.Abs(dx);
            else pasos = Math.Abs(dy);
            incrementoX = dx / (float)pasos;
            incrementoY = dy / (float)pasos;
            ar.Add(new Puntos(x1 * zoom, y1 * zoom));
            for (k = 0; k < pasos; k++)
            {
                x = x+ incrementoX;
                xfinal = Convert.ToInt32(x);
                y = y+ incrementoY;
                yfinal = Convert.ToInt32(y);
                ar.Add(new Puntos(xfinal * zoom, yfinal * zoom));
            }
        }


        private void Bresenham()
        {

            int x1 = Convert.ToInt32(txtX1.Text);
            int y1 = Convert.ToInt32(txtY1.Text);
            int x2 = Convert.ToInt32(txtX2.Text);
            int y2 = Convert.ToInt32(txtY2.Text);
            int error,dn,dp;
            int difx = x2 - x1;
            int dify = y2 - y1;
            if(Math.Abs(difx) > Math.Abs(dify))
            {
                error = (2 * dify) - difx;
                dn = error + difx;
                dp = error - difx;
                int y = y1;
                for (int i = x1; i <= x2; i++)
                {
                    ar.Add(new Puntos(i*zoom,y*zoom));
                    if(error < 0)
                    {
                        error = error + dn;
                    }
                    else
                    {
                        error = error + dp;
                        y = y + 1;
                    }
                }
            }
        }

        private void txtX1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Para obligar a que sólo se introduzcan números 
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else
              if (Char.IsControl(e.KeyChar)) //permitir teclas de control como retroceso 
            {
                e.Handled = false;
            }
            else
            {
                //el resto de teclas pulsadas se desactivan 
                e.Handled = true;
            }
        }

        private void txtX2_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Para obligar a que sólo se introduzcan números 
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else
              if (Char.IsControl(e.KeyChar)) //permitir teclas de control como retroceso 
            {
                e.Handled = false;
            }
            else
            {
                //el resto de teclas pulsadas se desactivan 
                e.Handled = true;
            }
        }

        private void txtY1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Para obligar a que sólo se introduzcan números 
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else
              if (Char.IsControl(e.KeyChar)) //permitir teclas de control como retroceso 
            {
                e.Handled = false;
            }
            else
            {
                //el resto de teclas pulsadas se desactivan 
                e.Handled = true;
            }
        }

        private void txtY2_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Para obligar a que sólo se introduzcan números 
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else
              if (Char.IsControl(e.KeyChar)) //permitir teclas de control como retroceso 
            {
                e.Handled = false;
            }
            else
            {
                //el resto de teclas pulsadas se desactivan 
                e.Handled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
                foreach (Puntos item in ar)
                {
                    item.x = item.x / zoom;
                    item.y = item.y / zoom;
                }
                zoom = zoom + 5;
                foreach (Puntos item in ar)
                {
                    item.x = item.x * zoom;
                    item.y = item.y * zoom;
                }
                repintar();
                pixel();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (zoom > 10)
            {
                foreach (Puntos item in ar)
                {
                    item.x = item.x / zoom;
                    item.y = item.y / zoom;
                }
                zoom = zoom - 5;
                foreach (Puntos item in ar)
                {
                    item.x = item.x * zoom;
                    item.y = item.y * zoom;
                }
                repintar();
                pixel();
            }
        }
    }
}