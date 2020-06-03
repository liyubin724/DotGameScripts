using System.Collections.Generic;

namespace PureMVCWPF.Model.Enum
{
    public class DeptEnum
    {
        public static readonly DeptEnum NONE_SELECTED = new DeptEnum("--None Selected--", -1);
        public static readonly DeptEnum ACCT = new DeptEnum("Accounting", 0);
        public static readonly DeptEnum SALES = new DeptEnum("Sales", 1);
        public static readonly DeptEnum PLANT = new DeptEnum("Plant", 2);
        public static readonly DeptEnum SHIPPING = new DeptEnum("Shippting", 3);
        public static readonly DeptEnum QC = new DeptEnum("Quality Control", 4);

        private int m_ordinal;
        public int Ordinal
        {
            get { return m_ordinal; }
        }

        public string m_value;
        public string Value
        {
            get
            {
                return m_value;
            }
        }

        public DeptEnum(string v,int o)
        {
            m_value = v;
            m_ordinal = o;
        }

        public static IList<DeptEnum> List
        {
            get 
            {
                List<DeptEnum> l = new List<DeptEnum>();
                
                l.Add(ACCT);
                l.Add(SALES);
                l.Add(PLANT);

                return l;
            }
        }

        public static IList<DeptEnum> ComboList
        {
            get
            {
                IList<DeptEnum> l = List;
                l.Insert(0, NONE_SELECTED);
                return l;
            }
        }

        public bool Euqals(DeptEnum e)
        {
            if (e == null) return false;
            return e.Value == Value && e.Ordinal == Ordinal;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
