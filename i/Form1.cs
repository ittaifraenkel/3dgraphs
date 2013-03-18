using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace i
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            comboBox1.Text = "choose a function";
            this.Size = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            panel1.Size = new Size(this.Width - 300, this.Height - 30);
        }
        Graphics g;
        float[,] points;
        float[,][] newpoints;

        public bool Check(string pro)
        {
            int i, g = 0;
            if (pro == "")
            {
                return false;
            }
            for (i = 0; i < pro.Length - 1; i++)
            {
                if ((pro[i] == '+' || pro[i] == '-' || pro[i] == '*' || pro[i] == '/' || pro[i] == '^' || pro[i] == '!') && (pro[i + 1] == '+' || pro[i + 1] == '*' || pro[i + 1] == '/' || pro[i + 1] == '^' || pro[i] == '!'))
                {
                    return false;
                }
            }
            if (pro[0] == '/' || pro[0] == '*' || pro[0] == '^' || pro[0] == '!' || pro[pro.Length - 1] == '/' || pro[pro.Length - 1] == '*' || pro[pro.Length - 1] == '^' || pro[pro.Length - 1] == '!' || pro[pro.Length - 1] == '+')
            {
                return false;
            }
            for (i = 0; i < pro.Length; i++)
            {
                if (pro[i] == '|')
                    g++;
            }
            if (g % 2 != 0)
            {
                return false;
            }
            g = 0;
            for (i = 0; i < pro.Length; i++)
            {
                if (pro[i] == '(')
                {
                    g++;
                }
                if (pro[i] == ')')
                {
                    g--;
                }
                if (g < 0)
                    return false;
            }
            if (g != 0)
                return false;
            for (i = pro.Length - 1; i > -1; i--)
            {
                if ((int)pro[i] < 47 || (int)pro[i] > 58)
                {
                    if (pro[i] != '+' && pro[i] != '-' && pro[i] != '*' && pro[i] != '/' && pro[i] != '(' && pro[i] != ')' && pro[i] != '^' && pro[i] != '!' && pro[i] != '|' && pro[i] != 'x' && pro[i] != 'y')
                    {
                        if (i >= 2)
                        {
                            if (pro[i] == 's' && pro[i - 1] == 'o' && pro[i - 2] == 'c')
                            {
                                i -= 3;
                            }
                            else if (pro[i] == 'n' && pro[i - 1] == 'i' && pro[i - 2] == 's')
                            {
                                i -= 3;
                            }
                            else if (pro[i] == 'n' && pro[i - 1] == 'a' && pro[i - 2] == 't')
                            {
                                i -= 3;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        public float Solve(string pro, float x, float y)
        {
            int i, g, sum = 1;
            for (i = 0; i < pro.Length; i++)
            {
                if (pro[i] == 'x')
                {
                    return Solve(pro.Substring(0, i) + '(' + x.ToString() + ')' + pro.Substring(i + 1), x, y);
                }
                if (pro[i] == 'y')
                {
                    return Solve(pro.Substring(0, i) + '(' + y.ToString() + ')' + pro.Substring(i + 1), x, y);
                }
            }
            for (i = 1; i < pro.Length; i++)
            {
                if (pro[i] == '(')
                {
                    if ((int)pro[i - 1] < 58 && (int)pro[i - 1] > 47)
                    {
                        return Solve(pro.Substring(0, i) + '*' + pro.Substring(i), x, y);
                    }
                }
            }
            for (i = 0; i < pro.Length; i++)
            {
                if (pro[i] == ')')
                {
                    for (g = i; g > -1; g--)
                    {
                        if (pro[g] == '(')
                        {
                            return Solve(pro.Substring(0, g) + Solve(pro.Substring(g + 1, i - g - 1), x, y).ToString() + pro.Substring(i + 1), x, y);
                        }
                    }
                }
            }
            for (i = 1; i < pro.Length; i++)
            {
                if (pro[i] == '|' && pro[i - 1] != '+' && pro[i - 1] != '-' && pro[i - 1] != '/' && pro[i - 1] != '*' && pro[i - 1] != '^' && pro[i - 1] != '!')
                {
                    for (g = i - 1; g > -1; g--)
                    {
                        if (pro[g] == '|')
                        {
                            return Solve(pro.Substring(0, g) + Math.Abs(Solve(pro.Substring(g + 1, i - g - 1), x, y)).ToString() + pro.Substring(i + 1), x, y);
                        }
                    }
                }
            }
            for (i = pro.Length - 1; i > 0; i--)
            {
                if (pro[i] == '+')
                {
                    return Solve(pro.Substring(0, i), x, y) + Solve(pro.Substring(i + 1), x, y);
                }
                if (pro[i] == '-')
                {
                    if (pro[i - 1] == '*' || pro[i - 1] == '/' || pro[i - 1] == '^')
                    {
                        i--;
                        continue;
                    }
                    if (pro[i - 1] == '-')
                    {
                        return Solve(pro.Substring(0, i - 1), x, y) - Solve(pro.Substring(i), x, y);
                    }
                    if (pro[i - 1] == '+')
                    {
                        return Solve(pro.Substring(0, i - 1), x, y) + Solve(pro.Substring(i), x, y);
                    }
                    if (pro[i - 1] == 's')
                    {
                        i -= 3;
                        continue;
                    }
                    if (pro[i - 1] == 'n')
                    {
                        i -= 3;
                        continue;
                    }
                    return Solve(pro.Substring(0, i), x, y) - Solve(pro.Substring(i + 1), x, y);
                }
            }
            for (i = pro.Length - 1; i > -1; i--)
            {
                if (pro[i] == '*')
                {
                    return Solve(pro.Substring(0, i), x, y) * Solve(pro.Substring(i + 1), x, y);
                }
                if (pro[i] == '/')
                {
                    return Solve(pro.Substring(0, i), x, y) / Solve(pro.Substring(i + 1), x, y);
                }
            }
            for (i = pro.Length - 1; i > -1; i--)
            {
                if (pro[i] == '^')
                {
                    return (float)Math.Pow(Solve(pro.Substring(0, i), x, y), Solve(pro.Substring(i + 1), x, y));
                }
                if (pro[i] == '!')
                {
                    for (g = 2; g < Solve(pro.Substring(0, i), x, y) + 1; g++)
                    {
                        sum *= g;
                    }
                    return sum;
                }
            }
            for (i = pro.Length - 1; i > -1; i--)
            {
                for (i = pro.Length - 1; i > -1; i--)
                {
                    if (pro[i] == 'c' && pro[i + 1] == 'o')
                    {
                        return (float)Math.Cos(Solve(pro.Substring(i + 3), x, y));
                    }
                    if (pro[i] == 's' && pro[i + 1] == 'i')
                    {
                        return (float)Math.Sin(Solve(pro.Substring(i + 3), x, y));
                    }
                    if (pro[i] == 't' && pro[i + 1] == 'a')
                    {
                        return (float)Math.Tan(Solve(pro.Substring(i + 3), x, y));
                    }
                }
            }
            if (pro[pro.Length - 1] == 'E')
            {
                if (pro[0] == '-')
                    return -0.00001F;
                else
                    return 0.00001F;
            }
            return float.Parse(pro);
        }


        
        private void DrawGraph(int angle, float s, int num, float min, float max, string func, int movelr, int moveud, int opacity, string prob)
        {
            float maxx = -1000, minx = 1000, miny = 1000, maxy = -1000, maxz = -1000, minz = 1000;
            float x, y, colormul = 0;
            newpoints = new float[num, num][];
            DrawAxis(angle, s, movelr, moveud);
            g = panel1.CreateGraphics();
            points = new float[num, num];
            for (int i = 0; i < points.GetLength(0); i++)
            {
                for (int j = 0; j < points.GetLength(1); j++)
                {
                    x = min + (float)i * (max - min) / num;
                    y = min + (float)j * (max - min) / num;
                    points[i, j] = Solve(prob, x, y);
                    if (maxz < points[i, j])
                        maxz = points[i, j];
                    if (minz > points[i, j])
                        minz = points[i, j];
                    newpoints[i, j] = new float[2];
                    newpoints[i, j][0] = (float)((x + s * y * Math.Cos(angle * Math.PI / 180)));
                    newpoints[i, j][1] = (float)(points[i, j] + s * y * Math.Sin(angle * Math.PI / 180));
                    if (maxy < newpoints[i, j][1])
                        maxy = newpoints[i, j][1];
                    if (miny > newpoints[i, j][1])
                        miny = newpoints[i, j][1];
                    if (maxx < newpoints[i, j][0])
                        maxx = newpoints[i, j][0];
                    if (minx > newpoints[i, j][0])
                        minx = newpoints[i, j][0];
                }
            }
            colormul = 250 / (Math.Max(Math.Abs(maxz), Math.Abs(minz)));
            int fitx = (int)(panel1.Width / (maxx - minx));
            int fity = (int)(panel1.Height / (maxx - minx));
            int fit = Math.Min(fitx, fity);
            for (int i = 1; i < points.GetLength(0); i++)
            {
                for (int j = 1; j < points.GetLength(1); j++)
                {
                    PointF[] p = new PointF[4];
                    p[0] = new PointF((float)(newpoints[i, j][0] * fit + panel1.Width / 2 + movelr), (float)(-newpoints[i, j][1] * fit + panel1.Height / 2 + moveud));
                    p[1] = new PointF((float)(newpoints[i - 1, j][0] * fit + panel1.Width / 2 + movelr), (float)(-newpoints[i - 1, j][1] * fit + panel1.Height / 2 + moveud));
                    p[2] = new PointF((float)(newpoints[i - 1, j - 1][0] * fit + panel1.Width / 2 + movelr), (float)(-newpoints[i - 1, j - 1][1] * fit + panel1.Height / 2 + moveud));
                    p[3] = new PointF((float)(newpoints[i, j - 1][0] * fit + panel1.Width / 2 + movelr), (float)(-newpoints[i, j - 1][1] * fit + panel1.Height / 2 + moveud));
                    g.FillPolygon(GetBrush((int)(points[i, j] * colormul), opacity), p);
                }
            }
            if (checkBox1.Checked == true)
            {
                for (int i = 0; i < points.GetLength(0) - 1; i++)
                {
                    for (int j = 0; j < points.GetLength(1) - 1; j++)
                    {
                        g.DrawLine(new Pen(Color.Black), newpoints[i, j][0] * fit + panel1.Width / 2 + movelr, -newpoints[i, j][1] * fit + panel1.Height / 2 + moveud, newpoints[i + 1, j][0] * fit + panel1.Width / 2 + movelr, -newpoints[i + 1, j][1] * fit + panel1.Height / 2 + moveud);
                        g.DrawLine(new Pen(Color.Black), newpoints[i, j][0] * fit + panel1.Width / 2 + movelr, -newpoints[i, j][1] * fit + panel1.Height / 2 + moveud, newpoints[i, j + 1][0] * fit + panel1.Width / 2 + movelr, -newpoints[i, j + 1][1] * fit + panel1.Height / 2 + moveud);
                    }
                }
                for (int j = 0; j < points.GetLength(1) - 1; j++)
                {
                    g.DrawLine(new Pen(Color.Black), newpoints[num - 1, j][0] * fit + panel1.Width / 2 + movelr, -newpoints[num - 1, j][1] * fit + panel1.Height / 2 + moveud, newpoints[num - 1, j + 1][0] * fit + panel1.Width / 2 + movelr, -newpoints[num - 1, j + 1][1] * fit + panel1.Height / 2 + moveud);
                    g.DrawLine(new Pen(Color.Black), newpoints[j, num - 1][0] * fit + panel1.Width / 2 + movelr, -newpoints[j, num - 1][1] * fit + panel1.Height / 2 + moveud, newpoints[j + 1, num - 1][0] * fit + panel1.Width / 2 + movelr, -newpoints[j + 1, num - 1][1] * fit + panel1.Height / 2 + moveud);
                }
            }
        }

        private Pen GetPen(int z)
        {
            if (z < 0)
            {
                int b = Math.Min(250, Math.Abs(z));
                return new Pen(Color.FromArgb(0, 0, b));
            }
            else
            {
                int a = Math.Min(z, 250);
                return new Pen(Color.FromArgb(a, 0, 0));
            }
        }

        private SolidBrush GetBrush(int z, int opacity)
        {
            if (z < 0)
            {
                int b = Math.Min(250, Math.Abs(z));
                return new SolidBrush(Color.FromArgb(opacity, 0, 0, b));
            }
            else
            {
                int a = Math.Min(z, 250);
                return new SolidBrush(Color.FromArgb(opacity, a, 0, 0));
            }
        }

        private void DrawAxis(int t, float s, int movelr, int moveup)
        {
            g = panel1.CreateGraphics();
            float x = 0 + 500 * (float)(Math.Cos(t * Math.PI / 180)) * s;
            float y = 0 + 500 * (float)(Math.Sin(t * Math.PI / 180)) * s;
            float x2 = 0 + -500 * (float)(Math.Cos(t * Math.PI / 180)) * s;
            float y2 = 0 + -500 * (float)(Math.Sin(t * Math.PI / 180)) * s;
            g.DrawLine(Pens.Green, panel1.Width / 2 + movelr, panel1.Height / 2 - 300 + moveud, panel1.Width / 2 + movelr, panel1.Height / 2 + 300 + moveud);
            g.DrawLine(Pens.Green, panel1.Width / 2 - 400 + movelr, panel1.Height / 2 + moveud, panel1.Width / 2 + 400 + movelr, panel1.Height / 2 + moveud);
            g.DrawLine(Pens.Green, x2 + panel1.Width / 2 + movelr, -y2 + panel1.Height / 2 + moveud, x + panel1.Width / 2 + movelr, -y + panel1.Height / 2 + moveud);
        }

        int angle = 50;
        float s = 0.5F;
        int res = 30;
        float min = -5;
        float max = 5;
        int movelr = 0;
        int moveud = 0;
        int opacity = 250;
        private void button1_Click(object sender, EventArgs e)
        {
            draw3D(false);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            draw3D(false);
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            draw3D(false);
        }

        public void draw3D(bool a)
        {
            string pro = comboBox1.Text;
            pro = pro.Replace(" ", "");
            if (Check(pro))
            {
                bool b = false;
                g = panel1.CreateGraphics();
                string order = comboBox1.Text;
                angle = int.Parse(numericUpDown1.Text);
                s = float.Parse(numericUpDown2.Text);
                res = hScrollBar1.Value;
                opacity = hScrollBar2.Value;
                if (!a)
                {
                    moveud = 0;
                    movelr = 0;
                }
                try
                {
                    min = float.Parse(textBox1.Text);
                    max = float.Parse(textBox2.Text);
                    b = true;
                }
                catch
                {
                    MessageBox.Show("You must enter a valid viewing range");
                    textBox1.BackColor = Color.Red;
                    textBox2.BackColor = Color.Red;
                }
                if (b)
                {
                    textBox1.BackColor = Color.White;
                    textBox2.BackColor = Color.White;
                    g.Clear(panel1.BackColor);
                    DrawGraph(angle, s, res, min, max, comboBox1.Text, movelr, moveud, opacity, pro);
                }
            }
            else
            {
                MessageBox.Show("your function is problematic...");
            }
        }

        public float Function(float x, float y, string func)
        {
            switch (func)
            {
                case "x^2 + y^2":
                    return (float)(Math.Pow(x, 2) + Math.Pow(y, 2));
                case "x + y":
                    return x + y;
                case "sin(x) + cos(y)":
                    return (float)(Math.Sin(x) + Math.Cos(y));
                case "cos(x + y)":
                    return (float)(Math.Cos(x + y));
                case "cos(x^2 + sin(y^2))":
                    return (float)(Math.Cos(Math.Pow(x, 2) + Math.Sin(Math.Pow(y, 2))));
                case "sin(sqrt(x^2+y^2))/sqrt(x^2+y^2)":
                    {
                        if (x == 0 && y == 0)
                        {
                            return 1;
                        }
                        else
                        {
                            double temp = (Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2)));
                            double tmp = Math.Sin(temp) / temp;
                            return (float)tmp;
                        }
                    }
            }
            return 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool b = false;
            try
            {
                movelr -= int.Parse(textBox3.Text);
                b = true;
            }
            catch
            {
                MessageBox.Show("Please check that you have entered a valid moving number");
                textBox3.BackColor = Color.Red;
            }
            if (b)
            {
                textBox3.BackColor = Color.White;
                draw3D(true);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            bool b = false;
            try
            {
                movelr += int.Parse(textBox3.Text);
                b = true;
            }
            catch
            {
                MessageBox.Show("Please check that you have entered a valid moving number");
                textBox3.BackColor = Color.Red;
            }
            if (b)
            {
                textBox3.BackColor = Color.White;
                draw3D(true);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            bool b = false;
            try
            {
                moveud -= int.Parse(textBox3.Text);
                b = true;
            }
            catch
            {
                MessageBox.Show("Please check that you have entered a valid moving number");
                textBox3.BackColor = Color.Red;
            }
            if (b)
            {
                textBox3.BackColor = Color.White;
                draw3D(true);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            bool b = false;
            try
            {
                moveud += int.Parse(textBox3.Text);
                b = true;
            }
            catch
            {
                MessageBox.Show("Please check that you have entered a valid moving number");
                textBox3.BackColor = Color.Red;
            }
            if (b)
            {
                textBox3.BackColor = Color.White;
                draw3D(true);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            DrawAxis(angle, s, movelr, moveud);
        }
    }
}
