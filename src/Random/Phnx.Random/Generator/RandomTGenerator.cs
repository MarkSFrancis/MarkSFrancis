﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Phnx.Random.Generator
{
    /// <summary>
    /// Provides methods for generating a random generic type, by creating a random instance of all CLR properties
    /// </summary>
    public static class RandomTGenerator
    {
        /// <summary>
        /// Get a random instance of a generic type by creating an instance of itself if it's a CLR property, or a random instance of all properties if it's a complex type
        /// </summary>
        /// <typeparam name="T">The type of object to create</typeparam>
        /// <param name="shallow">Whether to create a deep instance of a random, where all complex child properties are also randomized (non-recusively), or only shallow, where complex child properties are left as their defaults</param>
        /// <returns>A random instance of <typeparamref name="T"/></returns>
        public static T Get<T>(bool shallow)
        {
            Stack<Type> typeStack = new Stack<Type>();
            return (T)ComplexGetRandom(typeof(T), typeStack, shallow);
        }

        private static Array GetRandomOfArray(Type arrayElementsType, int maxArraySize, Stack<Type> knownTypes, bool shallow)
        {
            int arraySize = GetRandom.Int(0, maxArraySize);

            var newArray = Array.CreateInstance(arrayElementsType, arraySize);

            for (int index = 0; index < newArray.Length; index++)
            {
                var newValue = ComplexGetRandom(arrayElementsType, knownTypes, shallow);
                newArray.SetValue(newValue, index);
            }

            return newArray;
        }

        private static object ComplexGetRandom(Type instanceToCreate, Stack<Type> typeStack, bool shallow)
        {
            if (instanceToCreate.IsInterface)
            {
                return null;
            }

            // Check for recursive type
            if (typeStack.Contains(instanceToCreate))
            {
                if (instanceToCreate.IsClass)
                {
                    return null;
                }
                else
                {
                    // Non-nullable type, create instance instead
                    return Activator.CreateInstance(instanceToCreate);
                }
            }

            // Add to stack of known types
            typeStack.Push(instanceToCreate);
            object instance;

            if (instanceToCreate.IsArray)
            {
                var instanceArray = GetRandomOfArray(instanceToCreate.GetElementType(), 10, typeStack, shallow);

                instance = instanceArray;
            }
            else if (!TrySimpleGetRandom(instanceToCreate, out instance))
            {
                // Complex object

                // Create instance and then set each property to a random value
                instance = Activator.CreateInstance(instanceToCreate);

                foreach (var field in instanceToCreate.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).Where(p => !p.IsInitOnly))
                {
                    object propVal;
                    if (shallow)
                    {
                        // If it's a complex property, skip
                        TrySimpleGetRandom(field.FieldType, out propVal);
                    }
                    else
                    {
                        propVal = ComplexGetRandom(field.FieldType, typeStack, shallow);
                    }

                    field.SetValue(instance, propVal);
                }
            }

            typeStack.Pop();
            return instance;
        }

        /// <summary>
        /// Gets a random instance of a type if it's a known CLR type
        /// </summary>
        private static bool TrySimpleGetRandom(Type instanceToCreate, out object instance)
        {
            if (instanceToCreate == typeof(string))
            {
                instance = GetRandom.AlphanumericText();
                return true;
            }
            else if (instanceToCreate == typeof(int))
            {
                instance = GetRandom.Int();
                return true;
            }
            else if (instanceToCreate == typeof(bool))
            {
                instance = GetRandom.Bool();
                return true;
            }
            else if (instanceToCreate == typeof(byte))
            {
                instance = GetRandom.Byte();
                return true;
            }
            else if (instanceToCreate == typeof(DateTime))
            {
                instance = GetRandom.DateTime();
                return true;
            }
            else if (instanceToCreate == typeof(char))
            {
                instance = GetRandom.Letter();
                return true;
            }
            else if (instanceToCreate == typeof(long))
            {
                instance = GetRandom.Long();
                return true;
            }
            else if (instanceToCreate == typeof(sbyte))
            {
                instance = GetRandom.SByte();
                return true;
            }
            else if (instanceToCreate == typeof(short))
            {
                instance = GetRandom.Short();
                return true;
            }
            else if (instanceToCreate == typeof(uint))
            {
                instance = GetRandom.UInt();
                return true;
            }
            else if (instanceToCreate == typeof(ulong))
            {
                instance = GetRandom.ULong();
                return true;
            }
            else if (instanceToCreate == typeof(ushort))
            {
                instance = GetRandom.UShort();
                return true;
            }
            else if (instanceToCreate == typeof(Guid))
            {
                instance = Guid.NewGuid();
                return true;
            }
            else
            {
                instance = null;
                return false;
            }
        }
    }
}
