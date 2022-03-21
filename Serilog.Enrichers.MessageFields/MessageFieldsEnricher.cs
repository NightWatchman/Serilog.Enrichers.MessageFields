using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
namespace Serilog.Enrichers.MessageFields;

/// <summary>
/// Enriches Serilog log events by adding a "MessageFields" property
/// containing the structured data from the log event.
/// </summary>
///  
/// <example>
/// <code>Log.Information("User: {Username} Context: {Environment}", "joegauchoii", "Development");</code>
/// Will yield:
/// 
/// {
///   ...
///   "MessageFields": {
///     "Username": "joegauchoii",
///     "Environment": "Development"
///   }
/// }
///
/// whenever the serilog log event properties are rendered.
/// </example>
public class MessageFieldsEnricher : ILogEventEnricher
{
  private readonly IReadOnlyCollection<string> OmitProperties;
  
  /// <param name="omit">These fields will be omitted from the generated properties.</param>
  public MessageFieldsEnricher(IEnumerable<string> omit)
  {
    OmitProperties = omit.ToArray();
  }

  public MessageFieldsEnricher() : this(Array.Empty<string>()) { }

  public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
  {
    var fields = logEvent.Properties
      .Where(x => !OmitProperties.Contains(x.Key))
      .ToDictionary(x => x.Key, x => x.Value);
    var prop = propertyFactory.CreateProperty("MessageFields", fields);
    logEvent.AddPropertyIfAbsent(prop);
  }
}


public static class NamedEventLoggerConfigurationExtensions
{
  public static LoggerConfiguration WithMessageFields(this LoggerEnrichmentConfiguration @this) =>
    @this == null
      ? throw new ArgumentNullException(nameof(@this))
      : @this.With(new MessageFieldsEnricher());
  
  public static LoggerConfiguration WithMessageFields(
    this LoggerEnrichmentConfiguration @this,
    IEnumerable<string> omit)
  {
    return @this == null
      ? throw new ArgumentNullException(nameof(@this))
      : @this.With(new MessageFieldsEnricher(omit));
  }

  public static LoggerConfiguration WithMessageFields(
    this LoggerEnrichmentConfiguration @this,
    params string[] omit)
  {
    return WithMessageFields(@this, (IEnumerable<string>)omit);
  }
}
