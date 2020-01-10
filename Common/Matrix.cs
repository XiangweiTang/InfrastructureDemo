using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public class Matrix<TRow, TColumn, TValue>
    {
        public TValue DefaultValue { get; set; }
        public HashSet<TRow> RowList { get; private set; } = new HashSet<TRow>();
        public HashSet<TColumn> ColumnList { get; private set; } = new HashSet<TColumn>();
        Dictionary<TRow, Dictionary<TColumn, TValue>> InternalDict = new Dictionary<TRow, Dictionary<TColumn, TValue>>();
        public Matrix()
        {

        }
        public Matrix(TValue defaultValue)
        {
            DefaultValue = defaultValue;
        }
        public TValue this[TRow r, TColumn c]
        {
            get
            {
                Sanity.Requires(RowList.Contains(r), $"Missing row key {r}.");
                Sanity.Requires(ColumnList.Contains(c), $"Missing column key {c}.");
                return InternalDict[r].ContainsKey(c)
                    ? InternalDict[r][c]
                    : DefaultValue;
            }
            set
            {
                if (!RowList.Contains(r))
                {
                    RowList.Add(r);
                    InternalDict.Add(r, new Dictionary<TColumn, TValue>());
                }
                if (!ColumnList.Contains(c))
                    ColumnList.Add(c);
                if (!InternalDict[r].ContainsKey(c))
                    InternalDict[r].Add(c, value);
                else
                    InternalDict[r][c] = value;
            }
        }

        public Matrix<TColumn,TRow,TValue> Transpose(TValue defaultValue)
        {
            Matrix<TColumn, TRow, TValue> m = new Matrix<TColumn, TRow, TValue>(defaultValue);
            foreach (TColumn c in ColumnList)
                foreach (TRow r in RowList)
                    m[c, r] = this[r, c];
            return m;
        }
        public Matrix<TColumn, TRow, TValue> Transpose()
        {
            return Transpose(DefaultValue);
        }
        public Matrix<TRow, TColumn, TValue> RebuildMatrix(HashSet<TRow> newRowSet, HashSet<TColumn> newColumnList,TValue defaultValue)
        {
            Matrix<TRow, TColumn, TValue> m = new Matrix<TRow, TColumn, TValue>(defaultValue);
            foreach(TRow r in newRowSet)
            {
                foreach (TColumn c in newColumnList)
                    if (RowList.Contains(r) && ColumnList.Contains(c))
                        m[r, c] = this[r, c];
            }
            return m;
        }
        public Matrix<TRow, TColumn, TValue> RebuildMatrix(HashSet<TRow> newRowSet, HashSet<TColumn> newColumnList)
        {
            return RebuildMatrix(newRowSet, newColumnList, DefaultValue);
        }
    }
}
