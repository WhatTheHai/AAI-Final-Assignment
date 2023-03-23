using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAI_Final_Assignment_WinForms.Fuzzy.FuzzyTerms
{
    public class AndFuzzyTerm : FuzzyTerm
    {
        AndFuzzyTerm(FuzzyTerm t1)
        {
            terms.Add(t1);
        }

        AndFuzzyTerm(FuzzyTerm t1, FuzzyTerm t2) : this(t1)
        {
            terms.Add(t2);
        }

        AndFuzzyTerm(FuzzyTerm t1, FuzzyTerm t2, FuzzyTerm t3) : this(t1, t2)
        {
            terms.Add(t3);
        }


        public override FuzzyTerm Clone()
        {
            throw new NotImplementedException();
        }

        public override float GetDOM()
        {
            float lowest = float.MaxValue;
            foreach (FuzzyTerm t in terms)
            {
                if (t.GetDOM() < lowest) lowest = t.GetDOM();
            }

            return lowest;
        }

        public override void ClearDom()
        {
            throw new NotImplementedException();
        }

        public override void ORwithDOM(float value)
        {
            throw new NotImplementedException();
        }
    }
}