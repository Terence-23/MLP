using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MLP
{
    public partial class Form1 : Form
    {

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        List<(float[], float[])> test_inputs = new List<(float[], float[])>();
        Label[] labels;
        MLP mlp;
        int c_image = 0;
        Bitmap img;
        public Form1()
        {
            InitializeComponent();
            AllocConsole();
            labels = new Label[] { label1, label2, label3, label4, label5, label6, label7, label8, label9, label10 };

            mlp = new MLP(784, 3, new int[] { 784, 100, 10 });
            string fileName = @"D:\Programowanie\Programowanie zaawansowane - zajęcia\MLP\Data\digits.txt";

            foreach (var l in File.ReadLines(fileName))
            {
                var chars = l.ToString().Split(",");
                var list = new float[28*28];
                for (int i = 1; i < chars.Length; i++)
                {
                    list[i-1] = (Int32.Parse(chars[i]) / 255.0f);
                }
                float[] correct = new float[10];

                for (int i = 0; i < 10; i++)
                {
                    correct[i] = 0.0f;
                }
                correct[Int32.Parse(chars[0])] = 1.0f;

                test_inputs.Add((correct, list));
            }

            UpdateData();
        }
        public void UpdateData()
        {
            var expected = 0;
            mlp.Teach(test_inputs[c_image].Item2, test_inputs[c_image].Item1);
            var max = -10.0;
            var ind = 0;
            for (int i = 0;i < mlp.outputs.Length; i++)
            {
                if (test_inputs[c_image].Item1[i] == 1)
                {
                    expected = i;
                }
                if (mlp.outputs[i] > max)
                {
                    ind = i;
                    max = mlp.outputs[i];
                }
            }
            label11.Text = expected.ToString();

            
            for (int i = 0; i < 10; i++)
            {
                labels[i].Text = $"{i}: { mlp.outputs[i]}";
                labels[i].ForeColor = i == ind ? Color.Red : Color.Black;
            }
            Draw();
        }
        public void Draw()
        {
            img = new Bitmap(28, 28);
            for (int i = 0; i < 28; i++)
            {
                for (int j = 0; j < 28; j++)
                {
                    img.SetPixel(j, i, Color.FromArgb(
                        (int)(test_inputs[c_image].Item2[i * 28 + j] * 255),
                        (int)(test_inputs[c_image].Item2[i * 28 + j] * 255),
                        (int)(test_inputs[c_image].Item2[i * 28 + j] * 255)
                        ));
                }
            }
            pictureBox1.Invalidate();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            c_image--;
            if (c_image < 0) { c_image = test_inputs.Count-1; }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            var stopWatch = System.Diagnostics.Stopwatch.StartNew();
            Console.WriteLine(test_inputs.Count);
            for (int i = 0;i < test_inputs.Count;i++)
            {
                c_image = i;
                Console.WriteLine(i);
                UpdateData();
            }
            Console.WriteLine("Done");
            stopWatch.Stop();
            Console.WriteLine($"Elapsed: {stopWatch.Elapsed.Minutes}min, {stopWatch.Elapsed.TotalSeconds - stopWatch.Elapsed.Minutes * 60}s");
        }
        private void button1_Click(object sender, EventArgs e)
        {
            c_image++;
            if (c_image >= test_inputs.Count) { c_image = 0; }
            UpdateData();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.ScaleTransform(10, 10);
            g.DrawImageUnscaled(img, 0, 0);
            
        }
    }
}
