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

namespace typefoundry.Impl.Utility
{
    public static class Extensions
    {
        public static void ForEach<T>( this IEnumerable<T> enumerable, Action<T> action )
        {
            if ( enumerable == null )
                return;

            if ( action == null )
                throw new ArgumentNullException( "action" );

            foreach( var t in enumerable )
            {
                action( t );
            }
        }

        public static TValue GetOrDefault<TKey, TValue>( this ConcurrentDictionary<TKey, TValue> dictionary, TKey key )
        {
            TValue value = default(TValue);
            dictionary.TryGetValue( key, out value );
            return value;
        }

        public static object ToListOf<T>( this IEnumerable<T> enumerable, Type type )
        {
            var list = Activator.CreateInstance( typeof( List<> ).MakeGenericType( type ) );
            var add = list.GetType().GetMethod( "Add" );
            enumerable.ForEach( x => add.Invoke( list, new object[] { Reflector.Cast( x, type ) } ) );
            return list;
        }

        public static Tuple<bool, TValue> TryGet<TKey, TValue>( this IDictionary<TKey, TValue> dictionary, TKey key )
        {
            TValue value = default( TValue );
            var exists = dictionary.TryGetValue( key, out value );
            return Tuple.Create( exists, value );
        }

        public static TValue AddOrUpdate<TKey, TValue>( this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> addValue, Func<TKey, TValue, TValue> updateValue )
        {
            TValue value = default( TValue );
            if( dictionary.TryGetValue( key, out value ) )
            {
                value = updateValue( key, value );
            }
            else
            {
                value = addValue( key );
                dictionary.Add( key, value );
            }
            return value;
        }
    }
}