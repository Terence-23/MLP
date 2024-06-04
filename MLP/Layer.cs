using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MLP
{
    internal class Layer
    {
        public float[] inputs;
        public float[] outputs;

        Perceptron[] perceptrons;

        public Layer(Perceptron[] perceptrons)
        {
            this.perceptrons = perceptrons;
        }
        public void Randomize()
        {
            foreach (var p in perceptrons)
            {
                p.Randomize();
            }
        }
        public Layer(int NPerceptrons, int NInputs, Func<float, float> f) {
            perceptrons = new Perceptron[NPerceptrons];
            for (int i = 0; i < NPerceptrons; i++) { perceptrons[i] = new Perceptron(NInputs, f); }
        }
        public void Adjust()
        {
            foreach (var p in perceptrons)
            {
                p.Adjust();
            }
        }

        public float[] PropagateErrors(float[] errors)
        {
            float[] next_l = new float[inputs.Length];

            for (int i = 0; i < inputs.Length; i++)
            {
                next_l[i] = 0;
            }

            for (int i = 0; i< perceptrons.Length; i++)
            {
                perceptrons[i].error = errors[i];
                for (int j = 0; j < inputs.Length; j++)
                {
                    next_l[j] += perceptrons[i].Weights[j] * errors[i];
                }
            }

            return next_l;
        }

        public void Activate()
        {
            outputs = new float[perceptrons.Length];
            for (int i = 0; i < perceptrons.Length; i++) {
                perceptrons[i].Inputs = inputs;
                perceptrons[i].Activate();
                outputs[i] = perceptrons[i].Output;
            }
        }

    }
}
