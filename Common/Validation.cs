using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    /// <summary>
    /// This class is for validation, for pre check mostly.
    /// </summary>
    public static class Validation
    {
        /// <summary>
        /// Requires the validator to be true, otherwise throw exception.
        /// </summary>
        /// <param name="valid">The validator to be decided</param>
        public static void Requires(bool valid)
        {
            if (!valid)
                throw new InfException();
        }
        /// <summary>
        /// Requires the validator to be true, otherwise throw exception with certain message.
        /// </summary>
        /// <param name="valid">The validator to be decided.</param>
        /// <param name="errorMessage">The message of the exception</param>
        public static void Requires(bool valid, string errorMessage)
        {
            if (!valid)
                throw new InfException(errorMessage);
        }
    }
}
