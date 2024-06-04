using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLP
{
    static public class Activators
    {
        static public float Sigmoid5(float x)
        {
            return 1 / (1 + MathF.Exp(-5*x));
        }
        static public float Threshold (float x)
        {
            if (x > 0 ) { return 1; }
            return 0;
        }
        static public float Clamp(float x)
        {
            return Math.Clamp(x, 0, 1);
        }
    }
    internal class Perceptron
    {
        public int N;
        public float[] Weights;
        public float[] Inputs;
        public float Output;
        public float error;

        readonly Func<float, float> f;

        public Perceptron(float[] weights, Func<float, float> f)
        {
            N = weights.Length;
            Inputs = new float[N];
            Weights = weights;
            this.f = f;
        }
        public Perceptron(int N,Func<float, float>f) {
            var prng =new Random();
            this.N=N;
            Inputs = new float[N];
            Weights = new float[N];
            this.f = f;
            for (int i = 0; i < N; i++)
            {
                Weights[i] = (float)(prng.NextDouble()* 2 - 1);
            }
        }

        public void Activate()
        {
            var s = 0.0f;
            for (var i = 0; i < N; i++)
            {
                s += Weights[i] * Inputs[i];
            }
            Output = f(s);
        }

        public void Adjust()
        {
            for (var i = 0;i < N; i++)
            {
                Weights[i] += 0.01f * Inputs[i] * error * Output * (1 - Output);
            }
        }

        internal void Randomize()
        {
            var prng = new Random();
            for (int i = 0; i < N; i++)
            {
                Weights[i] = (float)(prng.NextDouble() * 2 - 1);
            }
        }
    }
}
