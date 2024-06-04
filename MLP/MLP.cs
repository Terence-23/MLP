using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLP
{
    internal class MLP
    {
        public int NInputs;
        public readonly int NOutputs;
        public int NLayers;
        public int[] NPerceptrons;

        public float[] outputs;
        Layer[] layers;

        public MLP(Layer[] layers)
        {
            this.layers = layers;
        }

        public MLP(int nInputs, int nLayers, int[] NPerceptrons)
        {
            NInputs = nInputs;
            NLayers = nLayers;
            this.NPerceptrons = NPerceptrons;
            layers = new Layer[nLayers];
            layers[0] = new Layer(NPerceptrons[0], NInputs, Activators.Sigmoid5);
            NOutputs = NPerceptrons[0];
            for (int i = 1; i < nLayers; i++)
            {
                layers[i] = new Layer(NPerceptrons[i], NPerceptrons[i - 1], Activators.Sigmoid5);
                NOutputs = NPerceptrons[i];
            }
        }
        public void RandomizeWeights()
        {
            foreach (var l in layers)
            {
                l.Randomize();
            }
        }
        public void Teach(float[] input, float[] expected_output)
        {
            var output = Activate(input);
            float[] errors = new float[output.Length];

            for (int i = 0; i < output.Length; i++)
            {
                errors[i] = expected_output[i] - output[i];
            }

            for (int i = layers.Length -1;  i >= 0; i--)
            {
                errors = layers[i].PropagateErrors(errors);
                layers[i].Adjust();
            }
           }

        public float[] Activate(float[] input)
        {
            outputs = input;
            foreach (var layer in layers)
            {
                layer.inputs = outputs;
                layer.Activate();
                outputs = layer.outputs;
            }
            return outputs;
        }
    }
}
