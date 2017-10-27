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

There are three main issues that had to be solved.

### Data protection API

Easy: there's a NuGet package that reimplements it.

### Making the new middleware model and the old middleware model play nice

Not that hard: we can write an adaptor that takes care of this.

### PerformanceCounters

Now this is where it gets a bit ugly: this is heavily Windows specific by default. But luckily it's possible to yank out some of the offending parts and modify the interface to make it work.

## Tests

We'll be actively using it so I'll update this section if I see any issues.
	
## LICENSE
[Apache 2.0 License](https://github.com/SignalR/SignalR/blob/master/LICENSE.txt)

## Contributing

See the [contribution  guidelines](https://github.com/SignalR/SignalR/blob/master/CONTRIBUTING.md)