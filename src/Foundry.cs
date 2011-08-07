using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using typefoundry.Impl;
using typefoundry.Impl.Scan;
using typefoundry.Impl.Utility;

namespace typefoundry
{
    public class Foundry
    {
        private static IDependencyAdapter _adapter;
        private static bool Initialized { get; set; }
        private static object _lock = new object();

        public static ScanIndex Index { get; set; }
        public static IDependencyAdapter Adapter 
        { 
            get 
            {
                if( _adapter == null )
                    EnforceInitialization();
                return _adapter;
            }
            set { _adapter = value; }
        }
        
        static Foundry()
        {
            EnforceInitialization();
        }

        private static void EnforceInitialization()
        {
            if( Initialized )
                return;
            lock(_lock)
            {
                if( Initialized )
                    return;
                Initialized = true;
                Index = new ScanIndex();
                Index.Start();
                Wireup();
            }
        }

        private static void Wireup()
        {
            Type defaultAdapter = typeof( SimpleDependencyRegistry );
            var dependencyAdapterType = Index
                .ImplementorsOfType[typeof( IDependencyAdapter )]
                .FirstOrDefault( x => !x.Equals( defaultAdapter ) ) ?? defaultAdapter;
            var dependencyAdapter = Activator.CreateInstance( dependencyAdapterType ) as IDependencyAdapter;
            Adapter = dependencyAdapter;
            
            var initializers = new List<Type>();
            Index.ConfiguredSymbiotes.Keys.ToList().ForEach( x => LoadSymbioteDependencies( x, initializers ) );
            var uniqueInitializers = initializers
                .Distinct();

            uniqueInitializers
                .ForEach( InitializeAssembly );

            Index
                .AssemblyInitializers
                .Except( uniqueInitializers )
                .Distinct()
                .ForEach( InitializeAssembly );
        }

        private static void LoadSymbioteDependencies( Assembly assembly, List<Type> initializers )
        {
            if ( Index.ConfiguredSymbiotes[assembly] )
                return;

            var dependencies = GetDependencies( assembly );
            dependencies.ForEach( x => LoadSymbioteDependencies( x, initializers ) );

            var scanInstructionType = Index.ScanningInstructions.FirstOrDefault( x => x.Assembly.Equals( assembly ) );
            var dependencyDefinitionType = Index.DependencyDefinitions.FirstOrDefault( x => x.Assembly.Equals( assembly ) );
            
            var scanInstructions = scanInstructionType != null 
                ? Activator.CreateInstance( scanInstructionType ) as IDefineScan
                : null;

            var dependencyDefinitions = dependencyDefinitionType != null
                ? Activator.CreateInstance( dependencyDefinitionType ) as IDefineDependnecies
                : null;

            Dependencies( x =>
                {
                    if( scanInstructions != null )
                        x.Scan( scanInstructions.Scan() );
                    
                    if ( dependencyDefinitions != null )
                        dependencyDefinitions.DefineDependencies()( x );
                } );

            Index.ConfiguredSymbiotes[ assembly ] = true;
            var initializerType = Index.AssemblyInitializers.FirstOrDefault( x => x.Assembly.Equals( assembly ) );
            if( initializerType != null )
                initializers.Add( initializerType );
        }

        private static void InitializeAssembly( Type initializerType )
        {
            var initializer = initializerType != null
                ? Activator.CreateInstance( initializerType ) as IInitialize
                : null;

            if ( initializer != null )
                initializer.Initialize();
        }

        private static List<Assembly> GetDependencies( Assembly assembly )
        {
            return Index
                .ReferenceLookup[assembly]
                .Where( x => Index.ConfiguredSymbiotes.Keys.Any( r => r.Equals( x ) ) )
                .ToList();
        }

        public static IEnumerable<T> GetAllInstancesOf<T>()
        {
            return Adapter.GetAllInstances<T>();
        }

        public static IEnumerable GetAllInstancesOf( Type type )
        {
            return Adapter.GetAllInstances( type );
        }

        public static T GetInstanceOf<T>()
        {
            return Adapter.GetInstance<T>();
        }

        public static T GetInstanceOf<T>( string name )
        {
            return Adapter.GetInstance<T>( name );
        }

        public static object GetInstanceOf( Type type )
        {
            return Adapter.GetInstance( type );
        }

        public static object GetInstanceOf( Type type, string name )
        {
            return Adapter.GetInstance( type, name );
        }

        public static void Dependencies( Action<DependencyConfigurator> configurator )
        {
            EnforceInitialization();
            var config = new DependencyConfigurator();
            configurator( config );
            config.RegisterDependencies( Adapter );
        }

        public static void Reset()
        {
            Initialized = false;
            Adapter.Reset();
        }
    }
}
