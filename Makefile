.PHONY: all ACadSharp ACadSharp.Tests ACadSharp.Examples test clean
ACadSharp: src/ACadSharp/bin/Release/net8.0/ACadSharp.dll
ACadSharp.Tests: src/ACadSharp.Tests/bin/Release/net8.0/ACadSharp.Tests.dll
ACadSharp.Examples: src/ACadSharp.Examples/bin/Release/net8.0/ACadSharp.Examples.dll

ACADSHARP_CS = \
  $(wildcard src/ACadSharp/**/*.cs) \
  $(wildcard src/ACadSharp/*.cs)

EXAMPLES_CS = \
  $(wildcard src/ACadSharp.Examples/**/*.cs) \
  $(wildcard src/ACadSharp.Examples/*.cs)

TESTS_CS = \
  $(wildcard src/ACadSharp.Tests/**/*.cs) \
  $(wildcard src/ACadSharp.Tests/*.cs)

all: ACadSharp ACadSharp.Tests ACadSharp.Examples

src/ACadSharp/bin/Release/net8.0/ACadSharp.dll: $(ACADSHARP_CS)
	cd src && dotnet build --configuration Release --no-restore

src/ACadSharp.Tests/bin/Release/net8.0/ACadSharp.Tests.dll: $(TESTS_CS)
	cd src && dotnet build --no-restore

src/ACadSharp.Examples/bin/Release/net8.0/ACadSharp.Examples.dll: $(EXAMPLES_CS)
	cd src && dotnet build --configuration Debug --no-restore

test: ACadSharp.Tests
	cd src && dotnet test --configuration Debug --no-restore

clean:
	rm -rf src/ACadSharp/bin/{Debug,Release}/net8.0/*
	rm -rf src/ACadSharp/obj/{Debug,Release}/net8.0/*
