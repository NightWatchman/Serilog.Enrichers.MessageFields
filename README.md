# Serilog.Enrichers.MessageFields

## WARNING

This library isn't designed for public consumption, and you probably don't need it. Most of the time
the native serilog structured logging support is sufficient, and will output these fields for you when
writing to a sink that supports structured logs.

## Instructions

To use the enricher, first install the NuGet package:

```powershell
Install-Package Serilog.Enrichers.MessageFields
```

Then, apply the enricher to your `LoggerConfiguration`:

```csharp
Log.Logger = new LoggerConfiguration()
  .Enrich.WithMessageFields()
  // ...other configuration...
  .CreateLogger();
```

Copyright (c) Eric Rushing 2022 - Provided under the [Apache License, Version 2.0](http://apache.org/licenses/LICENSE-2.0.html).
