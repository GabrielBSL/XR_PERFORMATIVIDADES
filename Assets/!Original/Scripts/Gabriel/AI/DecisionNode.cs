using System;
using System.Collections.Generic;
using System.Linq;

namespace Main.AI
{
    public class DecisionNode
    {
        public int AttributeIndex { get; set; }
        public float Threshold { get; set; }
        public DecisionNode TrueBranch { get; set; }
        public DecisionNode FalseBranch { get; set; }
        public int? Class { get; set; }
    }
}