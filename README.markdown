## typefoundry ##

A dependency injection system. It's quite possibly rubbish, but I like it. Beware, it's convention driven which is great if you know the conventions.

## All You Need To Know ##

I've decided to make all the .Net architects weep and designed it around a singleton. This might lead to a revocation of my cool kid license, but I never had it so, nothing lost.

### Defining Dependencies ###

Dependencies are defined in a fluent API within a lambda.

	Foundry.Dependencies( x => 
		{
			// A straight-forward definition
			x.For<IDependency>.Use<Dependency>();

			// Defining runtime type
			x.For<IDependency>.Use(typeof(Dependency));
			
			// Open generics...
			x.For(typeof(IDependency<>)).Use(typeof(Dependency<>));

			// Using a pre-instantiated singleton
			x.For<IDependency>.Use( Instance );

			// Singleton - instantiated only once
			x.For<IDependency>.Use<Singleton>().AsSingleton();

			// Adding a custom factory
			x.For<IDependency>.CreateWith( x => Factory.Create( x ) );

			// Adding a type to be instatiated
			x.For<IDependency>.Add<Dependency>();
		});	


### Retrieving Dependencies ###

Dependencies are retrieved from the Container type.

	// Gets a list of all dependencies defined for the interface
	IEnumerable list = Foundry.GetAllInstancesOf<IDependency>();

	// Gets a list of all dependencies defined for the interface
	IEnumerable list = Foundry.GetAllInstancesOf(typeof(IDependency));

	// Gets the instance defined for Type IDependency
	IDependency instance = Foundry.GetInstanceOf<IDependency>();

	// Gets the named instance defined for Type IDependency
	IDependency instance = Foundry.GetInstanceOf<IDependency>("alias");

	// Gets the instance defined for the runtime type
	IDependency instance = Foundry.GetInstanceOf(typeof(IDependency));

	// Gets the named instance defined for runtime type
	IDependency instance = Foundry.GetInstanceOf(typeof(IDependency), "alias");
 


### Conventions ###

#### Types With Only One Constructor ####

This is simple, if there's only 1 constructor, that gets used...

#### Types With Multiple Constructors ####

Typefoundry will always select the greediest constructor for which it can supply all the arguments based on its configuration. This means if there is a constructor with 5 arguments but typefoundry cannot supply all of them but there is a constructor with 4 arguments that typefoundry can supply all the arguments to, typefoundry uses the constructor with 4 arguments.

#### Single Implementations ####

If an interface has only one implementation, it will be wired into the container for you. This means you can just ask for the interface at any time without having to define it.

Yay.

#### Multiple Implementations ####

In the event an interface has multiple implementations, each implementation will be wired as a named dependency using the name of the type. This means there will be no default implementation defined and that you will still need to provide one in order for requests for an interface to be successfully fufilled when no name is given.

### Extras ###

There are three additional interfaces that can help define dependencies, scanning instructions and library/project level initialization. When implemented, these implementations are loaded an executed in the following order within their projects:

1.  scanning - IDefineScan
2.  dependencies - IDefineDependencies
3.  initialization - IInitialize

The order in which the projects go through these steps are determined based on dependencies between projects. Circular dependencies will crash this process, but then, you should never have circular dependencies in your projects.