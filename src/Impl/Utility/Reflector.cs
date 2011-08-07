// /* 
// Copyright 2008-2011 Alex Robson
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//    http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// */
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace typefoundry.Impl.Utility
{
    public class Reflector
    {
        private static MethodInfo _castMethod;
        private static List<Type> initializedTypes = new List<Type>();

        private static
            ConcurrentDictionary<Tuple<Type, string>, Tuple<Type, Func<object, object>, Action<object, object>>>
            memberCache =
                new ConcurrentDictionary<Tuple<Type, string>, Tuple<Type, Func<object, object>, Action<object, object>>>
                    ();

        private static BindingFlags bindingFlags = BindingFlags.FlattenHierarchy |
                                                   BindingFlags.Public |
                                                   BindingFlags.NonPublic |
                                                   BindingFlags.Instance;

        private static BindingFlags propertyFlags = BindingFlags.FlattenHierarchy |
                                                    BindingFlags.Public |
                                                    BindingFlags.Instance;
        private static MethodInfo CastMethod
        {
            get { return _castMethod = _castMethod ?? typeof(Reflector).GetMethod( "Cast", BindingFlags.Static | BindingFlags.NonPublic ); }
        }

        public static object Cast( object instance, Type type )
        {
            var closeCast = CastMethod.MakeGenericMethod( type );
            return closeCast.Invoke( instance, new[] { instance } );
        }

        private static T Cast<T>( object instance )
        {
            return (T) instance;
        }

        public static IEnumerable<Tuple<ConstructorInfo, ParameterInfo[]>> GetConstructorInfo<T>()
        {
            return GetConstructorInfo( typeof( T ) );
        }

        public static IEnumerable<Tuple<ConstructorInfo, ParameterInfo[]>> GetConstructorInfo( Type type )
        {
            return type.GetConstructors().Select( x => Tuple.Create<ConstructorInfo, ParameterInfo[]>( x, x.GetParameters() ) );
        }

        public static Tuple<ConstructorInfo, ParameterInfo[]> GetGreediestConstructor<T>()
        {
            return GetGreediestConstructor( typeof( T ) );
        }

        public static Tuple<ConstructorInfo, ParameterInfo[]> GetGreediestConstructor( Type type )
        {
            var constructors = GetConstructorInfo( type );
            var maxParams = constructors.Max( x => x.Item2.Length );
            return constructors.First( x => x.Item2.Length == maxParams );
        }

        public static IEnumerable<Type> GetInheritanceChain( Type type )
        {
            Type baseType = type.BaseType;
            Type simpleObject = typeof( object );

            while ( baseType != null && baseType != simpleObject )
            {
                yield return baseType;
                baseType = baseType.BaseType;
            }

            foreach( var baseInterface in type.GetInterfaces() )
            {
                yield return baseInterface;
            }
            yield break;
        }

        /// <summary>
        /// Finds all super-types that the given type descends from, including the type passed in
        /// </summary>
        /// <param name="type">The type to start at, when you want to find all super-types</param>
        /// <returns>list of types that the provided type inherits from, including the type argument passed in</returns>
        public static IEnumerable<Type> GetInheritanceChainFor( Type type )
        {
            yield return type;
            var chain = GetInheritanceChain( type );
            if ( chain != null )
            {
                foreach( var t in chain )
                {
                    yield return t;
                }
            }
            yield break;
        }

        /// <summary>
        /// Returns the types that inherit from the provided type.
        /// </summary>
        /// <param name="type">Super-type you want to find sub-types for.</param>
        /// <returns>list of types that inherit from the provided type.</returns>
        public static IEnumerable<Type> GetSubTypes( Type type )
        {
            yield return type;
            // Machine.Specifications and Moq-generated assemblies blow up when reading Types in, plus we don't need 'em.
            var children = AppDomain
                .CurrentDomain
                .GetAssemblies()
                .Where(
                    a =>
                    !a.FullName.Contains( "DynamicProxyGenAssembly2" ) &&
                    !a.FullName.Contains( "Machine.Specifications" ) )
                .SelectMany( s => s.GetTypes() )
                .Where( x => (x.IsSubclassOf( type ) || type.IsAssignableFrom( x )) && x != type )
                .ToList();
            foreach( var t in children.Distinct() )
            {
                yield return t;
            }
            yield break;
        }

        
        public static IEnumerable<Type> GetInterfaceChain( Type type )
        {
            return type.GetInterfaces();
        }
    }
}