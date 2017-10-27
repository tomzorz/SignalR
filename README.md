**For the original documentation please visit the original repository: [https://github.com/SignalR/SignalR](https://github.com/SignalR/SignalR)**

# The "old" ASP.NET SignalR on .NET Standard 2.0 working inside the "new" ASP.NET

## Why?

One of the projects I'm working on requires the following:

- RPC library
- which is cross platform
- supports a many clients, including Windows 10 AU (10.14393) because that's the latest Windows version available on the HoloLens
- preferably easy to use

Now the "new" SignalR would be great, except for its client which requires .NET Standard 2.0 support, which as explained above the HoloLens doesn't have at the moment.

## How

There are three main issues that had to be solved to get the SignalR.Core part running. (You'll notice that I removed most of everything else.)

### Data protection API

Easy: there's a NuGet package that reimplements it. (This is not part of this repo.)

### Making the new middleware model and the old middleware model play nice

Not that hard: we can write an adaptor that takes care of this. (This is not part of this repo.)

### PerformanceCounters, .NET Standard changes etc...

Now this is where it gets a bit ugly: this is heavily Windows specific by default. But luckily it's possible to yank out some of the offending parts and modify the interface to make it work.

**Details:**

- `AppDomain.CurrentDomain.DefineDynamicAssembly(...)` is now `AssemblyBuilder.DefineDynamicAssembly(...)` [TypedLineBuilder.cs:40]
- `TypeBuilder.CreateType()` is now `TypeBuilder.CreateTypeInfo()` [TypedClientBuilder.cs:64]
- The Windows specific `CounterSample` type was used in the `IPerformanceCounter` interface and its implementations before removal. [IPerformanceCounter.cs:14] [NoOpPerformanceCounter.cs:49-52]
- The Windows specific `PerformanceCounterType` enum was used in a lot of places - I rewrote this enum inside the project. [Replacements/PerformanceCounterType.cs] [PerformanceCounterAttribute.cs:6] [PerformanceCounterManager.cs:12]
- The Windows specific `PerformanceCounter` class was used in the `PerformanceCounterManager` class. I replaced this with an expected `PlatformNotSupportedException` throw. [PerformanceCounterManager.cs:440-774]
- The `PerformanceCounterWrapper` class wrapped the Windows specific `PerformanceCounter` class, but it's usage was only in the case above this one, so I commented it out.
- There were duplicate `AssemblyTitle` and `AssemblyDescription` attributes, so I commented them out. [AssemblyInfo.cs:7-8]
- The `Resources` class had to be modified to load data from the correct resource dictionary. [Resources.Designer.cs:42]

## Tests

We'll be actively using it so I'll update this section if I see any issues.
	
## LICENSE
[Apache 2.0 License](https://github.com/SignalR/SignalR/blob/master/LICENSE.txt)

## Contributing

See the [contribution  guidelines](https://github.com/SignalR/SignalR/blob/master/CONTRIBUTING.md)