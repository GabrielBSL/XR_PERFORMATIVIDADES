using System;
using System.Collections.Generic;
using System.Linq;

namespace Main.AI
{
    public class RandomForest
    {
        private List<DecisionNode> trees = new List<DecisionNode>();
        private int numberOfTrees;
        private Random random = new Random();

        public RandomForest(int numberOfTrees)
        {
            this.numberOfTrees = numberOfTrees;
        }

        public void Train(List<DataPoint> data)
        {
            for (int i = 0; i < numberOfTrees; i++)
            {
                var sample = BootstrapSample(data);
                var tree = new DecisionTree();
                var root = tree.BuildTree(sample);
                trees.Add(root);
            }

            ForestAnimationDecisionMaker.PrintValue("random forest trained");
        }

        public ForestResult Predict(DataPoint sample)
        {
            var predictions = new List<int>();
            foreach (var tree in trees)
            {
                predictions.Add(Classify(sample, tree));
            }

            ForestResult results = new ForestResult();
            predictions.GroupBy(x => x).OrderByDescending(x => x.Count());

            Dictionary<int, int> frequency = new Dictionary<int, int>();
            
            foreach (var prediction in predictions)
            {
                if (frequency.ContainsKey(prediction))
                {
                    frequency[prediction]++;
                }
                else
                {
                    frequency[prediction] = 1;
                }
            }
            List<(int, float)> result = new List<(int, float)>();

            foreach (var pair in frequency)
            {
                result.Add((pair.Key, (float)pair.Value / predictions.Count));
            }

            return new ForestResult() { results = result};
        }

        private List<DataPoint> BootstrapSample(List<DataPoint> data)
        {
            var sample = new List<DataPoint>();
            for (int i = 0; i < data.Count; i++)
            {
                int index = UnityEngine.Random.Range(0, data.Count);
                sample.Add(data[index]);
            }

            return sample;
        }

        private int Classify(DataPoint dataPoint, DecisionNode node)
        {
            if (node.Class.HasValue)
            {
                return node.Class.Value;
            }

            if (dataPoint.Attributes[node.AttributeIndex] >= node.Threshold)
            {
                return Classify(dataPoint, node.TrueBranch);
            }
            else
            {
                return Classify(dataPoint, node.FalseBranch);
            }
        }
    }
}