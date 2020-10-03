using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    /// <summary>
    /// The matrix class. This class is for dealing with 2-dimesion data.
    /// One simplest application is a rectangle table.
    /// </summary>
    /// <typeparam name="TRow">The type of row.</typeparam>
    /// <typeparam name="TColumn">The type of column.</typeparam>
    /// <typeparam name="TValue">The type of value.</typeparam>
    public class Matrix<TRow, TColumn, TValue>
    {
        /// <summary>
        /// The default value. If one cell has not value set, it will be set as the default value.
        /// </summary>
        TValue DefaultValue;
        public Matrix() { }
        public Matrix(TValue defaultValue) { DefaultValue = defaultValue; }
        /// <summary>
        /// The internal dictionary, which keeps the data.
        /// </summary>
        protected Dictionary<TRow, Dictionary<TColumn, TValue>> InternalDict { get; set; } = new Dictionary<TRow, Dictionary<TColumn, TValue>>();
        /// <summary>
        /// The set contains all the keys in the row.
        /// </summary>
        public HashSet<TRow> RowSet { get; protected set; } = new HashSet<TRow>();
        /// <summary>
        /// The set contains all the keys in the column.
        /// </summary>
        public HashSet<TColumn> ColumnSet { get; protected set; } = new HashSet<TColumn>();
        /// <summary>
        /// The external way of get/set the values in the matrix.
        /// </summary>
        /// <param name="r">The value of row.</param>
        /// <param name="c">The value of column.</param>
        /// <returns>The respective value.</returns>
        public TValue this[TRow r, TColumn c]
        {
            get
            {
                if (!RowSet.Contains(r))
                    throw new InfException($"Row [{r}] doesn't exist.");
                if (!ColumnSet.Contains(c))
                    throw new InfException($"Column [{c}] doesn't exist.");
                if (InternalDict[r].ContainsKey(c))
                    return InternalDict[r][c];
                return DefaultValue;
            }
            set
            {
                if (!RowSet.Contains(r))
                {
                    RowSet.Add(r);
                    InternalDict[r] = new Dictionary<TColumn, TValue>();
                }
                if (!ColumnSet.Contains(c))
                    ColumnSet.Add(c);
                InternalDict[r][c] = value;
            }
        }
        /// <summary>
        /// Generates a transposed matrix.
        /// </summary>
        /// <returns>The transposed matrix.</returns>
        public Matrix<TColumn,TRow,TValue> Transpose()
        {
            Matrix<TColumn, TRow, TValue> tMatrix = new Matrix<TColumn, TRow, TValue>(DefaultValue);
            foreach (TColumn c in ColumnSet)
            {
                foreach(TRow r in RowSet)
                {
                    tMatrix[c, r] = this[r, c];
                }
            }
            return tMatrix;
        }
        /// <summary>
        /// Generate a new matrix with a new row set and column set.
        /// If the new row/column is in the original row/column, then set the respective value.
        /// Otherwise, do nothing(and it will be assigned with the default value).
        /// </summary>
        /// <param name="newRowSet">The set contains new rows.</param>
        /// <param name="newColumnSet">The set contains new columns.</param>
        /// <returns></returns>
        public Matrix<TRow,TColumn,TValue>RebuldMatrix(HashSet<TRow> newRowSet, HashSet<TColumn> newColumnSet)
        {
            Matrix<TRow, TColumn, TValue> newMatrix = new Matrix<TRow, TColumn, TValue>(DefaultValue);
            foreach(TRow r in newRowSet)
            {
                foreach(TColumn c in newColumnSet)
                {
                    if (RowSet.Contains(r) && ColumnSet.Contains(C))
                        newMatrix[r, c] = this[r, c];
                }
            }
            return newMatrix;
        }
    }
}
