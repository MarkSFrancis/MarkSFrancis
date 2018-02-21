﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace MarkSFrancis.Console
{
    /// <summary>
    /// Stream data input and output between a given <see cref="TextReader"/> and <see cref="TextWriter"/> such as <see cref="System.Console.In"/>, a file, or a web stream
    /// </summary>
    public class TextIo
    {
        /// <summary>
        /// The text reader to get input from
        /// </summary>
        public TextReader Input { get; }

        /// <summary>
        /// The text writer to write output to
        /// </summary>
        public TextWriter Output { get; }

        /// <summary>
        /// Creates a new instance of <see cref="TextIo"/>
        /// </summary>
        /// <param name="input">Where to get input from</param>
        /// <param name="output">Where to send output to</param>
        public TextIo(TextReader input, TextWriter output)
        {
            Input = input;
            Output = output;
        }

        /// <summary>
        /// Write a line of text to the <see cref="Output"/>, followed by a newline
        /// </summary>
        /// <param name="s">The text to write</param>
        public void WriteLine(string s)
        {
            Output.WriteLine(s);
        }

        /// <summary>
        /// Write an oject to the <see cref="Output"/> using its <see cref="Object.ToString()"/>, followed by a newline
        /// </summary>
        /// <param name="o">The object to write</param>
        public void WriteLine(object o)
        {
            Output.WriteLine(o);
        }

        /// <summary>
        /// Write text to the <see cref="Output"/>
        /// </summary>
        /// <param name="s">The text to write</param>
        public void Write(string s)
        {
            Output.Write(s);
        }

        /// <summary>
        /// Write an oject to the <see cref="Output"/> using its <see cref="Object.ToString()"/>
        /// </summary>
        /// <param name="o">The object to write</param>
        public void Write(object o)
        {
            Output.Write(o);
        }

        /// <summary>
        /// Write a collection of text to the <see cref="Output"/>, with each item delimited by a newline
        /// </summary>
        /// <param name="collection">The collection of text to write</param>
        public void WriteCollection(params string[] collection)
        {
            WriteCollection((IEnumerable<string>)collection);
        }

        /// <summary>
        /// Write a collection of objects to the <see cref="Output"/> using their default <see cref="object.ToString()"/>, with each item delimited by a newline
        /// </summary>
        /// <param name="collection">The collection of objects to write</param>
        public void WriteCollection<T>(params T[] collection)
        {
            WriteCollection((IEnumerable<T>)collection);
        }
        
        /// <summary>
        /// Write a collection of text to the <see cref="Output"/>, with each item delimited by a newline
        /// </summary>
        /// <param name="collection">The collection of text to write</param>
        public void WriteCollection(IEnumerable<string> collection)
        {
            WriteCollection(collection, Environment.NewLine);
        }

        /// <summary>
        /// Write a collection of text to the <see cref="Output"/>, with each item delimited with custom text
        /// </summary>
        /// <param name="collection">The collection of text to write</param>
        /// <param name="delimiter">The delimiter to write between each item in the collection</param>
        public void WriteCollection(IEnumerable<string> collection, string delimiter)
        {
            WriteLine(string.Join(delimiter, collection));
        }

        /// <summary>
        /// Write a collection of objects to the <see cref="Output"/> using their default <see cref="object.ToString()"/>, with each item delimited by a newline
        /// </summary>
        /// <param name="collection">The collection of objects to write</param>
        public void WriteCollection<T>(IEnumerable<T> collection)
        {
            WriteCollection(collection.Select(o => o.ToString()));
        }

        /// <summary>
        /// Write a collection of objects to the <see cref="Output"/> using their default <see cref="object.ToString()"/>, with each item delimited with custom text
        /// </summary>
        /// <param name="collection">The collection of objects to write</param>
        /// <param name="delimiter">The delimiter to write between each item in the collection</param>
        public void WriteCollection<T>(IEnumerable<T> collection, string delimiter)
        {
            WriteCollection(collection.Select(o => o.ToString()), delimiter);
        }

        /// <summary>
        /// Get a line of text from the <see cref="Input"/>
        /// </summary>
        /// <returns></returns>
        public string GetString()
        {
            return Input.ReadLine();
        }

        /// <summary>
        /// Write a message to the <see cref="Output"/>, and then get a line of text from the <see cref="Input"/>
        /// </summary>
        /// <param name="message">The message to write</param>
        /// <returns></returns>
        public string GetString(string message)
        {
            message = FormatQuestion(message);

            Write(message);
            return GetString();
        }

        /// <summary>
        /// Write a message to the <see cref="Output"/>, and then get attempt to get an <see cref="int"/> from the <see cref="Input"/>. This process repeats if the received input is invalid. Any errors are written to <see cref="Debug"/>
        /// </summary>
        /// <param name="message">The message to write</param>
        /// <returns></returns>
        public int GetInt(string message = null)
        {
            return Get(int.Parse, message);
        }

        /// <summary>
        /// Write a message to the <see cref="Output"/>, and then get attempt to get a <see cref="long"/> from the <see cref="Input"/>. This process repeats if the received input is invalid. Any errors are written to <see cref="Debug"/>
        /// </summary>
        /// <param name="message">The message to write</param>
        /// <returns></returns>
        public long GetLong(string message = null)
        {
            return Get(long.Parse, message);
        }

        /// <summary>
        /// Write a message to the <see cref="Output"/>, and then get attempt to get a <see cref="decimal"/> from the <see cref="Input"/>. This process repeats if the received input is invalid. Any errors are written to <see cref="Debug"/>
        /// </summary>
        /// <param name="message">The message to write</param>
        /// <returns></returns>
        public decimal GetDecimal(string message = null)
        {
            return Get(decimal.Parse, message);
        }

        /// <summary>
        /// Write a message to the <see cref="Output"/>, and then get attempt to get a <see cref="DateTime"/> from the <see cref="Input"/> using <see cref="DateTime.Parse(string)"/>. This process repeats if the received input is invalid. Any errors are written to <see cref="Debug"/>
        /// </summary>
        /// <param name="message">The message to write</param>
        /// <returns></returns>
        public DateTime GetDateTime(string message = null)
        {
            return Get(DateTime.Parse, message);
        }

        /// <summary>
        /// Write a message to the <see cref="Output"/>, and then get attempt to get a value from the <see cref="Input"/> using the given converter. If the received input is invalid, this process repeats. Any errors are written to <see cref="Debug"/>
        /// </summary>
        /// <param name="converter">The method to use when converting from the text to the desired type</param>
        /// <param name="message">The message to write</param>
        /// <returns></returns>
        public virtual T Get<T>(Func<string, T> converter, string message = null)
        {
            if (converter == null)
            {
                throw new ArgumentNullException(nameof(converter));
            }

            message = FormatQuestion(message);

            T returnValue = default(T);

            {
                bool conversionWorked;
                do
                {
                    if (message != null)
                    {
                        Write(message);
                    }

                    var valueEntered = GetString();

                    try
                    {
                        returnValue = converter(valueEntered);

                        conversionWorked = true;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);

                        conversionWorked = false;
                    }
                } while (!conversionWorked);
            }

            return returnValue;
        }

        /// <summary>
        /// Format a question for displaying on the console by adding a ": " to the end of the message if it does not already contain one
        /// </summary>
        /// <param name="question">The question to format</param>
        /// <returns></returns>
        protected string FormatQuestion(string question)
        {
            if (!string.IsNullOrEmpty(question))
            {
                question = question.Trim();

                if (question.EndsWith(":"))
                {
                    question += " ";
                }
                else
                {
                    question += ": ";
                }
            }

            return question;
        }
    }
}