// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System;
using System.Collections.Generic;
using System.Text;

namespace SE.Apollo.Package
{
    public static partial class StringExtension
    {
        /// <summary>
        /// Converts a given string from package name format
        /// </summary>
        public static string FromPackageName(this string str)
        {
            StringBuilder result = new StringBuilder(str);
            bool turnToUpper = true;

            for (int i = 0; i < result.Length; i++)
            {
                switch (result[i])
                {
                    case '/':
                    case '.':
                        {
                            turnToUpper = true;
                        }
                        break;
                    case '-':
                        {
                            result.Remove(i, 1);
                            i--;

                            turnToUpper = true;
                        }
                        break;
                    default: if (turnToUpper)
                        {
                            result[i] = Char.ToUpperInvariant(result[i]);
                            turnToUpper = false;
                        }
                        break;
                }
            }
            return result.ToString();
        }
    }
}