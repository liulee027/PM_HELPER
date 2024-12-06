//=============================================================================
//D The CommandUtils class.
//
// ----------------------------------------------------------------------------
// Copyright 2011 Delcam plc., Birmingham, UK
// ----------------------------------------------------------------------------
//
// History.
// DICC  Who When     What
// ----- --- -------- ---------------------------------------------------------
// 93978 PSL 10/11/11 Written.
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Delcam.Plugins.Utils
{
    //=============================================================================
    /// <summary>
    /// A utility class to aid with parsing plugin commands.
    /// </summary>
    //
    // History.
    // DICC  Who When     What
    // ----- --- -------- ---------------------------------------------------------
    // 93978 PSL 10/11/11 Written.
    //-----------------------------------------------------------------------------
    public class CommandUtils
    {
        //=============================================================================
        /// <summary>
        /// A function to take a command, and break it into individual tokens.  The function is careful to
        /// not break up double quoted strings, and is careful to swap '\n' and '\"' and '\\' with 
        /// approriate values.
        /// </summary>
        /// <param name="command">The command to be parsed</param>
        /// <returns>A list of string tokens.</returns>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public static List<string> GetCommandTokens(string command)
        {
            // Create the token list
            List<string> tokens = new List<string>();

            // Sanity check
            if (command != null && command.Length > 0)
            {
                // Break it into tokens, but don't smash up double quoted strings
                StringBuilder token = new StringBuilder("", command.Length);
                bool quoted = false;
                bool escape = false;
                foreach (char c in command)
                {
                    if (quoted)
                    {
                        // Deal with escaped characters
                        if (escape)
                        {
                            // Test for known escaped chars
                            if (c == '"')
                            {
                                // They wanted a literal "
                                token.Append(c);
                            }
                            else if (c == '\\')
                            {
                                // They wanted a literal \
                                token.Append(c);
                            }
                            else if (c == 'n')
                            {
                                // They wanted a newline
                                token.Append('\n');
                            }
                            else
                            {
                                // We don't recognise what they're after, so pretend it wasn't escaped
                                token.Append(c);
                            }

                            // We're no longer escaped
                            escape = false;
                        }
                        else
                        {
                            if (c == '"')
                            {
                                // This is the end of the quoted string
                                token.Append(c);
                                quoted = false;

                                // Commit token - even if it's a blank string
                                if (token.Length > 0)
                                {
                                    tokens.Add(token.ToString());
                                }
                                else
                                {
                                    tokens.Add("");
                                }

                                // Start new token
                                token = new StringBuilder("", command.Length);
                            }
                            else if (c == '\\')
                            {
                                escape = true;
                            }
                            else
                            {
                                token.Append(c);
                            }
                        }
                    }
                    else
                    {
                        if (c == ' ')
                        {
                            // Commit token
                            if (token.Length > 0)
                            {
                                tokens.Add(token.ToString().ToUpper());
                            }

                            // Start new token
                            token = new StringBuilder("", command.Length);
                        }
                        else if (c == '"')
                        {
                            // Commit token - this would be weird, but possible...
                            if (token.Length > 0)
                            {
                                tokens.Add(token.ToString().ToUpper());
                            }

                            // Start new token with a "
                            token = new StringBuilder("\"", command.Length);

                            // The new token will be parsed as a doubly-quoted string
                            quoted = true;
                        }
                        else if (c == '\\')
                        {
                            // We don't expect escaped characters outside of quoted strings,
                            // so quietly ignore...
                        }
                        else
                        {
                            token.Append(c);
                        }
                    }
                }

                // Flush the last token
                if (token.Length > 0)
                {
                    tokens.Add(token.ToString().ToUpper());
                }
            }

            // Return the list
            return tokens;
        }

        //=============================================================================
        /// <summary>
        /// Removes and returns the first token from the list of tokens.
        /// </summary>
        /// <param name="tokens">The list of tokens.</param>
        /// <returns>The first token from the list.</returns>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public static string GetStringToken(ref List<string> tokens)
        {
            // Sanity check
            if (tokens.Count == 0) return null;

            // Extract the token
            string token = tokens[0];

            // Pop it from the list
            tokens.RemoveAt(0);

            // Return the token
            return token;
        }

        //=============================================================================
        /// <summary>
        /// Removes and returns the first token from the list of tokens.  If this token is double quoted, the quotation marks
        /// will be removed before returning the string.
        /// </summary>
        /// <param name="tokens">The list of tokens.</param>
        /// <returns>The first token from the list.</returns>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public static string GetDoubleQuotedStringToken(ref List<string> tokens)
        {
            // Get the string token
            string token = GetStringToken(ref tokens);
            if (token == null || token.Length < 2) return null;

            // Remove double quotes
            if (token[0] == '"')
            {
                token = token.Remove(0, 1);
            }
            if (token[token.Count() - 1] == '"')
            {
                token = token.Remove(token.Count() - 1);
            }

            // That's it
            return token;
        }

        //=============================================================================
        /// <summary>
        /// Removes and returns the first token from the list of tokens, parsing it as a double.
        /// If parsing fails, the return type will be null.
        /// </summary>
        /// <param name="tokens">The list of tokens.</param>
        /// <returns>The first token from the list, parsed as a double.  Will be null if parsing fails.</returns>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public static double? GetRealToken(ref List<string> tokens)
        {
            // Get the string token
            string token = GetStringToken(ref tokens);
            if (token == null) return null;

            // Attempt to parse it
            double num;
            if (double.TryParse(token, out num))
            {
                return num;
            }
            else
            {
                return null;
            }
        }

        //=============================================================================
        /// <summary>
        /// Removes and returns the first token from the list of tokens, parsing it as an int.
        /// If parsing fails, the return type will be null.
        /// </summary>
        /// <param name="tokens">The list of tokens.</param>
        /// <returns>The first token from the list, parsed as an int.  Will be null if parsing fails.</returns>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public static int? GetIntToken(ref List<string> tokens)
        {
            // Get the string token
            string token = GetStringToken(ref tokens);
            if (token == null) return null;

            // Attempt to parse it
            int num;
            if (int.TryParse(token, out num))
            {
                return num;
            }
            else
            {
                return null;
            }
        }

        //=============================================================================
        /// <summary>
        /// Removes and returns the first token from the list of tokens, parsing it as a bool.
        /// If parsing fails, the return type will be null.
        /// Acceptable true states are: 'ON', 'YES' and 'TRUE'.  Acceptable off states are: 'OFF', 'NO' and 'FALSE'.
        /// </summary>
        /// <param name="tokens">The list of tokens.</param>
        /// <returns>The first token from the list, parsed as a bool.  Will be null if parsing fails.</returns>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public static bool? GetBoolToken(ref List<string> tokens)
        {
            // Get the string token
            string token = GetStringToken(ref tokens);
            if (token == null) return null;

            // Attempt to parse it
            switch (token)
            {
                case "ON":
                    return true;
                case "YES":
                    return true;
                case "TRUE":
                    return true;
                case "OFF":
                    return false;
                case "NO":
                    return false;
                case "FALSE":
                    return false;
                default:
                    return null;
            }
        }

        //=============================================================================
        /// <summary>
        /// Removes and returns the first 3 first tokend from the list of tokens, parsing them as R G B shorts
        /// and returning a Color struct.  If parsing fails, the return type will be null.
        /// </summary>
        /// <param name="tokens">The list of tokens.</param>
        /// <returns>A colour struct, built from the first three tokens.  Will be null if parsing fails.</returns>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public static Color? GetColourTokens(ref List<string> tokens)
        {
            // Get the int tokens
            int? r = GetIntToken(ref tokens);
            int? g = GetIntToken(ref tokens);
            int? b = GetIntToken(ref tokens);
            if (r == null || g == null || b == null) return null;

            // Attempt to turn it into a colour
            int R = (int)r < 0 ? 0 : (int)r > 255 ? 255 : (int)r;
            int G = (int)g < 0 ? 0 : (int)g > 255 ? 255 : (int)g;
            int B = (int)b < 0 ? 0 : (int)b > 255 ? 255 : (int)b;
            Color c = Color.FromRgb((byte)R, (byte)G, (byte)B);
            return c;
        }
    }
}
