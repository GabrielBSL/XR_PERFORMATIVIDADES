
using System;
using System.Collections.Generic;
using System.Linq;

using Random = UnityEngine.Random;

namespace Main.AI
{
    public class DecisionTree
    {
        private int _maxDepth = 4;

        public DecisionNode BuildTree(List<DataPoint> data, int currentDepth = 0, int currentClass = -1)
        {
            if (currentDepth > _maxDepth || data.Select(d => d.Class).Distinct().Count() == 1)
            {
                return new DecisionNode { Class = currentClass };
            }

            (int randoAttributeIndex, float bestThreshold) = FindBestSplit(data);

            var trueData = data.Where(d => d.Attributes[randoAttributeIndex] >= bestThreshold).ToList();
            var falseData = data.Where(d => d.Attributes[randoAttributeIndex] < bestThreshold).ToList();

            return new DecisionNode
            {
                AttributeIndex = randoAttributeIndex,
                Threshold = bestThreshold,
                TrueBranch = BuildTree(trueData, currentDepth + 1, trueData[0].Class),
                FalseBranch = BuildTree(falseData, currentDepth + 1, falseData[0].Class)
            };
        }

        private (int, float) FindBestSplit(List<DataPoint> data)
        {
            int bestAttributeIndex = Random.Range(0, data[0].Attributes.Length);
            float bestThreshold = FindBestThreshold(data, bestAttributeIndex);
            return (bestAttributeIndex, bestThreshold);
        }

        private float FindBestThreshold(List<DataPoint> data, int attributeIndex)
        {
            float bestThreshold = float.NaN;
            double bestGini = double.MaxValue;

            var thresholds = data.Select(d => d.Attributes[attributeIndex]).Distinct().OrderBy(x => x).ToList();
            foreach (var threshold in thresholds)
            {
                var (trueData, falseData) = SplitData(data, attributeIndex, threshold);
                double gini = CalculateGiniImpurity(trueData, falseData);
                if (gini < bestGini)
                {
                    bestGini = gini;
                    bestThreshold = threshold;
                }
            }

            return bestThreshold;
        }

        private (List<DataPoint>, List<DataPoint>) SplitData(List<DataPoint> data, int attributeIndex, float threshold)
        {
            var trueData = new List<DataPoint>();
            var falseData = new List<DataPoint>();

            foreach (var point in data)
            {
                if (point.Attributes[attributeIndex] >= threshold)
                {
                    trueData.Add(point);
                }
                else
                {
                    falseData.Add(point);
                }
            }

            return (trueData, falseData);
        }

        private double CalculateGiniImpurity(List<DataPoint> trueData, List<DataPoint> falseData)
        {
            int totalCount = trueData.Count + falseData.Count;
            if (totalCount == 0) return 0.0;

            double trueProportion = (double)trueData.Count / totalCount;
            double falseProportion = (double)falseData.Count / totalCount;

            double trueGini = 1.0 - trueData.GroupBy(d => d.Class).Sum(g => Math.Pow((double)g.Count() / trueData.Count, 2));
            double falseGini = 1.0 - falseData.GroupBy(d => d.Class).Sum(g => Math.Pow((double)g.Count() / falseData.Count, 2));

            return trueProportion * trueGini + falseProportion * falseGini;
        }
    }

}