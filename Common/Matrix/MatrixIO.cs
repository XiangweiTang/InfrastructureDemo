using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Common
{
    /// <summary>
    /// This class is for the IO of matrix.
    /// </summary>
    public static class MatrixIO
    {
        /// <summary>
        /// From a matrix to text, with text seperated value.
        /// </summary>
        /// <typeparam name="TRow">The type of the row.</typeparam>
        /// <typeparam name="TColumn">The type of the column.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="matrix">The matrix.</param>
        /// <param name="withRowHeader">Whether to output row header or not.</param>
        /// <param name="withColumnHeader">Whether to output column header or not.</param>
        /// <returns>The output text.</returns>
        public static IEnumerable<string> ToText<TRow, TColumn, TValue>(this Matrix<TRow, TColumn, TValue> matrix, bool withRowHeader = true, bool withColumnHeader = true)
        {
            StringBuilder sb = new StringBuilder();
            if (withColumnHeader)
            {
                string pureColumnHeader = string.Join("\t", matrix.ColumnSet.Select(x => x.ToString()));
                if (withRowHeader)
                    yield return $"\t{pureColumnHeader}";
                else
                    yield return pureColumnHeader;
            }
            foreach (var row in matrix.RowSet)
            {
                string pureRow = string.Join("\t", matrix.ColumnSet.Select(column => matrix[row, column].ToString()));
                if (withRowHeader)
                    yield return $"{row}\t{pureRow}";
                else
                    yield return pureRow;
            }
        }

        /// <summary>
        /// From a tab seperated values to a raw string matrix.
        /// The row and columns will be labeled with 0/1/2/3...
        /// </summary>
        /// <param name="list">The input list.</param>
        /// <returns>The output matrix.</returns>
        public static Matrix<string, string, string> FromRawText(IEnumerable<string> list)
        {
            int rowCount = 0;
            Matrix<string, string, string> matrix = new Matrix<string, string, string>("");
            foreach(string line in list)
            {
                var split = line.Split('\t');
                for(int columnCount = 0; columnCount < split.Length; columnCount++)
                {
                    matrix[rowCount.ToString(), columnCount.ToString()] = split[columnCount];
                }
            }
            return matrix;
        }

        /// <summary>
        /// From a tab seperated values to a string matrix.
        /// The row and columns will use the first row/column if the shrinkTop/shrinkLeft is true.
        /// </summary>
        /// <param name="list">The input list.</param>
        /// <param name="shrinkLeft">Whether to use the leftest column as the row header.</param>
        /// <param name="shrinkTop">Whether to use the topest row as the column header.</param>
        /// <returns>The output matrix.</returns>
        public static Matrix<string,string,string> FromText(IEnumerable<string> list, bool shrinkLeft=true,bool shrinkTop = true)
        {
            var rawMatrix = FromRawText(list);
            if (shrinkLeft && shrinkTop)
                return rawMatrix.ShrinkTop().ShrinkLeft();
            if (shrinkLeft)
                return rawMatrix.ShrinkLeft();
            if (shrinkTop)
                return rawMatrix.ShrinkTop();
            return rawMatrix;
        }

        /// <summary>
        /// Shrink the leftest column from the matrix.
        /// The first column of the original matrix will be the new row set of the new matrix.
        /// Most of the time it will be used only with the FromRawText, otherwise, there are possibility of losing data.
        /// </summary>
        /// <param name="matrix">The input matrix.</param>
        /// <returns>The new matrix</returns>
        public static Matrix<string,string,string> ShrinkLeft(this Matrix<string,string,string> matrix)
        {
            string firstColumnName = matrix.ColumnSet.First();
            Dictionary<string, string> namingDict = new Dictionary<string, string>();
            foreach (string rowName in matrix.RowSet)
                namingDict[matrix[rowName, firstColumnName]] = rowName;
            Matrix<string, string, string> newMatrix = new Matrix<string, string, string>();
            foreach(string row in namingDict.Keys)
                foreach(string column in matrix.ColumnSet.Skip(1))
                    newMatrix[row, column] = matrix[namingDict[row], column];
            return newMatrix;
        }

        /// <summary>
        /// Shrink the topest column from the matrix.
        /// The first row of the original matrix will be the new row set of the new matrix.
        /// Most of the time it will be used only with the FromRawText, otherwise, there are possibility of losing data.
        /// </summary>
        /// <param name="matrix">The input matrix.</param>
        /// <returns>The new matrix</returns>
        public static Matrix<string,string,string> ShrinkTop(this Matrix<string,string,string> matrix)
        {
            string firstRowName = matrix.RowSet.First();
            Dictionary<string, string> namingDict = new Dictionary<string, string>();
            foreach (string columnName in matrix.ColumnSet)
                namingDict[matrix[firstRowName, columnName]] = columnName;
            Matrix<string, string, string> newMatrix = new Matrix<string, string, string>();
            foreach (string row in matrix.RowSet.Skip(1))
                foreach (string column in namingDict.Keys)
                    newMatrix[row, column] = matrix[row, namingDict[column]];
            return newMatrix;
        }
    }
}
